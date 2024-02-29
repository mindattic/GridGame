using System;
using System.Collections;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.EventTrigger;

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
            if (!enemy.IsAlive) continue;

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
            yield return new WaitForSeconds(0.5f);
        }

        turnManager.currentPhase = TurnPhase.Attack;
        EnemyAttack();
    }


    private void PlayerAttack()
    {
        //Reset values
        actors.Where(x => x.IsAlive).ToList().ForEach(x => x.render.thumbnail.color = Colors.Solid.White);
        supportLineManager.Clear();
        attackParticipants.Reset();

        //Find actors that share a column or row
        foreach (var actor1 in players)
        {
            foreach (var actor2 in players)
            {
                if (actor1.Equals(actor2)) break;

                if (!actor1.IsAlive || !actor2.IsAlive)
                    return;

                if (actor1.IsSameColumn(actor2.location))
                    attackParticipants.alignedPairs.Add(new ActorPair(actor1, actor2, Axis.Vertical));
                if (actor1.IsSameRow(actor2.location))
                    attackParticipants.alignedPairs.Add(new ActorPair(actor1, actor2, Axis.Horizontal));
            }
        }

        if (attackParticipants.alignedPairs.Count < 1)
        {
            turnManager.NextTurn();
            return;
        }

        //Find attacking pairs
        foreach (var pair in attackParticipants.alignedPairs)
        {
            if (pair.axis == Axis.Vertical)
            {
                var lowest = Math.Min(pair.actor1.location.y, pair.actor2.location.y);
                var heighest = Math.Max(pair.actor1.location.y, pair.actor2.location.y);
                pair.enemies = enemies.Where(x => x.IsAlive && x.IsSameColumn(pair.actor1.location) && x.location.y > lowest && x.location.y < heighest).ToList();
                pair.players = players.Where(x => x.IsAlive && x.IsSameColumn(pair.actor1.location) && x.location.y > lowest && x.location.y < heighest).ToList();
                pair.gaps = tiles.Where(x => !x.IsOccupied && pair.actor1.IsSameColumn(x.location) && x.location.y > lowest && x.location.y < heighest).ToList();
            }
            else if (pair.axis == Axis.Horizontal)
            {
                var lowest = Math.Min(pair.actor1.location.x, pair.actor2.location.x);
                var heighest = Math.Max(pair.actor1.location.x, pair.actor2.location.x);
                pair.enemies = enemies.Where(x => x.IsAlive && x.IsSameRow(pair.actor1.location) && x.location.x > lowest && x.location.x < heighest).ToList();
                pair.players = players.Where(x => x.IsAlive && x.IsSameRow(pair.actor1.location) && x.location.x > lowest && x.location.x < heighest).ToList();
                pair.gaps = tiles.Where(x => !x.IsOccupied && pair.actor1.IsSameRow(x.location) && x.location.x > lowest && x.location.x < heighest).ToList();
            }

            //Assign attacking pairs
            var hasEnemiesBetween = pair.enemies.Count > 0;
            var hasGapsBetween = pair.gaps.Count > 0;
            if (hasEnemiesBetween && !hasGapsBetween)
            {
                attackParticipants.attackingPairs.Add(pair);
                attackParticipants.attackers.Add(pair.actor1);
                attackParticipants.attackers.Add(pair.actor2);
            }
        }

        if (attackParticipants.attackers.Count < 1)
        {
            turnManager.NextTurn();
            return;
        }

        //Find defenders
        foreach (var attackers in attackParticipants.attackingPairs)
        {
            foreach (var enemy in attackers.enemies)
            {
                enemy.render.thumbnail.color = Colors.Solid.Red;
                attackParticipants.defenders.Add(enemy);
            }
        }

        //Find support pairs
        foreach (var pair in attackParticipants.alignedPairs)
        {
            var isDirectAttacker1 = attackParticipants.attackers.Contains(pair.actor1);
            var isDirectAttacker2 = attackParticipants.attackers.Contains(pair.actor2);
            var hasDirectAttacker = isDirectAttacker1 || isDirectAttacker2;
            var hasEnemiesBetween = pair.enemies.Count > 0;
            var hasPlayersBetween = pair.players.Count > 0;

            if (hasDirectAttacker && !hasEnemiesBetween && !hasPlayersBetween)
            {
                supportLineManager.Add(pair.actor1.currentTile.position, pair.actor2.currentTile.position);
                attackParticipants.supporters.Add(pair.actor1);
                attackParticipants.supporters.Add(pair.actor2);
            }
        }


        StartCoroutine(StartPlayerAttack());
    }

    private IEnumerator StartPlayerAttack()
    {

        foreach (var attackers in attackParticipants.attackingPairs)
        {
            attackers.actor1.render.thumbnail.color = Colors.Solid.Green;
<<<<<<< HEAD
            attackers.actor1.SetStatusAttack();
            yield return new WaitForSeconds(0.25f);

            attackers.actor1.render.thumbnail.color = Colors.Solid.Green;
            attackers.actor2.SetStatusAttack();
=======
            yield return new WaitForSeconds(0.25f);

            attackers.actor2.render.thumbnail.color = Colors.Solid.Green;
>>>>>>> 9ed93de3d8b4b5c200f65ffebea7502954325966
            yield return new WaitForSeconds(0.25f);
        }

        foreach (var supporter in attackParticipants.supporters)
        {
            supporter.render.thumbnail.color = Colors.Solid.Green;
<<<<<<< HEAD
            supporter.SetStatusSupport();
=======
>>>>>>> 9ed93de3d8b4b5c200f65ffebea7502954325966
            yield return new WaitForSeconds(0.25f);
        }

        yield return new WaitForSeconds(2f);

        foreach (var enemy in attackParticipants.defenders)
        {
            enemy.TakeDamage(Random.Int(15, 100));
        }

        yield return new WaitForSeconds(2f);

        foreach (var player in players)
        {
            if (!player.IsAlive) continue;
            player.SetStatusNone();
            player.render.thumbnail.color = Colors.Solid.White;
            player.scale = tileScale;
        }

        timer.Set(scale: 1f, start: false);

        yield return new WaitForSeconds(2f);

        //Reset values
        actors.Where(x => x.IsAlive).ToList().ForEach(x => x.render.thumbnail.color = Colors.Solid.White);
        supportLineManager.Clear();
        attackParticipants.Reset();

        turnManager.NextTurn();
    }


    private void EnemyAttack()
    {

        attackParticipants.Reset();

        foreach (var enemy in enemies)
        {
            if (!enemy.IsAlive) continue;
            if (enemy.turnDelay > 0) continue;

            foreach (var player in players)
            {
                if (!player.IsAlive || (!player.IsSameColumn(enemy.location) && !player.IsSameRow(enemy.location))) continue;

                var delta = enemy.location - player.location;
                if (Math.Abs(delta.x).Equals(1) || Math.Abs(delta.y).Equals(1))
                {
                    attackParticipants.attackers.Add(enemy);
                    attackParticipants.defenders.Add(player);
                }

            }
        }

        StartCoroutine(StartEnemyAttack());
    }


    private IEnumerator StartEnemyAttack()
    {
        turnManager.currentPhase = TurnPhase.Attack;

        foreach (var attacker in attackParticipants.attackers)
        {
<<<<<<< HEAD
            attacker.SetStatusAttack();
=======
            attacker.render.thumbnail.color = Colors.Solid.Green;
>>>>>>> 9ed93de3d8b4b5c200f65ffebea7502954325966
            yield return new WaitForSeconds(0.25f);
        }


        foreach (var player in attackParticipants.defenders)
        {
            player.TakeDamage(Random.Int(16, 33));
        }

        yield return new WaitForSeconds(1f);

        foreach (var enemy in enemies)
        {
            if (!enemy.IsAlive) continue;
            enemy.render.thumbnail.color = Colors.Solid.White;
            enemy.scale = tileScale;
            if (enemy.turnDelay < 1)
            {
                enemy.GenerateTurnDelay();
            }
        }

        attackParticipants.Reset();
        turnManager.NextTurn();
    }


    private void FixedUpdate()
    {
        if (!HasSelectedPlayer)
            return;

    }

    public void PickupPlayer()
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
        if (actor == null)
            return;

        if (!actor.IsAlive)
            return;

        //Determine if player Team: "Player"
        if (actor.team != Team.Player)
            return;

        //Select actor
        selectedPlayer = actor;
        selectedPlayer.sortingOrder = 10;
        selectedPlayer.render.frame.color = Colors.Solid.Gold;

        turnManager.currentPhase = TurnPhase.Move;

        //Assign mouse offset (how off center was selection)
        mouseOffset = selectedPlayer.transform.position - mousePosition3D;

        //Show portrait
        //portraitManager.Play(selectedPlayer, PortraitTransitionState.FadeIn);

        //Spawn ghost images of selected player
        ghostManager.Spawn();

        timer.Set(scale: 1f, start: true);
    }

    public void DropPlayer()
    {
        if (!turnManager.IsPlayerTurn || !turnManager.IsMovePhase || !HasSelectedPlayer)
            return;

        //Assign location and position
        var closestTile = Geometry.ClosestTileByPosition(selectedPlayer.position);
        selectedPlayer.location = closestTile.location;
        selectedPlayer.position = Geometry.PositionFromLocation(selectedPlayer.location);
        selectedPlayer.render.frame.color = Colors.Solid.Green;

        //Reset tiles
        tiles.ForEach(x => x.spriteRenderer.color = Colors.Translucent.White);
        ghostManager.Clear();

        //Hide portrait
        //portraitManager.Play(selectedPlayer, PortraitTransitionState.FadeOut);

        //Determine if two actors occupy same location
        //selectedPlayer.CheckLocationConflict();

        //Clear selected player
        selectedPlayer = null;

        timer.Set(scale: 0f, start: false);

        turnManager.currentPhase = TurnPhase.Attack;
        PlayerAttack();
    }










}
