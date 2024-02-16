using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
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
            //Constantly update active p location
            var closestTile = Geometry.ClosestTileByPosition(selectedPlayer.transform.position);
            selectedPlayer.location = closestTile.location;
        }

        
        CalculateBattleConditions();
    }

  

    private void CalculateBattleConditions()
    {

        //Retrieve all players
        var players = actors.Where(p => p.team == Team.Player).ToList();
        //players.ForEach(p => p.spriteRenderer.color = Color.white);

        //Retrieve all enemies
        var enemies = actors.Where(e => e.team == Team.Enemy).ToList();
        enemies.ForEach(e => e.spriteRenderer.color = Color.white);

        //Find actors that share a column or row
        List<ActorPair> alignedPlayers = new List<ActorPair>();
        foreach (var actor1 in players)
        {
            foreach (var actor2 in players)
            {
                if (actor1.Equals(actor2)) break;
                if (actor1.IsSameColumn(actor2))
                    alignedPlayers.Add(new ActorPair(actor1, actor2, Axis.Vertical));
                if (actor1.IsSameRow(actor2))
                    alignedPlayers.Add(new ActorPair(actor1, actor2, Axis.Horizontal));
            }
        }
        if (alignedPlayers.Count < 1)
            return;


        //Find gaps between each pair of aligned actors
        foreach (var pair in alignedPlayers)
        {

            List<TileBehavior> gaps = new List<TileBehavior>();
            if (pair.axis == Axis.Vertical)
            {
                var lowest = Math.Min(pair.actor1.location.y, pair.actor2.location.y);
                var heighest = Math.Max(pair.actor1.location.y, pair.actor2.location.y);
                gaps = tiles.Where(g =>
                {
                    return g.location.x == pair.actor1.location.x
                    && g.location.y > lowest
                    && g.location.y < heighest
                    && !g.isOccupied;
                }).ToList();
            }
            else if (pair.axis == Axis.Horizontal)
            {
                var lowest = Math.Min(pair.actor1.location.x, pair.actor2.location.x);
                var heighest = Math.Max(pair.actor1.location.x, pair.actor2.location.x);
                gaps = tiles.Where(g =>
                {
                    return g.location.y == pair.actor1.location.y
                    && g.location.x > lowest
                    && g.location.x < heighest
                    && !g.isOccupied;
                }).ToList();
            }

            pair.gapCount = gaps.Count;
        }

        //Find enemies between each pair of aligned actors
        foreach (var pair in alignedPlayers)
        {
            if (pair.gapCount > 0)
                break;

            //pair.actor1.spriteRenderer.color = Color.blue;
            //pair.actor2.spriteRenderer.color = Color.blue;

            List<ActorBehavior> pairEnemies = new List<ActorBehavior>();
            if (pair.axis == Axis.Vertical)
            {
                var lowest = Math.Min(pair.actor1.location.y, pair.actor2.location.y);
                var heighest = Math.Max(pair.actor1.location.y, pair.actor2.location.y);
                pairEnemies = enemies.Where(e =>
                {
                    return e.location.x == pair.actor1.location.x
                    && e.location.y > lowest
                    && e.location.y < heighest;
                }).ToList();
            }
            else if (pair.axis == Axis.Horizontal)
            {
                var lowest = Math.Min(pair.actor1.location.x, pair.actor2.location.x);
                var heighest = Math.Max(pair.actor1.location.x, pair.actor2.location.x);
                pairEnemies = enemies.Where(e =>
                {
                    return e.location.y == pair.actor1.location.y
                    && e.location.x > lowest
                    && e.location.x < heighest;
                }).ToList();
            }

            foreach (var enemy in pairEnemies)
            {
                enemy.spriteRenderer.color = Color.red;
            }
        }
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
