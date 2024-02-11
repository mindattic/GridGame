using UnityEngine;
using State = ActorState;

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
        //Only pickup player if no actor has State: "Active"
        if (HasSelectedPlayer)
            return;

        //Find actor overlaping mouse position
        Collider2D mouseCollider = Physics2D.OverlapPoint(mousePosition3D);
        if (mouseCollider == null || !mouseCollider.CompareTag(Tag.Actor))
            return;

        //Retrieve actor under mouse cursor
        var actor = mouseCollider.gameObject.GetComponent<ActorBehavior>();

        //Determine if actor Team: "Player" and State: "Idle"
        if (actor.team != Team.Player || actor.state != State.Idle)
            return;

        //Assign idle actor to active
        selectedPlayer = actor;
        selectedPlayer.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
       
        //Assign mouse offset (how off center was selection)
        mouseOffset = selectedPlayer.transform.position - mousePosition3D;

        //Reduce box collider size (allowing actors some 'wiggle room')
        //actors.ForEach(x => x.boxCollider2D.size = size50);

        timer.Set(scale: 1f, start: true);
    }

    public void DropPlayer()
    {
        //Only drop player if actor is active
        if (!HasSelectedPlayer)
            return;

        //Assign location and position
        var closestTile = Geometry.ClosestTileByPosition(selectedPlayer.transform.position);
        selectedPlayer.location = closestTile.location;
        selectedPlayer.transform.position = Geometry.PositionFromLocation(selectedPlayer.location);
        selectedPlayer.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
        selectedPlayer.state = State.Idle;

        //Clear active actor
        selectedPlayer = null;

        //Restore box collider size to 100%
        //actors.ForEach(x => x.boxCollider2D.size = size100);

        timer.Set(scale: 1f, start: false);
    }
}
