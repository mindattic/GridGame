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

    //MoveState Properties
    private bool IsIdle => moveState == MoveState.Moving;
    //private bool IsActive => moveState == MoveState.Active;
    private bool IsMoving => moveState == MoveState.Moving;

    private bool IsOnPlayerTeam => team == Team.Player;

    private bool IsSelectedPlayer => HasSelectedPlayer && this.Equals(selectedPlayer);

    private bool HasDirection => destination.direction != Direction.None;

    //Component properties
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
        moveState = MoveState.Idle;
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        if (this.IsSelectedPlayer)
        {
            //SetDestination active actor towards mouse cursor
            MoveTowardCursor();
        }
        else if (this.IsMoving)
        {
            this.MoveTowardDestination();
        }
        else if (this.IsIdle)
        {
            //Do nothing...
        }

    }


    private bool IsSameColumn(ActorBehavior other) => this.location.x == other.location.x;
    private bool IsSameRow(ActorBehavior other) => this.location.y == other.location.y;
    private bool IsAbove(ActorBehavior other) => IsSameColumn(this) && this.location.y == other.location.y + 1;
    private bool IsRightOf(ActorBehavior other) => IsSameRow(this) && this.location.x == other.location.x + 1;
    private bool IsBelow(ActorBehavior other) => IsSameColumn(this) && this.location.y == other.location.y - 1;
    private bool IsLeftOf(ActorBehavior other) => IsSameRow(this) && this.location.x == other.location.x - 1;


    private void SetDirection(ActorBehavior other)
    {
        if (other == null)
            return;

        if (this.IsAbove(other))
            this.destination.direction = Direction.Down;
        else if (this.IsRightOf(other))
            this.destination.direction = Direction.Left;
        else if (this.IsBelow(other))
            this.destination.direction = Direction.Up;
        else if (this.IsLeftOf(other))
            this.destination.direction = Direction.Right;
        else
            this.destination.direction = Direction.None;
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

        //Assign intended movement direction
        var sender = collider.gameObject.GetComponent<ActorBehavior>();
        this.SetDirection(sender);
    }

    private void SetDestination()
    {
        if (this.destination.direction == Direction.None)
            return;

        if (this.destination.direction == Direction.Up)
            this.destination.location = this.location + new Vector2Int(0, 1);
        else if (this.destination.direction == Direction.Right)
            this.destination.location = this.location + new Vector2Int(1, 0);
        else if (this.destination.direction == Direction.Down)
            this.destination.location = this.location + new Vector2Int(0, -1);
        else if (this.destination.direction == Direction.Left)
            this.destination.location = this.location + new Vector2Int(-1, 0);

        this.destination.position = Geometry.PositionFromLocation(this.destination.location.Value);
        this.moveState = MoveState.Moving;
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

        //var sender = collider.gameObject.GetComponent<ActorBehavior>();

        //Determine if two actors collided
        //if (!sender.CompareTag(Tag.Actor) || !this.CompareTag(Tag.Actor))
        //    return;

        this.SetDestination();
    }

    private void OnTriggerExit2D(Collider2D collider)
    {

    }

    private void MoveTowardCursor()
    {
        if (!IsSelectedPlayer)
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

        //SetDestination active actor towards mouse cursor
        transform.position
                = Vector2.MoveTowards(selectedPlayer.transform.position,
                                      cursorPosition,
                                      moveSpeed);
    }

    private void MoveTowardDestination()
    {
        //Verify destination *has* been set 
        if (!this.destination.IsValid)
            return;

        //Verify actor is MoveState: "Moving"
        if (this.moveState != MoveState.Moving)
            return;

        //SetDestination actor towards destination
        this.transform.position = Vector2.MoveTowards(transform.position, destination.position.Value, moveSpeed);

        //Determine if actor is close to destination
        bool isCloseToDestination = Vector2.Distance(transform.position, destination.position.Value) < snapDistance;
        if (isCloseToDestination)
        {
            //Snap to destination, clear destination, and set actor MoveState: "Idle"
            this.location = destination.location.Value;
            this.transform.position = destination.position.Value;
            this.destination.Clear();
            this.moveState = MoveState.Idle;
        }
    }



    void OnDrawGizmos()
    {
        //Gizmos.color = Color.green;
        //Gizmos.matrix = this.transform.localToWorldMatrix;
        //Gizmos.DrawCube(Vector3.zero, Vector3.one);
    }

}
