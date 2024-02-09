using UnityEngine;
using State = ActorState;

public class ActorBehavior : ExtendedMonoBehavior
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
        get => spriteRenderer.sprite;
        set => spriteRenderer.sprite = value;
    }

    public Transform parent
    {
        get => gameObject.transform.parent;
        set => gameObject.transform.SetParent(value, true);
    }

    protected bool IsSameColumn => HasActiveActor && activeActor.location.x == location.x;
    protected bool IsSameRow => HasActiveActor && activeActor.location.y == location.y;
    protected bool IsAbove => HasActiveActor && activeActor.location.y == location.y - 1;
    protected bool IsRight => HasActiveActor && activeActor.location.x == location.x + 1;
    protected bool IsBelow => HasActiveActor && activeActor.location.y == location.y + 1;
    protected bool IsLeft => HasActiveActor && activeActor.location.x == location.x - 1;

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

    }

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

        if ((IsSameColumn && IsAbove)
            || (IsSameRow && IsRight)
            || (IsSameColumn && IsBelow)
            || (IsSameRow && IsLeft))
        {
            //Assign destination location and position
            destination.location = activeActor.location;
            destination.position = Geometry.PositionFromLocation(destination.location.Value);
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

}
