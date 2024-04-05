using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using TMPro;
using UnityEngine;
using static Unity.VisualScripting.Member;


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
        public const int HealthBarBack = 4;
        public const int HealthBar = 5;
        public const int StatusIcon = 6;
        public const int TurnDelay = 7;
        public const int HealthText = 8;
    }

    //Variables
    [SerializeField] public Archetype archetype;
    [SerializeField] public Vector2Int location = Locations.nowhere;
    [SerializeField] public Vector3? destination = null;
    [SerializeField] public Team team = Team.Independant;
    [SerializeField] public int HP;
    [SerializeField] public int MaxHP;

    public int spawnTurn = -1;
    private int enemyTurnDelay = 0;

    [SerializeField] public AnimationCurve bobbing;

    public Guid guid;

    private void Awake()
    {
        render.glow = gameObject.transform.GetChild(Layer.Glow).GetComponent<SpriteRenderer>();
        render.back = gameObject.transform.GetChild(Layer.Back).GetComponent<SpriteRenderer>();
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
     
    }

    public void Init(bool spawn)
    {
        transform.localScale = tileScale;
        render.healthBar.transform.localScale = render.healthBarBack.transform.localScale;
        HP = MaxHP;
        PrintHealth();
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
            render.turnDelay.gameObject.SetActive(false);
            SetActionIcon(ActionIcon.None);
        }
        else if (this.IsEnemy)
        {
            render.SetColor(Colors.Transparent.White);
            render.SetBackColor(Colors.Transparent.Red);
            SetEnemyTurnDelay(EnemyTurnDelay.Random);
        }

       
        float delay = turnManager.currentTurn == 1 ? 0 : Random.Float(0f, 2f);
        StartCoroutine(SpawnIn(delay));
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
            render.back.sortingOrder = value + 1;
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


    public Vector3 HealthBarBackScale => render.healthBarBack.transform.localScale;


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
        var location = new Vector2Int(Random.Int(1, board.columns), Random.Int(1, board.rows));
        var closestTile = Geometry.ClosestTileByLocation(location);
        destination = closestTile.position;
    }


    #endregion





    void Update()
    {
        if (!IsAlive) return;

        if (IsTargettedPlayer || IsSelectedPlayer)
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
        if (!IsTargettedPlayer && !IsSelectedPlayer)
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
        render.back.transform.position = pos;
    }


    private void CheckThrobbing()
    {
        if (!IsAlive || !IsActive || !IsPlayer || !turnManager.IsStartPhase) return;

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
            var min = (damageTaken.ToFloat() * 0.1f).ToInt();
            var max = (damageTaken.ToFloat() * 0.3f).ToInt();
            var damage = Random.Int(min, max);
            HP -= damage;
            HP = Mathf.Clamp(HP, remainingHP, MaxHP);

            //SpawnIn damage text
            damageTextManager.Spawn(damage.ToString(), position);

            //Shake actor
            Shake(ShakeIntensity.Medium);

            //Resize health bar
            if (HP > 0)
            {
                render.healthBar.transform.localScale = new Vector3(
                    HealthBarBackScale.x * (HP.ToFloat() / MaxHP.ToFloat()), 
                    HealthBarBackScale.y, 
                    HealthBarBackScale.z);
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
        if (!IsActive) return;
        render.statusIcon.sprite = resourceManager.statusSprites.First(x => x.id.Equals(icon.ToString())).thumbnail;
    }


    public void StartGlow(Color color)
    {
        if (!IsActive) return;
        render.SetGlowColor(color);
        StartCoroutine(GlowIn());
    }

    public void StopGlow()
    {
        if (!IsActive) return;
        StartCoroutine(GlowOut());
    }


    public void Die()
    {
        if (!IsActive) return;
        StartCoroutine(Death());
    }

    public IEnumerator GlowIn()
    {
        float alpha = 0;
        render.SetGlowAlpha(alpha);

        while (alpha < 1)
        {
            alpha += Increment.OnePercent;
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
            alpha -= Increment.OnePercent;
            alpha = Mathf.Clamp(alpha, 0, 1);
            render.SetGlowAlpha(alpha);
            yield return Wait.Tick();
        }

        render.SetGlowAlpha(0);
    }

    public void SetEnemyTurnDelay(EnemyTurnDelay turnDelay, int min = 2, int max = 4)
    {
        if (!IsActive || turnDelay.Equals(EnemyTurnDelay.None)) return;

        enemyTurnDelay = Random.Int(min, max);
        render.turnDelay.text = $"x{enemyTurnDelay}";
        render.turnDelay.gameObject.SetActive(true);
        SetActionIcon(ActionIcon.Sleep);
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



    public void SetLocation(Vector2Int location)
    {
        this.location = location;
        this.position = Geometry.PositionFromLocation(this.location);
    }

}
