using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class SelectedPlayerManager : ExtendedMonoBehavior
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ////Verify currently players turn...
        //if (!turnManager.IsPlayerTurn)
        //    return;

        ////Verify turn phase is either "start" or "move"...
        //if (!turnManager.IsStartPhase && !turnManager.IsMovePhase)
        //    return;

        ////Verify either selected player or targetted player exists...
        //if (!HasSelectedPlayer && !HasTargettedPlayer)
        //    return;

        ////Assign tile color based on current player selection state
        //if (HasSelectedPlayer)
        //{
        //    //selectedPlayer.currentTile.spriteRenderer.color = Colors.Solid.Gold;
        //}
        //else if (HasTargettedPlayer)
        //{
        //    //targettedPlayer.currentTile.spriteRenderer.color = Colors.Solid.Gold;
        //}

    }


    public void Target()
    {
        //Verify is player turn...
        if (!turnManager.IsPlayerTurn)
            return;

        //Verify current phase is "start"...
        if (!turnManager.IsStartPhase)
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
        if (actor == null || !actor.IsAlive || !actor.IsActive || !actor.IsPlayer)
            return;

        //TODO: Update Card display...
        targettedPlayer = actor;
        targettedPlayer.sortingOrder = ZAxis.Max;

        //Assign mouse offset (how off center was selection)
        mouseOffset = targettedPlayer.position - mousePosition3D;

        cardManager.Set(targettedPlayer);
    }

    public void Untarget()
    {
        //Verify *HAS* targetted player...
        if (!HasTargettedPlayer)
            return;

        //Assign location and position
        //var closestTile = Geometry.ClosestTileByPosition(targettedPlayer.position);
        //closestTile.spriteRenderer.color = Colors.Translucent.White;
        //argettedPlayer.location = closestTile.location;
        targettedPlayer.position = targettedPlayer.currentTile.position;
        targettedPlayer.sortingOrder = ZAxis.Min;
        targettedPlayer = null;

        cardManager.Clear();
    }

    public void Select()
    {
        //Verify *HAS* targetted player...
        if (!HasTargettedPlayer)
            return;

        //Verify is player turn...
        if (!turnManager.IsPlayerTurn)
            return;

        //Verify current phase is "start"...
        if (!turnManager.IsStartPhase)
            return;

        //Select actor
        //selectedPlayer = players.FirstOrDefault(x => x.guid.Equals(targettedPlayer.guid));
        //if (!HasSelectedPlayer)
        //    return;

        selectedPlayer = targettedPlayer;
        if (!HasSelectedPlayer)
            return;

        targettedPlayer = null;
        turnManager.currentPhase = TurnPhase.Move;

        selectedPlayer.sortingOrder = ZAxis.Max;

      
        soundSource.PlayOneShot(resourceManager.SoundEffect($"Select"));

        //Clear bobbing position
        //ResetBobbing();

      

        //Assign mouse offset (how off center was selection)
        mouseOffset = selectedPlayer.position - mousePosition3D;

        //SpawnIn ghost images of selected player
        ghostManager.Spawn();

        timer.Set(scale: 1f, start: true);
    }

    public void Deselect()
    {
        //Verify is player turn...
        if (!turnManager.IsPlayerTurn)
            return;

        //Verify current phase is "move"...
        if (!turnManager.IsMovePhase)
            return;

        //Verify *HAS* selected player...
        if (!HasSelectedPlayer)
            return;

        //Assign location and position
        var closestTile = Geometry.ClosestTileByPosition(selectedPlayer.position);
        closestTile.spriteRenderer.color = Colors.Translucent.White;
        selectedPlayer.location = closestTile.location;
        selectedPlayer.position = Geometry.PositionFromLocation(selectedPlayer.location);
        selectedPlayer.sortingOrder = ZAxis.Min;
        selectedPlayer = null;


        tileManager.Reset();
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
