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
        if (Input.GetMouseButtonDown(0))
            PickupPlayer();
        else if (Input.GetMouseButtonUp(0))
            DropPlayer();

        if (HasSelectedPlayer)
        {
            //Constantly update selected player location
            selectedPlayer.previousLocation = selectedPlayer.location;
            var closestTile = Geometry.ClosestTileByPosition(selectedPlayer.transform.position);
            selectedPlayer.location = closestTile.location;
        }


        CalculateBattleConditions();
    }



    private void CalculateBattleConditions()
    {
        //Reset actors
        enemies.ForEach(x => x.spriteRenderer.color = Color.white);

        //Reset lines
        int i = 0;
        lines.ForEach(l => l.Hide());

        //Clear names
        attackerNames = new HashSet<string>();
        defenderNames = new HashSet<string>();

        //Clear pairs
        List<ActorPair> alignedPairs = new List<ActorPair>();
        List<ActorPair> attackingPairs = new List<ActorPair>();

        if (HasSelectedPlayer)
            return;

        //TODO: Execute this event 1x after dropping selected player...



        //Find actors that share a column or row

        foreach (var actor1 in players)
        {
            foreach (var actor2 in players)
            {
                if (actor1.Equals(actor2)) break;
                if (actor1.IsSameColumn(actor2))
                    alignedPairs.Add(new ActorPair(actor1, actor2, Axis.Vertical));
                if (actor1.IsSameRow(actor2))
                    alignedPairs.Add(new ActorPair(actor1, actor2, Axis.Horizontal));
            }
        }
        if (alignedPairs.Count < 1)
            return;

        foreach (var pair in alignedPairs)
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

            bool hasGaps = pair.gaps.Count > 0;
            bool hasTargets = pair.targets.Count > 0;
            if (!hasGaps && hasTargets)
            {
                attackingPairs.Add(pair);
                attackerNames.Add(pair.actor1.name);
                attackerNames.Add(pair.actor2.name);
            }

        }

        //find aligned pairs where only 1 actor is a direct attacker (e.g. is actor1 or actor 2 in any attackingPair entry) 
        foreach (var pair in alignedPairs)
        {
            var isAttacker1 = attackerNames.Contains(pair.actor1.name);
            var isAttacker2 = attackerNames.Contains(pair.actor2.name);

            if (isAttacker1 && !isAttacker2)
            {
                lines[i++].Set(pair.actor1.position, pair.actor2.position);
                attackerNames.Add(pair.actor1.name); //(Indirect Attacker)
            }
            else if (!isAttacker1 && isAttacker2)
            {
                lines[i++].Set(pair.actor1.position, pair.actor2.position);
                attackerNames.Add(pair.actor2.name); //(Indirect Attacker)
            }
        }

        foreach (var attackers in attackingPairs)
        {
            foreach (var target in attackers.targets)
            {
                target.spriteRenderer.color = Color.red;
                defenderNames.Add(target.name);
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
        Collider2D mouseCollider = Physics2D.OverlapPoint(mousePosition3D);
        if (mouseCollider == null || !mouseCollider.CompareTag(Tag.Actor))
            return;

        //Retrieve player under mouse cursor
        var actor = mouseCollider.gameObject.GetComponent<ActorBehavior>();
        if (actor == null)
            return;

        //Determine if player Team: "Player"
        if (actor.team != Team.Player)
            return;

        //Determine if player MoveState: "Idle"
        if (actor.moveState != MoveState.Idle)
            return;

        actor.currentTile.isOccupied = false;

        //Select actor
        selectedPlayer = actor;
        selectedPlayer.moveState = MoveState.Moving;
        selectedPlayer.sortingOrder = 2;
        selectedPlayer.trailRenderer.enabled = true;

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

        //Clear active p
        selectedPlayer = null;

        //Restore box collider size to 100%
        //actors.ForEach(p => p.boxCollider2D.size = size50);

        timer.Set(scale: 1f, start: false);
    }

}
