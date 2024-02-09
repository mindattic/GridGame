using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using State = ActorState;

public class ActorBehavior : MonoBehaviorBase
{
    //Variables
    public Vector2Int location { get; set; }
    public State state = State.Idle;
    public Team team = Team.Neutral;
    private Destination destination = new Destination();

    //State Properties
    private bool IsIdle => state == State.Moving;
    private bool IsActive => state == State.Active;
    private bool IsMoving => state == State.Moving;

    private bool IsOnPlayerTeam => team == Team.Player;

    //Component properties
    public BoxCollider2D boxCollider2D => gameObject.GetComponent<BoxCollider2D>();

    public SpriteRenderer spriteRenderer => gameObject.GetComponent<SpriteRenderer>();

    public Sprite sprite
    {
        get => gameObject.GetComponent<SpriteRenderer>().sprite;
        set => gameObject.GetComponent<SpriteRenderer>().sprite = value;
    }

    public Transform parent
    {
        get => gameObject.transform.parent;
        set => gameObject.transform.SetParent(value, true);
    }

    private void Awake()
    {

    }

    private void Start()
    {
        Init();
    }

    public void Init(Vector2Int? initialLocation = null)
    {
        if (initialLocation.HasValue)
            location = initialLocation.Value;

        transform.position = Geometry.PositionFromLocation(location);
        transform.localScale = tileScale;
        state = State.Idle;
    }

    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //    PickupPlayer();
        //else if (Input.GetMouseButtonUp(0))
        //    DropPlayer();

        if (!HasActiveActor)
            return;

        //Assign mouse offset (how off center was selection)
        //mouseOffset = activeActor.transform.position - mousePosition3D;

