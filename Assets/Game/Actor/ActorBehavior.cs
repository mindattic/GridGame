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
    //private TileBehavior targetTile = null;
    private Vector3? destination = null;



    //GameManager properties

    private float tileSize => GameManager.instance.tileSize;
    private Vector2 size50 => GameManager.instance.size50;
    private Vector2 size100 => GameManager.instance.size100;
    private Vector3 mousePosition3D => GameManager.instance.mousePosition3D;
    private List<ActorBehavior> actors => GameManager.instance.actors;
    private float followSpeed => GameManager.instance.followSpeed;
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
    private bool IsActiveActor => HasActiveActor && string.Equals(activeActor.name, name);

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
        transform.position = Geometry.PointFromGrid(location);
        transform.localScale = GameManager.instance.tileScale;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            PickupPlayer();
        else if (Input.GetMouseButtonUp(0))
            DropPlayer();
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

        //Reduce active actor box collider size by 50% to help it slip between other actors
        activeActor.boxCollider2D.size = size50;
    }

    private void DropPlayer()
    {
        //Only drop player if actor is active
        if (!HasActiveActor || !IsActiveActor)
            return;

        //Restore active actor box collider size to 100%
        activeActor.boxCollider2D.size = size100;

        //Find tile closest to active actor
        var closestTile = ClosestTile(activeActor.transform.position);

        //Assign actor new current location/position and drop
        activeActor.location = closestTile.location;
        activeActor.transform.position = Geometry.PointFromGrid(activeActor.location);
        activeActor.state = State.Idle;

        //Clear active actor
        activeActor = null;
    }

    void FixedUpdate()
    {
        if (IsActive && IsOnPlayerTeam)
        {
            //Move active actor towards mouse cursor
            FollowMouse();
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



    private void OnTriggerEnter2D(Collider2D collider)
    {
        //Determine if two actors collided
        if (!IsActorCollision(collider))
            return;

        //Verify a destination has *not* been set 
        if (destination.HasValue)
            return;

        //Determine target tile
        if (IsCollision(From.Above))
            destination = transform.position + Direction.Up;
        else if (IsCollision(From.Right))
            destination = transform.position + Direction.Right;
        else if (IsCollision(From.Below))
            destination = transform.position + Direction.Down;
        else if (IsCollision(From.Left))
            destination = transform.position + Direction.Left;
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        //Determine if two actors collided
        if (!IsActorCollision(collider))
            return;

        //Verify a destination *has* been set 
        if (!destination.HasValue)
            return;

        var a = activeActor.transform.position;
        var b = destination.Value;
        var range = tileSize / 2;

        //Determine if active actor has moved amount neccessary
        bool wasAbove = a.y >= b.y + range && (a.x <= b.x + range && a.x >= b.x - range);
        bool wasRight = a.x < b.x - range && (a.y <= b.y + range && a.y >= b.y - range);
        bool wasBelow = a.y < b.y - range && (a.x <= b.x + range && a.x >= b.x - range);
        bool wasLeft = a.x >= b.x + range && (a.y <= b.y + range && a.y >= b.y - range);

        if (!wasAbove && !wasRight && !wasBelow && !wasLeft)
            return;

        state = State.Moving;
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        
    }



    private void FollowMouse()
    {
        if (!HasActiveActor || !IsActive)
            return;

        activeActor.transform.position
                = Vector2.MoveTowards(activeActor.transform.position,
                                      GameManager.instance.mousePosition3D + mouseOffset,
                                      followSpeed);
    }

    private void MoveToDestination()
    {
        if (!destination.HasValue)
            return;

        //Move actor towards destination
        transform.position = Vector2.MoveTowards(transform.position, destination.Value, followSpeed);

        //Determine if actor is close to destination
        bool isCloseToDestination = Vector2.Distance(transform.position, destination.Value) < snapDistance;

        //Snap to destination
        if (isCloseToDestination)
        {
            transform.position = destination.Value;
            location = Geometry.GridFromPoint(transform.position);
            destination = null;
            state = State.Idle;
        }
    }


    private TileBehavior ClosestTile(Vector2 position)
    {
        //TODO: Determine if tile is occupied...
        return GameManager.instance.tiles
            .OrderBy(x => Vector3.Distance(x.transform.position, position))
            .First()
            .GetComponent<TileBehavior>();
    }

    private bool IsActorCollision(Collider2D collider)
    {
        var sender = collider.gameObject.GetComponent<ActorBehavior>();
        var receiver = gameObject.GetComponent<ActorBehavior>();

        //Determine if actor collision
        if (!sender.CompareTag(Tag.Actor) || !receiver.CompareTag(Tag.Actor))
            return false;

        if (!HasActiveActor)
            return false;

        //Verify sender is State: "Active"
        if (sender != activeActor)
            return false;

        //Ignore actors in motion
        if (receiver.state != State.Idle)
            return false;

        return true;
    }

    private bool IsCollision(From from)
    {
        var a = activeActor.transform.position;
        var b = transform.position;
        var range = tileSize / 2;

        return from switch
        {
            From.Above => a.y > b.y && (a.x <= b.x + range && a.x >= b.x - range),
            From.Right => a.x > b.x && (a.y <= b.y + range && a.y >= b.y - range),
            From.Below => a.y < b.y && (a.x <= b.x + range && a.x >= b.x - range),
            From.Left => a.x < b.x && (a.y <= b.y + range && a.y >= b.y - range),
            _ => false,
        };
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
    //            return GameManager.instance.tiles.FirstOrDefault(x => x.location == (location + Direction.Up));
    //        case From.Right:
    //            return GameManager.instance.tiles.FirstOrDefault(x => x.location == (location + Direction.Right));
    //        case From.Below:
    //            return GameManager.instance.tiles.FirstOrDefault(x => x.location == (location + Direction.Down));
    //        case From.Left:
    //            return GameManager.instance.tiles.FirstOrDefault(x => x.location == (location + Direction.Left));
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
