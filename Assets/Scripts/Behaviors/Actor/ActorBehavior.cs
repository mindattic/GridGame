using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActorBehavior : ExtendedMonoBehavior
{

    protected static class Layer
    {
        public const int Back = 0;
        public const int Parallax = 1;
        public const int Glow = 2;
        public const int Thumbnail = 3;
        public const int Frame = 4;
        public const int StatusIcon = 5;
        public const int HealthBarBack = 6;
        public const int HealthBar = 7;
        public const int HealthBarFront = 8;
        public const int HealthText = 9;
        public const int ActionBarBack = 10;
        public const int ActionBar = 11;
        public const int ActionText = 12;
        public const int RadialBack = 13;
        public const int RadialFill = 14;
        public const int RadialText = 15;
        public const int Selection = 16;
        public const int Mask = 17;

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

    public float randomBob = Random.Float(0.5f, 1f);
    public ActorBehavior targetPlayer = null;

    public ActorHealthBar HealthBar;
    public ActorThumbnail Thumbnail;
    public ActorRenderers Renderers = new ActorRenderers();


    private GameObject GameObjectByLayer(int layer)
    {
        return gameObject.transform.GetChild(layer).gameObject;
    }



    private void Awake()
    {

        Renderers.Back = gameObject.transform.GetChild(Layer.Back).GetComponent<SpriteRenderer>();
        Renderers.Parallax = gameObject.transform.GetChild(Layer.Parallax).GetComponent<SpriteRenderer>();
        Renderers.Glow = gameObject.transform.GetChild(Layer.Glow).GetComponent<SpriteRenderer>();
        Renderers.Thumbnail = gameObject.transform.GetChild(Layer.Thumbnail).GetComponent<SpriteRenderer>();
        Renderers.Frame = gameObject.transform.GetChild(Layer.Frame).GetComponent<SpriteRenderer>();
        Renderers.StatusIcon = gameObject.transform.GetChild(Layer.StatusIcon).GetComponent<SpriteRenderer>();
        Renderers.HealthBarBack = gameObject.transform.GetChild(Layer.HealthBarBack).GetComponent<SpriteRenderer>();
        Renderers.HealthBar = gameObject.transform.GetChild(Layer.HealthBar).GetComponent<SpriteRenderer>();
        Renderers.HealthBarFront = gameObject.transform.GetChild(Layer.HealthBarFront).GetComponent<SpriteRenderer>();
        Renderers.HealthText = gameObject.transform.GetChild(Layer.HealthText).GetComponent<TextMeshPro>();
        Renderers.ActionBarBack = gameObject.transform.GetChild(Layer.ActionBarBack).GetComponent<SpriteRenderer>();
        Renderers.ActionBar = gameObject.transform.GetChild(Layer.ActionBar).GetComponent<SpriteRenderer>();
        Renderers.ActionText = gameObject.transform.GetChild(Layer.ActionText).GetComponent<TextMeshPro>();
        Renderers.RadialBack = gameObject.transform.GetChild(Layer.RadialBack).GetComponent<SpriteRenderer>();
        Renderers.RadialFill = gameObject.transform.GetChild(Layer.RadialFill).GetComponent<SpriteRenderer>();
        Renderers.RadialText = gameObject.transform.GetChild(Layer.RadialText).GetComponent<TextMeshPro>();
        Renderers.Selection = gameObject.transform.GetChild(Layer.Selection).GetComponent<SpriteRenderer>();
        Renderers.Mask = gameObject.transform.GetChild(Layer.Mask).GetComponent<SpriteMask>();


        HealthBar = new ActorHealthBar(GameObjectByLayer(Layer.HealthBarBack), GameObjectByLayer(Layer.HealthBar));
        Thumbnail = new ActorThumbnail(GameObjectByLayer(Layer.Thumbnail));



    }

    private void Start()
    {

    }


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


    public Sprite thumbnail
    {
        get => Renderers.Thumbnail.sprite;
        set => Renderers.Thumbnail.sprite = value;
    }

    public int sortingOrder
    {
        set
        {
            Renderers.Back.sortingOrder = value + Layer.Back;
            Renderers.Parallax.sortingOrder = value + Layer.Parallax;
            Renderers.Glow.sortingOrder = value;
            Renderers.Thumbnail.sortingOrder = value + Layer.Thumbnail;
            Renderers.Frame.sortingOrder = value + Layer.Frame;
            Renderers.StatusIcon.sortingOrder = value + Layer.StatusIcon;
            Renderers.HealthBarBack.sortingOrder = value + Layer.HealthBarBack;
            Renderers.HealthBar.sortingOrder = value + Layer.HealthBar;
            Renderers.HealthBarFront.sortingOrder = value + Layer.HealthBarFront;
            Renderers.HealthText.sortingOrder = value + Layer.HealthText;
            Renderers.ActionBarBack.sortingOrder = value + Layer.ActionBarBack;
            Renderers.ActionBar.sortingOrder = value + Layer.ActionBar;
            Renderers.ActionText.sortingOrder = value + Layer.ActionText;
            Renderers.RadialBack.sortingOrder = value + Layer.RadialBack;
            Renderers.RadialFill.sortingOrder = value + Layer.RadialFill;
            Renderers.RadialText.sortingOrder = value + Layer.RadialText;
            Renderers.Selection.sortingOrder = value + Layer.Selection;
            Renderers.Mask.sortingOrder = value + Layer.Mask;
        }
    }


    public TileBehavior currentTile => tiles.First(x => x.location.Equals(location));
    public bool IsPlayer => team.Equals(Team.Player);
    public bool IsEnemy => team.Equals(Team.Enemy);
    public bool IsFocusedPlayer => HasFocusedPlayer && Equals(focusedPlayer);
    public bool IsSelectedPlayer => HasSelectedPlayer && Equals(selectedPlayer);
    public bool HasLocation => location != Locations.nowhere;
    public bool IsMoving => destination.HasValue;
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

    public Direction AdjacentDirectionTo(ActorBehavior other)
    {
        if (!IsAdjacentTo(other.location)) return Direction.None;
        if (IsNorthOf(other.location)) return Direction.South;
        if (IsEastOf(other.location)) return Direction.West;
        if (IsSouthOf(other.location)) return Direction.North;
        if (IsWestOf(other.location)) return Direction.East;

        return Direction.None;
    }

    private Vector2Int GoNorth() => location += new Vector2Int(0, -1);
    private Vector2Int GoEast() => location += new Vector2Int(1, 0);
    private Vector2Int GoSouth() => location += new Vector2Int(0, 1);
    private Vector2Int GoWest() => location += new Vector2Int(-1, 0);



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
            Renderers.SetBackColor(quality.Color);
            Renderers.SetParallaxSprite(resourceManager.Seamless("WhiteFire"));
            Renderers.SetParallaxMaterial(resourceManager.ActorMaterial("PlayerParallax"));
            Renderers.SetGlowColor(quality.Color);
            Renderers.SetFrameColor(Colors.Solid.White);
            Renderers.SetAlpha(0);
            Renderers.ActionBarBack.enabled = false;
            Renderers.ActionBar.enabled = false;
            Renderers.RadialText.enabled = false;
            Renderers.RadialBack.enabled = false;
            Renderers.RadialFill.enabled = false;
        }
        else if (this.IsEnemy)
        {
            Renderers.Back.enabled = false;
            //Renderers.SetBackColor(Colors.Solid.White);
            Renderers.SetParallaxSprite(resourceManager.Seamless("BlackFire"));
            Renderers.SetParallaxMaterial(resourceManager.ActorMaterial("EnemyParallax"));
            Renderers.SetGlowColor(Colors.Solid.White);
            Renderers.SetGlowAlpha(0);
            Renderers.SetFrameColor(Colors.Solid.Red);
            Renderers.SetAlpha(0);
            Renderers.RadialText.enabled = true;
            Renderers.RadialBack.enabled = true;
            Renderers.RadialFill.enabled = true;
            CalculateWait();
        }


        Renderers.Back.transform.localScale = new Vector3(backScale, backScale, 1);

        UpdateHealthBar();


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
        if (IsMoving)
            return;

        if (IsNorthOf(other.location) || IsNorthWestOf(other.location) || IsNorthEastOf(other.location))
            GoSouth();
        else if (IsEastOf(other.location))
            GoWest();
        else if (IsSouthOf(other.location) || IsSouthWestOf(other.location) || IsSouthEastOf(other.location))
            GoNorth();
        else if (IsWestOf(other.location))
            GoEast();

        var closestTile = Geometry.ClosestTileByLocation(location);
        this.destination = closestTile.position;

       

        soundSource.PlayOneShot(resourceManager.SoundEffect("Slide"));
    }

    Vector3 ClosestLocation(ActorBehavior other)
    {
        //Determine if already adjacent to player...
        if (IsAdjacentTo(other.location))
            return position;

        //...Otherwise, Find closest unoccupied tile adjacent to player...
        var closestUnoccupiedAdjacentTile = Geometry.ClosestUnoccupiedAdjacentTileByLocation(other.location);
        if (closestUnoccupiedAdjacentTile != null)
            return closestUnoccupiedAdjacentTile.position;

        //...Otherwise, Find closest tile adjacent to player...
        var closestAdjacentTile = Geometry.ClosestAdjacentTileByLocation(other.location);
        if (closestAdjacentTile != null)
            return closestAdjacentTile.position;

        //...Otherwise, find closest unoccupied tile to player...
        var closestUnoccupiedTile = Geometry.ClosestUnoccupiedTileByLocation(other.location);
        if (closestUnoccupiedTile != null)
            return closestUnoccupiedTile.position;

        //...Otherwise, find closest tile to player
        var closestTile = Geometry.ClosestTileByLocation(other.location);
        if (closestTile != null)
            return closestTile.position;

        return position;
    }


    public void SetAttackStrategy()
    {
        //Randomly select an attack attackStrategy
        int[] ratios = { 50, 20, 15, 10, 5 };
        var attackStrategy = Random.AttackStrategy(ratios);

        switch (attackStrategy)
        {
            case AttackStrategy.AttackClosest:
                var closestPlayer = players.Where(x => x.IsPlaying).OrderBy(x => Vector3.Distance(x.position, position)).FirstOrDefault();
                targetPlayer = closestPlayer;
                destination = ClosestLocation(targetPlayer);
                break;

            case AttackStrategy.AttackWeakest:
                var weakestPlayer = players.Where(x => x.IsPlaying).OrderBy(x => x.HP).FirstOrDefault();
                targetPlayer = weakestPlayer;
                destination = ClosestLocation(targetPlayer);
                break;

            case AttackStrategy.AttackStrongest:
                var strongestPlayer = players.Where(x => x.IsPlaying).OrderBy(x => x.HP).FirstOrDefault();
                targetPlayer = strongestPlayer;
                destination = ClosestLocation(targetPlayer);
                break;

            case AttackStrategy.AttackRandom:
                var randomPlayer = Random.Player();
                targetPlayer = randomPlayer;
                destination = ClosestLocation(targetPlayer);
                break;

            case AttackStrategy.MoveAnywhere:
                var location = Random.Location();
                targetPlayer = null;
                destination = Geometry.ClosestTileByLocation(location).position;
                break;
        }


    }

    private void MoveTowardCursor()
    {
        //Check abort state
        if (!IsFocusedPlayer && !IsSelectedPlayer)
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
        if (!IsMoving)
            return;

        var delta = this.destination.Value - position;
        if (Mathf.Abs(delta.x) >= snapDistance)
        {
            position = Vector2.MoveTowards(position, new Vector3(destination.Value.x, position.y, position.z), slideSpeed);
        }
        else if (Mathf.Abs(delta.y) >= snapDistance)
        {
            position = Vector2.MoveTowards(position, new Vector3(position.x, destination.Value.y, position.z), slideSpeed);
        }

        //Determine if actor is close to destination
        bool isSnapDistance = Vector2.Distance(position, destination.Value) <= snapDistance;
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
        if (!IsAlive || !IsActive || !turnManager.IsStartPhase)
            return;


        //Source: https://forum.unity.com/threads/how-to-make-an-object-move-up-and-down-on-a-loop.380159/
        //var pos = new Vector3(
        //    transform.position.x,
        //    transform.position.y + (bobbing.Evaluate(Time.time % bobbing.length) * (tileSize / 64)),
        //    transform.position.z);

        //var rot = new Vector3(
        //   transform.rotation.x,
        //   transform.rotation.y ,
        //   transform.rotation.z + (bobbing.Evaluate(Time.time % bobbing.length) * (tileSize / 128)));

        //Renderers.Thumbnail.transform.Rotate(Vector3.up * bobbing.Evaluate(Time.time % bobbing.length) * (tileSize / 3));

        //Renderers.Glow.transform.position = pos;
        //Renderers.Thumbnail.transform.position = pos;
        //Renderers.Frame.transform.position = pos;
        //Renderers.Thumbnail.transform.position = pos;
        //Renderers.Thumbnail.transform.rotation = rot;
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
        Renderers.SetBackScale(scale);
        Renderers.SetBackColor(IsPlayer ? quality.Color : Colors.Translucent.Black);




    }


    private void CheckFlicker()
    {
        //Check abort state
        if (!IsAlive || !IsActive || !turnManager.IsStartPhase || (turnManager.IsPlayerTurn && !IsPlayer) || (turnManager.IsEnemyTurn && !IsEnemy))
            return;

        var alpha = 0.5f + (bobbing.Evaluate(Time.time % bobbing.length) * (tileSize / 24));
        Renderers.SetGlowAlpha(alpha);
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



    void Update()
    {
        //Check abort state
        if (!IsAlive || !IsActive)
            return;

        if (IsFocusedPlayer || IsSelectedPlayer)
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
        if (!IsAlive || !IsActive || IsFocusedPlayer || IsSelectedPlayer)
            return;

        CheckMovement();
        CheckBobbing();
        CheckThrobbing();
        //CheckFlicker();

        CheckActionBar();
        CheckRadial();
    }


    enum BumpStage
    {
        Start,
        MoveToward,
        MoveAway,
        End
    }


    public IEnumerator Bump(Direction direction)
    {
        BumpStage stage = BumpStage.Start;
        var destination = position;
        var range = tileSize * percent33;


        while (stage != BumpStage.End)
        {
            switch (stage)
            {
                case BumpStage.Start:
                    {
                        sortingOrder = ZAxis.Max;
                        position = currentTile.position;
                        destination = Geometry.GetDirectionalPosition(position, direction, range);
                        stage = BumpStage.MoveToward;

                    }
                    break;

                case BumpStage.MoveToward:
                    {
                        var delta = destination - position;
                        if (Mathf.Abs(delta.x) > bumpSpeed)
                            position = Vector2.MoveTowards(position, new Vector3(destination.x, position.y, position.z), bumpSpeed);
                        else if (Mathf.Abs(delta.y) > bumpSpeed)
                            position = Vector2.MoveTowards(position, new Vector3(position.x, destination.y, position.z), bumpSpeed);

                        var isSnapDistance = Vector2.Distance(position, destination) <= bumpSpeed;
                        if (isSnapDistance)
                        {
                            position = destination;
                            destination = currentTile.position;
                            stage = BumpStage.MoveAway;
                        }
                    }
                    break;

                case BumpStage.MoveAway:
                    {
                        var delta = destination - position;
                        if (Mathf.Abs(delta.x) > bumpSpeed)
                            position = Vector2.MoveTowards(position, new Vector3(destination.x, position.y, position.z), bumpSpeed);
                        else if (Mathf.Abs(delta.y) > bumpSpeed)
                            position = Vector2.MoveTowards(position, new Vector3(position.x, destination.y, position.z), bumpSpeed);

                        var isSnapDistance = Vector2.Distance(position, destination) <= bumpSpeed;
                        if (isSnapDistance)
                        {
                            position = destination;
                            destination = currentTile.position;
                            stage = BumpStage.End;
                        }
                    }
                    break;

                case BumpStage.End:
                    {
                        sortingOrder = ZAxis.Min;
                        position = destination;
                    }
                    break;
            }

            yield return Wait.None();
        }
    }


    public IEnumerator TakeFlurryDamage(float damage)
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

            yield return Wait.For(Interval.FiveTicks);
        }

        Shake(ShakeIntensity.Stop);
        HP = remainingHP;
        position = currentTile.position;
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

            yield return Wait.For(Interval.FiveTicks);
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
            ticks += Interval.OneTick;
            Shake(ShakeIntensity.Low);
            yield return Wait.OneTick();
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

        if (wait == waitDuration)
        {
            StartCoroutine(RadialBackFadeOut());
        }


        UpdateActionBar();
    }


    private void UpdateActionBar()
    {
        var scale = Renderers.ActionBarBack.transform.localScale;
        var x = Mathf.Clamp(scale.x * (wait / waitDuration), 0, scale.x);
        Renderers.ActionBar.transform.localScale = new Vector3(x, scale.y, scale.z);

        //Percent complete
        Renderers.ActionText.text = wait < waitDuration ? $@"{Math.Round(wait / waitDuration * 100)}" : "";

        //Seconds remaining
        //Renderers.RadialText.text = wait < waitDuration ? $"{Math.Round(waitDuration - wait)}" : "";

    }


    public void CheckRadial()
    {
        UpdateRadial();
    }


    private void UpdateRadial()
    {
        //var fill = (360 * (wait / waitDuration));
        //Renderers.RadialFill.material.SetFloat("_Arc1", fill);

        //Renderers.RadialText.text = wait < waitDuration ? $"{Math.Round(waitDuration - wait)}" : "";
    }




    private void UpdateHealthBar()
    {
        var scale = Renderers.HealthBarBack.transform.localScale;
        var x = Mathf.Clamp(scale.x * (HP / MaxHP), 0, scale.x);
        Renderers.HealthBar.transform.localScale = new Vector3(x, scale.y, scale.z);
        Renderers.HealthText.text = $@"{HP}/{MaxHP}";
    }


    public IEnumerator RadialBackFadeIn()
    {
        var maxAlpha = 0.5f;
        var alpha = 0f;
        Renderers.RadialBack.color = new Color(0, 0, 0, alpha);

        while (alpha < maxAlpha)
        {
            alpha += Increment.OnePercent;
            alpha = Mathf.Clamp(alpha, 0, maxAlpha);
            Renderers.RadialBack.color = new Color(0, 0, 0, alpha);
            yield return Wait.OneTick();
        }

        Renderers.RadialBack.color = new Color(0, 0, 0, maxAlpha);
    }

    public IEnumerator RadialBackFadeOut()
    {
        var maxAlpha = 0.5f;
        var alpha = maxAlpha;
        Renderers.RadialBack.color = new Color(0, 0, 0, maxAlpha);

        while (alpha > 0)
        {
            alpha -= Increment.OnePercent;
            alpha = Mathf.Clamp(alpha, 0, maxAlpha);
            Renderers.RadialBack.color = new Color(0, 0, 0, alpha);
            yield return Wait.OneTick();
        }

        Renderers.RadialBack.color = new Color(0, 0, 0, 0);
    }

    public IEnumerator Dissolve()
    {
        var alpha = 1f;
        Renderers.SetAlpha(alpha);

        portraitManager.Dissolve(this);
        soundSource.PlayOneShot(resourceManager.SoundEffect("Death"));
        sortingOrder = ZAxis.Max;

        while (alpha > 0)
        {
            alpha -= Increment.OnePercent;
            alpha = Mathf.Clamp(alpha, 0, 1);
            Renderers.SetAlpha(alpha);
            yield return Wait.OneTick();
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
        float alpha = Renderers.StatusIcon.color.a;

        Renderers.StatusIcon.color = new Color(1, 1, 1, alpha);

        //Fade out
        while (alpha > 0)
        {
            alpha -= increment;
            alpha = Mathf.Clamp(alpha, 0, 1);
            Renderers.StatusIcon.color = new Color(1, 1, 1, alpha);
            yield return Wait.OneTick();
        }

        //Switch status status sprite
        Renderers.StatusIcon.sprite = resourceManager.statusSprites.First(x => x.id.Equals(status.ToString())).thumbnail;

        //Fade in
        alpha = 0;
        Renderers.StatusIcon.color = new Color(1, 1, 1, alpha);

        while (alpha < 1)
        {
            alpha += increment;
            alpha = Mathf.Clamp(alpha, 0, 1);
            Renderers.StatusIcon.color = new Color(1, 1, 1, alpha);

            yield return Wait.OneTick();
        }

        Renderers.StatusIcon.color = new Color(1, 1, 1, 1);

    }

    public void StartGlow()
    {
        //Check abort state
        if (!IsActive)
            return;

        Renderers.SetGlowColor(IsPlayer ? quality.Color : Colors.Solid.Black);
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
                Renderers.SetGlowAlpha(alpha);
                //Renderers.SetBackAlpha(alpha);
                yield return Wait.OneTick();
            }

            Renderers.SetGlowAlpha(0);
            Renderers.SetBackAlpha(0);
            Destroy(this.gameObject);
            actors.Remove(this);
        }

        StartCoroutine(Death());
    }

    public IEnumerator GlowIn()
    {
        float alpha = 0;
        Renderers.SetGlowAlpha(alpha);

        while (alpha < 1)
        {
            alpha += Increment.TwoPercent;
            alpha = Mathf.Clamp(alpha, 0, 1);
            Renderers.SetGlowAlpha(alpha);
            yield return Wait.OneTick();
        }

        Renderers.SetGlowAlpha(1);
    }

    public IEnumerator GlowOut()
    {
        float alpha = Renderers.glowColor.a;
        while (alpha > 0)
        {
            alpha -= Increment.TwoPercent;
            alpha = Mathf.Clamp(alpha, 0, 1);
            Renderers.SetGlowAlpha(alpha);
            yield return Wait.OneTick();
        }

        Renderers.SetGlowAlpha(0);
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
        UpdateRadial();
    }

    public void ReadyUp()
    {
        //Check abort state
        if (!IsAlive || !IsActive || !IsEnemy)
            return;

        wait = waitDuration;
        UpdateActionBar();
    }

    public IEnumerator SpawnIn(float delay = 0)
    {
        float alpha = 0;
        Renderers.SetAlpha(alpha);
        //Renderers.SetBackAlpha(alpha);
        //Renderers.SetGlowAlpha(Mathf.Clamp(alpha, 0.25f, 0.5f));

        yield return Wait.For(delay);

        while (alpha < 1)
        {
            alpha += Increment.OnePercent;
            alpha = Mathf.Clamp(alpha, 0, 1);
            Renderers.SetAlpha(alpha);
            //Renderers.SetBackAlpha(alpha);
            //Renderers.SetGlowAlpha(Mathf.Clamp(alpha, 0.25f, 0.5f));
            yield return Wait.OneTick();
        }

        Renderers.SetAlpha(1);
        //Renderers.SetBackAlpha(0.5f);
    }

    //public IEnumerator Death()
    //{
    //    float alpha = 1;
    //    while (alpha > 0)
    //    {
    //        alpha -= Increment.OnePercent;
    //        alpha = Mathf.Clamp(alpha, 0, 1);
    //        Renderers.SetGlowAlpha(alpha);
    //        //Renderers.SetBackAlpha(alpha);
    //        yield return Wait.OneTick();
    //    }

    //    Renderers.SetGlowAlpha(0);
    //    Renderers.SetBackAlpha(0);
    //    Destroy(this.gameObject);
    //    actors.Destroy(this);
    //}



    //public void CheckLocationConflict()
    //{
    //    var other = actors.FirstOrDefault(x => x != null && x.IsAlive && x.IsActive && !Equals(x) && location.Equals(x.location));
    //    if (other == null)
    //        return;

    //    SwapLocation(other);
    //}


}