        //Constantly update active actor location
        var closestTile = Geometry.ClosestTileByPosition(activeActor.transform.position);
        activeActor.location = closestTile.location;
    }


    //public void PickupPlayer()
    //{
    //    //Only pickup player if no actor has State: "Active"
    //    if (HasActiveActor)
    //        return;

    //    //Find actor overlaping mouse position
    //    Collider2D mouseCollider = Physics2D.OverlapPoint(mousePosition3D);
    //    if (mouseCollider == null || !mouseCollider.CompareTag(Tag.Actor))
    //        return;

    //    //Retrieve actor under mouse cursor
    //    var actor = mouseCollider.gameObject.GetComponent<ActorBehavior>();

    //    //Determine if actor Team: "Player" and State: "Idle"
    //    if (actor.team != Team.Player || actor.state != State.Idle)
    //        return;

    //    //Assign idle actor to active
    //    activeActor = actor;
    //    activeActor.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
    //    activeActor.state = State.Active;

    //    //Assign mouse offset (how off center was selection)
    //    mouseOffset = activeActor.transform.position - mousePosition3D;

    //    timer.Set(scale: 1f, start: true);
    //}

    //public void DropPlayer()
    //{
    //    //Only drop player if actor is active
    //    if (!HasActiveActor || !IsActive)
    //        return;

    //    //Restore box collider size to 100%
    //    //activeActor.boxCollider2D.size = size100;
    //    actors.ForEach(x => x.boxCollider2D.size = size100);

    //    //Assign location and position
    //    var closestTile = ClosestTileByPosition(transform.position);
    //    location = closestTile.location;
    //    transform.position = Geometry.PositionFromLocation(location);
    //    gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
    //    state = State.Idle;

    //    //Clear active actor
    //    activeActor = null;

    //    //Reduce box collider size by 50%
    //    //activeActor.boxCollider2D.size = size50;
    //    actors.ForEach(x => x.boxCollider2D.size = size50);

    //    timer.Set(scale: 1f, start: false);
    //}

    void FixedUpdate()
    {
        if (IsActive && IsOnPlayerTeam)
        {
            //Move active actor towards mouse cursor
            MoveTowardCursor();
        }
        else if (IsActive && !IsOnPlayerTeam)
        {
            //Enemies and Neutral actors AI here...

        }
        else if (IsMoving)
        {
            MoveTowardDestination();
        }
        else if (IsIdle)
        {
            //Do nothing...
        }

    }


    private bool IsActorCollision(Collider2D collider)
    {
        var sender = collider.gameObject.GetComponent<ActorBehavior>();
        //var receiver = gameObject.GetComponent<ActorBehavior>();

        //Determine if actor collision
        if (!sender.CompareTag(Tag.Actor) || !CompareTag(Tag.Actor))
            return false;

        //Verify sender is State: "Active"
        //if (sender != activeActor)
        //    return false;

        //Ignore active actor
        //if (state == State.Active)
        //    return false;

        //Ignore actors in motion
        if (state != State.Idle)
            return false;

        return true;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {

        //Determine if active actor collided with an idle actor
        if (!IsActorCollision(collider))
            return;

        //Verify destination has *not* been set 
        if (destination.IsValid)
            return;

        if ((IsSameColumn(location) && IsAbove(location))
            || (IsSameRow(location) && IsRight(location))
            || (IsSameColumn(location) && IsBelow(location))
            || (IsSameRow(location) && IsLeft(location)))
        {
            //Assign destination location and position
            destination.location = activeActor.location;
            destination.position = Geometry.PositionFromLocation(destination.location.Value);

            //var closestTile = ClosestTileByLocation(activeActor.location);
            //destination.position = closestTile.transform.position;
        }

    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        //Determine if active actor collided with an idle actor
        if (!IsActorCollision(collider))
            return;

        //Verify destination *has* been set 
        if (!destination.IsValid)
            return;

        //Assign actor location before movement so occupied tiles can be calculated accurately
        location = destination.location.Value;

        //Assign State: "Moving"; Actor will move towards it's destination
        state = State.Moving;
    }

    private void OnTriggerExit2D(Collider2D collider)
    {

    }

    private void MoveTowardCursor()
    {
        if (!HasActiveActor || !IsActive)
            return;

        var cursorPosition = GameManager.instance.mousePosition3D + mouseOffset;

        //Enforce board bounds
        if (cursorPosition.x < board.left)
            cursorPosition.x = board.left;
        else if (cursorPosition.x > board.right)
            cursorPosition.x = board.right;
        if (cursorPosition.y > board.top)
            cursorPosition.y = board.top;
        else if (cursorPosition.y < board.bottom)
            cursorPosition.y = board.bottom;

        //Move active actor towards mouse cursor
        transform.position
                = Vector2.MoveTowards(activeActor.transform.position,
                                      cursorPosition,
                                      moveSpeed);
    }

    private void MoveTowardDestination()
    {
        //Verify destination *has* been set 
        if (!destination.IsValid)
            return;

        //Verify actor is State: "Moving"
        if (state != State.Moving)
            return;

        //Move actor towards destination
        transform.position = Vector2.MoveTowards(transform.position, destination.position.Value, moveSpeed);

        //Determine if actor is close to destination
        bool isCloseToDestination = Vector2.Distance(transform.position, destination.position.Value) < snapDistance;
        if (isCloseToDestination)
        {
            //Snap to destination, clear destination, and set actor State: "Idle"
            transform.position = destination.position.Value;
            destination.Clear();
            state = State.Idle;
        }
    }



    private void OnCollisionExit2D(Collision2D other)
    {

    }




    void OnDrawGizmos()
    {
        //Gizmos.color = Color.green;
        //Gizmos.matrix = this.transform.localToWorldMatrix;
        //Gizmos.DrawCube(Vector3.zero, Vector3.one);
    }

    ///// <summary>
    ///// Method which is used to retrieve all unoccupied locations
    ///// via: https://stackoverflow.com/questions/5620266/the-opposite-of-intersect
    ///// </summary>
    ///// <returns></returns>
    //private List<Vector2Int> GetUnoccupiedLocations()
    //{
    //    List<Vector2Int> occupiedLocations = actors.Select(x => x.location).ToList();
    //    List<Vector2Int> tileLocations = GameManager.instance.tiles.Select(x => x.location).ToList();
    //    List<Vector2Int> unoccupiedLocations = occupiedLocations.Except(tileLocations).Union(tileLocations.Except(occupiedLocations)).ToList();
    //    return unoccupiedLocations;
    //}

    //private bool IsActorCollision(Collider2D collider)
    //{
    //    var sender = collider.gameObject.GetComponent<ActorBehavior>();
    //    var receiver = gameObject.GetComponent<ActorBehavior>();

    //    //Determine if actor collision
    //    if (!sender.CompareTag(Tag.Actor) || !receiver.CompareTag(Tag.Actor))
    //        return false;

    //    if (!HasActiveActor)
    //        return false;

    //    //Verify sender is State: "Active"
    //    if (sender != activeActor)
    //        return false;

    //    //Ignore actors in motion
    //    if (receiver.state != State.Idle)
    //        return false;

    //    return true;
    //}


    //private bool IsSameColumn(Vector3 a)
    //{
    //    var range = tileSize / 2;
    //    return a.x <= transform.position.x + range && a.x >= transform.position.x - range;
    //}

    //private bool IsSameRow(Vector3 a)
    //{
    //    var range = tileSize / 2;
    //    return a.y <= transform.position.y + range && a.y >= transform.position.y - range;
    //}




    //private bool IsOccupiedTile(Vector2Int location)
    //{
    //    var 
    //    return GameManager.instance.tiles.Where(x => x.location == location).Select(x => x.location).Intersect()
    //}


    //private bool WasCollision(From from)
    //{
    //    var a = activeActor.transform.position;
    //    //var b = transform.position;
    //    var d = destination.Value;
    //    var range = tileSize / 2;

    //    return from switch
    //    {
    //        From.Above => a.y < d.y - range && IsSameColumn(a),
    //        From.Right => a.x < d.x - range && IsSameRow(a),
    //        From.Below => a.y > d.y + range && IsSameColumn(a),
    //        From.Left => a.x > d.x + range && IsSameRow(a),
    //        _ => false,
    //    };
    //}

    //private bool WasTriggered(From from)
    //{
    //    return from switch
    //    {
    //        From.Above => activeActor.location.x == location.x && activeActor.location.y == location.y - 1,
    //        From.Right => activeActor.location.y == location.y && activeActor.location.x == location.x + 1,
    //        From.Below => activeActor.location.x == location.x && activeActor.location.y == location.y + 1,
    //        From.Left => activeActor.location.y == location.y && activeActor.location.x == location.x - 1,
    //        _ => false,
    //    };
    //}

    //private TileBehavior GetTile(Vector2Int movement)
    //{
    //    var gridPoint = new Vector2Int(location.x + movement.x, location.y + movement.y);
    //    return GameManager.instance.tiles.FirstOrDefault(x => x.location == gridPoint);
    //}


    //private TileBehavior GetTile(Vector2Int offset)
    //{
    //    Debug.Log($"{Time.time} | offset: {(location + offset)}");
    //    var tile = GameManager.instance.tiles.FirstOrDefault(x => x.location == (location + offset));


    //    return tile;
    //}


    //private TileBehavior GetTile(From from)
    //{
    //    switch (from)
    //    {
    //        case From.Above:
    //            return GameManager.instance.tiles.FirstOrDefault(x => x.location == (location + Vector3Direction.Up));
    //        case From.Right:
    //            return GameManager.instance.tiles.FirstOrDefault(x => x.location == (location + Vector3Direction.Right));
    //        case From.Below:
    //            return GameManager.instance.tiles.FirstOrDefault(x => x.location == (location + Vector3Direction.Down));
    //        case From.Left:
    //            return GameManager.instance.tiles.FirstOrDefault(x => x.location == (location + Vector3Direction.Left));
    //        default: return null;
    //    }
    //}


    //private TileBehavior GetTileAbove(ActorBehavior sender)
    //{
    //    return GameManager.instance.tiles.FirstOrDefault(x => x.location == sender.location + new Vector2Int(0, -1));
    //}

    //private TileBehavior GetTileBelow(ActorBehavior sender)
    //{
    //    return GameManager.instance.tiles.FirstOrDefault(x => x.location == sender.location + new Vector2Int(0, 1));
    //}

    //private TileBehavior GetTileRight(ActorBehavior sender)
    //{
    //    return GameManager.instance.tiles.FirstOrDefault(x => x.location == sender.location + new Vector2Int(1, 0));
    //}

    //private TileBehavior GetTileLeft(ActorBehavior sender)
    //{
    //    return GameManager.instance.tiles.FirstOrDefault(x => x.location == sender.location + new Vector2Int(-1, 0));
    //}



    //private bool CollisionFromAbove(GameObject sender, GameObject receiver)
    //{
    //    return sender.transform.position.y > receiver.transform.position.y
    //        && sender.transform.position.x <= receiver.transform.position.x + tileSize / 2
    //        && sender.transform.position.x >= receiver.transform.position.x - tileSize / 2;
    //}

    //private bool CollisionFromBelow(GameObject sender, GameObject receiver)
    //{
    //    return sender.transform.position.y < receiver.transform.position.y
    //        && sender.transform.position.x <= receiver.transform.position.x + tileSize / 2
    //        && sender.transform.position.x >= receiver.transform.position.x - tileSize / 2;
    //}

    //private bool CollisionFromRight(GameObject sender, GameObject receiver)
    //{
    //    return sender.transform.position.x > receiver.transform.position.x
    //        && sender.transform.position.y <= receiver.transform.position.y + tileSize / 2
    //        && sender.transform.position.y >= receiver.transform.position.y - tileSize / 2;
    //}

    //private bool CollisionFromLeft(GameObject sender, GameObject receiver)
    //{
    //    return sender.transform.position.x < receiver.transform.position.x
    //        && sender.transform.position.y <= receiver.transform.position.y + tileSize / 2
    //        && sender.transform.position.y >= receiver.transform.position.y - tileSize / 2;
    //}























}
