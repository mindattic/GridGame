using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using State = ActorState;

public class ActorBehavior : MonoBehaviour
{
    //Variables
    [field: SerializeField] public Vector2Int location { get; set; }
    [field: SerializeField] public State state = State.Idle;
    [field: SerializeField] public Team team = Team.Neutral;




    private Vector3? destination = null;

   
    private float tileSize => GameManager.instance.tileSize;
    private Vector2 size50 => GameManager.instance.size50;
    private Vector2 size100 => GameManager.instance.size100;
    private Vector3 mousePosition3D => GameManager.instance.mousePosition3D;
    private List<ActorBehavior> actors => GameManager.instance.actors;
    private float moveSpeed => GameManager.instance.followSpeed;
    private float snapDistance => GameManager.instance.snapDistance;

    private Vector3 mouseOffset
    {
        get { return GameManager.instance.mouseOffset; }
        set { GameManager.instance.mouseOffset = value; }
    }

    //State Properties
    private bool IsIdle => state == State.Moving;
    private bool IsActive => state == State.Active;
    private bool IsMoving => state == State.Moving;

    //Actor Properties
    private ActorBehavior activeActor
    {
        get { return GameManager.instance.activeActor; }
        set { GameManager.instance.activeActor = value; }
    }
    private bool HasActiveActor => activeActor != null;
    private bool IsSameColumn => HasActiveActor && activeActor.location.x == location.x;
    private bool IsSameRow => HasActiveActor && activeActor.location.y == location.y;
    private bool IsAbove => activeActor.location.y == location.y - 1;
    private bool IsRight => activeActor.location.x == location.x + 1;
    private bool IsBelow => activeActor.location.y == location.y + 1;
    private bool IsLeft => activeActor.location.x == location.x - 1;



    private bool IsOnPlayerTeam => team == Team.Player;

    //Component properties
    public BoxCollider2D boxCollider2D
    {
        get => gameObject.GetComponent<BoxCollider2D>();
        set => gameObject.GetComponent<BoxCollider2D>();
    }

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
        transform.position = Geometry.PositionFromLocation(location);
        transform.localScale = GameManager.instance.tileScale;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            PickupPlayer();
        else if (Input.GetMouseButtonUp(0))
            DropPlayer();


        if (HasActiveActor)
            activeActor.location = ClosestTile(activeActor.transform.position).location;
    }


    private void PickupPlayer()
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
        activeActor.state = State.Active;
        mouseOffset = activeActor.transform.position - mousePosition3D;

        //Reduce box collider size by 50%
        //activeActor.boxCollider2D.size = size50;
        actors.ForEach(x => x.boxCollider2D.size = size50);
    }

    private void DropPlayer()
    {
        //Only drop player if actor is active
        if (!HasActiveActor || !IsActive)
            return;

        //Restore box collider size to 100%
        //activeActor.boxCollider2D.size = size100;
        actors.ForEach(x => x.boxCollider2D.size = size100);

        //Assign location and position
        var closestTile = ClosestTile(transform.position);
        location = closestTile.location;
        transform.position = Geometry.PositionFromLocation(location);

        state = State.Idle;

        //Clear active actor
        activeActor = null;
    }

    void FixedUpdate()
    {
        if (IsActive && IsOnPlayerTeam)
        {
            //Move active actor towards mouse cursor
            MoveToCursor();
        }
        else if (IsActive && !IsOnPlayerTeam)
        {
            //Enemies and Neutral actors AI here...

        }
        else if (IsMoving)
        {
            MoveToDestination();
        }
        else if (IsIdle)
        {
            //Do nothing...
        }

    }


    private bool IsPlayerCollision(Collider2D collider)
    {
        var sender = collider.gameObject.GetComponent<ActorBehavior>();
        //var receiver = gameObject.GetComponent<ActorBehavior>();

        //Determine if actor collision
        if (!sender.CompareTag(Tag.Actor) || !CompareTag(Tag.Actor))
            return false;

        //Verify sender is State: "Active"
        if (sender != activeActor)
            return false;

        //Ignore active actor
        if (state == State.Active)
            return false;

        //Ignore actors in motion
        if (state != State.Idle)
            return false;

        return true;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {

        //Determine if active actor collided with an idle actor
        if (!IsPlayerCollision(collider))
            return;

        //Verify destination has *not* been set 
        if (destination.HasValue)
            return;

        if ((IsSameColumn && IsAbove) 
            || (IsSameRow && IsRight) 
            || (IsSameColumn && IsBelow) 
            || (IsSameRow && IsLeft))
        {
            destination = Geometry.PositionFromLocation(activeActor.location);

            //var closestTile = ClosestTile(activeActor.location);
            //destination = closestTile.transform.position;
        }
          
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        //Determine if active actor collided with an idle actor
        if (!IsPlayerCollision(collider))
            return;

        //Verify destination *has* been set 
        if (!destination.HasValue)
            return;

        state = State.Moving;
    }


    private void OnTriggerExit2D(Collider2D collider)
    {

    }

    private void MoveToCursor()
    {
        if (!HasActiveActor || !IsActive)
            return;

        activeActor.transform.position
                = Vector2.MoveTowards(activeActor.transform.position,
                                      GameManager.instance.mousePosition3D + mouseOffset,
                                      moveSpeed);
    }

    private void MoveToDestination()
    {
        if (!destination.HasValue)
            return;

        if (state != State.Moving)
            return;

        //Move actor towards destination
        transform.position = Vector2.MoveTowards(transform.position, destination.Value, moveSpeed);

        //Determine if actor is close to destination
        bool isCloseToDestination = Vector2.Distance(transform.position, destination.Value) < snapDistance;

        //Snap to destination
        if (isCloseToDestination)
        {
            transform.position = destination.Value;
            location = ClosestTile(transform.position).location;
            destination = null;
            state = State.Idle;
        }
    }


    private TileBehavior ClosestTile(Vector2 position)
    {
        //TODO: Determine if tile is occupied...
        return GameManager.instance.tiles
            .OrderBy(x => Vector3.Distance(x.transform.position, position))
            .First();
    }

    private TileBehavior ClosestTile(Vector2Int location)
    {
        //TODO: Determine if tile is occupied...
        return GameManager.instance.tiles
            .First(x => x.location == location);
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
