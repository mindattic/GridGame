using Game.Behaviors.Actor;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;
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
        if (actor == null || !actor.IsAlive || !actor.IsActive || !actor.IsPlayer)
            return;

        //TODO: Update Card display...
        actors.ForEach(x => x.Renderers.SetFocus(false));
        focusedPlayer = actor;
        focusedPlayer.sortingOrder = ZAxis.Max;
        focusedPlayer.Renderers.SetFocus(true);
        //Assign mouse offset (how off center was selection)
        mouseOffset = focusedPlayer.position - mousePosition3D;

        cardManager.Set(focusedPlayer);


        //MoveTowardCursor(focusedPlayer);
    }

    public void Unfocus()
    {
        //Verify *HAS* targetted player...
        if (!HasFocusedPlayer)
            return;

        if (!HasSelectedPlayer)
        {
            focusedPlayer.position = focusedPlayer.CurrentTile.position;
            focusedPlayer.sortingOrder = ZAxis.Min;
            //cardManager.Clear();
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
        actors.ForEach(x => x.sortingOrder = ZAxis.Min);
        selectedPlayer.sortingOrder = ZAxis.Max;

        soundSource.PlayOneShot(resourceManager.SoundEffect($"Select"));

        //Clear glowCurve position
        //ResetBobbing();



        

        ghostManager.Start(selectedPlayer);
        footstepManager.Start(selectedPlayer);

        timer.Set(scaleX: 1f, start: true);
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
        var closestTile = Geometry.ClosestTile(selectedPlayer.position);
        closestTile.spriteRenderer.color = Colors.Translucent.White;
        selectedPlayer.location = closestTile.location;
        selectedPlayer.position = closestTile.position;
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
            actor.Renderers.glow.transform.position = actor.position;
            actor.Renderers.thumbnail.transform.position = actor.position;
            actor.Renderers.frame.transform.position = actor.position;
        }
    }

}
