using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class ActorBehavior : ExtendedMonoBehavior
{
    //Constants
    const int Thumbnail = 0;
    const int Frame = 1;
    const int HealthBarBack = 2;
    const int HealthBar = 3;
    const int StatusIcon = 4;
    const int TurnDelay = 5;
    const int HealthText = 6;

    //Variables
    [SerializeField] public string id;
    [SerializeField] public Vector2Int location;
    [SerializeField] public Vector3? destination = null;
    [SerializeField] public Team team = Team.Independant;
    [SerializeField] public int HP;
    [SerializeField] public int MaxHP;

    private int enemyTurnDelay = 0;

    [SerializeField] public AnimationCurve bobbing;

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
            render.frame.sortingOrder = value + 1;
            render.healthBarBack.sortingOrder = value + 2;
            render.healthBar.sortingOrder = value + 3;
            render.statusIcon.sortingOrder = value + 4;
            render.turnDelay.sortingOrder = value + 5;
            render.healthText.sortingOrder = value + 6;
        }
    }

    public int turnDelay
    {
        get => enemyTurnDelay;
        set
        {
            enemyTurnDelay = value;
            render.turnDelay.text = enemyTurnDelay != 0 ? $"x{enemyTurnDelay}" : "";
        }
    }





    #endregion

    #region Properties

    public TileBehavior currentTile => tiles.First(x => x.location.Equals(location));
    public bool IsPlayer => this.team.Equals(Team.Player);
    public bool IsEnemy => this.team.Equals(Team.Enemy);
    public bool IsTargettedPlayer => HasTargettedPlayer && this.Equals(targettedPlayer);
    public bool IsSelectedPlayer => HasSelectedPlayer && this.Equals(selectedPlayer);
    public bool HasDestination => this.destination.HasValue;
    public bool IsNorthEdge => this.location.y == 1;
    public bool IsEastEdge => this.location.x == board.columns;
    public bool IsSouthEdge => this.location.y == board.rows;
    public bool IsWestEdge => this.location.x == 1;
    public bool IsAlive => this != null && this.isActiveAndEnabled && this.HP > 0;


    #endregion

    #region Methods

    public void GenerateTurnDelay()
    {
        //TODO: Use enemy statistics to determine turn delay...
        enemyTurnDelay = Random.Int(2, 4);
        render.turnDelay.text = $"x{enemyTurnDelay}";
        this.SetStatusSleep();


    }

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
        return Random.Int(1, 4) switch
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

        soundSource.PlayOneShot(resourceManager.SoundEffect("Slide"));
    }


    public void SetDestination()
    {
        var location = new Vector2Int(Random.Int(1, board.columns), Random.Int(1, board.rows));
        var closestTile = Geometry.ClosestTileByLocation(location);
        this.destination = closestTile.position;
    }


    #endregion

    private void Awake()
    {
        render.thumbnail = gameObject.transform.GetChild(Thumbnail).GetComponent<SpriteRenderer>();
        render.frame = gameObject.transform.GetChild(Frame).GetComponent<SpriteRenderer>();
        render.healthBarBack = gameObject.transform.GetChild(HealthBarBack).GetComponent<SpriteRenderer>();
        render.healthBar = gameObject.transform.GetChild(HealthBar).GetComponent<SpriteRenderer>();
        render.statusIcon = gameObject.transform.GetChild(StatusIcon).GetComponent<SpriteRenderer>();
        render.turnDelay = gameObject.transform.GetChild(TurnDelay).GetComponent<TextMeshPro>();
        render.healthText = gameObject.transform.GetChild(HealthText).GetComponent<TextMeshPro>();

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
        this.render.thumbnail.color = Colors.Solid.White;

        this.HP = MaxHP;
        this.render.healthBar.transform.localScale = render.healthBarBack.transform.localScale;
        this.render.healthText.text = $"{HP}/{MaxHP}";

        if (this.IsPlayer)
        {
            render.frame.color = Colors.Solid.White;
            render.turnDelay.gameObject.SetActive(false);
            SetStatusNone();
        }
        else
        {
            render.frame.color = Colors.Solid.Red;
            GenerateTurnDelay();
        }



        //original = render.thumbnail.transform.position;
    }




    void Update()
    {
        if (!IsAlive) return;

        if (this.IsSelectedPlayer)
            MoveTowardCursor();

        var closestTile = Geometry.ClosestTileByPosition(this.position);
        if (closestTile.location.Equals(this.location))
            return;

        soundSource.PlayOneShot(resourceManager.SoundEffect($"Move{Random.Int(1, 6)}"));
        //soundSource.PlayOneShot(resourceManager.SoundEffect($"Move1"));

        //Determine if selected player and another actor are occupying the same tile
        var actor = actors.FirstOrDefault(x => x.IsAlive && !x.Equals(selectedPlayer) && x.location.Equals(closestTile.location));
        if (actor != null)
        {
            actor.SwapLocation(this);
        }

        this.location = closestTile.location;

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
        CheckBobbing();


    }

    private void MoveTowardCursor()
    {
        if (!this.IsSelectedPlayer)
            return;

        var cursorPosition = mousePosition3D + mouseOffset;
        cursorPosition.x = Mathf.Clamp(cursorPosition.x, board.left, board.right);
        cursorPosition.y = Mathf.Clamp(cursorPosition.y, board.bottom, board.top);

        //Move selected player towards cursor
        this.position = Vector2.MoveTowards(position, cursorPosition, cursorSpeed);

        //Snap selected player to cursor
        //this.position = cursorPosition;
    }

    private void CheckMovement()
    {
        if (!HasDestination) return;

        var delta = this.destination.Value - this.position;
        if (Mathf.Abs(delta.x) > snapDistance)
        {
            this.position = Vector2.MoveTowards(this.position, new Vector3(this.destination.Value.x, this.position.y, this.position.z), slideSpeed);
        }
        else if (Mathf.Abs(delta.y) > snapDistance)
        {
            this.position = Vector2.MoveTowards(this.position, new Vector3(this.position.x, this.destination.Value.y, this.position.z), slideSpeed);
        }


        //Move actor towards destination
        //this.position = Vector2.MoveTowards(this.position, this.destination.Value, slideSpeed);

        //Determine if actor is close to destination
        bool isCloseToDestination = Vector2.Distance(this.position, this.destination.Value) < snapDistance;
        if (isCloseToDestination)
        {
            //Snap to destination, clear destination, and set actor MoveState: "Idle"
            this.transform.position = this.destination.Value;
            this.destination = null;
        }
    }


    private void CheckBobbing()
    {
        if (!this.IsAlive || !this.IsPlayer || !turnManager.IsStartPhase) return;

        //Source: https://forum.unity.com/threads/how-to-make-an-object-move-up-and-down-on-a-loop.380159/
        var pos = new Vector3(
            transform.position.x,
            transform.position.y + (bobbing.Evaluate(Time.time % bobbing.length) * (tileSize / 24)),
            transform.position.z);

        render.thumbnail.transform.position = pos;
        render.frame.transform.position = pos;


    }

    //public void TakeDamage(int amount)
    //{
    //    damageTaken = amount;
    //    StartCoroutine(StartTakingDamage());
    //}

    //private int damageTaken = 0;

    public IEnumerator TakeDamage(int damageTaken)
    {
        var remainingHP = Mathf.Clamp(HP - damageTaken, 0, MaxHP);

        while (HP > remainingHP)
        {

            //Decrease HP
            var damage = Random.Int(1, 3);
            HP -= damage;
            HP = Mathf.Clamp(HP, remainingHP, MaxHP);

            //Spawn damage text
            damageTextManager.Spawn(damage.ToString(), position);

            //Shake actor
            this.position = currentTile.position;
            this.position += new Vector3(Random.Range(tileSize / 12), Random.Range(tileSize / 12), 1);

            //Resize health bar
            var scale = new Vector3(
                render.healthBarBack.transform.localScale.x * (HP.ToFloat() / MaxHP.ToFloat()),
                render.healthBarBack.transform.localScale.y,
                render.healthBarBack.transform.localScale.z);
            this.render.healthBar.transform.localScale = scale;

            //Print health
            this.render.healthText.text = $"{HP}/{MaxHP}";

            //Play sfx
            soundSource.PlayOneShot(resourceManager.SoundEffect($"Slash{Random.Int(1, 7)}"));

            yield return new WaitForSeconds(Interval.Five);
        }

        this.HP = remainingHP;
        damageTaken = 0;
        this.position = currentTile.position;

        //Deactive enemy if killed
        if (this.HP < 1)
        {
            StartCoroutine(StartDying());
        }

    }




    private IEnumerator StartDying()
    {
        var alpha = 1f;
        var color = new Color(1, 1, 1, alpha);
        this.render.thumbnail.color = color;
        this.render.healthBarBack.color = color;
        this.render.healthBar.color = color;

        soundSource.PlayOneShot(resourceManager.SoundEffect("Death"));

        while (alpha > 0)
        {

            alpha -= Increment.Five;
            alpha = Mathf.Clamp(alpha, 0, 1);
            this.render.thumbnail.color = color;
            this.render.healthBarBack.color = color;
            this.render.healthBar.color = color;

            position = currentTile.position;
            position += new Vector3(Random.Range(tileSize / 12), Random.Range(tileSize / 12), 1);

            yield return new WaitForSeconds(Interval.One);
        }

        //this.gameObject.SetActive(false);
        actors.Remove(this);
        Destroy(this.gameObject);
    }





    public void SetStatusAttack()
    {
        render.statusIcon.sprite = resourceManager.statusSprites.First(x => x.id.Equals("Attack")).thumbnail;
    }

    public void SetStatusSleep()
    {
        render.statusIcon.sprite = resourceManager.statusSprites.First(x => x.id.Equals("Sleep")).thumbnail;
    }

    public void SetStatusSupport()
    {
        render.statusIcon.sprite = resourceManager.statusSprites.First(x => x.id.Equals("Support")).thumbnail;
    }

    public void SetStatusNone()
    {
        render.statusIcon.sprite = resourceManager.statusSprites.First(x => x.id.Equals("None")).thumbnail;
    }

}
