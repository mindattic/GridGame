using Mono.Cecil;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using MoveState = ActorMoveState;

public class ActorManager : ExtendedMonoBehavior
{
    private void Awake()
    {

    }

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            PickupPlayer();
        else if (Input.GetMouseButtonUp(0))
            DropPlayer();

        if (HasSelectedPlayer)
        {
            //Constantly update active actor location
            var closestTile = Geometry.ClosestTileByPosition(selectedPlayer.transform.position);
            selectedPlayer.location = closestTile.location;
        }


    }

    private void FixedUpdate()
    {





    }

    public void PickupPlayer()
    {
        //Only pickup player if no actor is selected
        if (HasSelectedPlayer)
            return;

        //Find actor overlaping mouse position
        Collider2D mouseCollider = Physics2D.OverlapPoint(mousePosition3D);
        if (mouseCollider == null || !mouseCollider.CompareTag(Tag.Actor))
            return;

        //Retrieve actor under mouse cursor
        var actor = mouseCollider.gameObject.GetComponent<ActorBehavior>();
        if (actor == null)
            return;

        //Determine if actor Team: "Player"
        if (actor.team != Team.Player)
            return;

        //Determine if actor MoveState: "Idle"
        if (actor.moveState != MoveState.Idle)
            return;

        //Assign idle actor to active
        selectedPlayer = actor;
        selectedPlayer.moveState = MoveState.Moving;
        selectedPlayer.sortingOrder = 2;

        //Assign mouse offset (how off center was selection)
        mouseOffset = selectedPlayer.transform.position - mousePosition3D;

        //Reduce box collider size (allowing actors some 'wiggle room')
        //actors.ForEach(x => x.boxCollider2D.size = size50);
        //selectedPlayer.boxCollider2D.size = size33;

        timer.Set(scale: 1f, start: true);
    }

    public void DropPlayer()
    {
        //Only drop player if has selected player
        if (!HasSelectedPlayer)
            return;

        //Assign location and position
        var closestTile = Geometry.ClosestTileByPosition(selectedPlayer.transform.position);
        selectedPlayer.location = closestTile.location;
        selectedPlayer.transform.position = Geometry.PositionFromLocation(selectedPlayer.location);
        selectedPlayer.sortingOrder = 1;
        selectedPlayer.moveState = MoveState.Idle;

        //Clear active actor
        selectedPlayer = null;

        //Restore box collider size to 100%
        //actors.ForEach(x => x.boxCollider2D.size = size50);

        timer.Set(scale: 1f, start: false);
    }

}
