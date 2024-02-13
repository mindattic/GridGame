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
        if (IsSelectedPlayer)
        {
            //Move active actor towards mouse cursor
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


    private bool IsSameColumn(ActorBehavior other) => this.location.x == other.location.x;
    private bool IsSameRow(ActorBehavior other) => this.location.y == other.location.y;
    private bool IsAbove(ActorBehavior other) => IsSameColumn(other) && this.location.y == other.location.y - 1;
    private bool IsRightOf(ActorBehavior other) => IsSameRow(other) && this.location.x == other.location.x + 1;
    private bool IsBelow(ActorBehavior other) => IsSameColumn(other) && this.location.y == other.location.y + 1;
    private bool IsLeftOf(ActorBehavior other) => IsSameRow(other) && this.location.x == other.location.x - 1;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //Verify direction has *NOT* been set 
        if (HasDirection) return;

        var sender = collider.gameObject.GetComponent<ActorBehavior>();

        //Ignore self-collisions
        if (sender.Equals(this)) return;

        //Determine if two actors collided
        if (!sender.CompareTag(Tag.Actor) || !this.CompareTag(Tag.Actor)) return;

        //Ignore actors in motion
        if (IsMoving) return;

        if (sender.IsAbove(this))
        {
            destination.direction = Direction.Up;
        }
        else if (sender.IsRightOf(this))
        {
            destination.direction = Direction.Right;
        }
        else if (sender.IsBelow(this))
        {
            destination.direction = Direction.Down;
        }
        else if (sender.IsLeftOf(this))
        {
            destination.direction = Direction.Left;
        }

    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        //Verify direction *HAS* been set 
        if (!HasDirection) return;

        var sender = collider.gameObject.GetComponent<ActorBehavior>();

        //Ignore self-collisions
        if (sender.Equals(this)) return;

        //Determine if two actors collided
        if (!sender.CompareTag(Tag.Actor) || !this.CompareTag(Tag.Actor)) return;

        //Ignore actors in motion
        if (IsMoving) return;

        if (destination.direction == Direction.Up)
        {
            this.destination.location = this.location + new Vector2Int(0, -1);
        }
        else if (destination.direction == Direction.Right)
        {
            this.destination.location = this.location + new Vector2Int(1, 0);
        }
        else if (destination.direction == Direction.Down)
        {
            this.destination.location = this.location + new Vector2Int(0, 1);
        }
        else if (destination.direction == Direction.Left)
        {
            this.destination.location = this.location + new Vector2Int(-1, 0);
        }

        this.destination.position = Geometry.PositionFromLocation(this.destination.location.Value);
        this.moveState = MoveState.Moving;
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

        //Move active actor towards mouse cursor
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

        //Move actor towards destination
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
        //Gizmos.matrix = this.transform.localToWorldMatrix;
        //Gizmos.DrawCube(Vector3.zero, Vector3.one);
    }

}
