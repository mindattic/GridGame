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
            //audioManager.Play("NextTurn");
            timer.Reset();
        }
        else if (IsEnemyTurn)
        {


            CheckEnemySpawn();
            CheckEnemyMove();
        }


        titleManager.Print($"{(IsPlayerTurn ? "Player" : "Enemy")} Turn", IsPlayerTurn ? Colors.Solid.White : Colors.Solid.Red);
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

            //Spawn supporting lines
            foreach (var pair in combatParticipants.supportingPairs)
            {
                pair.actor1.sortingOrder = SortingOrder.Supporter;
                pair.actor2.sortingOrder = SortingOrder.Supporter;
                supportLineManager.Spawn(pair);
            }

            int i = 1;
            foreach (var pair in combatParticipants.attackingPairs)
            {
                pair.actor1.sortingOrder = SortingOrder.Attacker;
                pair.actor2.sortingOrder = SortingOrder.Attacker;
                tooltipManager.Spawn($"Attack {i++}", pair.actor1.currentTile.position);
                tooltipManager.Spawn($"Attack {i++}", pair.actor1.currentTile.position);
                pair.alignment.enemies.ForEach(x => x.sortingOrder = SortingOrder.Defender);
                attackLineManager.Spawn(pair);
            }

            //Iterate through player attacks
            foreach (var pair in combatParticipants.attackingPairs)
            {
                yield return PlayerAttack(pair);
            }

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

        #region Parallax Fade In

        pair.actor1.ParallaxFadeInAsync();
        pair.actor2.ParallaxFadeInAsync();
        pair.alignment.enemies.ForEach(x => x.ParallaxFadeInAsync());

        #endregion

        #region Player portraits 

        yield return Wait.For(Interval.QuarterSecond);

        audioManager.Play("Portrait");
        var first = pair.axis == Axis.Vertical ? Direction.South : Direction.East;
        var second = pair.axis == Axis.Vertical ? Direction.North : Direction.West;
        var direction1 = pair.actor1 == pair.highestActor ? first : second;
        var direction2 = pair.actor2 == pair.highestActor ? first : second;
        portraitManager.SlideIn(pair.actor1, direction1);
        portraitManager.SlideIn(pair.actor2, direction2);

        yield return Wait.For(Interval.TwoSeconds);


        #endregion

        #region Player attack

        //yield return ap.For(Interval.QuarterSecond);

        foreach (var enemy in pair.alignment.enemies)
        {

            //var actor1 = pair.actor1;
            //var actor2 = pair.actor2;
            //var totalEnemies = pair.enemies.Count - 1;
            //var groupedEnemyModifier = Mathf.Max(1.0f - (0.1f * totalEnemies), 0.1f);



            //var attack1 = (actor1.attack + (actor1.attack * actor1.LevelModifier)) * Math.Pow(actor1.attack, actor1.LuckModifier);
            //var attack2 = (actor2.attack + (actor2.attack * actor2.LevelModifier)) * Math.Pow(actor2.attack, actor2.LuckModifier);

            //var defense = enemy.defense * groupedEnemyModifier * Math.Pow(enemy.defense, enemy.LuckModifier);

            //var amount = (float)((attack1 + attack2) / defense);

            //Debug.Log($"Attack1: ({attack1}) + Attack2: ({attack2}) / Enemy defense: ({defense}) = Damage: ({amount})");





            //Calculate hit chance

            var accuracy = 101f; //baseAccuracy + Random.Float(0, pair.actor1.accuracy + pair.actor2.accuracy) - Random.Float(0, enemy.evasion);
            //var accuracy = Mathf.Round(pair.actor1.accuracy + pair.actor2.accuracy) / 3 + Random.Float(0, Mathf.Round(pair.actor1.accuracy + pair.actor2.accuracy) / 2);
            var hit = accuracy > 100f;
            if (hit)
            {
                //attack enemy (one at a time)
                //TODO: Calculate based on attacker stats
                var amount = Random.Int(15, 33);
                pair.actor1.ChangeSpAsync(amount);
                pair.actor2.ChangeSpAsync(amount);

                yield return enemy.ChangeHp(-amount);
            }
            else
            {
                yield return enemy.MissAttack();
            }
        }


        foreach (var enemy in pair.alignment.enemies)
        {
            attackLineManager.DespawnAsync(pair);
            supportLineManager.DespawnAsync(pair);
        }

        var deadEnemies = pair.alignment.enemies.Where(x => x.IsDying).ToList();

        //Die dead enemies (one at a time)
        foreach (var enemy in deadEnemies)
        {
            yield return enemy.Die();
        }

        pair.actor1.ShrinkAsync();
        pair.actor2.ShrinkAsync();

        #endregion

        #region Parallax Fade Out

        pair.actor1.ParallaxFadeOutAsync();
        pair.actor2.ParallaxFadeOutAsync();
        pair.alignment.enemies.ForEach(x => x.ParallaxFadeOutAsync());

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
            var notReadyEnemies = enemies.Where(x => x.IsPlaying && !x.IsReady).ToList();
            foreach (var enemy in notReadyEnemies)
            {
                //TODO: Calculate based on attacker stats
                int amount = Convert.ToInt32(enemy.speed * 3 * enemy.LuckModifier);
                //int amount = Random.Int(15, 33);
                enemy.ChangeApAsync(amount);
            }

            currentPhase = TurnPhase.Move;
            yield return Wait.For(Interval.OneSecond);

            var readyEnemies = enemies.Where(x => x.IsPlaying && x.IsReady).ToList();
            foreach (var enemy in readyEnemies)
            {
                //enemy.sortingOrder = SortingOrder.Max;
                enemy.SetAttackStrategy();

                yield return enemy.MoveTowardDestination();
            }

            currentPhase = TurnPhase.Attack;
            CheckEnemyAttack();
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
            yield return Wait.For(Interval.OneSecond);

            var readyEnemies = enemies.Where(x => x.IsPlaying && x.IsReady).ToList();
            if (readyEnemies.Count > 0)
            {
                foreach (var enemy in readyEnemies)
                {
                    var defendingPlayers = players.Where(x => x.IsPlaying && x.IsAdjacentTo(enemy.location)).ToList();
                    if (defendingPlayers.Count > 0)
                    {
                        foreach (var player in defendingPlayers)
                        {
                            //yield return ap.For(Interval.HalfSecond);

                            var direction = enemy.GetAdjacentDirectionTo(player);
                            yield return enemy.Bump(direction);

                            //TODO: Calculate based on attacker accuracy vs defender evasion
                            var accuracy = enemy.accuracy + Random.Float(0, enemy.accuracy);
                            var hit = accuracy > player.evasion;
                            if (hit)
                            {
                                //attack enemy (one at a time)
                                //TODO: Calculate based on attacker stats
                                var amount = Random.Int(15, 33);
                                yield return player.ChangeHp(-amount);
                            }
                            else
                            {
                                yield return enemy.MissAttack();
                            }
                        }
                    }
                }


                foreach (var enemy in readyEnemies)
                {
                    //enemy.AssignActionWait();
                    enemy.ap = 0;
                    enemy.UpdateActionBar();
                }
            }





            var deadPlayers = actors.Where(x => x.IsDying).ToList();
            if (deadPlayers != null && deadPlayers.Count > 0)
            {
                //Die dead enemies (one at a time)
                foreach (var player in deadPlayers)
                {
                    yield return player.Die();
                }

                //Fade out (all at once)
                //foreach (var player in deadPlayers)
                //{
                //    player.Destroy();
                //}
            }





            NextTurn();
        }


        StartCoroutine(_());
    }


    #endregion







}
