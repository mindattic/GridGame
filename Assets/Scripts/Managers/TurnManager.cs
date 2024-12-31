using Assets.Scripts.Models;
using Assets.Scripts.Utilities;
using Game.Behaviors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Phase = TurnPhase;

public class TurnManager : MonoBehaviour
{
    protected SupportLineManager supportLineManager => GameManager.instance.supportLineManager;
    protected AttackLineManager attackLineManager => GameManager.instance.attackLineManager;
    protected CombatParticipants combatParticipants
    {
        get { return GameManager.instance.combatParticipants; }
        set { GameManager.instance.combatParticipants = value; }
    }
    protected List<ActorInstance> actors
    {
        get => GameManager.instance.actors;
        set => GameManager.instance.actors = value;
    }
    protected TimerBarInstance timerBar => GameManager.instance.timerBar;
    protected IQueryable<ActorInstance> players => GameManager.instance.players;
    protected IQueryable<ActorInstance> enemies => GameManager.instance.enemies;
    protected BoardOverlayInstance boardOverlay => GameManager.instance.boardOverlay;
    protected PortraitManager portraitManager => GameManager.instance.portraitManager;
    protected AudioManager audioManager => GameManager.instance.audioManager;












    //Variables
    [SerializeField] public int currentTurn = 1;
    [SerializeField] public Team currentTeam = Team.Player;
    [SerializeField] public Phase currentPhase = Phase.Start;

    //Properties
    public bool IsPlayerTurn => currentTeam.Equals(Team.Player);
    public bool IsEnemyTurn => currentTeam.Equals(Team.Enemy);
    public bool IsStartPhase => currentPhase.Equals(Phase.Start);
    public bool IsMovePhase => currentPhase.Equals(Phase.Move);
    public bool IsAttackPhase => currentPhase.Equals(Phase.Attack);

    public bool IsFirstTurn => currentTurn == 1;


    void Awake()
    {
    }

    void Start()
    {
        Reset();
    }

    void Update()
    {

    }

    void FixedUpdate()
    {

    }

    public void Reset()
    {
        currentTurn = 1;
        currentTeam = Team.Player;
        currentPhase = Phase.Start;

        //musicSource.Stop();
        //musicSource.PlayOneShot(resourceManager.MusicTrack($"MelancholyLull"));
    }

    public void NextTurn()
    {
        currentTeam = IsPlayerTurn ? Team.Enemy : Team.Player;
        currentPhase = Phase.Start;

        supportLineManager.Clear();
        attackLineManager.Clear();
        combatParticipants.Clear();

        //Reset actors sorting
        actors.ForEach(x => x.sortingOrder = SortingOrder.Default);

        if (IsPlayerTurn)
        {
            currentTurn++;
            timerBar.Reset();
        }
        else if (IsEnemyTurn)
        {
            timerBar.Hide();

            CheckEnemySpawn();
            ExecuteEnemyMove();
        }
    }


    #region Player Attack Methods

    private bool AssignAlignedPlayers()
    {
        foreach (var actor1 in players)
        {
            foreach (var actor2 in players)
            {
                if (actor1 == null || actor2 == null || actor1.Equals(actor2)
                    || !actor1.IsActive || !actor1.IsAlive || !actor2.IsActive || !actor2.IsAlive
                    || combatParticipants.HasAlignedPair(actor1, actor2))
                    continue;

                if (actor1.IsSameColumn(actor2.location))
                {
                    var pair = new ActorPair(actor1, actor2, Axis.Vertical);
                    combatParticipants.alignedPairs.Add(pair);
                }
                else if (actor1.IsSameRow(actor2.location))
                {
                    var pair = new ActorPair(actor1, actor2, Axis.Horizontal);
                    combatParticipants.alignedPairs.Add(pair);
                }
            }
        }

        return combatParticipants.alignedPairs.Count > 0;
    }

    private void AssignAttackingPairs()
    {
        foreach (var pair in combatParticipants.alignedPairs)
        {
            if (pair.alignment.hasEnemiesBetween &&
                !pair.alignment.hasPlayersBetween &&
                !pair.alignment.hasGapsBetween &&
                !combatParticipants.HasAttackingPair(pair))
            {
                // Add the pair to attacking pairs
                combatParticipants.attackingPairs.Add(pair);

                // Mark the actors as attacking
                pair.actor1.SetAttacking();
                pair.actor2.SetAttacking();

                // Assign all enemies in alignment as defending
                pair.alignment.enemies.ForEach(enemy => enemy.SetDefending());
            }
        }

        // Reorder attacking pairs after assignment
        combatParticipants.attackingPairs = GetOrderedAttackingPairs().ToHashSet();
    }


