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

            //audioManager.Play("NextTurn");
            timerBar.Reset();
        }
        else if (IsEnemyTurn)
        {
            timerBar.Hide();

            CheckEnemySpawn();
            CheckEnemyMove();
        }

        //titleManager.Print($"{(IsPlayerTurn ? "Player" : "Enemy")} Turn", IsPlayerTurn ? Colors.Solid.White : Colors.Solid.Red);
    }


    #region Player Attack Methods

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

        IEnumerator _()
        {

            //Sort all actors to default
            actors.ForEach(x => x.sortingOrder = SortingOrder.Default);

                 
            var allParticipants = combatParticipants.SelectAll();
            allParticipants.ForEach(x => x.sortingOrder = SortingOrder.BoardOverlay);

            boardOverlay.Show();

            //SpawnAsync supporting lines
            foreach (var pair in combatParticipants.supportingPairs)
            {
                pair.actor1.sortingOrder = SortingOrder.Supporter;
                pair.actor2.sortingOrder = SortingOrder.Supporter;
                supportLineManager.Spawn(pair);
            }

            foreach (var pair in combatParticipants.attackingPairs)
            {
                pair.actor1.sortingOrder = SortingOrder.Attacker;
                pair.actor2.sortingOrder = SortingOrder.Attacker;
                //tooltipManager.Spawn($"Strength {i++}", pair.actor1.currentTile.boardPosition);
                //tooltipManager.Spawn($"Strength {i++}", pair.actor1.currentTile.boardPosition);
                pair.alignment.enemies.ForEach(x => x.sortingOrder = SortingOrder.Defender);
                attackLineManager.Spawn(pair);
            }

            //Iterate through player attacks
            foreach (var pair in combatParticipants.attackingPairs)
            {
                yield return PlayerAttack(pair);
                var pairParticipants = combatParticipants.SelectAll(pair);
                pairParticipants.ForEach(x => x.sortingOrder = SortingOrder.Default);
            }


            boardOverlay.Hide();

            NextTurn();
        }

        StartCoroutine(_());
    }


    /// <summary>
    /// Method which is used to find actors that share first column or row
    /// </summary>
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

       
        if (combatParticipants.alignedPairs.Count < 1)
            return false;

        return true;
    }

    /// <summary>
    /// Method which is used to find actors surrounding enemies without gaps between
    /// </summary>
    private bool AssignCombatParticipants()
    {
        if (combatParticipants.alignedPairs.Count < 1)
            return false;

        //Assign attacking pairs
        foreach (var pair in combatParticipants.alignedPairs)
        {
            if (pair.alignment.hasEnemiesBetween
                && !pair.alignment.hasPlayersBetween
                && !pair.alignment.hasGapsBetween
                && !combatParticipants.HasAttackingPair(pair))
            {
                combatParticipants.attackingPairs.Add(pair);
            }
        }

        if (combatParticipants.attackingPairs.Count < 1)
            return false;

        //Assign supporting pairs
        foreach (var pair in combatParticipants.alignedPairs)
        {
            if (!pair.alignment.hasEnemiesBetween
                && !pair.alignment.hasPlayersBetween
                && (pair.actor1.IsAttacking || pair.actor2.IsAttacking)
                && !combatParticipants.HasSupportingPair(pair))
            {
                combatParticipants.supportingPairs.Add(pair);
            }
        }

        return true;
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

        //Attack each enemy between aligned players
        foreach (var enemy in pair.alignment.enemies)
        {
            var isHit = Formulas.IsHit(pair.actor1, enemy);
            if (isHit)
            {
                //pair.actor1.AddSpAsync(10);
                //pair.actor2.AddSpAsync(10);

                //TODO: Combine actor1 + actor2 + support actors stats somehow...
                //TODO: Generate adhoc ActorStats where you take highest or median stats between both actors in ActorPair???
                var damage = Formulas.CalculateDamage(pair.actor1, enemy);
                var isCriticalHit = false;
                yield return pair.actor1.Attack(enemy, damage, isCriticalHit);
            }
            else
            {
                yield return enemy.AttackMiss();
            }
        }

        //Despawn attack and support lines
        foreach (var enemy in pair.alignment.enemies)
        {
            attackLineManager.DespawnAsync(pair);
            supportLineManager.DespawnAsync(pair);
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

                            //TODO: Add triggeredEvent at moment bump reaches zenith
                            yield return enemy.Bump(direction);

                            var isHit = Formulas.IsHit(enemy, player);
                            if (isHit)
                            {
                                var damage = Formulas.CalculateDamage(enemy, player);
                                var isCriticalHit = false;
                                yield return enemy.Attack(player, damage, isCriticalHit);
                            }
                            else
                            {
                                yield return player.AttackMiss();
                            }
                        }
                    }
                }

                foreach (var enemy in readyEnemies)
                {
                    //enemy.AssignActionWait();
                    //enemy.ap = 0;
                    //enemy.UpdateActionBar();
                   
                    enemy.ResetActionBar();
                }

                //var deadPlayers = actors.Where(x => x.IsDying).ToList();
                //if (deadPlayers != null && deadPlayers.Count > 0)
                //{
                //    //Die dead enemies (one at a time)
                //    foreach (var player in deadPlayers)
                //    {
                //        yield return player.Die();
                //    }

                //    //Fade out (all at once)
                //    //foreach (var player in deadPlayers)
                //    //{
                //    //    player.Destroy();
                //    //}
                //}



            }

            

            NextTurn();
        }


        StartCoroutine(_());
    }


    #endregion







}
