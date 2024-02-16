using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
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

        //Clear attacker names
        GameManager.instance.attackerNames = new HashSet<string>();
        GameManager.instance.defenderNames = new HashSet<string>();

        List<ActorPair> alignedPairs = new List<ActorPair>();
        List<ActorPair> attackingPairs = new List<ActorPair>();

        if (HasSelectedPlayer)
            return;

        //TODO: Execute this event 1x after droping selected player...



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

            bool hasGaps = pair.gaps != null && pair.gaps.Count > 0;
            bool hasTargets = pair.targets != null && pair.targets.Count > 0;
            if (!hasGaps && hasTargets)
            {
                attackingPairs.Add(pair);
            }

        }

        foreach (var pair in alignedPairs)
        {
            lines[i++].Set(pair.actor1.currentTile.position, pair.actor2.currentTile.position);

            //foreach (var attackers in attackingPairs)
            //{
            //    bool isActor1Attacker = pair.actor1.Equals(attackers.actor1) && !pair.actor2.Equals(attackers.actor2);
            //    bool isActor2Attacker = pair.actor2.Equals(attackers.actor1) && !pair.actor1.Equals(attackers.actor2);

            //    if (isActor1Attacker || isActor2Attacker)
            //    {
            //        lines[i++].Set(pair.actor1.position, pair.actor2.position);
            //    }
            //}


            ////    if (!attackingPairs.Contains(pair))
            ////{
            ////    lines[i++].Set(pair.actor1.position, pair.actor2.position);
            ////}
        }

        foreach (var attackers in attackingPairs)
        {
            GameManager.instance.attackerNames.Add(attackers.actor1.name);
            GameManager.instance.attackerNames.Add(attackers.actor2.name);
            foreach (var target in attackers.targets)
            {
                target.spriteRenderer.color = Color.red;
                GameManager.instance.defenderNames.Add(target.name);
            }
        }

        //foreach (var attackers in attackingPairs)
        //{



        //    bool hasGaps = attackers.gaps != null && attackers.gaps.Count > 0;
        //    bool hasTargets = attackers.targets != null && attackers.targets.Count > 0;
        //    bool isDirectAttacker = GameManager.instance.attackerNames.Contains(attackers.actor1.name) || GameManager.instance.attackerNames.Contains(attackers.actor2.name);

        //    if (attackers.gaps.Count > 0 && !hasTargets && isDirectAttacker)
        //    {
        //        lines[i++].Set(attackers.actor1.position, attackers.actor2.position);
        //        GameManager.instance.attackerNames.Add(attackers.actor1.name);
        //        GameManager.instance.attackerNames.Add(attackers.actor2.name);
        //    }
        //    else if (!hasGaps && hasTargets)
        //    {
        //        attackers.targets.ForEach(x => x.spriteRenderer.color = Color.red);
        //    }

        //    GameManager.instance.attackerNames.Add(attackers.actor1.name);
        //    GameManager.instance.attackerNames.Add(attackers.actor2.name);
        //    attackers.targets.ForEach(x => GameManager.instance.defenderNames.Add(x.name));
        //}



    }

    private void FixedUpdate()
    {





    }

    public void PickupPlayer()
    {
        //Only pickup player if no p is selected
        if (HasSelectedPlayer)
            return;

        //Find p overlaping mouse position
        Collider2D mouseCollider = Physics2D.OverlapPoint(mousePosition3D);
        if (mouseCollider == null || !mouseCollider.CompareTag(Tag.Actor))
            return;

        //Retrieve p under mouse cursor
        var actor = mouseCollider.gameObject.GetComponent<ActorBehavior>();
        if (actor == null)
            return;

        //Determine if p Team: "Player"
        if (actor.team != Team.Player)
            return;

        //Determine if p MoveState: "Idle"
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
