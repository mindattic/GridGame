using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;


//public class ActorSubobject
//{
//    public GameObject root;
//    public int index;

//    public ActorSubobject(GameObject root, int index)
//    {
//        this.root = root;
//        this.index = index;
//    }

//    public GameObject gameObject
//    {
//        get => root.transform.GetChild(index).gameObject;
//    }

//    public Vector3 position
//    {
//        get => root.transform.GetChild(index).transform.position;
//        set => root.transform.GetChild(index).transform.position = value;
//    }

//    public SpriteRenderer spriteRenderer
//    {
//        get => root.transform.GetChild(index).GetComponent<SpriteRenderer>();
//    }


//    public Sprite sprite
//    {
//        get => spriteRenderer.sprite;
//        set => spriteRenderer.sprite = value;
//    }
//}

public class ActorBehavior : ExtendedMonoBehavior
{
    public static class Layer
    {
        public const int Glow = 0;
        public const int Back = 1;
        public const int Thumbnail = 2;
        public const int Frame = 3;
        public const int StatusIcon = 4;
        public const int HealthBarBack = 5;
        public const int HealthBar = 6;
        public const int HealthText = 7;
        public const int ActionBarBack = 8;
        public const int ActionBar = 9;
        public const int ActionText = 10;
    }

    //Variables
    [SerializeField] public Archetype archetype;
    [SerializeField] public Vector2Int location = Locations.nowhere;
    [SerializeField] public Vector3? destination = null;
    [SerializeField] public Team team = Team.Independant;

    [SerializeField] public float Level;
    [SerializeField] public float HP;
    [SerializeField] public float MaxHP;
    [SerializeField] public float Attack;
    [SerializeField] public float Defense;
    [SerializeField] public float Accuracy;
    [SerializeField] public float Evasion;
    [SerializeField] public float Speed;
    [SerializeField] public float Luck;



    [SerializeField] public float wait = 0;
    [SerializeField] public float waitDuration = -1;


    [SerializeField] public int spawnTurn = -1;

    [SerializeField] public AnimationCurve bobbing;
    [SerializeField] public Guid guid;

    private void Awake()
    {
        render.glow = gameObject.transform.GetChild(Layer.Glow).GetComponent<SpriteRenderer>();
        render.back = gameObject.transform.GetChild(Layer.Back).GetComponent<SpriteRenderer>();
        render.thumbnail = gameObject.transform.GetChild(Layer.Thumbnail).GetComponent<SpriteRenderer>();
        render.frame = gameObject.transform.GetChild(Layer.Frame).GetComponent<SpriteRenderer>();
        render.statusIcon = gameObject.transform.GetChild(Layer.StatusIcon).GetComponent<SpriteRenderer>();
        render.healthBarBack = gameObject.transform.GetChild(Layer.HealthBarBack).GetComponent<SpriteRenderer>();
        render.healthBar = gameObject.transform.GetChild(Layer.HealthBar).GetComponent<SpriteRenderer>();
        render.healthText = gameObject.transform.GetChild(Layer.HealthText).GetComponent<TextMeshPro>();
        render.actionBarBack = gameObject.transform.GetChild(Layer.ActionBarBack).GetComponent<SpriteRenderer>();
        render.actionBar = gameObject.transform.GetChild(Layer.ActionBar).GetComponent<SpriteRenderer>();
        render.actionText = gameObject.transform.GetChild(Layer.ActionText).GetComponent<TextMeshPro>();
    }

    private void Start()
    {

    }

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


