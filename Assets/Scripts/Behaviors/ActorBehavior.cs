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
    //Constants
    static class Layer
    {
        public const int Glow = 0;
        public const int Shadow = 1;
        public const int Thumbnail = 2;
        public const int Frame = 3;
        public const int HealthBarBack = 4;
        public const int HealthBar = 5;
        public const int StatusIcon = 6;
        public const int TurnDelay = 7;
        public const int HealthText = 8;
    }


    //Variables
    [SerializeField] public Archetype archetype;
    [SerializeField] public Vector2Int location = Vector2Int.zero;
    [SerializeField] public Vector3? destination = null;
    [SerializeField] public Team team = Team.Independant;
    [SerializeField] public int HP;
    [SerializeField] public int MaxHP;



    public Color glow;
    public Color shadow = new Color(1, 1, 1, 0.5f);


    public int spawnTurn = -1;
    private int enemyTurnDelay = 0;

    [SerializeField] public AnimationCurve bobbing;



    private void Awake()
    {
        render.glow = gameObject.transform.GetChild(Layer.Glow).GetComponent<SpriteRenderer>();
        render.shadow = gameObject.transform.GetChild(Layer.Shadow).GetComponent<SpriteRenderer>();
        render.thumbnail = gameObject.transform.GetChild(Layer.Thumbnail).GetComponent<SpriteRenderer>();
        render.frame = gameObject.transform.GetChild(Layer.Frame).GetComponent<SpriteRenderer>();
        render.healthBarBack = gameObject.transform.GetChild(Layer.HealthBarBack).GetComponent<SpriteRenderer>();
        render.healthBar = gameObject.transform.GetChild(Layer.HealthBar).GetComponent<SpriteRenderer>();
        render.statusIcon = gameObject.transform.GetChild(Layer.StatusIcon).GetComponent<SpriteRenderer>();
        render.turnDelay = gameObject.transform.GetChild(Layer.TurnDelay).GetComponent<TextMeshPro>();
        render.healthText = gameObject.transform.GetChild(Layer.HealthText).GetComponent<TextMeshPro>();
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        gameObject.SetActive(true);
        position = Geometry.PositionFromLocation(location);
        destination = null;
        transform.localScale = tileScale;
        render.healthBar.transform.localScale = render.healthBarBack.transform.localScale;
        HP = MaxHP;
        PrintHealth();

        if (turnManager.IsFirstTurn)
        {
            //render.SetColor(Color.white);

            if (this.IsPlayer)
            {
                //render.SetShadow(new Color(1, 1, 1, 0.5f));
                render.turnDelay.gameObject.SetActive(false);
                Set(ActionIcon.None);
            }
            else
            {
                //render.SetShadow(new Color(1, 0, 0, 0.5f));
                Set(EnemyTurnDelay.Random);
            }

        }

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
            render.shadow.sortingOrder = value + 1;
            render.thumbnail.sortingOrder = value + 2;
            render.frame.sortingOrder = value + 3;
            render.healthBarBack.sortingOrder = value + 4;
            render.healthBar.sortingOrder = value + 5;
            render.statusIcon.sortingOrder = value + 6;
            render.turnDelay.sortingOrder = value + 7;
            render.healthText.sortingOrder = value + 8;
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
    public bool HasSpawned => spawnTurn > turnManager.turnNumber;

    #endregion

    #region Methods



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
            //Debug.Log($"Conflict: {this.archetype} / {location.archetype}");

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





    void Update()
    {
        if (!IsAlive) return;

        if (IsSelectedPlayer)
            MoveTowardCursor();

        var closestTile = Geometry.ClosestTileByPosition(this.position);
        if (closestTile.location.Equals(this.location))
            return;

        soundSource.PlayOneShot(resourceManager.SoundEffect($"Move{Random.Int(1, 6)}"));

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
        //CheckBobbing();
        CheckThrobbing();

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

        //render.glow.transform.position = pos;
        //render.thumbnail.transform.position = pos;
        //render.frame.transform.position = pos;
        render.shadow.transform.position = pos;
    }


    private void CheckThrobbing()
    {
        if (!IsAlive || !IsActive || !IsPlayer || !turnManager.IsStartPhase) return;

        //Source: https://forum.unity.com/threads/how-to-make-an-object-move-up-and-down-on-a-loop.380159/
        var scale = new Vector3(
            1.5f + (bobbing.Evaluate(Time.time % bobbing.length) * (tileSize / 24)),
            1.5f + (bobbing.Evaluate(Time.time % bobbing.length) * (tileSize / 24)),
            1);
        render.shadow.transform.localScale = scale;

        //var color = new Color(0, 1, 0, 1);
        var color = new Color(1, 1, 1, 1);
        render.shadow.color = color;
    }

    private void Shake(float intensity)
    {
        gameObject.transform.GetChild(Layer.Thumbnail).gameObject.transform.position = currentTile.position;
        if (intensity > 0)
            gameObject.transform.GetChild(Layer.Thumbnail).gameObject.transform.position += new Vector3(Random.Range(-intensity), Random.Range(intensity), 1);
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
            Shake(ShakeIntensity.Medium);

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

            yield return Wait.For(Interval.Five);
        }

        Shake(ShakeIntensity.Stop);
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

        Destroy(this.gameObject);
        actors.Remove(this);
    }

    public void Set(ActionIcon icon)
    {
        if (!IsActive) return;
        render.statusIcon.sprite = resourceManager.statusSprites.First(x => x.id.Equals(icon.ToString())).thumbnail;
    }

    public void Set(GlowState state)
    {
        if (!IsActive) return;
        StartCoroutine(state == GlowState.On ? GlowFadeIn(new Color(1, 1, 1, 0)) : GlowFadeOut(new Color(1, 1, 1, 1)));
    }

    public void Set(GlowState state, Color color)
    {
        if (!IsActive) return;
        StartCoroutine(state == GlowState.On ? GlowFadeIn(color) : GlowFadeOut(color));
    }

    public IEnumerator GlowFadeIn(Color color)
    {
        float maxAlpha = 1;
        float alpha = 0;
        color.a = alpha;
        render.glow.color = color;

        while (alpha < maxAlpha)
        {
            alpha += Increment.OnePercent;
            alpha = Mathf.Clamp(alpha, 0, 1);
            color.a = alpha;
            render.glow.color = color;

            yield return Wait.Tick();
        }

        color.a = maxAlpha;
        render.glow.color = color;
    }

    public IEnumerator GlowFadeOut(Color color)
    {
        float minAlpha = 0;
        float alpha = 1;
        color.a = alpha;
        render.glow.color = color;

        while (alpha > minAlpha)
        {
            alpha -= Increment.OnePercent;
            alpha = Mathf.Clamp(alpha, 0, 1);
            color.a = alpha;
            render.glow.color = color;

            yield return Wait.Tick();
        }

        color.a = minAlpha;
        render.glow.color = color;
    }


    public void Set(EnemyTurnDelay turnDelay, int min = 2, int max = 4)
    {
        if (!IsActive || turnDelay.Equals(EnemyTurnDelay.None)) return;

        enemyTurnDelay = Random.Int(min, max);
        render.turnDelay.text = $"x{enemyTurnDelay}";
        render.turnDelay.gameObject.SetActive(true);
        Set(ActionIcon.Sleep);
    }

    public IEnumerator FadeIn(float delay = 0)
    {
        float maxAlpha = 1;
        float alpha = 0;
        render.SetColor(new Color(1, 1, 1, alpha));

        yield return Wait.For(delay);

        while (alpha < maxAlpha)
        {
            alpha += Increment.OnePercent;
            alpha = Mathf.Clamp(alpha, 0, 1);
            render.SetColor(new Color(1, 1, 1, alpha));
            render.SetShadow(new Color(shadow.r, shadow.g, shadow.b, Mathf.Min(alpha, 0.5f)));

            Shake(ShakeIntensity.Low);

            yield return Wait.Tick();
        }

        Shake(ShakeIntensity.Stop);
        render.SetColor(new Color(1, 1, 1, maxAlpha));
        position = currentTile.position;
    }

    public IEnumerator FadeOut(float delay = 0)
    {
        float minAlpha = 0;
        float alpha = 1;
        render.SetColor(new Color(1, 1, 1, alpha));
        render.SetShadow(new Color(shadow.r, shadow.g, shadow.b, Mathf.Max(alpha, 0)));

        yield return Wait.For(delay);

        while (alpha > minAlpha)
        {
            alpha -= Increment.OnePercent;
            alpha = Mathf.Clamp(alpha, 0, 1);
            render.SetColor(new Color(1, 1, 1, alpha));


            Shake(ShakeIntensity.Low);

            yield return Wait.Tick();
        }

        Shake(ShakeIntensity.Stop);
        render.SetColor(new Color(1, 1, 1, minAlpha));
        this.position = currentTile.position;
    }

    public IEnumerator FadeInOut(float fadeInDelay = 0, float intermissionDelay = 2f, float fadeOutDelay = 0)
    {
        yield return FadeIn(fadeInDelay);
        yield return Wait.For(intermissionDelay);
        yield return FadeOut(fadeOutDelay);
    }



}