    private List<ActorPair> GetOrderedAttackingPairs()
    {
        return combatParticipants.attackingPairs
        .OrderBy(pair => pair.axis == Axis.Horizontal ? 1 : 0)              // Prioritize vertical alignments (0) over horizontal (1)
        .ThenBy(pair => pair.axis == Axis.Vertical
            ? Mathf.Min(pair.actor1.location.y, pair.actor2.location.y)     // Top-to-bottom for vertical
            : Mathf.Min(pair.actor1.location.x, pair.actor2.location.x))    // Left-to-right for horizontal
        .ToList();
    }


    private void AssignSupportingPairs()
    {
        foreach (var pair in combatParticipants.alignedPairs)
        {
            if (!pair.alignment.hasEnemiesBetween &&
                !pair.alignment.hasPlayersBetween &&
                (pair.actor1.flags.IsAttacking || pair.actor2.flags.IsAttacking) &&
                !combatParticipants.HasSupportingPair(pair))
            {
                // Add the pair to supporting pairs
                combatParticipants.supportingPairs.Add(pair);

                // Mark the actors as supporting
                pair.actor1.SetSupporting();
                pair.actor2.SetSupporting();
            }
        }
    }

    private bool AssignCombatParticipants()
    {
        if (combatParticipants.alignedPairs.Count == 0)
            return false;

        // Assign attacking pairs and set flags
        AssignAttackingPairs();

        if (combatParticipants.attackingPairs.Count == 0)
            return false;

        // Assign supporting pairs based on updated flags
        AssignSupportingPairs();

        return true;
    }


    public void CheckPlayerAttack()
    {
        combatParticipants.Clear();

        bool hasAlignedPlayers = AssignAlignedPlayers();
        if (!hasAlignedPlayers)
        {
            NextTurn();
            return;
        }

        bool hasCombatParticipants = AssignCombatParticipants();
        if (!hasCombatParticipants)
        {
            NextTurn();
            return;
        }

        IEnumerator ExecuteCombat()
        {
            // Sort all actors to default
            var playingActors = actors.Where(x => x.IsActive && x.IsAlive).ToList();
            playingActors.ForEach(x => x.sortingOrder = SortingOrder.Default);

            // Sort all combat participants to above board overlay
            var allParticipants = combatParticipants.SelectAll();
            allParticipants.ForEach(x => x.sortingOrder = SortingOrder.BoardOverlay);

            boardOverlay.FadeIn();

            // Spawn support lines
            foreach (var pair in combatParticipants.supportingPairs)
            {
                supportLineManager.Spawn(pair);
            }

            // Iterate through player attacks
            foreach (var pair in combatParticipants.attackingPairs)
            {
                attackLineManager.Spawn(pair);
                yield return PlayerAttack(pair);

                // Reset participants after each attack
                ResetRolesAfterAttack(new[] { pair.actor1, pair.actor2 }); // Attackers
                ResetRolesAfterAttack(pair.alignment.enemies);             // Defenders
            }

            boardOverlay.FadeOut();
            NextTurn();
            ClearCombatState(); // Reset all roles and counts
        }

        StartCoroutine(ExecuteCombat());
    }






    private void ResetRolesAfterAttack(IEnumerable<ActorInstance> actors)
    {
        var playingActors = actors.Where(x => x.IsActive && x.IsAlive).ToList();
        foreach (var actor in playingActors)
        {
            if (actor.attackingPairCount > 0)
            {
                actor.attackingPairCount--;
                if (actor.attackingPairCount == 0)
                {
                    actor.SetDefault(); // Reset attackers
                }
            }

            if (actor.supportingPairCount > 0)
            {
                actor.supportingPairCount--;
                if (actor.supportingPairCount == 0)
                {
                    actor.SetDefault(); // Reset supporters
                }
            }

            // Always reset defenders
            if (actor.flags.IsDefending)
            {
                actor.SetDefault();
            }
        }
    }




    private void ClearCombatState()
    {
        foreach (var actor in combatParticipants.SelectAll())
        {
            actor.attackingPairCount = 0;
            actor.supportingPairCount = 0;
            actor.SetDefault();
        }
    }




