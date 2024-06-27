using System.Collections;
using System.Linq;
using System.Numerics;
using UnityEngine;

public class ActorManager : ExtendedMonoBehavior
{
    void Awake() { }
    void Start() { }
    void Update() { }
    void FixedUpdate() { }






    #region Player Attack Methods



    public void CheckPlayerAttack()
    {
        ClearAttack();
        if (!CheckAlignedPlayers()) return;
        if (!CheckAttackingPlayers()) return;
        CheckSupportingPairs();


        StartCoroutine(PlayerAttack());
    }

    private void ClearAttack()
    {
        supportLineManager.Clear();
        attackLineManager.Clear();
        AttackParticipants.Clear();
    }


    /// <summary>
    /// Method which is used to find actors that share first column or row
    /// </summary>
    private bool CheckAlignedPlayers()
    {
        foreach (var actor1 in players)
        {
            foreach (var actor2 in players)
            {
                if (actor1.Equals(actor2) || actor1 == null || actor2 == null
                    || !actor1.IsAlive || !actor1.IsActive || !actor2.IsAlive || !actor2.IsActive
                    || AttackParticipants.HasAlignedPair(actor1, actor2))
                    continue;

                if (actor1.IsSameColumn(actor2.location))
                {
                    var highest = actor1.location.y > actor2.location.y ? actor1 : actor2;
                    var lowest = highest == actor1 ? actor2 : actor1;
                    AttackParticipants.alignedPairs.Add(new ActorPair(highest, lowest, Axis.Vertical));
                }
                else if (actor1.IsSameRow(actor2.location))
                {
                    var highest = actor1.location.x > actor2.location.x ? actor1 : actor2;
                    var lowest = highest == actor1 ? actor2 : actor1;
                    AttackParticipants.alignedPairs.Add(new ActorPair(highest, lowest, Axis.Horizontal));
                }

            }
        }

        if (AttackParticipants.alignedPairs.Count < 1)
        {
            turnManager.NextTurn();
            return false;
        }

        return true;
    }

    /// <summary>
    /// Method which is used to find actors surrounding enemies without Gaps between
    /// </summary>
    private bool CheckAttackingPlayers()
    {
        foreach (var pair in AttackParticipants.alignedPairs)
        {
            CheckAlignment(pair, out bool hasEnemiesBetween, out bool hasPlayersBetween, out bool hasGapsBetween);

            if (hasEnemiesBetween && !hasPlayersBetween && !hasGapsBetween && !AttackParticipants.HasAttackingPair(pair))
            {
                AttackParticipants.attackingPairs.Add(pair);
            }
            else if (!hasEnemiesBetween && !hasPlayersBetween && hasGapsBetween && !AttackParticipants.HasSupportingPair(pair))
            {
                AttackParticipants.supportingPairs.Add(pair);
            }
        }

        if (AttackParticipants.attackingPairs.Count < 1)
        {
            turnManager.NextTurn();
            return false;
        }

        return true;
    }



    private void CheckSupportingPairs()
    {
        //Check abort state
        if (AttackParticipants.supportingPairs.Count < 1)
            return;

        foreach (var supportingPair in AttackParticipants.supportingPairs)
        {
            supportLineManager.Spawn(supportingPair);
        }
    }


    private IEnumerator PlayerAttack()
    {
        foreach (var pair in AttackParticipants.attackingPairs)
        {
            yield return PlayerStartAttack(pair);
            attackLineManager.Spawn(pair);
            yield return EnemyStartDefend(pair);
        }

        foreach (var pair in AttackParticipants.attackingPairs)
        {
            yield return PlayerPortrait(pair);
            yield return PlayerAttack(pair);
            yield return EnemyEndDefend(pair);
            yield return PlayerEndAttack(pair);
        }

        ClearAttack();
        turnManager.NextTurn();
    }


    private IEnumerator PlayerStartAttack(ActorPair pair)
    {
        pair.Actor1.SortingOrder = ZAxis.Max;
        pair.Actor2.SortingOrder = ZAxis.Max;

        soundSource.PlayOneShot(resourceManager.SoundEffect("PlayerGlow"));

        yield return Wait.None();
    }


    private IEnumerator PlayerPortrait(ActorPair pair)
    {
        yield return Wait.For(Interval.QuarterSecond);

        soundSource.PlayOneShot(resourceManager.SoundEffect("Portrait"));
        var first = pair.Axis == Axis.Vertical ? Direction.South : Direction.East;
        var second = pair.Axis == Axis.Vertical ? Direction.North : Direction.West;
        var direction1 = pair.Actor1 == pair.HighestActor ? first : second;
        var direction2 = pair.Actor2 == pair.HighestActor ? first : second;
        portraitManager.SlideIn(pair.Actor1, direction1);
        portraitManager.SlideIn(pair.Actor2, direction2);

        yield return Wait.For(Interval.TwoSeconds);
    }

    private IEnumerator EnemyStartDefend(ActorPair pair)
    {
        foreach (var enemy in pair.Enemies)
        {
            //enemy.StartGlow();
        }

        yield return Wait.None();
    }


