using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActorBehavior : ExtendedMonoBehavior
{
    //Constants
    const int Thumbnail = 0;
    const int Frame = 1;
    const int HealthBarBack = 2;
    const int HealthBar = 3;

    //Variables
    [SerializeField] public string id;
    public Vector2Int location;
    public Vector3? destination = null;
    public Team team = Team.Independant;
    public float MaxHP = 100f;
    public float HP;


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

    public Vector3 scale
    {
        get => gameObject.transform.localScale;
        set => gameObject.transform.localScale = value;
    }

    public ActorRenderers sprite = new ActorRenderers();

    public Sprite thumbnail
    {
        get => sprite.thumbnail.sprite;
        set => sprite.thumbnail.sprite = value;
    }

    public int sortingOrder
    {
        set
        {
            sprite.thumbnail.sortingOrder = value;
            sprite.frame.sortingOrder = value + 1;
            sprite.healthBarBack.sortingOrder = value + 2;
            sprite.healthBar.sortingOrder = value + 3;
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

    public void SwapLocation(ActorBehavior other)
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
            //Debug.Log($"Conflict: {this.id} / {location.id}");

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


    public void SetDestination(List<Vector2Int> locations)
    {

    }


    #endregion

    private void Awake()
    {
        sprite.thumbnail = gameObject.transform.GetChild(Thumbnail).GetComponent<SpriteRenderer>();
        sprite.frame = gameObject.transform.GetChild(Frame).GetComponent<SpriteRenderer>();
        sprite.healthBarBack = gameObject.transform.GetChild(HealthBarBack).GetComponent<SpriteRenderer>();
        sprite.healthBar = gameObject.transform.GetChild(HealthBar).GetComponent<SpriteRenderer>();
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
        this.sprite.thumbnail.color = Colors.Solid.White;

        this.HP = MaxHP;
        this.sprite.healthBar.transform.localScale = sprite.healthBarBack.transform.localScale;
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
            this.CheckLocationConflict();
        }


    }

    public void CheckLocationConflict()
    {
        var other = actors.FirstOrDefault(x => x.IsAlive && !this.Equals(x) && this.location.Equals(x.location));
        if (other == null)
            return;

        this.SwapLocation(other);
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
        var y = sprite.healthBarBack.transform.localScale.y;
        var z = sprite.healthBarBack.transform.localScale.z;
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
            var x = sprite.healthBarBack.transform.localScale.x * (HP / MaxHP);
            sprite.healthBar.transform.localScale = new Vector3(x, y, z);
            yield return new WaitForSeconds(0.05f);
        }
        damageTaken = 0;
        position = currentTile.position;

        yield return new WaitForSeconds(1);
        sprite.thumbnail.color = Colors.Solid.White;

        //Deactive enemy if killed
        if (HP < 1)
            StartCoroutine(Die());

        yield return new WaitForSeconds(1);

        //Reset board if all enemies are dead
        if (enemies.All(x => !x.IsAlive))
            board.ResetBoard();

    }


    private IEnumerator Die()
    {
        var alpha = 1f;
        while (alpha > 0)
        {
            alpha -= 0.05f;
            alpha = Mathf.Clamp(alpha, 0, 1);
            var color = new Color(1, 1, 1, alpha); 
            this.sprite.thumbnail.color = color;
            this.sprite.frame.color = color;
            this.sprite.healthBarBack.color = color;
            this.sprite.healthBar.color = color;
            yield return new WaitForSeconds(0.01f);
        }
        this.gameObject.SetActive(false);
    }

}
