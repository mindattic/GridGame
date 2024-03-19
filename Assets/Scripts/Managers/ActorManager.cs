using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;

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
        CheckSelectedPlayer();
        CheckEnemy();
    }

    private void CheckSelectedPlayer()
    {
        if (!turnManager.IsPlayerTurn || !turnManager.IsMovePhase || !HasSelectedPlayer)
            return;

        selectedPlayer.currentTile.spriteRenderer.color = Colors.Solid.Gold;
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


    private void CheckPlayerAttack()
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

    private IEnumerator PlayerStartGlow()
    {
        yield return Wait.For(Interval.QuarterSecond);

        foreach (var pair in attackParticipants.attackingPairs)
        {
            pair.actor1.Set(ActionIcon.Attack);
            pair.actor1.Set(GlowState.On);
            pair.actor1.sortingOrder = ZAxis.Max;
            soundSource.PlayOneShot(resourceManager.SoundEffect("PlayerGlow"));
            yield return Wait.For(Interval.QuarterSecond);
            pair.actor2.Set(ActionIcon.Attack);
            pair.actor2.Set(GlowState.On);
            pair.actor2.sortingOrder = ZAxis.Max;
            soundSource.PlayOneShot(resourceManager.SoundEffect("PlayerGlow"));
            yield return Wait.For(Interval.QuarterSecond);
        }
    }

    private IEnumerator PlayerPortraitSlideIn(ActorPair pair)
    {
        soundSource.PlayOneShot(resourceManager.SoundEffect("Portrait"));
        var first = pair.axis == Axis.Vertical ? Direction.South : Direction.East;
        var second = pair.axis == Axis.Vertical ? Direction.North : Direction.West;
        var direction1 = pair.actor1 == pair.highest ? first : second;
        var direction2 = pair.actor2 == pair.highest ? first : second;
        portraitManager.SlideIn(pair.actor1, direction1);
        portraitManager.SlideIn(pair.actor2, direction2);
        yield return Wait.For(Interval.ThreeSecond);
    }

    private IEnumerator PlayerAttackEnemy(ActorPair pair)
    {
        foreach (var enemy in pair.enemies)
        {
            attackLineManager.Spawn(pair);
            //var damage = Random.Int(15, 33); //TODO: Calculate based on attacker stats
            var damage = 100;
            yield return enemy.TakeDamage(damage);
        }

        yield return Wait.For(Interval.HalfSecond);
        attackLineManager.Destroy(pair);
    }

    private IEnumerator CheckEnemyDie(ActorPair pair)
    {
        foreach (var enemy in pair.enemies)
        {
            if (enemy.HP < 1)
            {
                yield return enemy.Die();
            }
               
        }
    }

    private void PlayerStopGlow(ActorPair pair)
    {
        pair.actor1.Set(ActionIcon.None);
        pair.actor1.Set(GlowState.Off);
        pair.actor1.sortingOrder = ZAxis.Min;
        pair.actor2.Set(ActionIcon.None);
        pair.actor2.Set(GlowState.Off);
        pair.actor2.sortingOrder = ZAxis.Min;
    }

    private IEnumerator PlayerAttack()
    {
        yield return PlayerStartGlow();

        foreach (var pair in attackParticipants.attackingPairs)
        {
            yield return PlayerPortraitSlideIn(pair);
            yield return PlayerAttackEnemy(pair);
            yield return CheckEnemyDie(pair);
            PlayerStopGlow(pair);
        }

        ClearAttack();
        turnManager.NextTurn();
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
                    enemy.Set(ActionIcon.Attack);
                    var damage = Random.Int(15, 33); //TODO: Calculate based on attacker stats
                    player.TakeDamage(damage);

                    yield return Wait.For(Interval.TwoSecond);

                    enemy.GenerateTurnDelay();
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



    public void TargetPlayer()
    {
        if (!turnManager.IsPlayerTurn || !turnManager.IsStartPhase || HasSelectedPlayer)
            return;

        //Collect collision data
        var collisions = Physics2D.OverlapPointAll(mousePosition3D);
        if (collisions == null)
            return;

        //Find collider attached to actor
        var collider = collisions.FirstOrDefault(x => x.CompareTag(Tag.Actor));
        if (collider == null)
            return;

        //Retrieve actor from collider
        var actor = collider.gameObject.GetComponent<ActorBehavior>();
        if (actor == null || !actor.IsAlive || !actor.IsActive) return;

        //Determine if player Team: "Player"
        if (actor.team != Team.Player)
            return;

        //TODO: Update Card display...
        targettedPlayer = actor;
        cardManager.Set(targettedPlayer);
    }



    public void SelectPlayer()
    {
        if (!turnManager.IsPlayerTurn || !turnManager.IsStartPhase || HasSelectedPlayer || !HasTargettedPlayer)
            return;

        //Select actor
        selectedPlayer = targettedPlayer;
        selectedPlayer.sortingOrder = 10;

        soundSource.PlayOneShot(resourceManager.SoundEffect($"Select"));

        //Clear bobbing position
        ResetBobbing();

        turnManager.currentPhase = TurnPhase.Move;

        //Assign mouse offset (how off center was selection)
        mouseOffset = selectedPlayer.transform.position - mousePosition3D;

        //Spawn ghost images of selected player
        ghostManager.Spawn();

        timer.Set(scale: 1f, start: true);
    }

    public void DeselectPlayer()
    {
        if (!turnManager.IsPlayerTurn || !turnManager.IsMovePhase || !HasSelectedPlayer)
            return;

        //Assign location and position
        var closestTile = Geometry.ClosestTileByPosition(selectedPlayer.position);
        selectedPlayer.location = closestTile.location;
        selectedPlayer.position = Geometry.PositionFromLocation(selectedPlayer.location);

        //Clear tiles
        tiles.ForEach(x => x.spriteRenderer.color = Colors.Translucent.White);
        //ghostManager.Clear();

        //Clear selected player
        selectedPlayer.sortingOrder = 0;
        selectedPlayer = null;
        targettedPlayer = null;
        cardManager.Clear();

        timer.Set(scale: 0f, start: false);

        turnManager.currentPhase = TurnPhase.Attack;

        CheckPlayerAttack();
    }

    public void Destroy(ActorBehavior actor)
    {
        if (actor == null) return;


    }



    public void Clear()
    {
        GameObject.FindGameObjectsWithTag(Tag.Actor).ToList().ForEach(x => Destroy(x));
        actors.Clear();
    }

    private void ResetBobbing()
    {
        foreach (var actor in actors)
        {
            if (actor == null || !actor.IsAlive || !actor.IsActive) continue;
            actor.render.glow.transform.position = actor.position;
            actor.render.thumbnail.transform.position = actor.position;
            actor.render.frame.transform.position = actor.position;
        }
    }


}
