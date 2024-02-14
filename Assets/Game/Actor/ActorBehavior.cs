using System.Linq;
using UnityEngine;
using MoveState = ActorMoveState;

public class ActorBehavior : ExtendedMonoBehavior
{
    //Variables
    [SerializeField] public Vector2Int location { get; set; }
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

    public BoxCollider2D boxCollider2D
    {
        get => gameObject.GetComponent<BoxCollider2D>();
        set => boxCollider2D = value;
    }

    public SpriteRenderer spriteRenderer
    {
        get => gameObject.GetComponent<SpriteRenderer>();
        set => spriteRenderer = value;
    }

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

    private bool IsIdle => moveState == MoveState.Moving;
    private bool IsMoving => moveState == MoveState.Moving;
    private bool IsOnPlayerTeam => team == Team.Player;
    private bool IsSelectedPlayer => HasSelectedPlayer && Equals(selectedPlayer);
    private bool HasDirection => destination.direction != Direction.None;
    public bool IsSameColumn(ActorBehavior other) => location.x == other.location.x;
    public bool IsSameRow(ActorBehavior other) => location.y == other.location.y;
    public bool IsNorthOf(ActorBehavior other) => IsSameColumn(other) && location.y == other.location.y + 1;
    public bool IsEastOf(ActorBehavior other) => IsSameRow(other) && location.x == other.location.x + 1;
    public bool IsSouthOf(ActorBehavior other) => IsSameColumn(other) && location.y == other.location.y - 1;
    public bool IsWestOf(ActorBehavior other) => IsSameRow(other) && location.x == other.location.x - 1;

    public bool IsNorthOf(ActorBehavior other, int tileDistance = 1)
    {
        return IsSameColumn(other) && location.y >= other.location.y + tileDistance;
    }

    public bool IsEastOf(ActorBehavior other, int tileDistance = 1)
    {
        return IsSameRow(other) && location.x >= other.location.x + tileDistance;
    }
    public bool IsSouthOf(ActorBehavior other, int tileDistance = 1)
    {
        return IsSameColumn(other) && location.y <= other.location.y - tileDistance;
    }

    public bool IsWestOf(ActorBehavior other, int tileDistance = 1)
    {
        return IsSameRow(other) && location.x <= other.location.x - tileDistance;
    }



    #endregion

    #region Methods

    public void Init(Vector2Int? initialLocation = null)
    {
        if (initialLocation.HasValue)
            location = initialLocation.Value;

        transform.position = Geometry.PositionFromLocation(location);
        transform.localScale = tileScale;
        moveState = MoveState.Idle;
        currentTile.isOccupied = true;
    }

    private void SetDirection(ActorBehavior other)
    {
        if (other == null)
            return;

        if (IsNorthOf(other))
            destination.direction = Direction.South;
        else if (IsEastOf(other))
            destination.direction = Direction.West;
        else if (IsSouthOf(other))
            destination.direction = Direction.North;
        else if (IsWestOf(other))
            destination.direction = Direction.East;
        else
            destination.direction = Direction.None;
    }

    private void MoveUp() => destination.location = location + new Vector2Int(0, 1);

    private void MoveRight() => destination.location = location + new Vector2Int(1, 0);

    private void MoveDown() => destination.location = location + new Vector2Int(0, -1);

    private void MoveLeft() => destination.location = location + new Vector2Int(-1, 0);

    private void SetDestination()
    {
        if (destination.direction == Direction.None)
            return;

        if (destination.direction == Direction.North)
            MoveUp();
        else if (destination.direction == Direction.East)
            MoveRight();
        else if (destination.direction == Direction.South)
            MoveDown();
        else if (destination.direction == Direction.West)
            MoveLeft();

        destination.position = Geometry.PositionFromLocation(destination.location.Value);
        moveState = MoveState.Moving;
    }


    public TileBehavior currentTile => tiles.First(x => x.location.Equals(location));
  

    #endregion

    
    private void Awake()
    {

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
        if (IsSelectedPlayer)
        {
            MoveTowardCursor();
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

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //Ignore selected actor
        if (IsSelectedPlayer)
            return;

        //Ignore actors in motion
        if (IsMoving)
            return;

        //Ignore actors with set direction
        if (HasDirection)
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
        if (IsSelectedPlayer)
            return;

        //Ignore actors in motion
        if (IsMoving)
            return;

        //Ignore actos without direction
        if (!HasDirection)
            return;

        //Determine if two actors collided
        var sender = collider.gameObject.GetComponent<ActorBehavior>();
        if (!sender.CompareTag(Tag.Actor) || !CompareTag(Tag.Actor))
            return;

        SetDestination();
    }

    private void OnTriggerExit2D(Collider2D collider)
    {

    }

    private void MoveTowardCursor()
    {
        if (!IsSelectedPlayer)
            return;

        var cursorPosition = mousePosition3D + mouseOffset;

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
        transform.position
                = Vector2.MoveTowards(selectedPlayer.transform.position,
                                      cursorPosition,
                                      moveSpeed);
    }

    private void MoveTowardDestination()
    {
        //Verify destination *has* been set 
        if (!destination.IsValid)
            return;

        //Verify actor is MoveState: "Moving"
        if (moveState != MoveState.Moving)
            return;

        //SetDestination actor towards destination
        transform.position = Vector2.MoveTowards(transform.position, destination.position.Value, moveSpeed);

        //Determine if actor is close to destination
        bool isCloseToDestination = Vector2.Distance(transform.position, destination.position.Value) < snapDistance;
        if (isCloseToDestination)
        {
            //Snap to destination, clear destination, and set actor MoveState: "Idle"
            currentTile.isOccupied = false;
            location = destination.location.Value;
            currentTile.isOccupied = true;
            tiles.First(t => t.location.Equals(location)).isOccupied = true;
            transform.position = destination.position.Value;
            destination.Clear();
            moveState = MoveState.Idle;
        }
    }

    void OnDrawGizmos()
    {
        //Gizmos.color = Color.green;
        //Gizmos.matrix = transform.localToWorldMatrix;
        //Gizmos.DrawCube(Vector3.zero, Vector3.one);
    }

}
