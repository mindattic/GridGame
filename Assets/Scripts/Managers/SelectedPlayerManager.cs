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
        selectedPlayer = actor;
        selectedPlayer.sortingOrder = ZAxis.Max;

        //Assign mouse offset (how off center was selection)
        mouseOffset = selectedPlayer.position - mousePosition3D;

        cardManager.Set(selectedPlayer);
    }

    public void Unselect()
    {
        //Verify *HAS* targetted player...
        if (!HasSelectedPlayer)
            return;

        if (!HasCurrentPlayer)
        {
            selectedPlayer.position = selectedPlayer.currentTile.position;
            selectedPlayer.sortingOrder = ZAxis.Min;
            //cardManager.Clear();
        }

        selectedPlayer = null;
     
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
        currentPlayer = selectedPlayer;
        if (!HasCurrentPlayer)
            return;


        currentPlayer.SetActionIcon(ActionIcon.Move);

        Unselect();
        turnManager.currentPhase = TurnPhase.Move;
        currentPlayer.sortingOrder = ZAxis.Max;

        soundSource.PlayOneShot(resourceManager.SoundEffect($"Select"));

        //Clear bobbing position
        //ResetBobbing();



        //Assign mouse offset (how off center was selection)
        mouseOffset = currentPlayer.position - mousePosition3D;

        
        ghostManager.Start(currentPlayer);
        footstepManager.Start(currentPlayer);

        timer.Set(scale: 1f, start: true);
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
        if (!HasCurrentPlayer)
            return;

        //Assign location and position
        var closestTile = Geometry.ClosestTileByPosition(currentPlayer.position);
        closestTile.spriteRenderer.color = Colors.Translucent.White;
        currentPlayer.location = closestTile.location;
        currentPlayer.position = Geometry.PositionFromLocation(currentPlayer.location);
        currentPlayer.sortingOrder = ZAxis.Min;
        currentPlayer.SetActionIcon(ActionIcon.None);
        currentPlayer = null;

        ghostManager.Stop();
        footstepManager.Stop();

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
