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

    }


    public void Select()
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
        actors.ForEach(x => x.render.SetFocus(false));
        focusedPlayer = actor;
        focusedPlayer.sortingOrder = ZAxis.Max;
        focusedPlayer.render.SetFocus(true);
        //Assign mouse offset (how off center was selection)
        mouseOffset = focusedPlayer.position - mousePosition3D;

        cardManager.Set(focusedPlayer);
    }

    public void Unselect()
    {
        //Verify *HAS* targetted player...
        if (!HasFocusedPlayer)
            return;

        if (!HasSelectedPlayer)
        {
            focusedPlayer.position = focusedPlayer.currentTile.position;
            focusedPlayer.sortingOrder = ZAxis.Min;
            //cardManager.Clear();
        }

        focusedPlayer = null;
     
    }

    public void Pickup()
    {
        //Verify is player turn...
        if (!turnManager.IsPlayerTurn)
            return;

        //Verify current phase is "start"...
        if (!turnManager.IsStartPhase)
            return;

        //Pickup actor
        selectedPlayer = focusedPlayer;
        if (!HasSelectedPlayer)
            return;

        Unselect();
        turnManager.currentPhase = TurnPhase.Move;
        selectedPlayer.sortingOrder = ZAxis.Max;

        soundSource.PlayOneShot(resourceManager.SoundEffect($"Select"));

        //Clear bobbing position
        //ResetBobbing();



        //Assign mouse offset (how off center was selection)
        mouseOffset = selectedPlayer.position - mousePosition3D;

        
        ghostManager.Start(selectedPlayer);
        footstepManager.Start(selectedPlayer);

        timer.Set(scaleX: 1f, start: true);
    }

    public void Drop()
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
        selectedPlayer.SetStatus(Status.None);
        selectedPlayer = null;

        ghostManager.Stop();
        footstepManager.Stop();

        tileManager.Reset();
        cardManager.Clear();
        timer.Set(scaleX: 0f, start: false);
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
