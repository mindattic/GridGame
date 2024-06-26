using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class ActorBehavior : ExtendedMonoBehavior
{

    //Variables
    [SerializeField] public Archetype Archetype;
    [SerializeField] public Vector2Int Location = Locations.Nowhere;
    [SerializeField] public Vector3? Destination = null;
    [SerializeField] public Team Team = Team.Independant;
    [SerializeField] public Quality Quality = Colors.Common;
    [SerializeField] public float Level;
    [SerializeField] public float HP;
    [SerializeField] public float MaxHP;
    [SerializeField] public float Attack;
    [SerializeField] public float Defense;
    [SerializeField] public float Accuracy;
    [SerializeField] public float Evasion;
    [SerializeField] public float Speed;
    [SerializeField] public float Luck;

    [SerializeField] public float Wait = 0;
    [SerializeField] public float WaitDuration = -1;

    [SerializeField] public int SpawnTurn = -1;

    [SerializeField] public AnimationCurve FlickeringCurve;
    [SerializeField] public AnimationCurve BobbingCurve;
    [SerializeField] public Guid Guid;


    [SerializeField] public float BackScale = 1.4f;

    public float RandomBob = Random.Float(0.5f, 1f);
    public ActorBehavior TargetPlayer = null;

    public ActorHealthBar HealthBar;
    public ActorThumbnail Thumbnail;
    public ActorRenderers Renderers = new ActorRenderers();


    private GameObject GameObjectByLayer(int layer)
    {
        return gameObject.transform.GetChild(layer).gameObject;
    }



    private void Awake()
    {

        Renderers.Back = gameObject.transform.GetChild(ActorLayer.Back).GetComponent<SpriteRenderer>();
        Renderers.Parallax = gameObject.transform.GetChild(ActorLayer.Parallax).GetComponent<SpriteRenderer>();
        Renderers.Glow = gameObject.transform.GetChild(ActorLayer.Glow).GetComponent<SpriteRenderer>();
        Renderers.Thumbnail = gameObject.transform.GetChild(ActorLayer.Thumbnail).GetComponent<SpriteRenderer>();
        Renderers.Frame = gameObject.transform.GetChild(ActorLayer.Frame).GetComponent<SpriteRenderer>();
        Renderers.StatusIcon = gameObject.transform.GetChild(ActorLayer.StatusIcon).GetComponent<SpriteRenderer>();
        Renderers.HealthBarBack = gameObject.transform.GetChild(ActorLayer.HealthBarBack).GetComponent<SpriteRenderer>();
        Renderers.HealthBar = gameObject.transform.GetChild(ActorLayer.HealthBar).GetComponent<SpriteRenderer>();
        Renderers.HealthBarFront = gameObject.transform.GetChild(ActorLayer.HealthBarFront).GetComponent<SpriteRenderer>();
        Renderers.HealthText = gameObject.transform.GetChild(ActorLayer.HealthText).GetComponent<TextMeshPro>();
        Renderers.ActionBarBack = gameObject.transform.GetChild(ActorLayer.ActionBarBack).GetComponent<SpriteRenderer>();
        Renderers.ActionBar = gameObject.transform.GetChild(ActorLayer.ActionBar).GetComponent<SpriteRenderer>();
        Renderers.ActionText = gameObject.transform.GetChild(ActorLayer.ActionText).GetComponent<TextMeshPro>();
        Renderers.RadialBack = gameObject.transform.GetChild(ActorLayer.RadialBack).GetComponent<SpriteRenderer>();
        Renderers.RadialFill = gameObject.transform.GetChild(ActorLayer.RadialFill).GetComponent<SpriteRenderer>();
        Renderers.RadialText = gameObject.transform.GetChild(ActorLayer.RadialText).GetComponent<TextMeshPro>();
        Renderers.Selection = gameObject.transform.GetChild(ActorLayer.Selection).GetComponent<SpriteRenderer>();
        Renderers.Mask = gameObject.transform.GetChild(ActorLayer.Mask).GetComponent<SpriteMask>();


        HealthBar = new ActorHealthBar(GameObjectByLayer(ActorLayer.HealthBarBack), GameObjectByLayer(ActorLayer.HealthBar));
        Thumbnail = new ActorThumbnail(GameObjectByLayer(ActorLayer.Thumbnail));



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
            Renderers.Back.sortingOrder = value + ActorLayer.Back;
            Renderers.Parallax.sortingOrder = value + ActorLayer.Parallax;
            Renderers.Glow.sortingOrder = value;
            Renderers.Thumbnail.sortingOrder = value + ActorLayer.Thumbnail;
            Renderers.Frame.sortingOrder = value + ActorLayer.Frame;
            Renderers.StatusIcon.sortingOrder = value + ActorLayer.StatusIcon;
            Renderers.HealthBarBack.sortingOrder = value + ActorLayer.HealthBarBack;
            Renderers.HealthBar.sortingOrder = value + ActorLayer.HealthBar;
            Renderers.HealthBarFront.sortingOrder = value + ActorLayer.HealthBarFront;
            Renderers.HealthText.sortingOrder = value + ActorLayer.HealthText;
            Renderers.ActionBarBack.sortingOrder = value + ActorLayer.ActionBarBack;
            Renderers.ActionBar.sortingOrder = value + ActorLayer.ActionBar;
            Renderers.ActionText.sortingOrder = value + ActorLayer.ActionText;
            Renderers.RadialBack.sortingOrder = value + ActorLayer.RadialBack;
            Renderers.RadialFill.sortingOrder = value + ActorLayer.RadialFill;
            Renderers.RadialText.sortingOrder = value + ActorLayer.RadialText;
            Renderers.Selection.sortingOrder = value + ActorLayer.Selection;
            Renderers.Mask.sortingOrder = value + ActorLayer.Mask;
        }
    }


    public TileBehavior currentTile => Tiles.First(x => x.Location.Equals(Location));
    public bool IsPlayer => Team.Equals(Team.Player);
    public bool IsEnemy => Team.Equals(Team.Enemy);
    public bool IsFocusedPlayer => HasFocusedPlayer && Equals(FocusedPlayer);
    public bool IsSelectedPlayer => HasSelectedPlayer && Equals(SelectedPlayer);
    public bool HasLocation => Location != Locations.Nowhere;
    public bool IsMoving => Destination.HasValue;
    public bool IsNorthEdge => Location.y == 1;
    public bool IsEastEdge => Location.x == Board.ColumnCount;
    public bool IsSouthEdge => Location.y == Board.RowCount;
    public bool IsWestEdge => Location.x == 1;
    public bool IsAlive => HP > 0;
    public bool IsDead => HP < 1;
    public bool IsActive => this != null && isActiveAndEnabled;
    public bool IsInactive => this == null || !isActiveAndEnabled;

    public bool IsSpawnable => !IsActive && IsAlive && SpawnTurn <= TurnManager.currentTurn;
    public bool IsPlaying => IsAlive && IsActive;
    public bool IsReady => Wait == WaitDuration;
    public float LevelModifier => 1.0f + Random.Float(0, Level * 0.01f);
    public float AttackModifier => 1.0f + Random.Float(0, Attack * 0.01f);
    public float DefenseModifier => 1.0f + Random.Float(0, Defense * 0.01f);
    public float AccuracyModifier => 1.0f + Random.Float(0, Accuracy * 0.01f);
    public float EvasionModifier => 1.0f + Random.Float(0, Evasion * 0.01f);
    public float LuckModifier => 1.0f + Random.Float(0, Luck * 0.01f);


    public bool IsSameColumn(Vector2Int other) => Location.x == other.x;
    public bool IsSameRow(Vector2Int other) => Location.y == other.y;
    public bool IsNorthOf(Vector2Int other) => IsSameColumn(other) && Location.y == other.y - 1;
    public bool IsEastOf(Vector2Int other) => IsSameRow(other) && Location.x == other.x + 1;
    public bool IsSouthOf(Vector2Int other) => IsSameColumn(other) && Location.y == other.y + 1;
    public bool IsWestOf(Vector2Int other) => IsSameRow(other) && Location.x == other.x - 1;
    public bool IsNorthWestOf(Vector2Int other) => Location.x == other.x - 1 && Location.y == other.y - 1;
    public bool IsNorthEastOf(Vector2Int other) => Location.x == other.x + 1 && Location.y == other.y - 1;
    public bool IsSouthWestOf(Vector2Int other) => Location.x == other.x - 1 && Location.y == other.y + 1;
    public bool IsSouthEastOf(Vector2Int other) => Location.x == other.x + 1 && Location.y == other.y + 1;
    public bool IsAdjacentTo(Vector2Int other) => (IsSameColumn(other) || IsSameRow(other)) && Vector2Int.Distance(Location, other).Equals(1);

    public Direction AdjacentDirectionTo(ActorBehavior other)
    {
        if (!IsAdjacentTo(other.Location)) return Direction.None;
        if (IsNorthOf(other.Location)) return Direction.South;
        if (IsEastOf(other.Location)) return Direction.West;
        if (IsSouthOf(other.Location)) return Direction.North;
        if (IsWestOf(other.Location)) return Direction.East;

        return Direction.None;
    }

    private Vector2Int GoNorth() => Location += new Vector2Int(0, -1);
    private Vector2Int GoEast() => Location += new Vector2Int(1, 0);
    private Vector2Int GoSouth() => Location += new Vector2Int(0, 1);
    private Vector2Int GoWest() => Location += new Vector2Int(-1, 0);



    public void Init(bool spawn)
    {
        transform.localScale = TileScale;
        gameObject.SetActive(false);

        if (spawn)
            Spawn();
    }


    public void Spawn(Vector2Int? startLocation = null)
    {
        if (startLocation.HasValue)
            Location = startLocation.Value;

        if (!HasLocation)
            return;

        gameObject.SetActive(true);

        position = Geometry.PositionFromLocation(Location);

        if (IsPlayer)
        {
            Renderers.SetBackColor(Quality.Color);
            Renderers.SetParallaxSprite(ResourceManager.Seamless("WhiteFire"));
            Renderers.SetParallaxMaterial(ResourceManager.ActorMaterial("PlayerParallax"));
            Renderers.SetGlowColor(Quality.Color);
            Renderers.SetFrameColor(Colors.Solid.White);
            Renderers.SetAlpha(0);
            Renderers.ActionBarBack.enabled = false;
            Renderers.ActionBar.enabled = false;
            Renderers.RadialText.enabled = false;
            Renderers.RadialBack.enabled = false;
            Renderers.RadialFill.enabled = false;
        }
        else if (IsEnemy)
        {
            Renderers.Back.enabled = false;
            //Renderers.SetBackColor(Colors.Solid.White);
            Renderers.SetParallaxSprite(ResourceManager.Seamless("BlackFire"));
            Renderers.SetParallaxMaterial(ResourceManager.ActorMaterial("EnemyParallax"));
            Renderers.SetGlowColor(Colors.Solid.White);
            Renderers.SetGlowAlpha(0);
            Renderers.SetFrameColor(Colors.Solid.Red);
            Renderers.SetAlpha(0);
            Renderers.RadialText.enabled = true;
            Renderers.RadialBack.enabled = true;
            Renderers.RadialFill.enabled = true;
            CalculateWait();
        }


        Renderers.Back.transform.localScale = new Vector3(BackScale, BackScale, 1);

        UpdateHealthBar();


        float delay = TurnManager.currentTurn == 1 ? 0 : Random.Float(0f, 2f);
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

        if (IsNorthOf(other.Location) || IsNorthWestOf(other.Location) || IsNorthEastOf(other.Location))
            GoSouth();
        else if (IsEastOf(other.Location))
            GoWest();
        else if (IsSouthOf(other.Location) || IsSouthWestOf(other.Location) || IsSouthEastOf(other.Location))
            GoNorth();
        else if (IsWestOf(other.Location))
            GoEast();

        var closestTile = Geometry.ClosestTileByLocation(Location);
        Destination = closestTile.position;



        SoundSource.PlayOneShot(ResourceManager.SoundEffect("Slide"));
    }



    public void SetAttackStrategy()
    {
        //Randomly select an attack attackStrategy
        int[] ratios = { 50, 20, 15, 10, 5 };
        var attackStrategy = Random.AttackStrategy(ratios);

        switch (attackStrategy)
        {
            case AttackStrategy.AttackClosest:
                TargetPlayer = Players.Where(x => x.IsPlaying).OrderBy(x => Vector3.Distance(x.position, position)).FirstOrDefault();
                Destination = Geometry.ClosestAttackPosition(this, TargetPlayer);
                break;

            case AttackStrategy.AttackWeakest:
                TargetPlayer = Players.Where(x => x.IsPlaying).OrderBy(x => x.HP).FirstOrDefault();
                Destination = Geometry.ClosestAttackPosition(this, TargetPlayer);
                break;

            case AttackStrategy.AttackStrongest:
                TargetPlayer = Players.Where(x => x.IsPlaying).OrderBy(x => x.HP).FirstOrDefault();
                Destination = Geometry.ClosestAttackPosition(this, TargetPlayer);
                break;

            case AttackStrategy.AttackRandom:
                TargetPlayer = Random.Player();
                Destination = Geometry.ClosestAttackPosition(this, TargetPlayer);
                break;

            case AttackStrategy.MoveAnywhere:
                var location = Random.Location();
                TargetPlayer = null;
                Destination = Geometry.ClosestTileByLocation(location).position;
                break;
        }


    }

    private void MoveTowardCursor()
    {
        //Check abort state
        if (!IsFocusedPlayer && !IsSelectedPlayer)
            return;

        var cursorPosition = MousePosition3D + MouseOffset;
        cursorPosition.x = Mathf.Clamp(cursorPosition.x, Board.Left, Board.Right);
        cursorPosition.y = Mathf.Clamp(cursorPosition.y, Board.Bottom, Board.Top);

        //Move selected player towards cursor
        //Position = Vector2.MoveTowards(Position, cursorPosition, CursorSpeed);

        //Snap selected player to cursor
        position = cursorPosition;
    }

    private void CheckMovement()
    {
        //Check abort state
        if (!IsMoving)
            return;

        var delta = Destination.Value - position;
        if (Mathf.Abs(delta.x) >= SnapDistance)
        {
            position = Vector2.MoveTowards(position, new Vector3(Destination.Value.x, position.y, position.z), SlideSpeed);
        }
        else if (Mathf.Abs(delta.y) >= SnapDistance)
        {
            position = Vector2.MoveTowards(position, new Vector3(position.x, Destination.Value.y, position.z), SlideSpeed);
        }

        //Determine if Actor is close to Destination
        bool isSnapDistance = Vector2.Distance(position, Destination.Value) <= SnapDistance;
        if (isSnapDistance)
        {
            //Snap to Destination, clear Destination, and set Actor MoveState: "Idle"
            transform.position = Destination.Value;
            Destination = null;
        }
    }


    private void CheckBobbing()
    {
        //Check abort state
        if (!IsAlive || !IsActive || !TurnManager.IsStartPhase)
            return;


        //Source: https://forum.unity.com/threads/how-to-make-an-object-move-up-and-down-on-a-loop.380159/
        //var pos = new Vector3(
        //    transform.Position.x,
        //    transform.Position.y + (BobbingCurve.Evaluate(Time.time % BobbingCurve.length) * (TileSize / 64)),
        //    transform.Position.z);

        //var rot = new Vector3(
        //   transform.Rotation.x,
        //   transform.Rotation.y ,
        //   transform.Rotation.z + (BobbingCurve.Evaluate(Time.time % BobbingCurve.length) * (TileSize / 128)));

        //Renderers.Thumbnail.transform.Rotate(Vector3.up * BobbingCurve.Evaluate(Time.time % BobbingCurve.length) * (TileSize / 3));

        //Renderers.Glow.transform.Position = pos;
        //Renderers.Thumbnail.transform.Position = pos;
        //Renderers.Frame.transform.Position = pos;
        //Renderers.Thumbnail.transform.Position = pos;
        //Renderers.Thumbnail.transform.Rotation = rot;
    }


    private void CheckThrobbing()
    {
        //Check abort state
        if (!IsAlive || !IsActive || !TurnManager.IsStartPhase || (TurnManager.IsPlayerTurn && !IsPlayer) || (TurnManager.IsEnemyTurn && !IsEnemy))
            return;

        //Source: https://forum.unity.com/threads/how-to-make-an-object-move-up-and-down-on-a-loop.380159/
        var scale = new Vector3(
            BackScale + (BobbingCurve.Evaluate(Time.time % BobbingCurve.length) * (TileSize / 24)),
            BackScale + (BobbingCurve.Evaluate(Time.time % BobbingCurve.length) * (TileSize / 24)),
            1);
        Renderers.SetBackScale(scale);
        Renderers.SetBackColor(IsPlayer ? Quality.Color : Colors.Translucent.Black);




    }


    private void CheckFlicker()
    {
        //Check abort state
        if (!IsAlive || !IsActive || !TurnManager.IsStartPhase || (TurnManager.IsPlayerTurn && !IsPlayer) || (TurnManager.IsEnemyTurn && !IsEnemy))
            return;

        var alpha = 0.5f + (BobbingCurve.Evaluate(Time.time % BobbingCurve.length) * (TileSize / 24));
        Renderers.SetGlowAlpha(alpha);
    }

    public void Shake(float intensity)
    {
        gameObject.transform.GetChild(ActorLayer.Thumbnail).gameObject.transform.position = currentTile.position;

        if (intensity > 0)
        {
            var amount = new Vector3(Random.Range(-intensity), Random.Range(intensity), 1);
            gameObject.transform.GetChild(ActorLayer.Thumbnail).gameObject.transform.position += amount;
        }

    }



    void Update()
    {
        //Check abort state
        if (!IsAlive || !IsActive)
            return;

        if (IsFocusedPlayer || IsSelectedPlayer)
            MoveTowardCursor();

        var closestTile = Geometry.ClosestTileByPosition(position);
        if (closestTile.Location.Equals(Location))
            return;

        SoundSource.PlayOneShot(ResourceManager.SoundEffect($"Move{Random.Int(1, 6)}"));

        //Determine if selected player and another Actor are occupying the same tile
        var actor = Actors.FirstOrDefault(x => x != null && x.IsAlive && x.IsActive && !x.Equals(SelectedPlayer) && x.Location.Equals(closestTile.Location));
        if (actor != null)
        {
            actor.SwapLocation(this);
        }

        Location = closestTile.Location;

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
        var range = TileSize * Percent33;


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
                        if (Mathf.Abs(delta.x) > BumpSpeed)
                            position = Vector2.MoveTowards(position, new Vector3(destination.x, position.y, position.z), BumpSpeed);
                        else if (Mathf.Abs(delta.y) > BumpSpeed)
                            position = Vector2.MoveTowards(position, new Vector3(position.x, destination.y, position.z), BumpSpeed);

                        var isSnapDistance = Vector2.Distance(position, destination) <= BumpSpeed;
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
                        if (Mathf.Abs(delta.x) > BumpSpeed)
                            position = Vector2.MoveTowards(position, new Vector3(destination.x, position.y, position.z), BumpSpeed);
                        else if (Mathf.Abs(delta.y) > BumpSpeed)
                            position = Vector2.MoveTowards(position, new Vector3(position.x, destination.y, position.z), BumpSpeed);

                        var isSnapDistance = Vector2.Distance(position, destination) <= BumpSpeed;
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

            yield return global::Wait.None();
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

            DamageTextManager.Spawn(hit.ToString(), position);

            //Shake Actor
            Shake(ShakeIntensity.Medium);

            UpdateHealthBar();

            //SlideIn sfx
            SoundSource.PlayOneShot(ResourceManager.SoundEffect($"Slash{Random.Int(1, 7)}"));

            yield return global::Wait.For(Interval.FiveTicks);
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

            DamageTextManager.Spawn(hit.ToString(), position);

            //Shake Actor
            Shake(ShakeIntensity.Medium);

            UpdateHealthBar();

            //SlideIn sfx
            SoundSource.PlayOneShot(ResourceManager.SoundEffect($"Slash{Random.Int(1, 7)}"));

            yield return global::Wait.For(Interval.FiveTicks);
        }

        Shake(ShakeIntensity.Stop);
        HP = remainingHP;
        position = currentTile.position;
    }



    public IEnumerator MissAttack()
    {
        yield return global::Wait.For(Interval.QuarterSecond);

        float ticks = 0;
        float duration = Interval.QuarterSecond;

        DamageTextManager.Spawn("Miss", position);

        while (ticks < duration)
        {
            ticks += Interval.OneTick;
            Shake(ShakeIntensity.Low);
            yield return global::Wait.OneTick();
        }

        Shake(ShakeIntensity.Stop);
    }

    public void CheckActionBar()
    {
        //Check abort state
        if (TurnManager.IsEnemyTurn || (!TurnManager.IsStartPhase && !TurnManager.IsMovePhase) || !IsAlive || !IsActive || Wait == WaitDuration)
            return;

        if (Wait < WaitDuration)
        {
            Wait += Time.deltaTime;
            Wait = Math.Clamp(Wait, 0, WaitDuration);
        }

        if (Wait == WaitDuration)
        {
            StartCoroutine(RadialBackFadeOut());
        }


        UpdateActionBar();
    }


    private void UpdateActionBar()
    {
        var scale = Renderers.ActionBarBack.transform.localScale;
        var x = Mathf.Clamp(scale.x * (Wait / WaitDuration), 0, scale.x);
        Renderers.ActionBar.transform.localScale = new Vector3(x, scale.y, scale.z);

        //Percent complete
        Renderers.ActionText.text = Wait < WaitDuration ? $@"{Math.Round(Wait / WaitDuration * 100)}" : "";

        //Seconds remaining
        //Renderers.RadialText.Text = Wait < WaitDuration ? $"{Math.Round(WaitDuration - Wait)}" : "";

    }


    public void CheckRadial()
    {
        UpdateRadial();
    }


    private void UpdateRadial()
    {
        //var fill = (360 * (Wait / WaitDuration));
        //Renderers.RadialFill.material.SetFloat("_Arc1", fill);

        //Renderers.RadialText.Text = Wait < WaitDuration ? $"{Math.Round(WaitDuration - Wait)}" : "";
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
            yield return global::Wait.OneTick();
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
            yield return global::Wait.OneTick();
        }

        Renderers.RadialBack.color = new Color(0, 0, 0, 0);
    }

    public IEnumerator Dissolve()
    {
        var alpha = 1f;
        Renderers.SetAlpha(alpha);

        PortraitManager.Dissolve(this);
        SoundSource.PlayOneShot(ResourceManager.SoundEffect("Death"));
        sortingOrder = ZAxis.Max;

        while (alpha > 0)
        {
            alpha -= Increment.OnePercent;
            alpha = Mathf.Clamp(alpha, 0, 1);
            Renderers.SetAlpha(alpha);
            yield return global::Wait.OneTick();
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
            yield return global::Wait.OneTick();
        }

        //Switch status status Sprite
        Renderers.StatusIcon.sprite = ResourceManager.statusSprites.First(x => x.id.Equals(status.ToString())).thumbnail;

        //Fade in
        alpha = 0;
        Renderers.StatusIcon.color = new Color(1, 1, 1, alpha);

        while (alpha < 1)
        {
            alpha += increment;
            alpha = Mathf.Clamp(alpha, 0, 1);
            Renderers.StatusIcon.color = new Color(1, 1, 1, alpha);

            yield return global::Wait.OneTick();
        }

        Renderers.StatusIcon.color = new Color(1, 1, 1, 1);

    }

    public void StartGlow()
    {
        //Check abort state
        if (!IsActive)
            return;

        Renderers.SetGlowColor(IsPlayer ? Quality.Color : Colors.Solid.Black);
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
                yield return global::Wait.OneTick();
            }

            Renderers.SetGlowAlpha(0);
            Renderers.SetBackAlpha(0);
            Destroy(gameObject);
            Actors.Remove(this);
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
            yield return global::Wait.OneTick();
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
            yield return global::Wait.OneTick();
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

        Wait = 0;
        WaitDuration = Random.Float(min, max);
        StartCoroutine(RadialBackFadeIn());
        UpdateRadial();
    }

    public void ReadyUp()
    {
        //Check abort state
        if (!IsAlive || !IsActive || !IsEnemy)
            return;

        Wait = WaitDuration;
        UpdateActionBar();
    }

    public IEnumerator SpawnIn(float delay = 0)
    {
        float alpha = 0;
        Renderers.SetAlpha(alpha);
        //Renderers.SetBackAlpha(alpha);
        //Renderers.SetGlowAlpha(Mathf.Clamp(alpha, 0.25f, 0.5f));

        yield return global::Wait.For(delay);

        while (alpha < 1)
        {
            alpha += Increment.OnePercent;
            alpha = Mathf.Clamp(alpha, 0, 1);
            Renderers.SetAlpha(alpha);
            //Renderers.SetBackAlpha(alpha);
            //Renderers.SetGlowAlpha(Mathf.Clamp(alpha, 0.25f, 0.5f));
            yield return global::Wait.OneTick();
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
    //    Destroy(GameObject);
    //    Actors.Destroy(this);
    //}



    //public void CheckLocationConflict()
    //{
    //    var other = Actors.FirstOrDefault(x => x != null && x.IsAlive && x.IsActive && !Equals(x) && Location.Equals(x.Location));
    //    if (other == null)
    //        return;

    //    SwapLocation(other);
    //}


}
