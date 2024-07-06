using System.Collections;
using System.Linq;
using System.Numerics;
using UnityEngine;

public class ActorManager : ExtendedMonoBehavior
{
    #region Player Attack Methods

    public void CheckPlayerAttack()
    {
        //Clear attack variables
        supportLineManager.Clear();
        attackLineManager.Clear();
        attackParticipants.Clear();

        var hasAlignedPlayers = AssignAlignedPlayers();
        if (!hasAlignedPlayers)
        {
            turnManager.NextTurn();
            return;
        }

        var hasAttackParticipants = AssignAttackParticipants();
        if (!hasAttackParticipants)
        {
            turnManager.NextTurn();
            return;
        }

        IEnumerator _()
        {
            //Spawn attack lines
            foreach (var pair in attackParticipants.attackingPairs)
            {
                pair.sortingOrder = ZAxis.Max;
                attackLineManager.Spawn(pair);
            }

            //Spawn supporting lines
            foreach (var pair in attackParticipants.supportingPairs)
            {
                pair.sortingOrder = ZAxis.Max;
                supportLineManager.Spawn(pair);
            }

            //Iterate through player attacks
            foreach (var pair in attackParticipants.attackingPairs)
            {
                yield return PlayerAttack(pair);
            }

            turnManager.NextTurn();
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
                    || attackParticipants.HasAlignedPair(actor1, actor2))
                    continue;

                if (actor1.IsSameColumn(actor2.location))
                {
                    var pair = new ActorPair(actor1, actor2, Axis.Vertical);
                    pair.alignment = Common.AssignAlignment(pair);
                    attackParticipants.alignedPairs.Add(pair);
                }
                else if (actor1.IsSameRow(actor2.location))
                {
                    var pair = new ActorPair(actor1, actor2, Axis.Horizontal);
                    pair.alignment = Common.AssignAlignment(pair);
                    attackParticipants.alignedPairs.Add(pair);
                }
            }
        }

        if (attackParticipants.alignedPairs.Count < 1)
            return false;

        return true;
    }

    /// <summary>
    /// Method which is used to find actors surrounding enemies without gaps between
    /// </summary>
    private bool AssignAttackParticipants()
    {
        if (attackParticipants.alignedPairs.Count < 1)
            return false;

        //Assign attacking pairs
        foreach (var pair in attackParticipants.alignedPairs)
        {
            if (pair.alignment.hasEnemiesBetween && !pair.alignment.hasPlayersBetween && !pair.alignment.hasGapsBetween && !attackParticipants.HasAttackingPair(pair))
                attackParticipants.attackingPairs.Add(pair);
        }

        //If has no attacking pairs...
        if (attackParticipants.attackingPairs.Count < 1)
            return false;

        //...Otherwise, Assign supporting pairs
        foreach (var pair in attackParticipants.attackingPairs)
        {
            if (!pair.alignment.hasEnemiesBetween && !pair.alignment.hasPlayersBetween && !attackParticipants.HasSupportingPair(pair))
                attackParticipants.supportingPairs.Add(pair);
        }

        return true;
    }

    private IEnumerator PlayerAttack(ActorPair pair)
    {
        #region Player portraits 

        yield return Wait.For(Interval.QuarterSecond);

        soundSource.PlayOneShot(resourceManager.SoundEffect("Portrait"));
        var first = pair.axis == Axis.Vertical ? Direction.South : Direction.East;
        var second = pair.axis == Axis.Vertical ? Direction.North : Direction.West;
        var direction1 = pair.actor1 == pair.highestActor ? first : second;
        var direction2 = pair.actor2 == pair.highestActor ? first : second;
        portraitManager.SlideIn(pair.actor1, direction1);
        portraitManager.SlideIn(pair.actor2, direction2);

        yield return Wait.For(Interval.TwoSeconds);


        #endregion

        #region Player attack

        //yield return actionWait.For(Interval.QuarterSecond);

        foreach (var enemy in pair.alignment.enemies)
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
                pair.actor1.GainSkill(damage);
                pair.actor2.GainSkill(damage);
                yield return enemy.TakeDamage(damage);
            }
            else
            {
                yield return enemy.MissAttack();
            }

        }

        var deadEnemies = pair.alignment.enemies.Where(x => x.IsDead).ToList();

        //Dissolve dead enemies (one at a time)
        foreach (var enemy in deadEnemies)
        {
            yield return enemy.Dissolve();
        }

        //Fade out (all at once)
        deadEnemies.ForEach(x => x.Destroy());

        #endregion

        #region Cleanup

        attackLineManager.DespawnAsync(pair);
        supportLineManager.DespawnAsync(pair);

        #endregion
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

        IEnumerator _()
        {
            actors.ForEach(x => x.sortingOrder = ZAxis.Min);
            turnManager.currentPhase = TurnPhase.Move;

            yield return Wait.For(Interval.HalfSecond);
            var readyEnemies = enemies.Where(x => x.IsPlaying && x.IsReady).ToList();
            foreach (var enemy in readyEnemies)
            {
                enemy.sortingOrder = ZAxis.Max;
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
                            //yield return actionWait.For(Interval.HalfSecond);

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
                            enemy.CalculateActionWait();
                        }
                    }
                    else
                    {
                        //enemy.SetStatus(Status.None);
                        //yield return enemy.MissAttack();
                        enemy.CalculateActionWait();
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
