using UnityEngine;
using MoveState = ActorMoveState;

public class ActorBehavior : ExtendedMonoBehavior
{
    //Variables
    public Vector2Int location { get; set; }
    public MoveState moveState = MoveState.Idle;
    public Destination destination = new Destination();
    public Team team = Team.Neutral;

    #region Properties

    private bool IsIdle => moveState == MoveState.Moving;
    private bool IsMoving => moveState == MoveState.Moving;
    private bool IsOnPlayerTeam => team == Team.Player;
    private bool IsSelectedPlayer => HasSelectedPlayer && Equals(selectedPlayer);
    private bool HasDirection => destination.direction != Direction.None;
    private bool IsSameColumn(ActorBehavior other) => location.x == other.location.x;
    private bool IsSameRow(ActorBehavior other) => location.y == other.location.y;
    private bool IsAbove(ActorBehavior other) => IsSameColumn(other) && location.y == other.location.y + 1;
    private bool IsRightOf(ActorBehavior other) => IsSameRow(other) && location.x == other.location.x + 1;
    private bool IsBelow(ActorBehavior other) => IsSameColumn(other) && location.y == other.location.y - 1;
    private bool IsLeftOf(ActorBehavior other) => IsSameRow(other) && location.x == other.location.x - 1;

    #endregion

    #region Methods

    public void Init(Vector2Int? initialLocation = null)
    {
        if (initialLocation.HasValue)
            location = initialLocation.Value;

        transform.position = Geometry.PositionFromLocation(location);
        transform.localScale = tileScale;
        moveState = MoveState.Idle;
    }

    private void SetDirection(ActorBehavior other)
    {
        if (other == null)
            return;

        if (IsAbove(other))
            destination.direction = Direction.Down;
        else if (IsRightOf(other))
            destination.direction = Direction.Left;
        else if (IsBelow(other))
            destination.direction = Direction.Up;
        else if (IsLeftOf(other))
            destination.direction = Direction.Right;
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

        if (destination.direction == Direction.Up)
            MoveUp();
        else if (destination.direction == Direction.Right)
            MoveRight();
        else if (destination.direction == Direction.Down)
            MoveDown();
        else if (destination.direction == Direction.Left)
            MoveLeft();

        destination.position = Geometry.PositionFromLocation(destination.location.Value);
        moveState = MoveState.Moving;
    }

    #endregion

    #region Components

    public BoxCollider2D boxCollider2D => gameObject.GetComponent<BoxCollider2D>();
    public SpriteRenderer spriteRenderer => gameObject.GetComponent<SpriteRenderer>();

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

    public Transform parent
    {
        get => gameObject.transform.parent;
        set => gameObject.transform.SetParent(value, true);
    }

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
            location = destination.location.Value;
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
