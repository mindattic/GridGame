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
                if (actor1.Equals(actor2) || actor1 == null || actor2 == null
                    || !actor1.IsAlive || !actor1.IsActive || !actor2.IsAlive || !actor2.IsActive
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
            if (pair.axis == Axis.Vertical)
            {
                pair.highest = pair.actor1.location.y > pair.actor2.location.y ? pair.actor1 : pair.actor2;
                pair.lowest = pair.highest == pair.actor1 ? pair.actor2 : pair.actor1;
                pair.enemies = enemies.Where(x => x != null && x.IsAlive && x.IsActive && x.IsSameColumn(pair.actor1.location) && Common.IsBetween(x.location.y, pair.floor, pair.ceiling)).OrderBy(x => x.location.y).ToList();
                pair.players = players.Where(x => x != null && x.IsAlive && x.IsActive && x.IsSameColumn(pair.actor1.location) && Common.IsBetween(x.location.y, pair.floor, pair.ceiling)).OrderBy(x => x.location.y).ToList();
                pair.gaps = tiles.Where(x => !x.IsOccupied && pair.actor1.IsSameColumn(x.location) && Common.IsBetween(x.location.y, pair.floor, pair.ceiling)).OrderBy(x => x.location.y).ToList();
            }
            else if (pair.axis == Axis.Horizontal)
            {
                pair.highest = pair.actor1.location.x > pair.actor2.location.x ? pair.actor1 : pair.actor2;
                pair.lowest = pair.highest == pair.actor1 ? pair.actor2 : pair.actor1;
                pair.enemies = enemies.Where(x => x != null && x.IsAlive && x.IsActive && x.IsSameRow(pair.actor1.location) && Common.IsBetween(x.location.x, pair.floor, pair.ceiling)).OrderBy(x => x.location.x).ToList();
                pair.players = players.Where(x => x != null && x.IsAlive && x.IsActive && x.IsSameRow(pair.actor1.location) && Common.IsBetween(x.location.x, pair.floor, pair.ceiling)).OrderBy(x => x.location.x).ToList();
                pair.gaps = tiles.Where(x => !x.IsOccupied && pair.actor1.IsSameRow(x.location) && Common.IsBetween(x.location.x, pair.floor, pair.ceiling)).OrderBy(x => x.location.x).ToList();
            }


            var hasEnemiesBetween = pair.enemies.Any();
            var hasPlayersBetween = pair.players.Any();
            //var hasDuplicate = attackParticipants.attackingPairs.Count(x => (x.actor1 == pair.actor1 && x.actor2 == pair.actor2) || (x.actor1 == pair.actor2 && x.actor2 == pair.actor1)) > 0;
            var hasGapsBetween = pair.gaps.Any();

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
        if (attackParticipants.supportingPairs.Count < 1)
            return;

        foreach (var supportingPair in attackParticipants.supportingPairs)
        {
            supportLineManager.Add(supportingPair);
        }
    }


    private IEnumerator PlayerAttack()
    {
        foreach (var pair in attackParticipants.attackingPairs)
        {
            yield return PlayerStartAttack(pair);
            yield return EnemyStartDefend(pair);
        }

        foreach (var pair in attackParticipants.attackingPairs)
        {
            yield return PlayerPortrait(pair);
            yield return PlayerAttack(pair);

            yield return PlayerEndAttack(pair);
            yield return EnemyEndDefend(pair);
        }

        ClearAttack();
        turnManager.NextTurn();
    }


    private IEnumerator PlayerStartAttack(ActorPair pair)
    {
        pair.actor1.sortingOrder = ZAxis.Max;
        pair.actor2.sortingOrder = ZAxis.Max;

        soundSource.PlayOneShot(resourceManager.SoundEffect("PlayerGlow"));

        yield return Wait.Continue();
    }


    private IEnumerator PlayerPortrait(ActorPair pair)
    {
        yield return Wait.For(Interval.QuarterSecond);

        soundSource.PlayOneShot(resourceManager.SoundEffect("Portrait"));
        var first = pair.axis == Axis.Vertical ? Direction.South : Direction.East;
        var second = pair.axis == Axis.Vertical ? Direction.North : Direction.West;
        var direction1 = pair.actor1 == pair.highest ? first : second;
        var direction2 = pair.actor2 == pair.highest ? first : second;
        portraitManager.SlideIn(pair.actor1, direction1);
        portraitManager.SlideIn(pair.actor2, direction2);

        yield return Wait.For(Interval.TwoSecond);
    }

    private IEnumerator EnemyStartDefend(ActorPair pair)
    {
        foreach (var enemy in pair.enemies)
        {
            //enemy.StartGlow();
        }

        yield return Wait.Continue();
    }


    private IEnumerator PlayerAttack(ActorPair pair)
    {
        //yield return Wait.For(Interval.QuarterSecond);

        foreach (var enemy in pair.enemies)
        {

            //var actor1 = pair.actor1;
            //var actor2 = pair.actor2;
            //var totalEnemies = pair.enemies.Count - 1;
            //var groupedEnemyModifier = Mathf.Max(1.0f - (0.1f * totalEnemies), 0.1f);



            //var attack1 = (actor1.Attack + (actor1.Attack * actor1.LevelModifier)) * Math.Pow(actor1.Attack, actor1.LuckModifier);
            //var attack2 = (actor2.Attack + (actor2.Attack * actor2.LevelModifier)) * Math.Pow(actor2.Attack, actor2.LuckModifier);

            //var defense = enemy.Defense * groupedEnemyModifier * Math.Pow(enemy.Defense, enemy.LuckModifier);

            //var damage = (float)((attack1 + attack2) / defense);

            //Debug.Log($"Attack1: ({attack1}) + Attack2: ({attack2}) / Enemy Defense: ({defense}) = Damage: ({damage})");





            //Calculate hit chance

            var accuracy = 101f; //baseAccuracy + Random.Float(0, pair.actor1.Accuracy + pair.actor2.Accuracy) - Random.Float(0, enemy.Evasion);
            //var accuracy = Mathf.Round(pair.actor1.Accuracy + pair.actor2.Accuracy) / 3 + Random.Float(0, Mathf.Round(pair.actor1.Accuracy + pair.actor2.Accuracy) / 2);
            var hit = accuracy > 100f;
            if (hit)
            {
                //Attack enemy (one at a time)
                //TODO: Calculate based on attacker stats
                var damage = Random.Int(15, 33);
                yield return enemy.TakeDamage(damage);
            }
            else
            {
                yield return enemy.MissAttack();
            }





        }


        //Dissolve dead enemies (one at a time)
        foreach (var enemy in pair.enemies.Where(x => x.IsDead))
        {
            yield return enemy.Dissolve();
        }

        //Fade out (all at once)
        foreach (var enemy in pair.enemies.Where(x => x.IsDead))
        {
            enemy.Destroy();
        }
        yield return Wait.Ticks(100);

    }

    private IEnumerator PlayerEndAttack(ActorPair pair)
    {
        pair.actor1.sortingOrder = ZAxis.Min;
        pair.actor2.sortingOrder = ZAxis.Min;

        yield return Wait.Continue();
    }

    private IEnumerator EnemyEndDefend(ActorPair pair)
    {
        //Fade out Glow (all at once)
        foreach (var enemy in pair.enemies.Where(x => x.IsAlive))
        {
            //enemy.StopGlow();
        }

        yield return Wait.Continue();
    }



    #endregion

    #region Enemy Attack Methods



    public void CheckEnemySpawn()
    {
        //Check abort state
        if (!turnManager.IsEnemyTurn || !turnManager.IsStartPhase)
            return;

        var spawnableEnemies = enemies.Where(x => x.IsSpawnable).ToList();
        spawnableEnemies.ForEach(x => x.Spawn(unoccupiedTile.location));
    }

    public void CheckEnemyMove()
    {
        //Check abort state
        if (!turnManager.IsEnemyTurn || !turnManager.IsStartPhase)
            return;

        //Give boost to radial fill
        //var waitingEnemies = enemies.Where(x => !x.IsReady).ToList();
        //waitingEnemies.ForEach(x => x.CheckActionBar(x.Speed));

        StartCoroutine(StartEnemyMove());
    }

    private IEnumerator StartEnemyMove()
    {
        yield return Wait.For(Interval.HalfSecond);

        turnManager.currentPhase = TurnPhase.Move;

        var readyEnemies = enemies.Where(x => x.IsAlive && x.IsActive && x.IsReady).ToList();
        foreach (var enemy in readyEnemies)
        {
            enemy.SetDestination();
            while (enemy.HasDestination)
            {
                yield return Wait.Tick();
            }

            yield return Wait.For(Interval.QuarterSecond);
        }

        turnManager.currentPhase = TurnPhase.Attack;
        CheckEnemyAttack();

    }


    private void CheckEnemyAttack()
    {
        StartCoroutine(EnemyAttack());
    }

    private IEnumerator EnemyAttack()
    {
        yield return Wait.For(Interval.OneSecond);

        var readyEnemies = enemies.Where(x => x != null && x.IsAlive && x.IsActive && x.IsReady).ToList();
        if (readyEnemies.Count > 0)
        {
            foreach (var enemy in readyEnemies)
            {
                var defendingPlayers = players.Where(x => x != null && x.IsAlive && x.IsActive && x.IsAdjacentTo(enemy.location)).ToList();
                if (defendingPlayers.Count > 0)
                {
                    foreach (var player in defendingPlayers)
                    {
                        yield return Wait.For(Interval.HalfSecond);


                        //TODO: Calculate based on attacker accuracy vs defender evasion
                        var accuracy = enemy.Accuracy + Random.Float(0, enemy.Accuracy);
                        var hit = accuracy > player.Evasion;
                        if (hit)
                        {
                            //Attack enemy (one at a time)
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

        yield return Wait.For(Interval.HalfSecond);
        turnManager.NextTurn();
    }

    #endregion


    public void Clear()
    {
        GameObject.FindGameObjectsWithTag(Tag.Actor).ToList().ForEach(x => Destroy(x));
        actors.Clear();
    }

}
