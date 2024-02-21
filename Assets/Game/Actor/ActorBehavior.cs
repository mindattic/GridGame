using System.Linq;
using UnityEngine;

public class ActorBehavior : ExtendedMonoBehavior
{
    //Variables
    public Vector2Int location { get; set; }
    public Vector3? destination { get; set; } = null;
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

    public TileBehavior currentTile => tiles.First(x => x.location.Equals(location));
    private bool IsOnPlayerTeam => this.team == Team.Player;
    private bool IsSelectedPlayer => HasSelectedPlayer && this.Equals(selectedPlayer);
    private bool HasDestination => this.destination.HasValue;
    public bool IsNorthEdge => this.location.y == 1;
    public bool IsEastEdge => this.location.x == board.columns;
    public bool IsSouthEdge => this.location.y == board.rows;
    public bool IsWestEdge => this.location.x == 1;

    #endregion

    #region Methods

    public bool IsSameColumn(Vector2Int location) => this.location.x == location.x;
    public bool IsSameRow(Vector2Int location) => this.location.y == location.y;
    public bool IsNorthOf(Vector2Int location) => this.IsSameColumn(location) && this.location.y == location.y - 1;
    public bool IsEastOf(Vector2Int location) => this.IsSameRow(location) && this.location.x == location.x + 1;
    public bool IsSouthOf(Vector2Int location) => this.IsSameColumn(location) && this.location.y == location.y + 1;
    public bool IsWestOf(Vector2Int location) => this.IsSameRow(location) && this.location.x == location.x - 1;
    public bool IsNorthWestOf(Vector2Int location) => this.location.x == location.x - 1 && this.location.y == location.y - 1;
    public bool IsNorthEastOf(Vector2Int location) => this.location.x == location.x + 1 && this.location.y == location.y - 1;
    public bool IsSouthWestOf(Vector2Int location) => this.location.x == location.x - 1 && this.location.y == location.y + 1;
    public bool IsSouthEastOf(Vector2Int location) => this.location.x == location.x + 1 && this.location.y == location.y + 1;

    public void Init(Vector2Int? initialLocation = null)
    {
        if (initialLocation.HasValue)
            location = initialLocation.Value;

        this.position = Geometry.PositionFromLocation(location);
        this.destination = null;
        this.transform.localScale = tileScale;
        this.spriteRenderer.color = Colors.Solid.White;
    }

    private Vector2Int GoNorth() => this.location += new Vector2Int(0, -1);
    private Vector2Int GoEast() => this.location += new Vector2Int(1, 0);
    private Vector2Int GoSouth() => this.location += new Vector2Int(0, 1);
    private Vector2Int GoWest() => this.location += new Vector2Int(-1, 0);

    private Vector2Int GoRandomDirection()
    {
        return RNG.RandomInt(1, 4) switch
        {
            1 => GoNorth(),
            2 => GoEast(),
            3 => GoSouth(),
            _ => GoWest(),
        };
    }

    private void GoToward(Vector2Int other)
    {
        if (this.IsNorthOf(other) || this.IsNorthWestOf(other) || this.IsNorthEastOf(other))
            GoSouth();
        else if (this.IsEastOf(other))
            GoWest();
        else if (this.IsSouthOf(other) || this.IsSouthWestOf(other) || this.IsSouthEastOf(other))
            GoNorth();
        else if (this.IsWestOf(other))
            GoEast();
    }

    public void SetDestination(ActorBehavior other)
    {
        if (other == null)
            return;

        if (HasDestination)
            return;

        if (this.IsNorthOf(other.location) || this.IsNorthWestOf(other.location) || this.IsNorthEastOf(other.location))
            GoSouth();
        else if (this.IsEastOf(other.location))
            GoWest();
        else if (this.IsSouthOf(other.location) || this.IsSouthWestOf(other.location) || this.IsSouthEastOf(other.location))
            GoNorth();
        else if (this.IsWestOf(other.location))
            GoEast();
        else
        {
            //Actors are on top of eachother
            //TODO: Make sure this never happens in the first place...
            //Debug.Log($"Conflict: {this.name} / {location.name}");

            var closestUnoccupiedTile = Geometry.ClosestUnoccupiedTileByLocation(this.location);
            if (closestUnoccupiedTile != null)
                GoToward(closestUnoccupiedTile.location);
            else if (IsNorthEdge)
                GoSouth();
            else if (IsEastEdge)
                GoWest();
            else if (IsSouthEdge)
                GoNorth();
            else if (IsWestEdge)
                GoEast();
            else
                GoRandomDirection();
        }

        var closestTile = Geometry.ClosestTileByLocation(this.location);
        this.destination = closestTile.position;
    }


   


    #endregion

    private void Awake()
    {
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
            //Determine if two actors occupy same location
            this.CheckLocation();
        }
    }

    public void CheckLocation()
    {
        var other = actors.FirstOrDefault(x => !this.Equals(x) && this.location.Equals(x.location));
        if (other == null)
            return;

        this.SetDestination(other);
    }

    void FixedUpdate()
    {
        if (this.IsSelectedPlayer)
            return;

        CheckMovement();
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

    private void CheckMovement()
    {
        if (!HasDestination)
            return;

        //Move actor towards destination
        this.position = Vector2.MoveTowards(this.position, this.destination.Value, slideSpeed);

        //Determine if actor is close to destination
        bool isCloseToDestination = Vector2.Distance(this.position, this.destination.Value) < snapDistance;
        if (isCloseToDestination)
        {
            //Snap to destination, clear destination, and set actor MoveState: "Idle"
            this.transform.position = this.destination.Value;
            this.destination = null;
        }
    }

}
