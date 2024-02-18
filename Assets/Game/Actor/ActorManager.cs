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

        //Constantly update selected player location
        var closestTile = Geometry.ClosestTileByPosition(selectedPlayer.transform.position);
        selectedPlayer.location = closestTile.location;
        //selectedPlayer.currentTile.spriteRenderer.color = Color.yellow;
    }

    private void ResetBattle()
    {
        //Reset actors
        actors.ForEach(x => x.spriteRenderer.color = Color.white);

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
                if (actor1.IsSameColumn(actor2))
                    battle.alignedPairs.Add(new ActorPair(actor1, actor2, Axis.Vertical));
                if (actor1.IsSameRow(actor2))
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
                pair.gaps = tiles.Where(x => x.location.x == pair.actor1.location.x && x.location.y > lowest && x.location.y < heighest && !x.isOccupied).ToList();
                pair.targets = enemies.Where(x => x.IsSameColumn(pair.actor1) && x.location.y > lowest && x.location.y < heighest).ToList();
            }
            else if (pair.axis == Axis.Horizontal)
            {
                var lowest = Math.Min(pair.actor1.location.x, pair.actor2.location.x);
                var heighest = Math.Max(pair.actor1.location.x, pair.actor2.location.x);
                pair.gaps = tiles.Where(x => x.location.y == pair.actor1.location.y && x.location.x > lowest && x.location.x < heighest && !x.isOccupied).ToList();
                pair.targets = enemies.Where(x => x.IsSameRow(pair.actor1) && x.location.x > lowest && x.location.x < heighest).ToList();
            }

            //Assign attacking pairs
            if (pair.gaps.Count < 1 && pair.targets.Count > 0)
            {
                battle.attackingPairs.Add(pair);
                battle.attackers.Add(pair.actor1);
                battle.attackers.Add(pair.actor2);
            }
        }

        //Find support pairs
        int i = 0;
        foreach (var pair in battle.alignedPairs)
        {
            var isAttacker1 = battle.attackers.Contains(pair.actor1);
            var isAttacker2 = battle.attackers.Contains(pair.actor2);
            if ((isAttacker1 && !isAttacker2) || (!isAttacker1 && isAttacker2))
            {
                lines[i++].Set(pair.actor1.position, pair.actor2.position);
                battle.supports.Add(isAttacker1 ? pair.actor2 : pair.actor1);
            }
        }

        //Finding defenders
        foreach (var attackers in battle.attackingPairs)
        {
            foreach (var target in attackers.targets)
            {
                target.spriteRenderer.color = Color.red;
                battle.defenders.Add(target);
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

        //Find player overlaping mouse position
        var mouseColliders = Physics2D.OverlapPointAll(mousePosition3D);
        if (mouseColliders == null)
            return;

        var collider = mouseColliders.FirstOrDefault(x => x.CompareTag(Tag.Select));
        if (collider == null)
            return;

        //Retrieve player under mouse cursor
        var actor = collider.gameObject.GetComponentInParent<ActorBehavior>();
        if (actor == null)
            return;

        //Determine if player Team: "Player"
        if (actor.team != Team.Player)
            return;

        //Determine if player MoveState: "Idle"
        if (actor.moveState != MoveState.Idle)
            return;

        if (actor.name == "Paladin")
            playerArt.Show();
        else 
            playerArt.Hide();

        actor.currentTile.isOccupied = false;

        ResetBattle();

        //Select actor
        selectedPlayer = actor;
        selectedPlayer.moveState = MoveState.Moving;
        selectedPlayer.sortingOrder = 2;
        selectedPlayer.trailRenderer.enabled = true;
        selectedPlayer.spriteRenderer.color = Color.yellow;

        //Assign mouse offset (how off center was selection)
        mouseOffset = selectedPlayer.transform.position - mousePosition3D;

        //Reduce box collider size (allowing actors some 'wiggle room')
        //actors.ForEach(p => p.boxCollider2D.size = size50);
        //selectedPlayer.boxCollider2D.size = size33;

        timer.Set(scale: 1f, start: true);
    }

    public void DropPlayer()
    {
        //Only drop player if has selected player
        if (!HasSelectedPlayer)
            return;

        //Assign location and position
        var closestTile = Geometry.ClosestTileByPosition(selectedPlayer.transform.position);
        selectedPlayer.location = closestTile.location;
        selectedPlayer.currentTile.isOccupied = true;
        selectedPlayer.transform.position = Geometry.PositionFromLocation(selectedPlayer.location);
        selectedPlayer.sortingOrder = 1;
        selectedPlayer.moveState = MoveState.Idle;
        selectedPlayer.trailRenderer.enabled = false;
        selectedPlayer.spriteRenderer.color = Color.white;
        playerArt.Hide();

        //Clear selected player
        selectedPlayer = null;

        CalculateBattle();

        //Restore box collider size to 100%
        //actors.ForEach(p => p.boxCollider2D.size = size50);

        timer.Set(scale: 1f, start: false);
    }

}
