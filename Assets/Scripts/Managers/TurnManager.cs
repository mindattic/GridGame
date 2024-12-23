using Assets.Scripts.Models;
using Assets.Scripts.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Phase = TurnPhase;

public class TurnManager : ExtendedMonoBehavior
{
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
            actorManager.CheckEnemyReadiness();
            timerBar.Reset();
        }
        else if (IsEnemyTurn)
        {
            timerBar.Hide();

            CheckEnemySpawn();
            CheckEnemyMove();
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
                    || !actor1.IsPlaying || !actor2.IsPlaying
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
            var playingActors = actors.Where(x => x.IsPlaying).ToList();
            playingActors.ForEach(x => x.sortingOrder = SortingOrder.Default);

            // Sort all combat participants to above board overlay
            var allParticipants = combatParticipants.SelectAll();
            allParticipants.ForEach(x => x.sortingOrder = SortingOrder.BoardOverlay);

            boardOverlay.Show();

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

            boardOverlay.Hide();
            NextTurn();
            ClearCombatState(); // Reset all roles and counts
        }

        StartCoroutine(ExecuteCombat());
    }


    private List<ActorPair> GetOrderedAttackingPairs()
    {
        return combatParticipants.attackingPairs
            .OrderBy(pair => pair.axis == Axis.Vertical
                ? Mathf.Min(pair.actor1.location.y, pair.actor2.location.y) // Top-to-bottom for vertical
                : Mathf.Min(pair.actor1.location.x, pair.actor2.location.x)) // Left-to-right for horizontal
            .ToList();
    }



    private void ResetRolesAfterAttack(IEnumerable<ActorInstance> actors)
    {
        var playingActors = actors.Where(x => x.IsPlaying).ToList();
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

        yield return Wait.For(Interval.QuarterSecond);

        audioManager.Play("Portrait");
        var first = pair.axis == Axis.Vertical ? Direction.South : Direction.East;
        var second = pair.axis == Axis.Vertical ? Direction.North : Direction.West;
        var direction1 = pair.actor1 == pair.originActor ? first : second;
        var direction2 = pair.actor2 == pair.originActor ? first : second;
        portraitManager.SlideIn(pair.actor1, direction1);
        portraitManager.SlideIn(pair.actor2, direction2);

        yield return Wait.For(Interval.TwoSeconds);

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

        // Attack each enemy and handle deaths
        foreach (var attack in attacks)
        {
            if (attack.IsHit)
            {
                yield return pair.actor1.Attack(attack);

                // Trigger DieAsync for all dying enemies except the last
                if (attack.IsFatal && dyingEnemies.Count > 1 && attack.Opponent != dyingEnemies.Last())
                {
                    attack.Opponent.DieAsync();
                }
            }
            else
            {
                yield return attack.Opponent.AttackMiss();
            }
        }

        // Despawn attack and support lines
        foreach (var enemy in pair.alignment.enemies)
        {
            attackLineManager.DespawnAsync(pair);
            supportLineManager.DespawnAsync(pair);
        }

        // Trigger synchronous death for the last dying enemy
        if (dyingEnemies.Count > 0)
        {
            yield return dyingEnemies.Last().Die();
            yield return Wait.For(Interval.HalfSecond);
        }

        #endregion
    }

    #endregion

    #region Enemy Attack Methods


    public void CheckEnemySpawn()
    {
        //Check abort state
        if (!IsEnemyTurn || !IsStartPhase)
            return;

        var spawnableEnemies = enemies.Where(x => x.IsSpawnable).ToList();
        foreach (var enemy in spawnableEnemies)
        {
            enemy.Spawn(Random.UnoccupiedLocation);
        }
    }



    public void CheckEnemyMove()
    {
        //Check abort state
        if (!IsEnemyTurn || !IsStartPhase)
            return;

        IEnumerator _()
        {
            currentPhase = Phase.Move;

            var readyEnemies = enemies.Where(x => x.IsPlaying && x.IsReady).ToList();
            if (readyEnemies.Count > 0)
            {
                yield return Wait.For(Interval.OneSecond);

                foreach (var enemy in readyEnemies)
                {
                    enemy.CalculateAttackStrategy();
                    yield return enemy.MoveTowardDestination();
                }

                currentPhase = Phase.Attack;
                CheckEnemyAttack();
            }
            else
            {
                NextTurn();
            }


        }

        StartCoroutine(_());
    }

    private void CheckEnemyAttack()
    {
        //Check abort state
        if (!IsEnemyTurn || !IsAttackPhase)
            return;

        IEnumerator _()
        {
            var readyEnemies = enemies.Where(x => x.IsPlaying && x.IsReady).ToList();
            if (readyEnemies.Count > 0)
            {
                yield return Wait.For(Interval.HalfSecond);

                foreach (var enemy in readyEnemies)
                {
                    var defendingPlayers = players.Where(x => x.IsPlaying && x.IsAdjacentTo(enemy.location)).ToList();
                    if (defendingPlayers.Count > 0)
                    {
                        foreach (var player in defendingPlayers)
                        {
                            var direction = enemy.GetAdjacentDirectionTo(player);

                            IEnumerator _()
                            {
                                var isHit = Formulas.IsHit(enemy, player);
                                if (isHit)
                                {
                                    var damage = Formulas.CalculateDamage(enemy, player);
                                    var attack = new AttackResult()
                                    {
                                        Opponent = player,
                                        IsHit = isHit,
                                        IsCriticalHit = false,
                                        Damage = damage
                                    };

                                    yield return enemy.Attack(attack);
                                }
                                else
                                {
                                    yield return player.AttackMiss();
                                }
                            }


                            yield return enemy.Bump(direction, _());


                        }
                    }
                }

                //TODO: Put player.Die here so that it resolves after attacks...
                var dyingPlayers = actors.Where(x => x.IsDying).ToList();
                foreach(var player in dyingPlayers)
                {
                    yield return player.Die();
                }

                readyEnemies.ForEach(x => x.ResetActionBar());
            }



            NextTurn();
        }


        StartCoroutine(_());
    }


    #endregion







    //public void CheckPlayerAttack()
    //{
    //    combatParticipants.Reset();

    //    bool hasAlignedPlayers = AssignAlignedPlayers();
    //    if (!hasAlignedPlayers)
    //    {
    //        NextTurn();
    //        return;
    //    }

    //    bool hasCombatParticipants = AssignCombatParticipants();
    //    if (!hasCombatParticipants)
    //    {
    //        NextTurn();
    //        return;
    //    }

    //    IEnumerator _()
    //    {

    //        //Sort all actors to default
    //        var playingActors = actors.Where(x => x.IsPlaying).ToList();
    //        playingActors.ForEach(x => x.sortingOrder = SortingOrder.Default);

    //        //Sort all combat participants to above board overlay
    //        var allParticipants = combatParticipants.SelectAll();
    //        allParticipants.ForEach(x => x.sortingOrder = SortingOrder.BoardOverlay);

    //        boardOverlay.Show();

    //        //Spawn support lines
    //        foreach (var pair in combatParticipants.supportingPairs)
    //        {
    //            supportLineManager.Spawn(pair);
    //        }

    //        //Spawn attack line
    //        //foreach (var pair in combatParticipants.attackingPairs)
    //        //{
    //        //    attackLineManager.Spawn(pair);
    //        //}

    //        //Iterate through player attacks
    //        foreach (var pair in combatParticipants.attackingPairs)
    //        {
    //            attackLineManager.Spawn(pair);
    //            yield return PlayerAttack(pair);
    //            var pairParticipants = combatParticipants.SelectAll(pair);
    //            pairParticipants.ForEach(x => x.SetDefault());
    //        }


    //        boardOverlay.Hide();

    //        NextTurn();
    //    }

    //    StartCoroutine(_());
    //}


    ///// <summary>
    ///// Method which is used to find actors that share first column or row
    ///// </summary>
    //private bool AssignAlignedPlayers()
    //{
    //    foreach (var actor1 in players)
    //    {
    //        foreach (var actor2 in players)
    //        {
    //            if (actor1 == null || actor2 == null || actor1.Equals(actor2)
    //                || !actor1.IsPlaying || !actor2.IsPlaying
    //                || combatParticipants.HasAlignedPair(actor1, actor2))
    //                continue;

    //            if (actor1.IsSameColumn(actor2.location))
    //            {
    //                var pair = new ActorPair(actor1, actor2, Axis.Vertical);
    //                combatParticipants.alignedPairs.Add(pair);
    //            }
    //            else if (actor1.IsSameRow(actor2.location))
    //            {
    //                var pair = new ActorPair(actor1, actor2, Axis.Horizontal);
    //                combatParticipants.alignedPairs.Add(pair);
    //            }
    //        }
    //    }


    //    if (combatParticipants.alignedPairs.Count < 1)
    //        return false;

    //    return true;
    //}

    ///// <summary>
    ///// Method which is used to find actors surrounding enemies without gaps between
    ///// </summary>
    //private bool AssignCombatParticipants()
    //{
    //    if (combatParticipants.alignedPairs.Count < 1)
    //        return false;

    //    //Assign attacking pairs
    //    foreach (var pair in combatParticipants.alignedPairs)
    //    {
    //        if (pair.alignment.hasEnemiesBetween
    //            && !pair.alignment.hasPlayersBetween
    //            && !pair.alignment.hasGapsBetween
    //            && !combatParticipants.HasAttackingPair(pair))
    //        {
    //            //pair.actor1.SetAttacking();
    //            //pair.actor2.SetAttacking();
    //            //pair.alignment.enemies.ForEach(x => x.SetDefending());
    //            combatParticipants.attackingPairs.Add(pair);
    //        }
    //    }

    //    if (combatParticipants.attackingPairs.Count < 1)
    //        return false;

    //    //Assign supporting pairs
    //    foreach (var pair in combatParticipants.alignedPairs)
    //    {
    //        if (!pair.alignment.hasEnemiesBetween
    //            && !pair.alignment.hasPlayersBetween
    //            && (pair.actor1.flags.IsAttacking || pair.actor2.flags.IsAttacking)
    //            && !combatParticipants.HasSupportingPair(pair))
    //        {
    //            //pair.actor1.SetSupporting();
    //            //pair.actor2.SetSupporting();
    //            combatParticipants.supportingPairs.Add(pair);
    //        }
    //    }

    //    return true;
    //}

}
