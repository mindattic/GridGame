using System.Collections;
using System.Linq;
using UnityEngine;

public class ActorBehavior : ExtendedMonoBehavior
{
    //Constants
    const int Thumbnail = 0;
    const int HealthBarBack = 1;
    const int HealthBar = 2;

    //Variables
    public Vector2Int location { get; set; }
    public Vector3? destination { get; set; } = null;
    public Team team = Team.Neutral;

    public float MaxHP { get; set; } = 100f;
    public float HP { get; set; }


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

    public ActorRenderers render = new ActorRenderers();


    public Sprite thumbnail
    {
        get => render.thumbnail.sprite;
        set => render.thumbnail.sprite = value;
    }

    public int sortingOrder
    {
        set
        {
            render.thumbnail.sortingOrder = value;
            render.healthBarBack.sortingOrder = value + 1;
            render.healthBar.sortingOrder = value + 2;
        }
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


    public bool IsAlive => this.HP > 0 && this.isActiveAndEnabled;
 
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
        render.thumbnail = gameObject.transform.GetChild(Thumbnail).GetComponent<SpriteRenderer>();
        render.healthBarBack = gameObject.transform.GetChild(HealthBarBack).GetComponent<SpriteRenderer>();
        render.healthBar = gameObject.transform.GetChild(HealthBar).GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        Init();
    }

    public void Init(Vector2Int? initialLocation = null)
    {
        if (initialLocation.HasValue)
            location = initialLocation.Value;

        this.gameObject.SetActive(true);
        this.position = Geometry.PositionFromLocation(location);
        this.destination = null;
        this.transform.localScale = tileScale;
        //this.transform.GetChild(Thumbnail).transform.localScale = tileScale;
        this.render.thumbnail.color = Colors.Solid.White;

        this.HP = MaxHP;
        this.render.healthBar.transform.localScale = render.healthBarBack.transform.localScale;
    }


    void Update()
    {
        if (!IsAlive)
            return;

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
        var other = actors.FirstOrDefault(x => !this.Equals(x) && this.location.Equals(x.location) && x.IsAlive);
        if (other == null)
            return;

        this.SetDestination(other);
    }

    void FixedUpdate()
    {
        if (!IsAlive)
            return;

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



    public void TakeDamage(int amount)
    {
        damageTaken = amount;
        StartCoroutine(TakeDamage());
    }

    private int damageTaken = 0;

    private IEnumerator TakeDamage()
    {
        var y = render.healthBarBack.transform.localScale.y;
        var z = render.healthBarBack.transform.localScale.z;
        var remainingHP = HP - damageTaken;

        while (HP > remainingHP)
        {
            position = currentTile.position;
           
            var damage = RNG.RandomInt(1, 3);
            HP -= damage;
            HP = Mathf.Clamp(HP, 0, MaxHP);
            if (HP < 1)
                break;

            position += new Vector3(RNG.RandomRange(tileSize / 12), RNG.RandomRange(tileSize / 12), 1);
            damageTextManager.Add(damage.ToString(), position);
            var x = render.healthBarBack.transform.localScale.x * (HP / MaxHP);
            render.healthBar.transform.localScale = new Vector3(x, y, z);
            yield return new WaitForSeconds(0.05f); // update interval
        }
        damageTaken = 0;
        position = currentTile.position;
        if (HP < 1)
            this.gameObject.SetActive(false);
    }


    private IEnumerator Die()
    {
        var alpha = 1f;
        while (alpha > 0)
        {
            alpha -= 0.01f;

            var color = Common.ColorRGBA(255, 255, 255, alpha);

            this.render.thumbnail.color = color;
            yield return new WaitForSeconds(0.01f); // update interval
        }
        this.gameObject.SetActive(false);
    }

}
