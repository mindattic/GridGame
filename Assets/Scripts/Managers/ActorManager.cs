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
        attackParticipants.Clear();
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
                if (actor1 == null || actor2 == null || actor1.Equals(actor2)
                    || !actor1.IsPlaying || !actor2.IsPlaying
                    || attackParticipants.HasAlignedPair(actor1, actor2))
                    continue;

                if (actor1.IsSameColumn(actor2.location))
                {
                    var highest = actor1.location.y > actor2.location.y ? actor1 : actor2;
                    var lowest = highest == actor1 ? actor2 : actor1;
                    attackParticipants.alignedPairs.Add(new ActorPair(highest, lowest, Axis.Vertical));
                }
                else if (actor1.IsSameRow(actor2.location))
                {
                    var highest = actor1.location.x > actor2.location.x ? actor1 : actor2;
                    var lowest = highest == actor1 ? actor2 : actor1;
                    attackParticipants.alignedPairs.Add(new ActorPair(highest, lowest, Axis.Horizontal));
                }

            }
        }

        if (attackParticipants.alignedPairs.Count < 1)
        {
            turnManager.NextTurn();
            return false;
        }

        return true;
    }

    /// <summary>
    /// Method which is used to find actors surrounding enemies without gaps between
    /// </summary>
    private bool CheckAttackingPlayers()
    {
        foreach (var pair in attackParticipants.alignedPairs)
        {
            CheckAlignment(pair, out bool hasEnemiesBetween, out bool hasPlayersBetween, out bool hasGapsBetween);

            if (hasEnemiesBetween && !hasPlayersBetween && !hasGapsBetween && !attackParticipants.HasAttackingPair(pair))
            {
                attackParticipants.attackingPairs.Add(pair);
            }
            else if (!hasEnemiesBetween && !hasPlayersBetween && hasGapsBetween && !attackParticipants.HasSupportingPair(pair))
            {
                attackParticipants.supportingPairs.Add(pair);
            }
        }

        if (attackParticipants.attackingPairs.Count < 1)
        {
            turnManager.NextTurn();
            return false;
        }

        return true;
    }



    private void CheckSupportingPairs()
    {
        //Check abort state
        if (attackParticipants.supportingPairs.Count < 1)
            return;

        foreach (var supportingPair in attackParticipants.supportingPairs)
        {
            supportLineManager.Spawn(supportingPair);
        }
    }


    private IEnumerator PlayerAttack()
    {
        foreach (var pair in attackParticipants.attackingPairs)
        {
            yield return PlayerStartAttack(pair);
            attackLineManager.Spawn(pair);
            yield return EnemyStartDefend(pair);
        }

        foreach (var pair in attackParticipants.attackingPairs)
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
        pair.actor1.sortingOrder = ZAxis.Max;
        pair.actor2.sortingOrder = ZAxis.Max;

        soundSource.PlayOneShot(resourceManager.SoundEffect("PlayerGlow"));

        yield return Wait.None();
    }


    private IEnumerator PlayerPortrait(ActorPair pair)
    {
        yield return Wait.For(Interval.QuarterSecond);

        soundSource.PlayOneShot(resourceManager.SoundEffect("Portrait"));
        var first = pair.axis == Axis.Vertical ? Direction.South : Direction.East;
        var second = pair.axis == Axis.Vertical ? Direction.North : Direction.West;
        var direction1 = pair.actor1 == pair.highestActor ? first : second;
        var direction2 = pair.actor2 == pair.highestActor ? first : second;
        portraitManager.SlideIn(pair.actor1, direction1);
        portraitManager.SlideIn(pair.actor2, direction2);

        yield return Wait.For(Interval.TwoSeconds);
    }

    private IEnumerator EnemyStartDefend(ActorPair pair)
    {
        foreach (var enemy in pair.enemies)
        {
            //enemy.StartGlow();
        }

        yield return Wait.None();
    }


    private IEnumerator PlayerAttack(ActorPair pair)
    {
        //yield return wait.For(Interval.QuarterSecond);

        foreach (var enemy in pair.enemies)
        {

            //var actor1 = pair.actor1;
            //var actor2 = pair.actor2;
            //var totalEnemies = pair.enemies.Count - 1;
            //var groupedEnemyModifier = Mathf.Max(1.0f - (0.1f * totalEnemies), 0.1f);



            //var attack1 = (actor1.attack + (actor1.attack * actor1.LevelModifier)) * Math.Pow(actor1.attack, actor1.LuckModifier);
            //var attack2 = (actor2.attack + (actor2.attack * actor2.LevelModifier)) * Math.Pow(actor2.attack, actor2.LuckModifier);

            //var defense = enemy.defense * groupedEnemyModifier * Math.Pow(enemy.defense, enemy.LuckModifier);

            //var damage = (float)((attack1 + attack2) / defense);

            //Debug.Log($"Attack1: ({attack1}) + Attack2: ({attack2}) / Enemy defense: ({defense}) = Damage: ({damage})");





            //Calculate hit chance

            var accuracy = 101f; //baseAccuracy + Random.Float(0, pair.actor1.accuracy + pair.actor2.accuracy) - Random.Float(0, enemy.evasion);
            //var accuracy = Mathf.Round(pair.actor1.accuracy + pair.actor2.accuracy) / 3 + Random.Float(0, Mathf.Round(pair.actor1.accuracy + pair.actor2.accuracy) / 2);
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

        var deadEnemies = pair.enemies.Where(x => x.IsDead);

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

    }

    private IEnumerator PlayerEndAttack(ActorPair pair)
    {
        pair.actor1.sortingOrder = ZAxis.Min;
        pair.actor2.sortingOrder = ZAxis.Min;
        yield return Wait.None();
    }

    private IEnumerator EnemyEndDefend(ActorPair pair)
    {
        //Fade out glow (all at once)
        foreach (var enemy in pair.enemies.Where(x => x.IsAlive))
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

        IEnumerator _()
        {
            turnManager.currentPhase = TurnPhase.Move;

            yield return Wait.For(Interval.HalfSecond);

    
            actors.ForEach(x => x.sortingOrder = ZAxis.Min);

            var readyEnemies = enemies.Where(x => x.IsPlaying && x.IsReady).ToList();
            foreach (var enemy in readyEnemies)
            {
                enemy.SetAttackStrategy();
                enemy.sortingOrder = ZAxis.Max;
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


        StartCoroutine(_());
    }


    private void CheckEnemyAttack()
    {
        //Check abort state
        if (!turnManager.IsEnemyTurn || !turnManager.IsAttackPhase)
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

            var deadPlayers = actors.Where(x => x.IsDead);

            //Dissolve dead enemies (one at a time)
            foreach (var player in deadPlayers)
            {
                yield return player.Dissolve();
            }

            //Fade out (all at once)
            foreach (var player in deadPlayers)
            {
                player.Destroy();
            }

            //yield return wait.For(Interval.HalfSecond);
            turnManager.NextTurn();
        }


        StartCoroutine(_());
    }

   
    #endregion


    public void Clear()
    {
        actors.ForEach(x => Destroy(x));
        actors.Clear();
    }

}