    private IEnumerator PlayerAttack(ActorPair pair)
    {
        #region Player portraits

        yield return Wait.For(Intermission.Before.Player.Attack);

        audioManager.Play("Portrait");
        var first = pair.axis == Axis.Vertical ? Direction.South : Direction.East;
        var second = pair.axis == Axis.Vertical ? Direction.North : Direction.West;
        var direction1 = pair.actor1 == pair.originActor ? first : second;
        var direction2 = pair.actor2 == pair.originActor ? first : second;
        portraitManager.SlideIn(pair.actor1, direction1);
        portraitManager.SlideIn(pair.actor2, direction2);

        yield return Wait.For(Intermission.Before.Portrait.SlideIn);

        #endregion

        #region Player attack

        //Precalculate attack results and determine dying enemies
        var attacks = pair.alignment.enemies
        .Select(enemy =>
        {
            var isHit = Formulas.IsHit(pair.actor1, enemy);
            var damage = isHit ? Formulas.CalculateDamage(pair.actor1, enemy) : 0;
            return new AttackResult
            {
                Opponent = enemy,
                IsHit = isHit,
                IsCriticalHit = false,
                Damage = damage
            };
        })
        .ToList();


        // Extract dying enemies
        var dyingEnemies = attacks
            .Where(result => result.IsFatal)
            .Select(result => result.Opponent)
            .ToList();

        ActorInstance lastDyingEnemy = null;

        if (dyingEnemies.Count > 1)
        {
            lastDyingEnemy = dyingEnemies.Last();
            dyingEnemies.Remove(lastDyingEnemy);
        }

        // Attack each enemy and handle deaths
        foreach (var attack in attacks)
        {
            yield return pair.actor1.Attack(attack);
            if (dyingEnemies.Contains(attack.Opponent))
                attack.Opponent.Die();
        }

        // _Despawn attack and support lines
        //foreach (var enemy in pair.alignment.enemies)
        //{
        //    attackLineManager._Despawn(pair);
        //    supportLineManager._Despawn(pair);
        //}
        attackLineManager.Despawn(pair);
        supportLineManager.Despawn(pair);

        // Trigger synchronous death for the last dying enemy

        if (lastDyingEnemy != null)
            yield return lastDyingEnemy._Die();

        yield return Wait.For(Intermission.After.Player.Attack);

        #endregion
    }

    #endregion

    #region Enemy Attack Methods


    private void CheckEnemySpawn()
    {
        //Check abort conditions
        if (!IsEnemyTurn || !IsStartPhase)
            return;

        var spawnableEnemies = enemies.Where(x => x.IsSpawnable).ToList();
        foreach (var enemy in spawnableEnemies)
        {
            enemy.Spawn(Random.UnoccupiedLocation);
        }
    }

    private void ExecuteEnemyMove()
    {
        StartCoroutine(EnemyMove());
    }

    private IEnumerator EnemyMove()
    {
        //Check abort conditions
        if (!IsEnemyTurn || !IsStartPhase)
            yield break;

        currentPhase = Phase.Move;

        var readyEnemies = enemies.Where(x => x.IsActive && x.IsAlive && x.HasMaxAP).ToList();
        if (readyEnemies.Count > 0)
        {
            yield return Wait.For(Intermission.Before.Enemy.Move);

            foreach (var enemy in readyEnemies)
            {
                enemy.CalculateAttackStrategy();
                yield return enemy.move.TowardDestination();
            }

            currentPhase = Phase.Attack;
            ExecuteEnemyAttack();
        }
        else
        {
            NextTurn();
        }


    }





    private void ExecuteEnemyAttack()
    {
        StartCoroutine(EnemyAttack());
    }

    private IEnumerator EnemyAttack()
    {
        //Check abort conditions
        if (!IsEnemyTurn || !IsAttackPhase)
            yield break;

        var readyEnemies = enemies.Where(x => x.IsActive && x.IsAlive && x.HasMaxAP).ToList();
        if (readyEnemies.Count > 0)
        {
            yield return Wait.For(Intermission.Before.Enemy.Attack);

            foreach (var enemy in readyEnemies)
            {
                var defendingPlayers = players.Where(x => x.IsActive && x.IsAlive && x.IsAdjacentTo(enemy.location)).ToList();
                foreach (var player in defendingPlayers)
                {
                    var direction = enemy.GetAdjacentDirectionTo(player);

                    IEnumerator _()
                    {
                        var isHit = Formulas.IsHit(enemy, player);
                        var isCriticalHit = false;
                        var damage = Formulas.CalculateDamage(enemy, player);
                        var attack = new AttackResult()
                        {
                            Opponent = player,
                            IsHit = isHit,
                            IsCriticalHit = isCriticalHit,
                            Damage = damage
                        };
                        yield return enemy.Attack(attack);
                    }

                    enemy.actionBar.Reset();
                    yield return enemy.action._Bump(direction, _());


                }

            }

            //TODO: Put player._Die here so that it resolves after attacks...
            var dyingPlayers = actors.Where(x => x.IsDying).ToList();
            foreach (var player in dyingPlayers)
            {
                yield return player._Die();
            }

            readyEnemies.ForEach(x => x.actionBar.Reset());
        }



        NextTurn();
    }

    #endregion

}
