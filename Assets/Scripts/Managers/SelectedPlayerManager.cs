using System.Linq;
using UnityEngine;

public class SelectedPlayerManager : ExtendedMonoBehavior
{
    // Play is called before the first frame update
    void Start()
    {

    }

    // SaveProfile is called once per frame
    void Update()
    {

    }


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
        if (actor == null || !actor.IsPlaying)
            return;

        //TODO: SaveProfile Card display...
        actors.ForEach(x => x.renderers.SetSelectionBoxEnabled(isEnabled: false));
        focusedActor = actor;
        focusedActor.sortingOrder = SortingOrder.Max;
        focusedActor.renderers.SetSelectionBoxEnabled(isEnabled: true);

        //Assign mouse relativeOffset (how off center was selectionBox)
        mouseOffset = focusedActor.position - mousePosition3D;

        cardManager.Set(focusedActor);

        if (focusedActor.IsPlayer)
            StartCoroutine(focusedActor.MoveTowardCursor());
    }

    public void Unfocus()
    {
        //Verify *HAS* focused actor...
        if (!HasFocusedActor)
            return;

        if (!HasSelectedPlayer)
        {
            focusedActor.position = focusedActor.currentTile.position;
            focusedActor.sortingOrder = SortingOrder.Default;
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
        actors.ForEach(x => x.sortingOrder = SortingOrder.Default);
        selectedPlayer.sortingOrder = SortingOrder.Max;

        audioManager.Play("Select");

        //DespawnAll glowCurve boardPosition
        //ResetBobbing();





        //ghostManager.Play(selectedPlayer);
        //footstepManager.Play(selectedPlayer);

        timerBar.Play();

        actorManager.CheckAccumulateAP();
        StartCoroutine(selectedPlayer.MoveTowardCursor());

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
        closestTile.spriteRenderer.color = Colors.Translucent.White;
        selectedPlayer.location = closestTile.location;
        selectedPlayer.position = closestTile.position;
        selectedPlayer.sortingOrder = SortingOrder.Default;
        //selectedPlayer.SetStatus(Status.None);
        previousSelectedPlayer = selectedPlayer;
        selectedPlayer = null;

        //ghostManager.Stop();
        //footstepManager.Stop();

        tileManager.Reset();
        cardManager.Clear();
        timerBar.Pause();
        turnManager.currentPhase = TurnPhase.Attack;

        turnManager.CheckPlayerAttack();
    }


    private void ResetBobbing()
    {
        foreach (var actor in actors)
        {
            if (actor == null || !actor.IsAlive || !actor.IsActive) continue;
            actor.renderers.glow.transform.position = actor.position;
            actor.renderers.thumbnail.transform.position = actor.position;
            actor.renderers.frame.transform.position = actor.position;
        }
    }

}
