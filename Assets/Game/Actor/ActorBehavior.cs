using System.Linq;
using UnityEngine;
using MoveState = ActorMoveState;

public class ActorBehavior : ExtendedMonoBehavior
{
    //Variables


    public Vector2Int location { get; set; }

    public Vector3 destination { get; set; }
    public Direction direction { get; set; }
    // public Destination destination { get; set; } = new Destination();



    public MoveState moveState = MoveState.Idle;
    public Team team = Team.Neutral;

    #region Components

    public Transform parent
    {
        get => gameObject.transform.parent;
        set => gameObject.transform.SetParent(value, true);
    }

    public Vector3 position
    {
        get => gameObject.transform.position;
        set => gameObject.transform.position = value;
    }

    public BoxCollider2D boxCollider2D;
    public SpriteRenderer spriteRenderer;
    public TrailRenderer trailRenderer;

    public Sprite sprite
    {
        get => spriteRenderer.sprite;
        set => spriteRenderer.sprite = value;
    }

    public int sortingOrder
    {
        get => spriteRenderer.sortingOrder;
        set => spriteRenderer.sortingOrder = value;
    }

    #endregion

    #region Properties

    private bool IsIdle => this.moveState == MoveState.Idle;
    private bool IsMoving => this.moveState == MoveState.Moving;

    private bool IsOnPlayerTeam => this.team == Team.Player;
    private bool IsSelectedPlayer => HasSelectedPlayer && this.Equals(selectedPlayer);
    private bool HasDirection => this.direction != Direction.None;
    public bool IsSameColumn(ActorBehavior other) => this.location.x == other.location.x;
    public bool IsSameRow(ActorBehavior other) => this.location.y == other.location.y;
    public bool IsNorthOf(ActorBehavior other) => this.IsSameColumn(other) && this.location.y == other.location.y - 1;
    public bool IsEastOf(ActorBehavior other) => this.IsSameRow(other) && this.location.x == other.location.x + 1;
    public bool IsSouthOf(ActorBehavior other) => this.IsSameColumn(other) && this.location.y == other.location.y + 1;
    public bool IsWestOf(ActorBehavior other) => this.IsSameRow(other) && this.location.x == other.location.x - 1;

    //TODO: Make diagonal checks too...
    public bool IsNorthWestOf(ActorBehavior other) => this.location.x == other.location.x - 1 && this.location.y == other.location.y - 1;
    public bool IsNorthEastOf(ActorBehavior other) => this.location.x == other.location.x + 1 && this.location.y == other.location.y - 1;
    public bool IsSouthWestOf(ActorBehavior other) => this.location.x == other.location.x - 1 && this.location.y == other.location.y + 1;
    public bool IsSouthEastOf(ActorBehavior other) => this.location.x == other.location.x + 1 && this.location.y == other.location.y + 1;


    public bool IsNorthEdge => this.location.y == 1;
    public bool IsEastEdge => this.location.x == board.columns;
    public bool IsSouthEdge => this.location.y == board.rows;
    public bool IsWestEdge => this.location.x == 1;


    public bool IsNorthOf(ActorBehavior other, int tileDistance = 1)
    {
        return IsSameColumn(other) && this.location.y >= other.location.y + tileDistance;
    }

    public bool IsEastOf(ActorBehavior other, int tileDistance = 1)
    {
        return IsSameRow(other) && this.location.x >= other.location.x + tileDistance;
    }
    public bool IsSouthOf(ActorBehavior other, int tileDistance = 1)
    {
        return IsSameColumn(other) && this.location.y <= other.location.y - tileDistance;
    }

    public bool IsWestOf(ActorBehavior other, int tileDistance = 1)
    {
        return IsSameRow(other) && this.location.x <= other.location.x - tileDistance;
    }



    #endregion

    #region Methods

    public void Init(Vector2Int? initialLocation = null)
    {
        if (initialLocation.HasValue)
            location = initialLocation.Value;

        this.position = Geometry.PositionFromLocation(location);
        this.transform.localScale = tileScale;
        this.moveState = MoveState.Idle;
        this.spriteRenderer.color = Colors.Solid.White;
    }

