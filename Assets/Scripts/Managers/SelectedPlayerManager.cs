using Game.Behaviors;
using Game.Manager;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelectedPlayerManager : MonoBehaviour
{
    protected CardManager cardManager => GameManager.instance.cardManager;
    protected TurnManager turnManager => GameManager.instance.turnManager;
    protected Vector3 mousePosition3D => GameManager.instance.mousePosition3D;
    protected Vector3 mouseOffset
    {
        get { return GameManager.instance.mouseOffset; }
        set { GameManager.instance.mouseOffset = value; }
    }






    protected ActorInstance focusedActor
    {
        get { return GameManager.instance.focusedActor; }
        set { GameManager.instance.focusedActor = value; }
    }

    protected ActorInstance previousSelectedPlayer
    {
        get { return GameManager.instance.previousSelectedPlayer; }
        set { GameManager.instance.previousSelectedPlayer = value; }
    }

    protected ActorInstance selectedPlayer
    {
        get { return GameManager.instance.selectedPlayer; }
        set { GameManager.instance.selectedPlayer = value; }
    }
    protected List<ActorInstance> actors
    {
        get => GameManager.instance.actors;
        set => GameManager.instance.actors = value;
    }
    protected bool hasFocusedActor => focusedActor != null;
    protected bool hasSelectedPlayer => selectedPlayer != null;
    protected AudioManager audioManager => GameManager.instance.audioManager;
    protected TimerBarInstance timerBar => GameManager.instance.timerBar;
    protected ActorManager actorManager => GameManager.instance.actorManager;
    protected TileManager tileManager => GameManager.instance.tileManager;






    public void Focus()
    {
        //Verify is player turn...
        if (!turnManager.isPlayerTurn)
            return;

        //Verify currentFps phase is "originActor"...
        if (!turnManager.isStartPhase)
            return;

        //Find collider attached to Actor
        var collisions = Physics2D.OverlapPointAll(mousePosition3D);
        if (collisions == null)
            return;

        var collider = collisions.FirstOrDefault(x => x.CompareTag(Tag.Actor));
        if (collider == null)
            return;

        //GetProfile Actor from collider
        var actor = collider.gameObject.GetComponent<ActorInstance>();
        if (actor == null || !actor.isActive || !actor.isAlive)
            return;

        //TODO: SaveProfile Card display...
        actors.ForEach(x => x.render.SetSelectionBoxEnabled(isEnabled: false));
        focusedActor = actor;
        focusedActor.render.SetSelectionBoxEnabled(isEnabled: true);

        //Assign mouse relativeOffset (how off center was selectionBox)
        mouseOffset = focusedActor.position - mousePosition3D;

        cardManager.Assign(focusedActor);

        if (focusedActor.isPlayer)
            StartCoroutine(focusedActor.move.TowardCursor());
    }

    public void Unfocus()
    {
        //Verify *HAS* focused actor...
        if (!hasFocusedActor)
            return;

        if (!hasSelectedPlayer)
        {
            focusedActor.position = focusedActor.currentTile.position;
            //focusedActor.sortingOrder = SortingOrder.Default;
            //cardManager.DespawnAll();
        }

        focusedActor = null;
    }

    public void Select()
    {
        //Verify is player turn...
        if (!turnManager.isPlayerTurn)
            return;

        //Verify currentFps phase is "originActor"...
        if (!turnManager.isStartPhase)
            return;

        //Verify focused actor exists...
        if (focusedActor == null || focusedActor.isEnemy)
            return;

        //Verify focused actor is player...
        if (focusedActor.isEnemy)
            return;

        //Select player
        selectedPlayer = focusedActor;

        Unfocus();
        turnManager.currentPhase = TurnPhase.Move;
        audioManager.Play("Select");
        timerBar.Play();
        actorManager.CheckEnemyAP();
        StartCoroutine(selectedPlayer.move.TowardCursor());
    }

    public void Unselect()
    {
        //Verify is player turn...
        if (!turnManager.isPlayerTurn)
            return;

        //Verify currentFps phase is "move"...
        if (!turnManager.isMovePhase)
            return;

        //Verify *HAS* selected player...
        if (!hasSelectedPlayer)
            return;

        //Assign boardLocation and boardPosition
        var closestTile = Geometry.GetClosestTile(selectedPlayer.position);
        closestTile.spriteRenderer.color = ColorHelper.Translucent.White;
        selectedPlayer.location = closestTile.location;
        selectedPlayer.position = closestTile.position;
        previousSelectedPlayer = selectedPlayer;
        selectedPlayer = null;

        //ghostManager.Stop();
        //footstepManager.Stop();

        tileManager.Reset();
        cardManager.Reset();
        timerBar.Pause();
        turnManager.currentPhase = TurnPhase.Attack;

        turnManager.CheckPlayerAttack();
    }


}
