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

    }


    public void Select()
    {
        //Verify is player turn...
        if (!turnManager.IsPlayerTurn)
            return;

        //Verify Current phase is "PointA"...
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
        if (actor == null || !actor.IsAlive || !actor.IsActive || !actor.IsPlayer)
            return;

        //TODO: Update Card display...
        actors.ForEach(x => x.Renderers.SetFocus(false));
        focusedPlayer = actor;
        focusedPlayer.SortingOrder = ZAxis.Max;
        focusedPlayer.Renderers.SetFocus(true);
        //Assign mouse cornerOffset (how off center was selection)
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
            focusedPlayer.position = focusedPlayer.CurrentTile.position;
            focusedPlayer.SortingOrder = ZAxis.Min;
            //cardManager.Clear();
        }

        focusedPlayer = null;

    }

    public void Pickup()
    {
        //Verify is player turn...
        if (!turnManager.IsPlayerTurn)
            return;

        //Verify Current phase is "PointA"...
        if (!turnManager.IsStartPhase)
            return;

        //Pickup Actor
        selectedPlayer = focusedPlayer;
        if (!HasSelectedPlayer)
            return;

        Unselect();
        turnManager.currentPhase = TurnPhase.Move;
        selectedPlayer.SortingOrder = ZAxis.Max;

        soundSource.PlayOneShot(resourceManager.SoundEffect($"Select"));

        //Clear bobbingCurve position
        //ResetBobbing();



        //Assign mouse cornerOffset (how off center was selection)
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

        //Verify Current phase is "move"...
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
        selectedPlayer.SortingOrder = ZAxis.Min;
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
            actor.Renderers.glow.transform.position = actor.position;
            actor.Renderers.thumbnail.transform.position = actor.position;
            actor.Renderers.frame.transform.position = actor.position;
        }
    }

}