    public void SetDestination(ActorBehavior other)
    {
        if (other == null)
            return;

        if (this.HasDirection)
            return;

        if (this.IsNorthWestOf(other))
        {
            if (other.position.y > other.currentTile.position.y)
            {
                this.direction = Direction.South;
            }
            else
            {
                this.direction = !IsEastEdge ? Direction.East : Direction.West;
            }
        }
        else if (this.IsNorthEastOf(other))
        {
            if (other.position.y > other.currentTile.position.y)
            {
                this.direction = Direction.South;
            }
            else
            {
                this.direction = !IsWestEdge ? Direction.West : Direction.East;
            }
        }
        else if (this.IsSouthWestOf(other))
        {
            if (other.position.y < other.currentTile.position.y)
            {
                this.direction = Direction.North;
            }
            else
            {
                this.direction = !IsEastEdge ? Direction.East : Direction.West;
            }
        }
        else if (this.IsSouthEastOf(other))
        {
            if (other.position.y < other.currentTile.position.y)
            {
                this.direction = Direction.North;
            }
            else
            {
                this.direction = !IsWestEdge ? Direction.West : Direction.East;
            }
        }
        else if (this.IsNorthOf(other))
        {
            this.direction = Direction.South;
        }
        else if (this.IsEastOf(other))
        {
            this.direction = Direction.West;
        }
        else if (this.IsSouthOf(other))
        {
            this.direction = Direction.North;
        }
        else if (this.IsWestOf(other))
        {
            this.direction = Direction.East;
        }
        else
        {

            //Actors are on top of eachother
            if (IsNorthEdge)
                this.direction = Direction.South;
            else if (IsEastEdge)
                this.direction = Direction.West;
            else if (IsSouthEdge)
                this.direction = Direction.North;
            else if (IsWestEdge)
                this.direction = Direction.East;
            else
                this.direction = RNG.RandomDirection();
        }

        switch (this.direction)
        {
            case Direction.North:
                this.location = this.location + new Vector2Int(0, -1);
                break;
            case Direction.East:
                this.location = this.location + new Vector2Int(1, 0);
                break;
            case Direction.South:
                this.location = this.location + new Vector2Int(0, 1);
                break;
            case Direction.West:
                this.location = this.location + new Vector2Int(-1, 0);
                break;
        }

        //ActorBehavior conflictActor = actors.FirstOrDefault(x =>
        //{
        //    return !x.Equals(this)
        //    && !x.Equals(selectedPlayer)
        //    && x.location.Equals(this.destination.location.Value);
        //});
        //if (conflictActor != null)
        //{
        //    Debug.Log($"Conflict: {this.name} / {conflictActor.name}");

        //    //conflictActor.SetDirection(this);
        //    //conflictActor.SetDestination();
        //}


        var closestTile = Geometry.ClosestTileByLocation(this.location);
        this.destination = closestTile.position;
        this.moveState = MoveState.Moving;
    }


    public TileBehavior currentTile => tiles.First(x => x.location.Equals(location));


    #endregion


    private void Awake()
    {
        boxCollider2D = gameObject.GetComponent<BoxCollider2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        trailRenderer = gameObject.GetComponent<TrailRenderer>();
        trailRenderer.startWidth = tileSize * percent75;
        trailRenderer.time = percent33;
    }

    private void Start()
    {
        Init();
    }

    void Update()
    {
        if (this.IsSelectedPlayer)
        {
            MoveTowardCursor();
        }
        else
        {
            this.CheckLocation();
        }
        //else if (this.IsMoving)
        //{

        //}
        //else if (this.IsIdle)
        //{

        //}


    }


    public void CheckLocation()
    {
        var other = actors.FirstOrDefault(x => !this.Equals(x) && this.location.Equals(x.location));
        if (other == null)
            return;

        //Assign intended movement
        this.SetDestination(other);
    }

    void FixedUpdate()
    {
        if (this.IsSelectedPlayer)
        {

        }
        else if (this.IsMoving)
        {
            MoveTowardDestination();
        }
        else if (this.IsIdle)
        {


        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {

    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        ////Ignore selected actor
        //if (this.IsSelectedPlayer)
        //    return;

        ////Ignore actors in motion
        //if (this.IsMoving)
        //    return;

        ////Determine if two actors collided
        //if (!collider.gameObject.CompareTag(Tag.Actor))
        //    return;

        //var other = collider.gameObject.GetComponent<ActorBehavior>();
        //if (other.IsSelectedPlayer)
        //    return;

        ////Ignore actos without direction
        //if (!this.HasDirection)
        //{
        //    //Assign intended movement direction
        //    SetDirection(other);
        //}
        //else if (IsTriggerReady(other))
        //{
        //    //Assign intended destination
        //    SetDestination();
        //}

    }


    //private bool IsTriggerReady(ActorBehavior other)
    //{
    //    if (!this.HasDirection)
    //        return false;

    //    switch (this.direction)
    //    {
    //        case Direction.North: return this.IsSameColumn(other) && other.position.y < this.position.y + tileSize / 2;
    //        case Direction.East: return this.IsSameRow(other) && other.position.x < this.position.x + tileSize / 2;
    //        case Direction.South: return this.IsSameColumn(other) && other.position.y > this.position.y - tileSize / 2;
    //        case Direction.West: return this.IsSameRow(other) && other.position.x > this.position.x - tileSize / 2;
    //        default: return false;
    //    }
    //}


    private void OnTriggerExit2D(Collider2D collider)
    {

    }

    private void MoveTowardCursor()
    {
        if (!this.IsSelectedPlayer)
            return;

        var cursorPosition = mousePosition3D + mouseOffset;
        cursorPosition.x = Mathf.Clamp(cursorPosition.x, board.left, board.right);
        cursorPosition.y = Mathf.Clamp(cursorPosition.y, board.bottom, board.top);

        //Move selected player towards cursor
        this.position = Vector2.MoveTowards(selectedPlayer.position, cursorPosition, cursorSpeed);

        //Snap selected player to cursor
        //this.position = cursorPosition;


    }

    private void MoveTowardDestination()
    {
        //Verify actor is MoveState: "Moving"
        if (!this.IsMoving)
            return;

        //Move actor towards destination
        this.position = Vector2.MoveTowards(this.position, this.destination, slideSpeed);

        //Determine if actor is close to destination
        bool isCloseToDestination = Vector2.Distance(this.position, this.destination) < snapDistance;
        if (isCloseToDestination)
        {
            //Snap to destination, clear destination, and set actor MoveState: "Idle"
            this.transform.position = this.destination;
            this.direction = Direction.None;
            this.moveState = MoveState.Idle;
        }
    }

}
