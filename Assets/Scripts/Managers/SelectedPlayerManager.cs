using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class SelectedPlayerManager : ExtendedMonoBehavior
{
    // Start is called before the first Frame update
    void Start()
    {

    }

    // Update is called once per Frame
    void Update()
    {

    }


    public void Select()
    {
        //Verify is player turn...
        if (!TurnManager.IsPlayerTurn)
            return;

        //Verify Current phase is "PointA"...
        if (!TurnManager.IsStartPhase)
            return;

        //Find collider attached to Actor
        var collisions = Physics2D.OverlapPointAll(MousePosition3D);
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
        Actors.ForEach(x => x.Renderers.SetFocus(false));
        FocusedPlayer = actor;
        FocusedPlayer.sortingOrder = ZAxis.Max;
        FocusedPlayer.Renderers.SetFocus(true);
        //Assign mouse Offset (how off center was selection)
        MouseOffset = FocusedPlayer.position - MousePosition3D;

        CardManager.Set(FocusedPlayer);
    }

    public void Unselect()
    {
        //Verify *HAS* targetted player...
        if (!HasFocusedPlayer)
            return;

        if (!HasSelectedPlayer)
        {
            FocusedPlayer.position = FocusedPlayer.currentTile.position;
            FocusedPlayer.sortingOrder = ZAxis.Min;
            //CardManager.Clear();
        }

        FocusedPlayer = null;
     
    }

    public void Pickup()
    {
        //Verify is player turn...
        if (!TurnManager.IsPlayerTurn)
            return;

        //Verify Current phase is "PointA"...
        if (!TurnManager.IsStartPhase)
            return;

        //Pickup Actor
        SelectedPlayer = FocusedPlayer;
        if (!HasSelectedPlayer)
            return;

        Unselect();
        TurnManager.currentPhase = TurnPhase.Move;
        SelectedPlayer.sortingOrder = ZAxis.Max;

        SoundSource.PlayOneShot(ResourceManager.SoundEffect($"Select"));

        //Clear BobbingCurve Position
        //ResetBobbing();



        //Assign mouse Offset (how off center was selection)
        MouseOffset = SelectedPlayer.position - MousePosition3D;

        
        GhostManager.Start(SelectedPlayer);
        FootstepManager.Start(SelectedPlayer);

        Timer.Set(scaleX: 1f, start: true);
    }

    public void Drop()
    {
        //Verify is player turn...
        if (!TurnManager.IsPlayerTurn)
            return;

        //Verify Current phase is "move"...
        if (!TurnManager.IsMovePhase)
            return;

        //Verify *HAS* selected player...
        if (!HasSelectedPlayer)
            return;

        //Assign Location and Position
        var closestTile = Geometry.ClosestTileByPosition(SelectedPlayer.position);
        closestTile.spriteRenderer.color = Colors.Translucent.White;
        SelectedPlayer.Location = closestTile.Location;
        SelectedPlayer.position = Geometry.PositionFromLocation(SelectedPlayer.Location);
        SelectedPlayer.sortingOrder = ZAxis.Min;
        SelectedPlayer.SetStatus(Status.None);
        SelectedPlayer = null;

        GhostManager.Stop();
        FootstepManager.Stop();

        TileManager.Reset();
        CardManager.Clear();
        Timer.Set(scaleX: 0f, start: false);
        TurnManager.currentPhase = TurnPhase.Attack;
        ActorManager.CheckPlayerAttack();
    }

    private void ResetBobbing()
    {
        foreach (var actor in Actors)
        {
            if (actor == null || !actor.IsAlive || !actor.IsActive) continue;
            actor.Renderers.Glow.transform.position = actor.position;
            actor.Renderers.Thumbnail.transform.position = actor.position;
            actor.Renderers.Frame.transform.position = actor.position;
        }
    }

}
