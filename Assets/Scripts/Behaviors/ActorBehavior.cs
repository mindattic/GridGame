using System;
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
    [SerializeField] public Vector2Int location = Vector2Int.zero;
    [SerializeField] public Vector3? destination = null;
    [SerializeField] public Team team = Team.Independant;
    [SerializeField] public int HP;
    [SerializeField] public int MaxHP;


    public int? spawnTurn = null;
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
    public bool IsPlayer => team.Equals(Team.Player);
    public bool IsEnemy => team.Equals(Team.Enemy);
    public bool IsTargettedPlayer => HasTargettedPlayer && Equals(targettedPlayer);
    public bool IsSelectedPlayer => HasSelectedPlayer && Equals(selectedPlayer);
    public bool HasDestination => destination.HasValue;
    public bool IsNorthEdge => location.y == 1;
    public bool IsEastEdge => location.x == board.columns;
    public bool IsSouthEdge => location.y == board.rows;
    public bool IsWestEdge => location.x == 1;
    public bool IsAlive => HP > 0;
    public bool IsActive => this != null && this.isActiveAndEnabled;
    public bool HasSpawned => !spawnTurn.HasValue || (spawnTurn.HasValue && spawnTurn.Value >= turnManager.turnNumber);

    #endregion

    #region Methods

    public void GenerateTurnDelay()
    {
        //TODO: Use enemy statistics to determine turn delay...
        enemyTurnDelay = Random.Int(2, 4);
        render.turnDelay.text = $"x{enemyTurnDelay}";
        render.turnDelay.gameObject.SetActive(true);
        Set(ActionIcon.Sleep);
    }

    public bool IsSameColumn(Vector2Int location) => this.location.x == location.x;
    public bool IsSameRow(Vector2Int location) => this.location.y == location.y;
    public bool IsNorthOf(Vector2Int location) => IsSameColumn(location) && this.location.y == location.y - 1;
    public bool IsEastOf(Vector2Int location) => IsSameRow(location) && this.location.x == location.x + 1;
    public bool IsSouthOf(Vector2Int location) => IsSameColumn(location) && this.location.y == location.y + 1;
    public bool IsWestOf(Vector2Int location) => IsSameRow(location) && this.location.x == location.x - 1;
    public bool IsNorthWestOf(Vector2Int location) => this.location.x == location.x - 1 && this.location.y == location.y - 1;
    public bool IsNorthEastOf(Vector2Int location) => this.location.x == location.x + 1 && this.location.y == location.y - 1;
    public bool IsSouthWestOf(Vector2Int location) => this.location.x == location.x - 1 && this.location.y == location.y + 1;
    public bool IsSouthEastOf(Vector2Int location) => this.location.x == location.x + 1 && this.location.y == location.y + 1;

    private Vector2Int GoNorth() => location += new Vector2Int(0, -1);
    private Vector2Int GoEast() => location += new Vector2Int(1, 0);
    private Vector2Int GoSouth() => location += new Vector2Int(0, 1);
    private Vector2Int GoWest() => location += new Vector2Int(-1, 0);

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
        if (IsNorthOf(other) || IsNorthWestOf(other) || IsNorthEastOf(other))
            GoSouth();
        else if (IsEastOf(other))
            GoWest();
        else if (IsSouthOf(other) || IsSouthWestOf(other) || IsSouthEastOf(other))
            GoNorth();
        else if (IsWestOf(other))
            GoEast();
    }

    public void SwapLocation(ActorBehavior other)
    {
        if (other == null)
            return;

        if (HasDestination)
            return;

        if (IsNorthOf(other.location) || IsNorthWestOf(other.location) || IsNorthEastOf(other.location))
            GoSouth();
        else if (IsEastOf(other.location))
            GoWest();
        else if (IsSouthOf(other.location) || IsSouthWestOf(other.location) || IsSouthEastOf(other.location))
            GoNorth();
        else if (IsWestOf(other.location))
            GoEast();
        else
        {
            //Actors are on top of eachother
            //TODO: Make sure this never happens in the first place...
            //Debug.Log($"Conflict: {this.id} / {location.id}");

            var closestUnoccupiedTile = Geometry.ClosestUnoccupiedTileByLocation(location);
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

        var closestTile = Geometry.ClosestTileByLocation(location);
        this.destination = closestTile.position;

        soundSource.PlayOneShot(resourceManager.SoundEffect("Slide"));
    }


    public void SetDestination()
    {
        var location = new Vector2Int(Random.Int(1, board.columns), Random.Int(1, board.rows));
        var closestTile = Geometry.ClosestTileByLocation(location);
        destination = closestTile.position;
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

        gameObject.SetActive(true);
        position = Geometry.PositionFromLocation(location);
        destination = null;
        transform.localScale = tileScale;
        render.thumbnail.color = Colors.Solid.White;

        HP = MaxHP;
        render.healthBar.transform.localScale = render.healthBarBack.transform.localScale;
        PrintHealth();

        if (this.IsPlayer)
        {
            render.turnDelay.gameObject.SetActive(false);
            Set(ActionIcon.None);
        }
        else
        {
            GenerateTurnDelay();

            if (!spawnTurn.HasValue)
            {
                gameObject.SetActive(true);
            }
            else
            {
                var color = new Color(1, 1, 1, 0);
                render.thumbnail.color = color;
                render.frame.color = color;
                render.healthBarBack.color = color;
                render.healthBar.color = color;
                gameObject.SetActive(false);
            }

        }


    }




    void Update()
    {
        if (!IsAlive) return;

        if (IsSelectedPlayer)
            MoveTowardCursor();

        var closestTile = Geometry.ClosestTileByPosition(this.position);
        if (closestTile.location.Equals(this.location))
            return;

        soundSource.PlayOneShot(resourceManager.SoundEffect($"Move{Random.Int(1, 6)}"));
        //soundSource.PlayOneShot(resourceManager.SoundEffect($"Move1"));

        //Determine if selected player and another actor are occupying the same tile
        var actor = actors.FirstOrDefault(x => x != null && x.IsAlive && x.IsActive && !x.Equals(selectedPlayer) && x.location.Equals(closestTile.location));
        if (actor != null)
        {
            actor.SwapLocation(this);
        }

        location = closestTile.location;

    }

    public void CheckLocationConflict()
    {
        var other = actors.FirstOrDefault(x => x != null && x.IsAlive && x.IsActive && !Equals(x) && location.Equals(x.location));
        if (other == null)
            return;

        SwapLocation(other);
    }

    void FixedUpdate()
    {
        if (!IsAlive || !IsActive || IsSelectedPlayer) return;

        CheckMovement();
        CheckBobbing();


    }

    private void MoveTowardCursor()
    {
        if (!IsSelectedPlayer)
            return;

        var cursorPosition = mousePosition3D + mouseOffset;
        cursorPosition.x = Mathf.Clamp(cursorPosition.x, board.left, board.right);
        cursorPosition.y = Mathf.Clamp(cursorPosition.y, board.bottom, board.top);

        //Move selected player towards cursor
        position = Vector2.MoveTowards(position, cursorPosition, cursorSpeed);

        //Snap selected player to cursor
        //this.position = cursorPosition;
    }

    private void CheckMovement()
    {
        if (!HasDestination) return;

        var delta = this.destination.Value - position;
        if (Mathf.Abs(delta.x) > snapDistance)
        {
            position = Vector2.MoveTowards(position, new Vector3(destination.Value.x, position.y, position.z), slideSpeed);
        }
        else if (Mathf.Abs(delta.y) > snapDistance)
        {
            position = Vector2.MoveTowards(position, new Vector3(position.x, destination.Value.y, position.z), slideSpeed);
        }

        //Determine if actor is close to destination
        bool isSnapDistance = Vector2.Distance(position, destination.Value) < snapDistance;
        if (isSnapDistance)
        {
            //Snap to destination, clear destination, and set actor MoveState: "Idle"
            transform.position = destination.Value;
            destination = null;
        }
    }


    private void CheckBobbing()
    {
        if (!IsAlive || !IsActive || !IsPlayer || !turnManager.IsStartPhase) return;

        //Source: https://forum.unity.com/threads/how-to-make-an-object-move-up-and-down-on-a-loop.380159/
        var pos = new Vector3(
            transform.position.x,
            transform.position.y + (bobbing.Evaluate(Time.time % bobbing.length) * (tileSize / 24)),
            transform.position.z);

        render.thumbnail.transform.position = pos;
        render.frame.transform.position = pos;


    }

    private void Shake(float intensity)
    {
        position = currentTile.position;
        position += new Vector3(Random.Range(intensity), Random.Range(intensity), 1);
    }


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
            Shake(shakeIntensity.Medium);

            //Resize health bar
            if (HP > 0)
            {
                var x = render.healthBarBack.transform.localScale.x;
                var y = render.healthBarBack.transform.localScale.y;
                var z = render.healthBarBack.transform.localScale.z;
                var scale = new Vector3(x * (HP.ToFloat() / MaxHP.ToFloat()), y, z);
                render.healthBar.transform.localScale = scale;

            }
            else
            {
                render.healthBar.enabled = false;
            }

            PrintHealth();

            //SlideIn sfx
            soundSource.PlayOneShot(resourceManager.SoundEffect($"Slash{Random.Int(1, 7)}"));

            yield return new WaitForSeconds(Interval.Five);
        }

        HP = remainingHP;
        position = currentTile.position;
    }

    private void PrintHealth()
    {
        render.healthText.text = $@"{Math.Round(HP.ToFloat() / MaxHP.ToFloat() * 100)}%";
    }

    public IEnumerator Die()
    {
        var alpha = 1f;
        var color = new Color(1, 1, 1, alpha);
        render.thumbnail.color = color;
        render.frame.color = color;
        render.healthBarBack.color = color;
        render.healthBar.color = color;

        portraitManager.Dissolve(this);
        soundSource.PlayOneShot(resourceManager.SoundEffect("Death"));

        yield return new WaitForSeconds(1);

        while (alpha > 0)
        {
            alpha -= Increment.Two;
            alpha = Mathf.Clamp(alpha, 0, 1);
            color = new Color(1, 1, 1, alpha);
            render.thumbnail.color = color;
            render.frame.color = color;
            render.healthBarBack.color = color;
            render.healthBar.color = color;
            yield return Wait.Tick();
        }

        Destroy(this.gameObject);
        actors.Remove(this);
    }

    public void Set(ActionIcon icon)
    {
        var id = icon.ToString();
        render.statusIcon.sprite = resourceManager.statusSprites.First(x => x.id.Equals(id)).thumbnail;
    }

    public IEnumerator FadeIn(float increment = 0.01f)
    {
        float alpha = 0;
        var color = new Color(1, 1, 1, alpha);
        render.thumbnail.color = color;
        render.frame.color = color;
        render.healthBarBack.color = color;
        render.healthBar.color = color;

        while (alpha < 1)
        {
            alpha += increment;
            alpha = Mathf.Clamp(alpha, 0, 1);
            color = new Color(1, 1, 1, alpha);
            render.thumbnail.color = color;
            render.frame.color = color;
            render.healthBarBack.color = color;
            render.healthBar.color = color;

            //Shake actor
            Shake(shakeIntensity.Low);

            yield return Wait.Tick();
        }

        position = currentTile.position;
    }

    public IEnumerator FadeOut(float increment = 0.01f)
    {
        float alpha = 1f;
        var color = new Color(1, 1, 1, alpha);
        render.thumbnail.color = color;
        render.frame.color = color;
        render.healthBarBack.color = color;
        render.healthBar.color = color;

        while (alpha > 0f)
        {
            alpha -= increment;
            alpha = Mathf.Clamp(alpha, 0, 1);
            color = new Color(1, 1, 1, alpha);
            render.thumbnail.color = color;
            render.frame.color = color;
            render.healthBarBack.color = color;
            render.healthBar.color = color;

            //Shake actor
            Shake(shakeIntensity.Low);

            yield return Wait.Tick();
        }

        this.position = currentTile.position;
    }

    public IEnumerator FadeInOut(float fadeInIncrement = 0.01f, float intermission = 2f, float fadeOuIncrement = 0.01f)
    {
        yield return FadeIn(fadeInIncrement);
        yield return new WaitForSeconds(intermission);
        yield return FadeOut(fadeOuIncrement);
    }



}
