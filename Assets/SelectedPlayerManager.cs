using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelectedPlayerManager : ExtendedMonoBehavior
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Verify currently players turn...
        if (!turnManager.IsPlayerTurn)
            return;

        //Verify turn phase is either "start" or "move"...
        if (!turnManager.IsStartPhase && !turnManager.IsMovePhase)
            return;

        //Verify either selected player or targetted player exists...
        if (!HasSelectedPlayer && !HasTargettedPlayer)
            return;

        //Assign tile color based on current player selection state
        if (HasSelectedPlayer)
        {
            selectedPlayer.currentTile.spriteRenderer.color = Colors.Solid.Gold;
        }
        else if (HasTargettedPlayer)
        {
            targettedPlayer.currentTile.spriteRenderer.color = Colors.Solid.Gold;
        }

    }


    public void Target()
    {
        //Verify currently players turn...
        if (!turnManager.IsPlayerTurn)
            return;

        //Verify current phase is "start"...
        if (!turnManager.IsStartPhase)
            return;

        //Verify *DOES NOT HAVE* targetted player...
        if (HasTargettedPlayer)
            return;

        //Find collider attached to actor
        var collisions = Physics2D.OverlapPointAll(mousePosition3D);
        if (collisions == null)
            return;
        var collider = collisions.FirstOrDefault(x => x.CompareTag(Tag.Actor));
        if (collider == null)
            return;

        //Retrieve actor from collider
        var actor = collider.gameObject.GetComponent<ActorBehavior>();
        if (actor == null || !actor.IsAlive || !actor.IsActive)
            return;

        //Determine if player Team: "Player"
        if (actor.team != Team.Player)
            return;

        //TODO: Update Card display...
        targettedPlayer = actor;
        targettedPlayer.sortingOrder = ZAxis.Max;
        cardManager.Set(targettedPlayer);
    }

    public void Untarget()
    {
        //Verify currently players turn...
        if (!turnManager.IsPlayerTurn)
            return;

        //Verify current phase is "start"...
        if (!turnManager.IsStartPhase)
            return;

        //Verify *HAS* targetted player...
        if (!HasTargettedPlayer)
            return;

        //Assign location and position
        var closestTile = Geometry.ClosestTileByPosition(targettedPlayer.position);
        targettedPlayer.SetLocation(closestTile.location);

        //Clear tiles
        tiles.ForEach(x => x.spriteRenderer.color = Colors.Translucent.White);
        //ghostManager.Clear();

        //Clear selected player
        targettedPlayer.sortingOrder = ZAxis.Min;
        targettedPlayer = null;

        cardManager.Clear();
    }

    public void Select()
    {
        if (!turnManager.IsPlayerTurn || !turnManager.IsStartPhase)
            return;

        if (!HasTargettedPlayer)
            return;

        //Select actor
        selectedPlayer = players.FirstOrDefault(x => x.guid.Equals(targettedPlayer.guid));
        selectedPlayer.sortingOrder = ZAxis.Max;

        soundSource.PlayOneShot(resourceManager.SoundEffect($"Select"));

        //Clear bobbing position
        //ResetBobbing();

        turnManager.currentPhase = TurnPhase.Move;

        //Assign mouse offset (how off center was selection)
        mouseOffset = selectedPlayer.transform.position - mousePosition3D;

        //SpawnIn ghost images of selected player
        ghostManager.Spawn();

        timer.Set(scale: 1f, start: true);
    }

    public void Deselect()
    {
        if (!turnManager.IsPlayerTurn)
            return;

        if (!turnManager.IsMovePhase)
            return;

        if (!HasSelectedPlayer)
            return;

        //Assign location and position
        var closestTile = Geometry.ClosestTileByPosition(selectedPlayer.position);
        selectedPlayer.SetLocation(closestTile.location);

        //Clear tiles
        tiles.ForEach(x => x.spriteRenderer.color = Colors.Translucent.White);
        //ghostManager.Clear();

        //Clear selected player
        selectedPlayer.sortingOrder = ZAxis.Min;
        selectedPlayer = null;

        cardManager.Clear();

        timer.Set(scale: 0f, start: false);

        turnManager.currentPhase = TurnPhase.Attack;

        actorManager.CheckPlayerAttack();
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
