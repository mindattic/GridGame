using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MoveState = ActorMoveState;

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
        if (!HasSelectedPlayer)
            return;

        var closestTile = Geometry.ClosestTileByPosition(selectedPlayer.position);
        if (closestTile.location.Equals(selectedPlayer.location))
            return;

        var actor = actors.FirstOrDefault(x => !x.Equals(selectedPlayer) && x.location.Equals(closestTile.location));
        if (actor != null)
        {
            //Assign intended movement
            actor.SetDestination(selectedPlayer);
        }

        selectedPlayer.location = closestTile.location;
        selectedPlayer.currentTile.spriteRenderer.color = Colors.Solid.Gold;
    }

    private void ResetBattle()
    {
        //Reset actors
        actors.ForEach(x => x.spriteRenderer.color = Colors.Solid.White);

        //Reset tiles

        //Reset lines
        lineManager.Reset();

        //Reset battle
        battle.Reset();
    }

    private void CalculateBattle()
    {
        ResetBattle();

        if (HasSelectedPlayer)
            return;

        //Find actors that share a column or row

        foreach (var actor1 in players)
        {
            foreach (var actor2 in players)
            {
                if (actor1.Equals(actor2)) break;
                if (actor1.IsSameColumn(actor2.location))
                    battle.alignedPairs.Add(new ActorPair(actor1, actor2, Axis.Vertical));
                if (actor1.IsSameRow(actor2.location))
                    battle.alignedPairs.Add(new ActorPair(actor1, actor2, Axis.Horizontal));
            }
        }
        if (battle.alignedPairs.Count < 1)
            return;

        //Find attacking pairs
        foreach (var pair in battle.alignedPairs)
        {
            if (pair.axis == Axis.Vertical)
            {
                var lowest = Math.Min(pair.actor1.location.y, pair.actor2.location.y);
                var heighest = Math.Max(pair.actor1.location.y, pair.actor2.location.y);
                pair.enemies = enemies.Where(x => x.IsSameColumn(pair.actor1.location) && x.location.y > lowest && x.location.y < heighest).ToList();
                pair.allies = enemies.Where(x => x.IsSameColumn(pair.actor1.location) && x.location.y > lowest && x.location.y < heighest).ToList();
                pair.gaps = tiles.Where(x => pair.actor1.IsSameColumn(x.location) && x.location.y > lowest && x.location.y < heighest && !x.IsOccupied).ToList();
            }
            else if (pair.axis == Axis.Horizontal)
            {
                var lowest = Math.Min(pair.actor1.location.x, pair.actor2.location.x);
                var heighest = Math.Max(pair.actor1.location.x, pair.actor2.location.x);
                pair.enemies = enemies.Where(x => x.IsSameRow(pair.actor1.location) && x.location.x > lowest && x.location.x < heighest).ToList();
                pair.allies = players.Where(x => x.IsSameRow(pair.actor1.location) && x.location.x > lowest && x.location.x < heighest).ToList();
                pair.gaps = tiles.Where(x => pair.actor1.IsSameColumn(x.location) && x.location.x > lowest && x.location.x < heighest && !x.IsOccupied).ToList();
            }

            //Assign attacking pairs
            var hasEnemiesBetween = pair.enemies.Count > 0;
            var hasGapsBetween = pair.gaps.Count > 0;        
            if (hasEnemiesBetween && !hasGapsBetween)
            {
                battle.attackingPairs.Add(pair);
                battle.attackers.Add(pair.actor1);
                battle.attackers.Add(pair.actor2);
            }
        }

        //Find defenders
        foreach (var attackers in battle.attackingPairs)
        {
            foreach (var target in attackers.enemies)
            {
                target.spriteRenderer.color = Colors.Solid.Red;
                battle.defenders.Add(target);
            }
        }

        //Find support pairs
        int i = 0;
        foreach (var pair in battle.alignedPairs)
        {
            var isAttacker1 = battle.attackers.Contains(pair.actor1);
            var isAttacker2 = battle.attackers.Contains(pair.actor2);
            var hasDirectAttacker = isAttacker1 || isAttacker2;
            var hasEnemiesBetween = pair.enemies.Count > 0;
            var hasAlliesBetween = pair.allies.Count > 0;

            if (hasDirectAttacker && !hasEnemiesBetween && !hasAlliesBetween)
            {
                lines[i++].Set(pair.actor1.currentTile.position, pair.actor2.currentTile.position);
                if (isAttacker1)
                    battle.supports.Add(pair.actor1);
                if (isAttacker2)
                    battle.supports.Add(pair.actor2);
            }
        }

    }

    private void FixedUpdate()
    {

    }

    public void PickupPlayer()
    {
        //Only pickup player if no player is selected
        if (HasSelectedPlayer)
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

        //Determine if player Team: "Player"
        if (actor.team != Team.Player)
            return;

        //TODO: Show/Hide art for every selected player
        if (actor.name == "Paladin")
            playerArt.Show();
        else
            playerArt.Hide();


        ResetBattle();

        //Select actor
        selectedPlayer = actor;

        selectedPlayer.sortingOrder = 2;
        selectedPlayer.trailRenderer.enabled = true;
        selectedPlayer.spriteRenderer.color = Colors.Solid.Gold;

        //Assign mouse offset (how off center was selection)
        mouseOffset = selectedPlayer.transform.position - mousePosition3D;

        timer.Set(scale: 1f, start: true);
    }

    public void DropPlayer()
    {
        //Only drop player if has selected player
        if (!HasSelectedPlayer)
            return;

        //Assign location and position
        var closestTile = Geometry.ClosestTileByPosition(selectedPlayer.position);
        selectedPlayer.location = closestTile.location;
        selectedPlayer.position = Geometry.PositionFromLocation(selectedPlayer.location);
        selectedPlayer.spriteRenderer.color = Colors.Solid.White;
        selectedPlayer.sortingOrder = 1;
        selectedPlayer.trailRenderer.enabled = false;

        //Reset tiles
        tiles.ForEach(x => x.spriteRenderer.color = Colors.Transparent.White);


        playerArt.Hide();

        //Determine if two actors occupy same location
        selectedPlayer.CheckLocation();

        //Clear selected player
        selectedPlayer = null;

        CalculateBattle();

        timer.Set(scale: 1f, start: false);
    }

}
