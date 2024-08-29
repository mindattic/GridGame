using System.Linq;
using UnityEngine;

public class SelectedPlayerManager : ExtendedMonoBehavior
{
    // Play is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void Focus()
    {
        //Verify is player turn...
        if (!turnManager.IsPlayerTurn)
            return;

        //Verify Current phase is "start"...
        if (!turnManager.IsStartPhase)
            return;

        //Find collider attached to Actor
        var collisions = Physics2D.OverlapPointAll(mousePosition3D);
        if (collisions == null)
            return;

        var collider = collisions.FirstOrDefault(x => x.CompareTag(Tag.Actor));
        if (collider == null)
            return;

        //Retrieve Actor from collider
        var actor = collider.gameObject.GetComponent<ActorBehavior>();
        if (actor == null || !actor.IsPlaying || !actor.IsPlayer)
            return;

        //TODO: Update Card display...
        actors.ForEach(x => x.renderers.SetSelectionActive(false));
        focusedPlayer = actor;
        focusedPlayer.sortingOrder = SortingOrder.Max;
        focusedPlayer.renderers.SetSelectionActive(true);

        //Assign mouse offset (how off center was selection)
        mouseOffset = focusedPlayer.position - mousePosition3D;

        cardManager.Set(focusedPlayer);


        StartCoroutine(focusedPlayer.MoveTowardCursor());
        //MoveTowardCursor(focusedPlayer);
    }

    public void Unfocus()
    {
        //Verify *HAS* targetted player...
        if (!HasFocusedPlayer)
            return;

        if (!HasSelectedPlayer)
        {
            focusedPlayer.position = focusedPlayer.currentTile.position;
            focusedPlayer.sortingOrder = SortingOrder.Min;
            //cardManager.DespawnAll();
        }

        focusedPlayer = null;

    }

    public void Select()
    {
        //Verify is player turn...
        if (!turnManager.IsPlayerTurn)
            return;

        //Verify Current phase is "start"...
        if (!turnManager.IsStartPhase)
            return;

        //Verify focused player exists...
        if (focusedPlayer == null)
            return;


        //Select player
        selectedPlayer = focusedPlayer;

        Unfocus();
        turnManager.currentPhase = TurnPhase.Move;
        actors.ForEach(x => x.sortingOrder = SortingOrder.Default);
        selectedPlayer.sortingOrder = SortingOrder.Max;

        audioManager.Play("Select");

        //DespawnAll glowCurve position
        //ResetBobbing();





        ghostManager.Play(selectedPlayer);
        footstepManager.Play(selectedPlayer);

        timer.Restart();


        StartCoroutine(selectedPlayer.MoveTowardCursor());
    }

    public void Unselect()
    {
        //Verify is player turn...
        if (!turnManager.IsPlayerTurn)
            return;

        //Verify Current phase is "move"...
        if (!turnManager.IsMovePhase)
            return;

        //Verify *HAS* selected player...
        if (!HasSelectedPlayer)
            return;

        //Assign location and position
        var closestTile = Geometry.GetClosestTileByPosition(selectedPlayer.position);
        closestTile.spriteRenderer.color = Colors.Translucent.White;
        selectedPlayer.location = closestTile.location;
        selectedPlayer.position = closestTile.position;
        selectedPlayer.sortingOrder = SortingOrder.Min;
        selectedPlayer.SetStatus(Status.None);
        selectedPlayer = null;

        ghostManager.Stop();
        footstepManager.Stop();

        tileManager.Reset();
        cardManager.Clear();
        timer.Empty();
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