    private IEnumerator PlayerAttack(ActorPair pair)
    {
        //yield return wait.For(Interval.QuarterSecond);

        foreach (var enemy in pair.Enemies)
        {

            //var Actor1 = pair.Actor1;
            //var Actor2 = pair.Actor2;
            //var totalEnemies = pair.enemies.Count - 1;
            //var groupedEnemyModifier = Mathf.Max(1.0f - (0.1f * totalEnemies), 0.1f);



            //var attack1 = (Actor1.attack + (Actor1.attack * Actor1.LevelModifier)) * Math.Pow(Actor1.attack, Actor1.LuckModifier);
            //var attack2 = (Actor2.attack + (Actor2.attack * Actor2.LevelModifier)) * Math.Pow(Actor2.attack, Actor2.LuckModifier);

            //var defense = enemy.defense * groupedEnemyModifier * Math.Pow(enemy.defense, enemy.LuckModifier);

            //var damage = (float)((attack1 + attack2) / defense);

            //Debug.Log($"Attack1: ({attack1}) + Attack2: ({attack2}) / Enemy defense: ({defense}) = Damage: ({damage})");





            //Calculate hit chance

            var accuracy = 101f; //baseAccuracy + Random.Float(0, pair.Actor1.accuracy + pair.Actor2.accuracy) - Random.Float(0, enemy.evasion);
            //var accuracy = Mathf.Round(pair.Actor1.accuracy + pair.Actor2.accuracy) / 3 + Random.Float(0, Mathf.Round(pair.Actor1.accuracy + pair.Actor2.accuracy) / 2);
            var hit = accuracy > 100f;
            if (hit)
            {
                //attack enemy (one at a time)
                //TODO: Calculate based on attacker stats
                var damage = Random.Int(15, 33);
                yield return enemy.TakeDamage(damage);
            }
            else
            {
                yield return enemy.MissAttack();
            }

        }

        var deadEnemies = pair.Enemies.Where(x => x.IsDead);

        //Dissolve dead enemies (one at a time)
        foreach (var enemy in deadEnemies)
        {
            yield return enemy.Dissolve();
        }

        //Fade out (all at once)
        foreach (var enemy in deadEnemies)
        {
            enemy.Destroy();
        }

        //yield return wait.Ticks(100);
        yield return Wait.None();
    }

    private IEnumerator PlayerEndAttack(ActorPair pair)
    {
        pair.Actor1.SortingOrder = ZAxis.Min;
        pair.Actor2.SortingOrder = ZAxis.Min;

        attackLineManager.Destroy(pair);

        yield return Wait.None();
    }

    private IEnumerator EnemyEndDefend(ActorPair pair)
    {
        //Fade out glow (all at once)
        foreach (var enemy in pair.Enemies.Where(x => x.IsAlive))
        {
            //enemy.StopGlow();
        }

        yield return Wait.None();
    }



    #endregion

    #region Enemy Attack Methods



    public void CheckEnemySpawn()
    {
        //Check abort state
        if (!turnManager.IsEnemyTurn || !turnManager.IsStartPhase)
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
        if (!turnManager.IsEnemyTurn || !turnManager.IsStartPhase)
            return;

        //Give boost to radial fill
        //var waitingEnemies = enemies.Where(x => !x.IsReady).ToList();
        //waitingEnemies.ForEach(x => x.CheckActionBar(x.speed));


        StartCoroutine(EnemyMove());
    }

    private IEnumerator EnemyMove()
    {
        yield return Wait.For(Interval.HalfSecond);

        turnManager.currentPhase = TurnPhase.Move;

        var readyEnemies = enemies.Where(x => x.IsPlaying && x.IsReady).ToList();
        foreach (var enemy in readyEnemies)
        {
            enemy.SetAttackStrategy();
            while (enemy.IsMoving)
            {
                yield return Wait.OneTick();
            }

            enemy.targetPlayer = null;
            yield return Wait.For(Interval.QuarterSecond);
        }

        turnManager.currentPhase = TurnPhase.Attack;
        CheckEnemyAttack();

    }


    private void CheckEnemyAttack()
    {
        //Check abort state
        if (!turnManager.IsEnemyTurn || !turnManager.IsAttackPhase)
            return;


        StartCoroutine(EnemyAttack());
    }

    private IEnumerator EnemyAttack()
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
                        //yield return wait.For(Interval.HalfSecond);

                        var direction = enemy.AdjacentDirectionTo(player);
                        yield return enemy.Bump(direction);

                        //TODO: Calculate based on attacker accuracy vs defender evasion
                        var accuracy = enemy.accuracy + Random.Float(0, enemy.accuracy);
                        var hit = accuracy > player.evasion;
                        if (hit)
                        {
                            //attack enemy (one at a time)
                            //TODO: Calculate based on attacker stats
                            var damage = Random.Int(15, 33);
                            yield return player.TakeDamage(damage);
                        }
                        else
                        {
                            yield return enemy.MissAttack();
                        }



                        //enemy.StopGlow();
                        //player.StopGlow();
                        enemy.CalculateWait();
                    }
                }
                else
                {
                    //enemy.SetStatus(Status.None);
                    //yield return enemy.MissAttack();
                    enemy.CalculateWait();
                }
            }
        }

        //yield return wait.For(Interval.HalfSecond);
        turnManager.NextTurn();
    }

    #endregion


    public void Clear()
    {
        actors.ForEach(x => Destroy(x));
        actors.Clear();
    }

}