    public Vector3 thumbnailPosition
    {
        get => gameObject.transform.GetChild(Layer.Thumbnail).position;
        set => gameObject.transform.GetChild(Layer.Thumbnail).position = value;
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
            render.glow.sortingOrder = value;
            render.back.sortingOrder = value + Layer.Back;
            render.thumbnail.sortingOrder = value + Layer.Thumbnail;
            render.frame.sortingOrder = value + Layer.Frame;
            render.statusIcon.sortingOrder = value + Layer.StatusIcon;
            render.healthBarBack.sortingOrder = value + Layer.HealthBarBack;
            render.healthBar.sortingOrder = value + Layer.HealthBar;
            render.healthText.sortingOrder = value + Layer.HealthText;
            render.actionBarBack.sortingOrder = value + Layer.ActionBarBack;
            render.actionBar.sortingOrder = value + Layer.ActionBar;
            render.actionText.sortingOrder = value + Layer.ActionText;
        }
    }

    #endregion

    #region Properties

    public TileBehavior currentTile => tiles.First(x => x.location.Equals(location));
    public bool IsPlayer => team.Equals(Team.Player);
    public bool IsEnemy => team.Equals(Team.Enemy);
    public bool IsSelectedPlayer => HasSelectedPlayer && Equals(selectedPlayer);
    public bool IsCurrentPlayer => HasCurrentPlayer && Equals(currentPlayer);
    public bool HasLocation => location != Locations.nowhere;
    public bool HasDestination => destination.HasValue;
    public bool IsNorthEdge => location.y == 1;
    public bool IsEastEdge => location.x == board.columns;
    public bool IsSouthEdge => location.y == board.rows;
    public bool IsWestEdge => location.x == 1;
    public bool IsAlive => HP > 0;
    public bool IsDead => HP < 1;
    public bool IsActive => this != null && isActiveAndEnabled;
    public bool IsInactive => this == null || !isActiveAndEnabled;
    public bool IsSpawnable => !IsActive && IsAlive && spawnTurn <= turnManager.currentTurn;
    public bool IsPlaying => IsAlive && IsActive;
    public bool IsReady => wait == waitDuration;

    public float LevelModifier => 1.0f + Random.Float(0, Level * 0.01f);
    public float AttackModifier => 1.0f + Random.Float(0, Attack * 0.01f);
    public float DefenseModifier => 1.0f + Random.Float(0, Defense * 0.01f);
    public float AccuracyModifier => 1.0f + Random.Float(0, Accuracy * 0.01f);
    public float EvasionModifier => 1.0f + Random.Float(0, Evasion * 0.01f);
    public float LuckModifier => 1.0f + Random.Float(0, Luck * 0.01f);


    public bool IsSameColumn(Vector2Int other) => this.location.x == other.x;
    public bool IsSameRow(Vector2Int other) => this.location.y == other.y;
    public bool IsNorthOf(Vector2Int other) => IsSameColumn(other) && this.location.y == other.y - 1;
    public bool IsEastOf(Vector2Int other) => IsSameRow(other) && this.location.x == other.x + 1;
    public bool IsSouthOf(Vector2Int other) => IsSameColumn(other) && this.location.y == other.y + 1;
    public bool IsWestOf(Vector2Int other) => IsSameRow(other) && this.location.x == other.x - 1;
    public bool IsNorthWestOf(Vector2Int other) => this.location.x == other.x - 1 && this.location.y == other.y - 1;
    public bool IsNorthEastOf(Vector2Int other) => this.location.x == other.x + 1 && this.location.y == other.y - 1;
    public bool IsSouthWestOf(Vector2Int other) => this.location.x == other.x - 1 && this.location.y == other.y + 1;
    public bool IsSouthEastOf(Vector2Int other) => this.location.x == other.x + 1 && this.location.y == other.y + 1;
    public bool IsAdjacentTo(Vector2Int other) => (IsSameColumn(other) || IsSameRow(other)) && Vector2Int.Distance(this.location, other).Equals(1);



    private Vector2Int GoNorth() => location += new Vector2Int(0, -1);
    private Vector2Int GoEast() => location += new Vector2Int(1, 0);
    private Vector2Int GoSouth() => location += new Vector2Int(0, 1);
    private Vector2Int GoWest() => location += new Vector2Int(-1, 0);

    #endregion

    #region Methods

    public void Init(bool spawn)
    {
        transform.localScale = tileScale;
        gameObject.SetActive(false);

        if (spawn && HasLocation)
            Spawn();
    }


    public void Spawn(Vector2Int? startLocation = null)
    {
        gameObject.SetActive(true);

        if (startLocation.HasValue)
            location = startLocation.Value;

        position = Geometry.PositionFromLocation(location);

        if (this.IsPlayer)
        {
            render.SetColor(Colors.Transparent.White);
            render.SetBackColor(Colors.Transparent.White);
            render.actionBarBack.enabled = false;
            render.actionBar.enabled = false;
            render.actionText.enabled = false;
            SetActionIcon(ActionIcon.None);
        }
        else if (this.IsEnemy)
        {
            render.SetColor(Colors.Transparent.White);
            render.SetBackColor(Colors.Transparent.Red);
            CalculateWait();
            wait = turnManager.currentTurn == 1 ? Random.Float(0, waitDuration) : 0;
        }

        UpdateHealthBar();
        //UpdateActionBar();

        float delay = turnManager.currentTurn == 1 ? 0 : Random.Float(0f, 2f);
        StartCoroutine(SpawnIn(delay));
    }

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
        //Check abort state
        if (other == null || HasDestination)
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
            //Debug.Log($"Conflict: {this.archetype} / {startLocation.archetype}");

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

        //TODO: Move based on enum (MoveAnywhere, MoveNearest, MoveStrongest, MoveWeakest, etc)...

        SetActionIcon(ActionIcon.Move);

        var location = new Vector2Int(Random.Int(1, board.columns), Random.Int(1, board.rows));
        var closestTile = Geometry.ClosestTileByLocation(location);
        destination = closestTile.position;
    }

    private void MoveTowardCursor()
    {
        //Check abort state
        if (!IsSelectedPlayer && !IsCurrentPlayer)
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
        //Check abort state
        if (!HasDestination) 
            return;

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
        //Check abort state
        if (!IsAlive || !IsActive || !IsPlayer || !turnManager.IsStartPhase) 
            return;

        //Source: https://forum.unity.com/threads/how-to-make-an-object-move-up-and-down-on-a-loop.380159/
        var pos = new Vector3(
            transform.position.x,
            transform.position.y + (bobbing.Evaluate(Time.time % bobbing.length) * (tileSize / 24)),
            transform.position.z);

        //render.glow.transform.position = pos;
        //render.thumbnail.transform.position = pos;
        //render.frame.transform.position = pos;
        render.back.transform.position = pos;
    }


    private void CheckThrobbing()
    {
        //Check abort state
        if (!IsAlive || !IsActive || !IsPlayer || !turnManager.IsStartPhase) 
            return;

        //Source: https://forum.unity.com/threads/how-to-make-an-object-move-up-and-down-on-a-loop.380159/
        var scale = new Vector3(
            1.5f + (bobbing.Evaluate(Time.time % bobbing.length) * (tileSize / 24)),
            1.5f + (bobbing.Evaluate(Time.time % bobbing.length) * (tileSize / 24)),
            1);
        render.back.transform.localScale = scale;

        //var color = new Color(0, 1, 0, 1);
        var color = new Color(1, 1, 1, 1);
        render.back.color = color;
    }

    public void Shake(float intensity)
    {
        gameObject.transform.GetChild(Layer.Thumbnail).gameObject.transform.position = currentTile.position;
        gameObject.transform.GetChild(Layer.Frame).gameObject.transform.position = currentTile.position;

        if (intensity > 0)
        {
            var amount = new Vector3(Random.Range(-intensity), Random.Range(intensity), 1);
            gameObject.transform.GetChild(Layer.Thumbnail).gameObject.transform.position += amount;
            gameObject.transform.GetChild(Layer.Frame).gameObject.transform.position += amount;
        }

    }



    #endregion


    void Update()
    {
        //Check abort state
        if (!IsAlive || !IsActive) 
            return;

        if (IsSelectedPlayer || IsCurrentPlayer)
            MoveTowardCursor();

        var closestTile = Geometry.ClosestTileByPosition(this.position);
        if (closestTile.location.Equals(this.location))
            return;

        soundSource.PlayOneShot(resourceManager.SoundEffect($"Move{Random.Int(1, 6)}"));

        //Determine if selected player and another actor are occupying the same tile
        var actor = actors.FirstOrDefault(x => x != null && x.IsAlive && x.IsActive && !x.Equals(currentPlayer) && x.location.Equals(closestTile.location));
        if (actor != null)
        {
            actor.SwapLocation(this);
        }

        location = closestTile.location;

    }



    void FixedUpdate()
    {
        //Check abort state
        if (!IsAlive || !IsActive || IsSelectedPlayer || IsCurrentPlayer) 
            return;

        CheckMovement();
        //CheckBobbing();
        CheckThrobbing();


        FillActionBar();

    }

    public IEnumerator TakeDamage(float damage)
    {
        var remainingHP = Mathf.Clamp(HP - damage, 0, MaxHP);

        while (HP > remainingHP)
        {

            //Decrease HP
            var min = (int)Math.Max(1, damage * 0.1f);
            var max = (int)Math.Max(1, damage * 0.3f);
            var hit = Random.Int(min, max);
            HP -= hit;
            HP = Mathf.Clamp(HP, remainingHP, MaxHP);

            //SpawnIn hit text
            damageTextManager.Spawn(hit.ToString(), position);

            //Shake actor
            Shake(ShakeIntensity.Medium);

            UpdateHealthBar();

            //SlideIn sfx
            soundSource.PlayOneShot(resourceManager.SoundEffect($"Slash{Random.Int(1, 7)}"));

            yield return Wait.For(Interval.Five);
        }

        Shake(ShakeIntensity.Stop);
        HP = remainingHP;
        position = currentTile.position;
    }


    public IEnumerator MissAttack()
    {

        float ticks = 0;
        float duration = Interval.QuarterSecond;

        while (ticks < duration)
        {
            ticks += Interval.One;
            Shake(ShakeIntensity.Low);
            yield return Wait.Tick();
        }

        Shake(ShakeIntensity.Stop);
    }

    public void FillActionBar()
    {
        //Check abort state
        if (turnManager.IsEnemyTurn || !turnManager.IsStartPhase || titleManager.color.a > 0.5f || !IsAlive || !IsActive) 
            return;
  
        if (wait < waitDuration)
            wait += Time.deltaTime;
        else
            wait = waitDuration;

        UpdateActionBar();
    }

    private void UpdateActionBar()
    {
        var scale = render.actionBarBack.transform.localScale;
        var x = Mathf.Clamp(scale.x * (wait / waitDuration), 0, scale.x);
        render.actionBar.transform.localScale = new Vector3(x, scale.y, scale.z);
        render.actionText.text = wait < waitDuration ? $@"{Math.Round(wait / waitDuration * 100)}%" : "Ready!";
    }

    private void UpdateHealthBar()
    {
        var scale = render.healthBarBack.transform.localScale;
        var x = Mathf.Clamp(scale.x * (HP / MaxHP), 0, scale.x);
        render.healthBar.transform.localScale = new Vector3(x, scale.y, scale.z);
        render.healthText.text = $@"{HP}/{MaxHP}";
    }


    public IEnumerator Dissolve()
    {
        var alpha = 1f;
        render.SetColor(new Color(1, 1, 1, alpha));

        portraitManager.Dissolve(this);
        soundSource.PlayOneShot(resourceManager.SoundEffect("Death"));
        sortingOrder = ZAxis.Max;

        while (alpha > 0)
        {
            alpha -= Increment.OnePercent;
            alpha = Mathf.Clamp(alpha, 0, 1);
            render.SetColor(new Color(1, 1, 1, alpha));
            yield return Wait.Tick();
        }
    }

    public void SetActionIcon(ActionIcon icon)
    {
        //Check abort state
        if (!IsActive) 
            return;

        render.statusIcon.sprite = resourceManager.statusSprites.First(x => x.id.Equals(icon.ToString())).thumbnail;
    }


    public void StartGlow(Color color)
    {
        //Check abort state
        if (!IsActive)
            return;

        render.SetGlowColor(color);
        StartCoroutine(GlowIn());
    }

    public void StopGlow()
    {
        //Check abort state
        if (!IsActive) 
            return;

        StartCoroutine(GlowOut());
    }


    public void Die()
    {
        //Check abort state
        if (!IsActive) 
            return;

        StartCoroutine(Death());
    }

    public IEnumerator GlowIn()
    {
        float alpha = 0;
        render.SetGlowAlpha(alpha);

        while (alpha < 1)
        {
            alpha += Increment.TwoPercent;
            alpha = Mathf.Clamp(alpha, 0, 1);
            render.SetGlowAlpha(alpha);
            yield return Wait.Tick();
        }

        render.SetGlowAlpha(1);
    }

    public IEnumerator GlowOut()
    {
        float alpha = render.glowColor.a;
        while (alpha > 0)
        {
            alpha -= Increment.TwoPercent;
            alpha = Mathf.Clamp(alpha, 0, 1);
            render.SetGlowAlpha(alpha);
            yield return Wait.Tick();
        }

        render.SetGlowAlpha(0);
    }

    public void CalculateWait()
    {
        //Check abort state
        if (!IsAlive && !IsActive) 
            return;

        //TODO: Calculate based on stats....
        float min = Interval.OneSecond * 20;
        float max = Interval.OneSecond * 40;

        wait = 0;
        waitDuration = Random.Float(min, max);

        UpdateActionBar();

        SetActionIcon(ActionIcon.None);
    }



    public IEnumerator SpawnIn(float delay = 0)
    {
        float alpha = 0;
        render.SetColor(new Color(1, 1, 1, alpha));
        render.SetBackAlpha(alpha);
        render.SetGlowAlpha(0);

        yield return Wait.For(delay);

        while (alpha < 1)
        {
            alpha += Increment.OnePercent;
            alpha = Mathf.Clamp(alpha, 0, 1);
            render.SetColor(new Color(1, 1, 1, alpha));
            render.SetBackAlpha(alpha);
            yield return Wait.Tick();
        }

        render.SetColor(Colors.Solid.White);
        render.SetBackAlpha(0.5f);
    }

    public IEnumerator Death()
    {
        float alpha = 1;
        while (alpha > 0)
        {
            alpha -= Increment.OnePercent;
            alpha = Mathf.Clamp(alpha, 0, 1);
            render.SetGlowAlpha(alpha);
            render.SetBackAlpha(alpha);
            yield return Wait.Tick();
        }

        render.SetGlowAlpha(0);
        render.SetBackAlpha(0);
        Destroy(this.gameObject);
        actors.Remove(this);
    }



    //public void CheckLocationConflict()
    //{
    //    var other = actors.FirstOrDefault(x => x != null && x.IsAlive && x.IsActive && !Equals(x) && location.Equals(x.location));
    //    if (other == null)
    //        return;

    //    SwapLocation(other);
    //}


}
