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
    //private bool IsActive => state == State.Active;
    private bool IsMoving => state == State.Moving;

    private bool IsOnPlayerTeam => team == Team.Player;

    private bool IsSelectedPlayer(ActorBehavior actor) => HasSelectedPlayer && actor.Equals(selectedPlayer);


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

    private bool IsSameColumn(ActorBehavior sender, ActorBehavior receiver) => sender.location.x == receiver.location.x;
    private bool IsSameRow(ActorBehavior sender, ActorBehavior receiver) => sender.location.y == receiver.location.y;
    private bool IsAbove(ActorBehavior sender, ActorBehavior receiver) => sender.location.y == receiver.location.y - 1;
    private bool IsRight(ActorBehavior sender, ActorBehavior receiver) => sender.location.x == receiver.location.x + 1;
    private bool IsBelow(ActorBehavior sender, ActorBehavior receiver) => sender.location.y == receiver.location.y + 1;
    private bool IsLeft(ActorBehavior sender, ActorBehavior receiver) => sender.location.x == receiver.location.x - 1;

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
        if (IsSelectedPlayer(this))
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


    private void OnTriggerEnter2D(Collider2D collider)
    {
        var sender = collider.gameObject.GetComponent<ActorBehavior>();
        var receiver = gameObject.GetComponent<ActorBehavior>();

        //Ignore "self collisions"
        if (sender.Equals(receiver))
            return;

        //Determine if two actors collided
        if (!sender.CompareTag(Tag.Actor) || !receiver.CompareTag(Tag.Actor))
            return;

        //Ignore trigger when against selected player
        if (IsSelectedPlayer(receiver))
            return;

        //Ignore actors in motion
        if (sender.state != State.Idle || receiver.state != State.Idle)
            return;

        //Verify destination has *not* been set 
        if (receiver.destination.IsValid)
            return;

        if ((IsSameColumn(sender, receiver) && IsAbove(sender, receiver))
            || (IsSameRow(sender, receiver) && IsRight(sender, receiver))
            || (IsSameColumn(sender, receiver) && IsBelow(sender, receiver))
            || (IsSameRow(sender, receiver) && IsLeft(sender, receiver)))
        {
            //Assign destination location and position
            receiver.destination.location = sender.location;
            receiver.destination.position = Geometry.PositionFromLocation(receiver.destination.location.Value);
        }

    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        var sender = collider.gameObject.GetComponent<ActorBehavior>();
        var receiver = gameObject.GetComponent<ActorBehavior>();

        //Ignore "self collisions"
        if (sender.Equals(receiver))
            return;

        //Determine if two actors collided
        if (!sender.CompareTag(Tag.Actor) || !receiver.CompareTag(Tag.Actor))
            return;

        //Ignore actors in motion
        if (sender.state != State.Idle || receiver.state != State.Idle)
            return;

        //Ignore trigger when against selected player
        if (IsSelectedPlayer(receiver))
            return;

        //Verify destination *has* been set 
        if (!receiver.destination.IsValid)
            return;

        //Assign actor location before movement so occupied tiles can be calculated accurately
        receiver.location = destination.location.Value;

        //Assign State: "Moving"; Actor will move towards it's destination
        receiver.state = State.Moving;
    }

    private void OnTriggerExit2D(Collider2D collider)
    {

    }

    private void MoveTowardCursor()
    {
        if (!HasSelectedPlayer || !IsSelectedPlayer(this))
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
            location = destination.location.Value;
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
