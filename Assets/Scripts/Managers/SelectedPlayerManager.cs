using System.Linq;
using UnityEngine;

public class SelectedPlayerManager : ExtendedMonoBehavior
{

    public void Focus()
    {
        //Verify is player turn...
        if (!turnManager.IsPlayerTurn)
            return;

        //Verify currentFps phase is "originActor"...
        if (!turnManager.IsStartPhase)
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
        if (actor == null || !actor.IsActive || !actor.IsAlive)
            return;

        //TODO: SaveProfile Card display...
        actors.ForEach(x => x.render.SetSelectionBoxEnabled(isEnabled: false));
        focusedActor = actor;
        //focusedActor.sortingOrder = SortingOrder.Max;
        focusedActor.render.SetSelectionBoxEnabled(isEnabled: true);

        //Assign mouse relativeOffset (how off center was selectionBox)
        mouseOffset = focusedActor.position - mousePosition3D;

        cardManager.Assign(focusedActor);

        if (focusedActor.IsPlayer)
            StartCoroutine(focusedActor.move.TowardCursor());
    }

    public void Unfocus()
    {
        //Verify *HAS* focused actor...
        if (!HasFocusedActor)
            return;

        if (!HasSelectedPlayer)
        {
            focusedActor.position = focusedActor.CurrentTile.position;
            //focusedActor.sortingOrder = SortingOrder.Default;
            //cardManager.DespawnAll();
        }

        focusedActor = null;
    }

    public void Select()
    {
        //Verify is player turn...
        if (!turnManager.IsPlayerTurn)
            return;

        //Verify currentFps phase is "originActor"...
        if (!turnManager.IsStartPhase)
            return;

        //Verify focused actor exists...
        if (focusedActor == null || focusedActor.IsEnemy)
            return;

        //Verify focused actor is player...
        if (focusedActor.IsEnemy)
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
        if (!turnManager.IsPlayerTurn)
            return;

        //Verify currentFps phase is "move"...
        if (!turnManager.IsMovePhase)
            return;

        //Verify *HAS* selected player...
        if (!HasSelectedPlayer)
            return;

        //Assign boardLocation and boardPosition
        var closestTile = Geometry.GetClosestTile(selectedPlayer.position);
        closestTile.spriteRenderer.color = ColorHelper.Translucent.White;
        selectedPlayer.location = closestTile.location;
        selectedPlayer.position = closestTile.position;
        //selectedPlayer.sortingOrder = SortingOrder.Default;
        //selectedPlayer.SetStatus(Status.None);
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
