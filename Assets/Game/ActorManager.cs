using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using State = ActorState;

public class ActorManager : MonoBehaviorBase
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
    }

    private void FixedUpdate()
    {

    }

    public void PickupPlayer()
    {
        //Only pickup player if no actor has State: "Active"
        if (HasActiveActor)
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
        activeActor = actor;
        activeActor.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
        activeActor.state = State.Active;

        //Assign mouse offset (how off center was selection)
        mouseOffset = activeActor.transform.position - mousePosition3D;

        //Reduce box collider size (allowing actors some 'wiggle room')
        actors.ForEach(x => x.boxCollider2D.size = size50);

        timer.Set(scale: 1f, start: true);
    }

    public void DropPlayer()
    {
        //Only drop player if actor is active
        if (!HasActiveActor)
            return;

        //Assign location and position
        var closestTile = Geometry.ClosestTileByPosition(activeActor.transform.position);
        activeActor.location = closestTile.location;
        activeActor.transform.position = Geometry.PositionFromLocation(activeActor.location);
        activeActor.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
        activeActor.state = State.Idle;

        //Clear active actor
        activeActor = null;

        //Restore box collider size to 100%
        actors.ForEach(x => x.boxCollider2D.size = size100);

        timer.Set(scale: 1f, start: false);
    }

    /// <summary>
    /// Method which is used to retrieve all unoccupied locations
    /// via: https://stackoverflow.com/questions/5620266/the-opposite-of-intersect
    /// </summary>
    /// <returns></returns>
    private List<Vector2Int> GetUnoccupiedLocations()
    {
        List<Vector2Int> occupiedLocations = actors.Select(x => x.location).ToList();
        List<Vector2Int> tileLocations = GameManager.instance.tiles.Select(x => x.location).ToList();
        List<Vector2Int> unoccupiedLocations = occupiedLocations.Except(tileLocations).Union(tileLocations.Except(occupiedLocations)).ToList();
        return unoccupiedLocations;
    }

}
