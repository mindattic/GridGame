using System.Linq;
using UnityEngine;
using MoveState = ActorMoveState;

public class ActorBehavior : ExtendedMonoBehavior
{
    //Variables

    public Vector2Int location { get; set; }

    public MoveState moveState = MoveState.Idle;
    public Destination destination = new Destination();
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
    private bool HasDirection => this.destination.direction != Direction.None;
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
        this.currentTile.isOccupied = true;
    }

    private void SetDirection(ActorBehavior other)
    {
        if (other == null)
            return;

        if (this.IsNorthWestOf(other))
            this.destination.direction = this.location.x < board.columns ? Direction.East : Direction.West;
        else if (this.IsNorthEastOf(other))
            this.destination.direction = this.location.x > 0 ? Direction.West : Direction.East;
        else if (this.IsSouthWestOf(other))
            this.destination.direction = this.location.x > 0 ? Direction.West : Direction.East;
        else if (this.IsSouthEastOf(other))
            this.destination.direction = this.location.x < board.columns ? Direction.East : Direction.West;
        else if (this.IsNorthOf(other))
            this.destination.direction = Direction.South;
        else if (this.IsEastOf(other))
            this.destination.direction = Direction.West;
        else if (this.IsSouthOf(other))
            this.destination.direction = Direction.North;
        else if (this.IsWestOf(other))
            this.destination.direction = Direction.East;
        else
            this.destination.direction = Direction.None;

        if (this.destination.direction == Direction.None)
            return;
    }

    public void SetDestination(Direction forceDirection = Direction.None)
    {
        if (forceDirection != Direction.None)
            this.destination.direction = forceDirection;

        if (this.destination.direction == Direction.None)
            return;

        switch (destination.direction)
        {
            case Direction.North:
                this.destination.location = this.location + new Vector2Int(0, -1);
                break;
            case Direction.East:
                this.destination.location = this.location + new Vector2Int(1, 0);
                break;
            case Direction.South:
                this.destination.location = this.location + new Vector2Int(0, 1);
                break;
            case Direction.West:
                this.destination.location = this.location + new Vector2Int(-1, 0);
                break;
            default: return;
        }


        //DEBUG: Cheap way to avoid conflicts...
        //bool hasLocationConflict = actors.FirstOrDefault(x =>
        //{
        //    return x.IsIdle && (x.location.Equals(this.destination.location));
        //}) != null;

        //if (hasLocationConflict)
        //{
        //    this.destination.Clear();
        //    return;
        //}



        this.destination.position = Geometry.PositionFromLocation(destination.location.Value);
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

    }

    void FixedUpdate()
    {
        if (this.IsSelectedPlayer)
        {
            MoveTowardCursor();
        }
        else if (this.IsMoving)
        {
            MoveTowardDestination();
        }
        else if (this.IsIdle)
        {
            //Do nothing...
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //Ignore selected actor
        if (this.IsSelectedPlayer)
            return;

        //Ignore actors in motion
        if (this.IsMoving)
            return;

        //Ignore actors with set direction
        if (this.HasDirection)
            return;

        //Determine if two actors collided
        var sender = collider.gameObject.GetComponent<ActorBehavior>();
        if (!sender.CompareTag(Tag.Actor) || !CompareTag(Tag.Actor))
            return;

        //Assign intended movement direction
        SetDirection(sender);
    }



    private void OnTriggerStay2D(Collider2D collider)
    {
        //Ignore selected actor
        if (this.IsSelectedPlayer)
            return;

        //Ignore actors in motion
        if (this.IsMoving)
            return;

        //Ignore actos without direction
        if (!this.HasDirection)
            return;

        //Determine if two actors collided
        var sender = collider.gameObject.GetComponent<ActorBehavior>();
        if (!sender.CompareTag(Tag.Actor) || !CompareTag(Tag.Actor))
            return;

        if (IsTriggerReady(sender))
            SetDestination();
    }


    private bool IsTriggerReady(ActorBehavior other)
    {
        if (!this.HasDirection)
            return false;

        switch (destination.direction)
        {
            case Direction.North: return this.IsSameColumn(other) && other.position.y < this.position.y + tileSize / 2;
            case Direction.East: return this.IsSameRow(other) && other.position.x < this.position.x + tileSize / 2;
            case Direction.South: return this.IsSameColumn(other) && other.position.y > this.position.y - tileSize / 2;
            case Direction.West: return this.IsSameRow(other) && other.position.x > this.position.x - tileSize / 2;
            default: return false;
        }
    }



    private void OnTriggerExit2D(Collider2D collider)
    {
 
    }

    private void MoveTowardCursor()
    {
        if (!this.IsSelectedPlayer)
            return;

        var cursorPosition = mousePosition3D + mouseOffset;

        //TODO: use nested Math.Min/Max...
        //Enforce board bounds
        if (cursorPosition.x < board.left)
            cursorPosition.x = board.left;
        else if (cursorPosition.x > board.right)
            cursorPosition.x = board.right;
        if (cursorPosition.y > board.top)
            cursorPosition.y = board.top;
        else if (cursorPosition.y < board.bottom)
            cursorPosition.y = board.bottom;

        //SetDestination active actor towards mouse cursor
        this.position = Vector2.MoveTowards(selectedPlayer.position, cursorPosition, moveSpeed);
    }

    private void MoveTowardDestination()
    {
        //Verify destination *has* been set 
        if (!this.destination.IsValid)
            return;

        //Verify actor is MoveState: "Moving"
        if (!this.IsMoving)
            return;

        //SetDestination actor towards destination
        this.position = Vector2.MoveTowards(this.position, this.destination.position.Value, slideSpeed);

        //Determine if actor is close to destination
        bool isCloseToDestination = Vector2.Distance(this.position, this.destination.position.Value) < snapDistance;
        if (isCloseToDestination)
        {
            //Snap to destination, clear destination, and set actor MoveState: "Idle"
            this.currentTile.isOccupied = false;
            this.location = destination.location.Value;
            this.currentTile.isOccupied = true;
            this.transform.position = destination.position.Value;
            this.destination.Clear();
            this.moveState = MoveState.Idle;
        }
    }

}
