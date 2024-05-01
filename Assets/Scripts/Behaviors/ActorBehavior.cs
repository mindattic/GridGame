using System;
using System.Collections;
using System.Linq;
using TMPro;
using Unity.Burst.CompilerServices;
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
        public const int Back = 0;
        public const int Glow = 1;
        public const int Thumbnail = 2;
        public const int Frame = 3;
        public const int StatusIcon = 4;
        public const int HealthBarBack = 5;
        public const int HealthBar = 6;
        public const int HealthBarFront = 7;
        public const int HealthText = 8;
        public const int ActionBarBack = 9;
        public const int ActionBar = 10;
        public const int ActionText = 11;
        public const int RadialBack = 12;
        public const int RadialFill = 13;
        public const int Focused = 14;
    }

    //Variables
    [SerializeField] public Archetype archetype;
    [SerializeField] public Vector2Int location = Locations.nowhere;
    [SerializeField] public Vector3? destination = null;
    [SerializeField] public Team team = Team.Independant;
    [SerializeField] public Quality quality = Colors.Common;
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

    [SerializeField] public AnimationCurve flickering;
    [SerializeField] public AnimationCurve bobbing;
    [SerializeField] public Guid guid;


    [SerializeField] public float backScale = 1.4f;

    private void Awake()
    {
        render.back = gameObject.transform.GetChild(Layer.Back).GetComponent<SpriteRenderer>();
        render.glow = gameObject.transform.GetChild(Layer.Glow).GetComponent<SpriteRenderer>();
        render.thumbnail = gameObject.transform.GetChild(Layer.Thumbnail).GetComponent<SpriteRenderer>();
        render.frame = gameObject.transform.GetChild(Layer.Frame).GetComponent<SpriteRenderer>();
        render.statusIcon = gameObject.transform.GetChild(Layer.StatusIcon).GetComponent<SpriteRenderer>();
        render.healthBarBack = gameObject.transform.GetChild(Layer.HealthBarBack).GetComponent<SpriteRenderer>();
        render.healthBar = gameObject.transform.GetChild(Layer.HealthBar).GetComponent<SpriteRenderer>();
        render.healthBarFront = gameObject.transform.GetChild(Layer.HealthBarFront).GetComponent<SpriteRenderer>();
        render.healthText = gameObject.transform.GetChild(Layer.HealthText).GetComponent<TextMeshPro>();
        render.actionBarBack = gameObject.transform.GetChild(Layer.ActionBarBack).GetComponent<SpriteRenderer>();
        render.actionBar = gameObject.transform.GetChild(Layer.ActionBar).GetComponent<SpriteRenderer>();
        render.actionText = gameObject.transform.GetChild(Layer.ActionText).GetComponent<TextMeshPro>();
        render.radialBack = gameObject.transform.GetChild(Layer.RadialBack).GetComponent<SpriteRenderer>();
        render.radialFill = gameObject.transform.GetChild(Layer.RadialFill).GetComponent<SpriteRenderer>();
        render.focused = gameObject.transform.GetChild(Layer.Focused).GetComponent<SpriteRenderer>();

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
            render.back.sortingOrder = value + Layer.Back;
            render.glow.sortingOrder = value;
            render.thumbnail.sortingOrder = value + Layer.Thumbnail;
            render.frame.sortingOrder = value + Layer.Frame;
            render.statusIcon.sortingOrder = value + Layer.StatusIcon;
            render.healthBarBack.sortingOrder = value + Layer.HealthBarBack;
            render.healthBar.sortingOrder = value + Layer.HealthBar;
            render.healthBarFront.sortingOrder = value + Layer.HealthBarFront;
            render.healthText.sortingOrder = value + Layer.HealthText;
            render.actionBarBack.sortingOrder = value + Layer.ActionBarBack;
            render.actionBar.sortingOrder = value + Layer.ActionBar;
            render.actionText.sortingOrder = value + Layer.ActionText;
            render.radialBack.sortingOrder = value + Layer.RadialBack;
            render.radialFill.sortingOrder = value + Layer.RadialFill;
            render.focused.sortingOrder = value + Layer.Focused;




        }
    }

    #endregion

    #region Properties

    public TileBehavior currentTile => tiles.First(x => x.location.Equals(location));
    public bool IsPlayer => team.Equals(Team.Player);
    public bool IsEnemy => team.Equals(Team.Enemy);
    public bool IsSelectedPlayer => HasFocusedPlayer && Equals(focusedPlayer);
    public bool IsCurrentPlayer => HasSelectedPlayer && Equals(selectedPlayer);
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

        if (spawn)
            Spawn();
    }


    public void Spawn(Vector2Int? startLocation = null)
    {
        if (startLocation.HasValue)
            location = startLocation.Value;

        if (!HasLocation)
            return;

        gameObject.SetActive(true);

        position = Geometry.PositionFromLocation(location);

        if (this.IsPlayer)
        {
            render.SetAlpha(0);
            render.SetBackColor(quality.Color);
            render.SetGlowColor(quality.Color);
            render.actionBarBack.enabled = false;
            render.actionBar.enabled = false;
            render.actionText.enabled = false;
            render.radialBack.enabled = false;
            render.radialFill.enabled = false;
        }
        else if (this.IsEnemy)
        {
            render.SetAlpha(0);
            render.SetBackColor(Colors.Translucent.Red);
            render.SetGlowColor(Colors.Translucent.Red);
            render.SetGlowAlpha(0);
            CalculateWait();
            wait = turnManager.currentTurn == 1 ? Random.Float(0, waitDuration / 4) : 0;
        }


        render.back.transform.localScale = new Vector3(backScale, backScale, 1);

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






        //Randomy select an attack strategy
        //var strategy = Random.Strategy();
        var strategy = AttackStrategy.AttackClosest;

        switch (strategy)
        {
            case AttackStrategy.MoveAnywhere:
                var location = new Vector2Int(Random.Int(1, board.columns), Random.Int(1, board.rows));
                destination = Geometry.ClosestTileByLocation(location).position;
                break;

            case AttackStrategy.AttackClosest:
                #region Attack Strategy: Attack Closest

                //Find closest player
                var closestPlayer = players.Where(x => x != null && x.IsAlive && x.IsActive).OrderBy(x => Vector3.Distance(x.position, position)).FirstOrDefault();

                //Determine if already adjacent to player...
                if (IsAdjacentTo(closestPlayer.location))
                {
                    destination = position;
                    return;
                }

                //...Otherwise, Find closest unoccupied tile adjacent to player...
                var closestUnoccupiedAdjacentTile = Geometry.ClosestUnoccupiedAdjacentTileByLocation(closestPlayer.location);
                if (closestUnoccupiedAdjacentTile != null)
                {
                    destination = closestUnoccupiedAdjacentTile.position;
                    return;
                }

                //...Otherwise, Find closest tile adjacent to player...
                var closestAdjacentTile = Geometry.ClosestAdjacentTileByLocation(closestPlayer.location);
                if (closestAdjacentTile != null)
                {
                    destination = closestAdjacentTile.position;
                    return;
                }

                //...Otherwise, find closest unoccupied tile to player...
                var closestUnoccupiedTile = Geometry.ClosestUnoccupiedTileByLocation(closestPlayer.location);
                if (closestUnoccupiedTile != null)
                {
                    destination = closestUnoccupiedTile.position;
                    return;
                }

                //...Otherwise, find closest tile to player
                var closestTile = Geometry.ClosestTileByLocation(closestPlayer.location);
                if (closestTile != null)
                {
                    destination = closestTile.position;
                    return;
                }

                #endregion
                break;

            case AttackStrategy.AttackWeakest:
                var weakestPlayer = players.Where(x => x != null && x.IsAlive && IsActive).OrderBy(x => x.HP).FirstOrDefault();
                destination = Geometry.ClosestUnoccupiedAdjacentTileByLocation(weakestPlayer.location).position;
                break;
        }


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
        //position = Vector2.MoveTowards(position, cursorPosition, cursorSpeed);

        //Snap selected player to cursor
        position = cursorPosition;
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
        if (!IsAlive || !IsActive || !turnManager.IsStartPhase || (turnManager.IsPlayerTurn && !IsPlayer) || (turnManager.IsEnemyTurn && !IsEnemy))
            return;

        //Source: https://forum.unity.com/threads/how-to-make-an-object-move-up-and-down-on-a-loop.380159/
        var scale = new Vector3(
            backScale + (bobbing.Evaluate(Time.time % bobbing.length) * (tileSize / 24)),
            backScale + (bobbing.Evaluate(Time.time % bobbing.length) * (tileSize / 24)),
            1);
        render.SetBackScale(scale);
        render.SetBackColor(IsPlayer ? quality.Color : Colors.Translucent.Red);




    }


    private void CheckFlicker()
    {
        //Check abort state
        if (!IsAlive || !IsActive || !turnManager.IsStartPhase || (turnManager.IsPlayerTurn && !IsPlayer) || (turnManager.IsEnemyTurn && !IsEnemy))
            return;

        var alpha = 0.5f + (bobbing.Evaluate(Time.time % bobbing.length) * (tileSize / 24));
        render.SetGlowAlpha(alpha);
    }

    public void Shake(float intensity)
    {
        gameObject.transform.GetChild(Layer.Thumbnail).gameObject.transform.position = currentTile.position;

        if (intensity > 0)
        {
            var amount = new Vector3(Random.Range(-intensity), Random.Range(intensity), 1);
            gameObject.transform.GetChild(Layer.Thumbnail).gameObject.transform.position += amount;
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
        var actor = actors.FirstOrDefault(x => x != null && x.IsAlive && x.IsActive && !x.Equals(selectedPlayer) && x.location.Equals(closestTile.location));
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
        //CheckFlicker();

        CheckActionBar();

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
        yield return Wait.For(Interval.QuarterSecond);

        float ticks = 0;
        float duration = Interval.QuarterSecond;

        damageTextManager.Spawn("Miss", position);

        while (ticks < duration)
        {
            ticks += Interval.One;
            Shake(ShakeIntensity.Low);
            yield return Wait.Tick();
        }

        Shake(ShakeIntensity.Stop);
    }

    public void CheckActionBar()
    {
        //Check abort state
        if (turnManager.IsEnemyTurn || (!turnManager.IsStartPhase && !turnManager.IsMovePhase) || !IsAlive || !IsActive || wait == waitDuration)
            return;

        if (wait < waitDuration)
        {
            wait += Time.deltaTime;
            wait = Math.Clamp(wait, 0, waitDuration);
        }
      
        if(wait == waitDuration)
        {
            StartCoroutine(RadialBackFadeOut());
        }

        UpdateActionBar();
        UpdateRadialFill();
    }

    private void UpdateActionBar()
    {
        var scale = render.actionBarBack.transform.localScale;
        var x = Mathf.Clamp(scale.x * (wait / waitDuration), 0, scale.x);
        render.actionBar.transform.localScale = new Vector3(x, scale.y, scale.z);
        //render.actionText.text = wait < waitDuration ? $@"{Math.Round(wait / waitDuration * 100)}%" : "Ready!";
        render.actionText.text = wait < waitDuration ? $"{Math.Round(waitDuration - wait)}" : "";
    }

    private void UpdateRadialFill()
    {

        var fill = (360 * (wait / waitDuration));
        render.radialFill.material.SetFloat("_Arc1", fill);
    }



    private void UpdateHealthBar()
    {
        var scale = render.healthBarFront.transform.localScale;
        var x = Mathf.Clamp(scale.x * (HP / MaxHP), 0, scale.x);
        render.healthBar.transform.localScale = new Vector3(x, scale.y, scale.z);
        render.healthText.text = $@"{HP}/{MaxHP}";
    }


    public IEnumerator RadialBackFadeIn()
    {
        var maxAlpha = 0.5f;
        var alpha = 0f;
        render.radialBack.color = new Color(0, 0, 0, alpha);

        while (alpha < maxAlpha)
        {
            alpha += Increment.OnePercent;
            alpha = Mathf.Clamp(alpha, 0, maxAlpha);
            render.radialBack.color = new Color(0, 0, 0, alpha);
            yield return Wait.Tick();
        }

        render.radialBack.color = new Color(0, 0, 0, maxAlpha);
    }

    public IEnumerator RadialBackFadeOut()
    {
        var maxAlpha = 0.5f;
        var alpha = maxAlpha;
        render.radialBack.color = new Color(0, 0, 0, maxAlpha);

        while (alpha > 0)
        {
            alpha -= Increment.OnePercent;
            alpha = Mathf.Clamp(alpha, 0, maxAlpha);
            render.radialBack.color = new Color(0, 0, 0, alpha);
            yield return Wait.Tick();
        }

        render.radialBack.color = new Color(0, 0, 0, 0);
    }

    public IEnumerator Dissolve()
    {
        var alpha = 1f;
        render.SetAlpha(alpha);

        portraitManager.Dissolve(this);
        soundSource.PlayOneShot(resourceManager.SoundEffect("Death"));
        sortingOrder = ZAxis.Max;

        while (alpha > 0)
        {
            alpha -= Increment.OnePercent;
            alpha = Mathf.Clamp(alpha, 0, 1);
            render.SetAlpha(alpha);
            yield return Wait.Tick();
        }
    }

    public void SetStatus(Status icon)
    {
        //Check abort state
        if (!IsActive)
            return;

        StartCoroutine(SetStatusIcon(icon));
    }

    private IEnumerator SetStatusIcon(Status status)
    {
        float increment = Increment.FivePercent;
        float alpha = render.statusIcon.color.a;

        render.statusIcon.color = new Color(1, 1, 1, alpha);

        //Fade out
        while (alpha > 0)
        {
            alpha -= increment;
            alpha = Mathf.Clamp(alpha, 0, 1);
            render.statusIcon.color = new Color(1, 1, 1, alpha);
            yield return Wait.Tick();
        }

        //Switch status status sprite
        render.statusIcon.sprite = resourceManager.statusSprites.First(x => x.id.Equals(status.ToString())).thumbnail;

        //Fade in
        alpha = 0;
        render.statusIcon.color = new Color(1, 1, 1, alpha);

        while (alpha < 1)
        {
            alpha += increment;
            alpha = Mathf.Clamp(alpha, 0, 1);
            render.statusIcon.color = new Color(1, 1, 1, alpha);

            yield return Wait.Tick();
        }

        render.statusIcon.color = new Color(1, 1, 1, 1);

    }

    public void StartGlow()
    {
        //Check abort state
        if (!IsActive)
            return;

        render.SetGlowColor(IsPlayer ? quality.Color : Colors.Solid.Red);
        StartCoroutine(GlowIn());
    }

    public void StopGlow()
    {
        //Check abort state
        if (!IsActive)
            return;

        StartCoroutine(GlowOut());
    }


    public void Destroy()
    {
        //Check abort state
        if (!IsActive)
            return;


        IEnumerator Death()
        {
            float alpha = 1;
            while (alpha > 0)
            {
                alpha -= Increment.OnePercent;
                alpha = Mathf.Clamp(alpha, 0, 1);
                render.SetGlowAlpha(alpha);
                //render.SetBackAlpha(alpha);
                yield return Wait.Tick();
            }

            render.SetGlowAlpha(0);
            render.SetBackAlpha(0);
            Destroy(this.gameObject);
            actors.Remove(this);
        }

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
        float min = (Interval.OneSecond * 20) - Speed * LuckModifier;
        float max = (Interval.OneSecond * 40) - Speed * LuckModifier;

        wait = 0;
        waitDuration = Random.Float(min, max);
        StartCoroutine(RadialBackFadeIn());
        UpdateActionBar();
    }

    public IEnumerator SpawnIn(float delay = 0)
    {
        float alpha = 0;
        render.SetAlpha(alpha);
        //render.SetBackAlpha(alpha);
        //render.SetGlowAlpha(Mathf.Clamp(alpha, 0.25f, 0.5f));

        yield return Wait.For(delay);

        while (alpha < 1)
        {
            alpha += Increment.OnePercent;
            alpha = Mathf.Clamp(alpha, 0, 1);
            render.SetAlpha(alpha);
            //render.SetBackAlpha(alpha);
            //render.SetGlowAlpha(Mathf.Clamp(alpha, 0.25f, 0.5f));
            yield return Wait.Tick();
        }

        render.SetAlpha(1);
        //render.SetBackAlpha(0.5f);
    }

    //public IEnumerator Death()
    //{
    //    float alpha = 1;
    //    while (alpha > 0)
    //    {
    //        alpha -= Increment.OnePercent;
    //        alpha = Mathf.Clamp(alpha, 0, 1);
    //        render.SetGlowAlpha(alpha);
    //        //render.SetBackAlpha(alpha);
    //        yield return Wait.Tick();
    //    }

    //    render.SetGlowAlpha(0);
    //    render.SetBackAlpha(0);
    //    Destroy(this.gameObject);
    //    actors.Remove(this);
    //}



    //public void CheckLocationConflict()
    //{
    //    var other = actors.FirstOrDefault(x => x != null && x.IsAlive && x.IsActive && !Equals(x) && location.Equals(x.location));
    //    if (other == null)
    //        return;

    //    SwapLocation(other);
    //}


}
