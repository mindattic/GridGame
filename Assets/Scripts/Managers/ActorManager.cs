using System;
using System.Collections;
using System.Linq;
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
        CheckPlayerMove();
        CheckEnemyMove();
    }

    private void CheckPlayerMove()
    {
        if (!turnManager.IsPlayerActive || !turnManager.IsMovePhase || !HasSelectedPlayer)
            return;

        var closestTile = Geometry.ClosestTileByPosition(selectedPlayer.position);
        if (closestTile.location.Equals(selectedPlayer.location))
            return;

        //Determine if selected player and another actor are occupying the same tile
        var actor = actors.FirstOrDefault(x => x.IsAlive && !x.Equals(selectedPlayer) && x.location.Equals(closestTile.location));
        if (actor != null)
        {
            actor.SwapLocation(selectedPlayer);
        }

        selectedPlayer.location = closestTile.location;
        selectedPlayer.currentTile.spriteRenderer.color = Colors.Solid.Gold;
    }

    private void CheckEnemyMove()
    {
        if (!turnManager.IsEnemyActive || !turnManager.IsStartPhase)
            return;

        StartCoroutine(EnemyMove());
    }

    IEnumerator EnemyMove()
    {
        turnManager.phase = TurnPhase.Move;

        overlayManager.color = new Color(1f, 0f, 0f, 0.5f);
        titleManager.text = "Enemy Turn";

        yield return new WaitForSeconds(3f);

        overlayManager.color = new Color(0f, 0f, 0f, 0f);
        titleManager.text = "";

        turnManager.NextTurn();
    }

    private void CalculateBattle()
    {
        //Reset objects
        actors.ForEach(x => x.renderers.thumbnail.color = Colors.Solid.White);
        supportLineManager.Clear();
        battle.Reset();

        //Find actors that share a column or row

        foreach (var actor1 in players)
        {
            foreach (var actor2 in players)
            {
                if (actor1.Equals(actor2)) break;

                if (!actor1.IsAlive || !actor2.IsAlive)
                    return;

                if (actor1.IsSameColumn(actor2.location))
                    battle.alignedPairs.Add(new ActorPair(actor1, actor2, Axis.Vertical));
                if (actor1.IsSameRow(actor2.location))
                    battle.alignedPairs.Add(new ActorPair(actor1, actor2, Axis.Horizontal));
            }
        }
        if (battle.alignedPairs.Count < 1)
            return;


        //bool isBetween(float a, float b, float c)
        //{
        //    return a > b && a < c;
        //}


        //Find attacking pairs
        foreach (var pair in battle.alignedPairs)
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
                battle.attackingPairs.Add(pair);
                battle.attackers.Add(pair.actor1);
                battle.attackers.Add(pair.actor2);
            }
        }

        //Find defenders
        foreach (var attackers in battle.attackingPairs)
        {
            foreach (var enemy in attackers.enemies)
            {
                enemy.renderers.thumbnail.color = Colors.Solid.Red;
                battle.defenders.Add(enemy);
            }
        }

        //Find support pairs
        foreach (var pair in battle.alignedPairs)
        {
            var isDirectAttacker1 = battle.attackers.Contains(pair.actor1);
            var isDirectAttacker2 = battle.attackers.Contains(pair.actor2);
            var hasDirectAttacker = isDirectAttacker1 || isDirectAttacker2;
            var hasEnemiesBetween = pair.enemies.Count > 0;
            var hasPlayersBetween = pair.players.Count > 0;

            if (hasDirectAttacker && !hasEnemiesBetween && !hasPlayersBetween)
            {
                supportLineManager.Add(pair.actor1.currentTile.position, pair.actor2.currentTile.position);
                battle.supporters.Add(pair.actor1);
                battle.supporters.Add(pair.actor2);
            }
        }


        StartCoroutine(StartBattle());

    }

    private void FixedUpdate()
    {
        if (!HasSelectedPlayer)
            return;

    }

    public void PickupPlayer()
    {
        if (!turnManager.IsPlayerActive || !turnManager.IsMovePhase || HasSelectedPlayer)
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
        selectedPlayer.renderers.frame.color = Colors.Solid.Gold;

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
        if (!turnManager.IsPlayerActive || !turnManager.IsMovePhase || !HasSelectedPlayer)
            return;

        turnManager.phase = TurnPhase.Attack;

        //Assign location and position
        var closestTile = Geometry.ClosestTileByPosition(selectedPlayer.position);
        selectedPlayer.location = closestTile.location;
        selectedPlayer.position = Geometry.PositionFromLocation(selectedPlayer.location);
        selectedPlayer.renderers.frame.color = Colors.Solid.White;
        selectedPlayer.sortingOrder = 1;

        //Reset tiles
        tiles.ForEach(x => x.spriteRenderer.color = Colors.Transparent.White);
        ghostManager.Clear();

        //Hide portrait
        //portraitManager.Play(selectedPlayer, PortraitTransitionState.FadeOut);

        //Determine if two actors occupy same location
        selectedPlayer.CheckLocationConflict();

        //Clear selected player
        selectedPlayer = null;

        CalculateBattle();
    }



    private IEnumerator StartBattle()
    {
        Vector3 enlarged = new Vector3(tileSize * 1.25f, tileSize * 1.25f, 1);

        foreach (var attackers in battle.attackingPairs)
        {
            attackers.actor1.renderers.frame.color = Colors.Solid.Red;
            attackers.actor1.scale = enlarged;
            attackers.actor1.sortingOrder = 10;
            yield return new WaitForSeconds(0.25f);

            attackers.actor2.renderers.frame.color = Colors.Solid.Red;
            attackers.actor2.scale = enlarged;
            attackers.actor2.sortingOrder = 10;
            yield return new WaitForSeconds(0.25f);
        }

        foreach (var supporter in battle.supporters)
        {
            supporter.renderers.frame.color = Colors.Solid.Red;
            supporter.scale = enlarged;
            supporter.sortingOrder = 10;
            yield return new WaitForSeconds(0.25f);
        }

        foreach (var enemy in battle.defenders)
        {
            enemy.TakeDamage(RNG.RandomInt(16, 33));
        }

        yield return new WaitForSeconds(0.25f);

        players.ForEach(x =>
        {
            x.sortingOrder = 1;
            x.renderers.frame.color = Colors.Solid.White;
            x.scale = tileScale;
        });

        timer.Set(scale: 1f, start: false);

        yield return new WaitForSeconds(1f);


        //Reset objects
        actors.ForEach(x => x.renderers.thumbnail.color = Colors.Solid.White);
        supportLineManager.Clear();
        battle.Reset();

        turnManager.activeTeam = Team.Enemy;
        turnManager.phase = TurnPhase.Start;
    }









}
