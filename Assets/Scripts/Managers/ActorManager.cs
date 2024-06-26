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
        SupportLineManager.Clear();
        AttackLineManager.Clear();
        AttackParticipants.Clear();
    }


    /// <summary>
    /// Method which is used to find Actors that share first column or row
    /// </summary>
    private bool CheckAlignedPlayers()
    {
        foreach (var actor1 in Players)
        {
            foreach (var actor2 in Players)
            {
                if (actor1.Equals(actor2) || actor1 == null || actor2 == null
                    || !actor1.IsAlive || !actor1.IsActive || !actor2.IsAlive || !actor2.IsActive
                    || AttackParticipants.HasAlignedPair(actor1, actor2))
                    continue;

                if (actor1.IsSameColumn(actor2.Location))
                {
                    var highest = actor1.Location.y > actor2.Location.y ? actor1 : actor2;
                    var lowest = highest == actor1 ? actor2 : actor1;
                    AttackParticipants.alignedPairs.Add(new ActorPair(highest, lowest, Axis.Vertical));
                }
                else if (actor1.IsSameRow(actor2.Location))
                {
                    var highest = actor1.Location.x > actor2.Location.x ? actor1 : actor2;
                    var lowest = highest == actor1 ? actor2 : actor1;
                    AttackParticipants.alignedPairs.Add(new ActorPair(highest, lowest, Axis.Horizontal));
                }

            }
        }

        if (AttackParticipants.alignedPairs.Count < 1)
        {
            TurnManager.NextTurn();
            return false;
        }

        return true;
    }

    /// <summary>
    /// Method which is used to find Actors surrounding Enemies without Gaps between
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
            TurnManager.NextTurn();
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
            SupportLineManager.Spawn(supportingPair);
        }
    }


    private IEnumerator PlayerAttack()
    {
        foreach (var pair in AttackParticipants.attackingPairs)
        {
            yield return PlayerStartAttack(pair);
            AttackLineManager.Spawn(pair);
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
        TurnManager.NextTurn();
    }


    private IEnumerator PlayerStartAttack(ActorPair pair)
    {
        pair.Actor1.SortingOrder = ZAxis.Max;
        pair.Actor2.SortingOrder = ZAxis.Max;

        SoundSource.PlayOneShot(ResourceManager.SoundEffect("PlayerGlow"));

        yield return Wait.None();
    }


    private IEnumerator PlayerPortrait(ActorPair pair)
    {
        yield return Wait.For(Interval.QuarterSecond);

        SoundSource.PlayOneShot(ResourceManager.SoundEffect("Portrait"));
        var first = pair.Axis == Axis.Vertical ? Direction.South : Direction.East;
        var second = pair.Axis == Axis.Vertical ? Direction.North : Direction.West;
        var direction1 = pair.Actor1 == pair.HighestActor ? first : second;
        var direction2 = pair.Actor2 == pair.HighestActor ? first : second;
        PortraitManager.SlideIn(pair.Actor1, direction1);
        PortraitManager.SlideIn(pair.Actor2, direction2);

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
        //yield return Wait.For(Interval.QuarterSecond);

        foreach (var enemy in pair.Enemies)
        {

            //var Actor1 = Pair.Actor1;
            //var Actor2 = Pair.Actor2;
            //var totalEnemies = Pair.Enemies.Count - 1;
            //var groupedEnemyModifier = Mathf.Max(1.0f - (0.1f * totalEnemies), 0.1f);



            //var attack1 = (Actor1.Attack + (Actor1.Attack * Actor1.LevelModifier)) * Math.Pow(Actor1.Attack, Actor1.LuckModifier);
            //var attack2 = (Actor2.Attack + (Actor2.Attack * Actor2.LevelModifier)) * Math.Pow(Actor2.Attack, Actor2.LuckModifier);

            //var defense = enemy.Defense * groupedEnemyModifier * Math.Pow(enemy.Defense, enemy.LuckModifier);

            //var damage = (float)((attack1 + attack2) / defense);

            //Debug.Log($"Attack1: ({attack1}) + Attack2: ({attack2}) / Enemy Defense: ({defense}) = Damage: ({damage})");





            //Calculate hit chance

            var accuracy = 101f; //baseAccuracy + Random.Float(0, Pair.Actor1.Accuracy + Pair.Actor2.Accuracy) - Random.Float(0, enemy.Evasion);
            //var accuracy = Mathf.Round(Pair.Actor1.Accuracy + Pair.Actor2.Accuracy) / 3 + Random.Float(0, Mathf.Round(Pair.Actor1.Accuracy + Pair.Actor2.Accuracy) / 2);
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

        var deadEnemies = pair.Enemies.Where(x => x.IsDead);

        //Dissolve dead Enemies (one at a time)
        foreach (var enemy in deadEnemies)
        {
            yield return enemy.Dissolve();
        }

        //Fade out (all at once)
        foreach (var enemy in deadEnemies)
        {
            enemy.Destroy();
        }

        //yield return Wait.Ticks(100);
        yield return Wait.None();
    }

    private IEnumerator PlayerEndAttack(ActorPair pair)
    {
        pair.Actor1.SortingOrder = ZAxis.Min;
        pair.Actor2.SortingOrder = ZAxis.Min;

        AttackLineManager.Destroy(pair);

        yield return Wait.None();
    }

    private IEnumerator EnemyEndDefend(ActorPair pair)
    {
        //Fade out Glow (all at once)
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
        if (!TurnManager.IsEnemyTurn || !TurnManager.IsStartPhase)
            return;

        var spawnableEnemies = Enemies.Where(x => x.IsSpawnable).ToList();
        foreach (var enemy in spawnableEnemies)
        {
            enemy.Spawn(UnoccupiedTile.Location);
        }
    }

    public void CheckEnemyMove()
    {
        //Check abort state
        if (!TurnManager.IsEnemyTurn || !TurnManager.IsStartPhase)
            return;

        //Give boost to radial fill
        //var waitingEnemies = Enemies.Where(x => !x.IsReady).ToList();
        //waitingEnemies.ForEach(x => x.CheckActionBar(x.Speed));


        StartCoroutine(EnemyMove());
    }

    private IEnumerator EnemyMove()
    {
        yield return Wait.For(Interval.HalfSecond);

        TurnManager.currentPhase = TurnPhase.Move;

        var readyEnemies = Enemies.Where(x => x.IsPlaying && x.IsReady).ToList();
        foreach (var enemy in readyEnemies)
        {
            enemy.SetAttackStrategy();
            while (enemy.IsMoving)
            {
                yield return Wait.OneTick();
            }

            enemy.TargetPlayer = null;
            yield return Wait.For(Interval.QuarterSecond);
        }

        TurnManager.currentPhase = TurnPhase.Attack;
        CheckEnemyAttack();

    }


    private void CheckEnemyAttack()
    {
        //Check abort state
        if (!TurnManager.IsEnemyTurn || !TurnManager.IsAttackPhase)
            return;


        StartCoroutine(EnemyAttack());
    }

    private IEnumerator EnemyAttack()
    {
        yield return Wait.For(Interval.OneSecond);

        var readyEnemies = Enemies.Where(x => x.IsPlaying && x.IsReady).ToList();
        if (readyEnemies.Count > 0)
        {
            foreach (var enemy in readyEnemies)
            {
                var defendingPlayers = Players.Where(x => x.IsPlaying && x.IsAdjacentTo(enemy.Location)).ToList();
                if (defendingPlayers.Count > 0)
                {
                    foreach (var player in defendingPlayers)
                    {
                        //yield return Wait.For(Interval.HalfSecond);

                        var direction = enemy.AdjacentDirectionTo(player);
                        yield return enemy.Bump(direction);

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

        //yield return Wait.For(Interval.HalfSecond);
        TurnManager.NextTurn();
    }

    #endregion


    public void Clear()
    {
        Actors.ForEach(x => Destroy(x));
        Actors.Clear();
    }

}
