using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;

public class ActorManager : ExtendedMonoBehavior
{
    private void Awake()
    {

    }

    void Start()
    {

    }

    void Update()
    {

        CheckEnemy();
    }

 
    private void CheckEnemy()
    {
        if (!turnManager.IsEnemyTurn || !turnManager.IsStartPhase)
            return;

        foreach (var enemy in enemies)
        {
            if (enemy != null && !enemy.IsAlive || !enemy.IsActive) continue;

            if (enemy.turnDelay > 0)
            {
                enemy.turnDelay--;
                if (enemy.turnDelay == 0)
                {
                    enemy.SetDestination();
                }
            }
        }

        StartCoroutine(StartEnemyMove());
    }

    private IEnumerator StartEnemyMove()
    {
        turnManager.currentPhase = TurnPhase.Move;

        while (enemies.Any(x => x.HasDestination))
        {
            yield return Wait.For(Interval.HalfSecond);
        }

        turnManager.currentPhase = TurnPhase.Attack;
        CheckEnemyAttack();
    }


    public void CheckPlayerAttack()
    {
        ClearAttack();
        if (!HasAlignedPlayers()) return;
        if (!HasAttackingPlayers()) return;
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
    private bool HasAlignedPlayers()
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
    private bool HasAttackingPlayers()
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

            //Assign attacking pairs
            var hasEnemiesBetween = pair.enemies.Any();
            var hasPlayersBetween = pair.players.Any();
            var hasGapsBetween = pair.gaps.Any();
            if (!hasEnemiesBetween || hasPlayersBetween || hasGapsBetween)
                continue;

            attackParticipants.attackingPairs.Add(pair);
        }

        if (attackParticipants.attackingPairs.Count < 1)
        {
            turnManager.NextTurn();
            return false;
        }

        return true;
    }


    #region Player Attack Methods

    private IEnumerator PlayerAttack()
    {
        foreach (var pair in attackParticipants.attackingPairs)
        {
            yield return PlayerStartAttack(pair);
            yield return EnemyStartDefend(pair);
            yield return PlayerAttack(pair);
            yield return EnemyDefend(pair);
            yield return PlayerStopAttack(pair);
            yield return EnemyStopDefend(pair);
        }

        ClearAttack();
        turnManager.NextTurn();
    }

    private IEnumerator PlayerStartAttack(ActorPair pair)
    {
        yield return Wait.For(Interval.QuarterSecond);

        pair.actor1.SetActionIcon(ActionIcon.Attack);
        pair.actor1.StartGlow(Color.white);
        pair.actor1.sortingOrder = ZAxis.Max;
        soundSource.PlayOneShot(resourceManager.SoundEffect("PlayerGlow"));

        yield return Wait.For(Interval.QuarterSecond);
        pair.actor2.SetActionIcon(ActionIcon.Attack);
        pair.actor2.StartGlow(Color.white);
        pair.actor2.sortingOrder = ZAxis.Max;
        soundSource.PlayOneShot(resourceManager.SoundEffect("PlayerGlow"));

        yield return Wait.For(Interval.QuarterSecond);

        soundSource.PlayOneShot(resourceManager.SoundEffect("Portrait"));
        var first = pair.axis == Axis.Vertical ? Direction.South : Direction.East;
        var second = pair.axis == Axis.Vertical ? Direction.North : Direction.West;
        var direction1 = pair.actor1 == pair.highest ? first : second;
        var direction2 = pair.actor2 == pair.highest ? first : second;
        portraitManager.SlideIn(pair.actor1, direction1);
        portraitManager.SlideIn(pair.actor2, direction2);

        yield return Wait.For(Interval.ThreeSecond);
    }

    private IEnumerator EnemyStartDefend(ActorPair pair)
    {
        foreach (var enemy in pair.enemies)
        {
            enemy.StartGlow(Colors.Solid.Red);
        }

        yield return Wait.Continue();
    }


    private IEnumerator PlayerAttack(ActorPair pair)
    {
        yield return Wait.For(Interval.HalfSecond);

        foreach (var enemy in pair.enemies)
        {
            //TODO: Calculate based on attacker stats
            //var damage = Random.Int(15, 33);
            var damage = 100;

            //Attack enemy (one at a time)
            yield return enemy.TakeDamage(damage);
        }

        yield return Wait.For(Interval.HalfSecond);
    }

    private IEnumerator EnemyDefend(ActorPair pair)
    {
        //Dissolve dead enemies (one at a time)
        foreach (var enemy in pair.enemies.Where(x => x.IsDead))
        {
            yield return enemy.Dissolve();
        }

        //Fade out (all at once)
        foreach (var enemy in pair.enemies.Where(x => x.IsDead))
        {
            enemy.Die();
        }
        yield return Wait.Ticks(100);
    }

    private IEnumerator PlayerStopAttack(ActorPair pair)
    {
        pair.actor1.SetActionIcon(ActionIcon.None);
        pair.actor1.StopGlow();
        pair.actor1.sortingOrder = ZAxis.Min;

        pair.actor2.SetActionIcon(ActionIcon.None);
        pair.actor2.StopGlow();
        pair.actor2.sortingOrder = ZAxis.Min;

        yield return Wait.Continue();
    }

    private IEnumerator EnemyStopDefend(ActorPair pair)
    {
        //Fade out glow (all at once)
        foreach (var enemy in pair.enemies.Where(x => x.IsAlive))
        {
            enemy.StopGlow();
        }
        yield return Wait.Ticks(100);
    }



    #endregion

    #region Enemy Attack Methods

    private void CheckEnemyAttack()
    {
        StartCoroutine(EnemyAttack());
    }

    private IEnumerator EnemyAttack()
    {
        ClearAttack();

        foreach (var enemy in enemies)
        {
            if (enemy == null || !enemy.IsAlive || enemy.turnDelay > 0) continue;

            foreach (var player in players)
            {
                if (player == null
                    || !player.IsAlive || !player.IsActive
                    || (!player.IsSameColumn(enemy.location) && !player.IsSameRow(enemy.location))) continue;

                var delta = enemy.location - player.location;
                if (Math.Abs(delta.x) == 1 || Math.Abs(delta.y) == 1)
                {
                    enemy.SetActionIcon(ActionIcon.Attack);
                    var damage = Random.Int(15, 33); //TODO: Calculate based on attacker stats
                    player.TakeDamage(damage);

                    yield return Wait.For(Interval.TwoSecond);

                    enemy.SetEnemyTurnDelay(EnemyTurnDelay.Random);
                }

            }
        }

        ClearAttack();
        turnManager.NextTurn();
    }

    #endregion



    private void FixedUpdate()
    {

    }



 


    public void Clear()
    {
        GameObject.FindGameObjectsWithTag(Tag.Actor).ToList().ForEach(x => Destroy(x));
        actors.Clear();
    }

 


}
