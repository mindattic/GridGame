using Assets.Scripts.Behaviors.Actor;
using Assets.Scripts.Instances.Actor;
using Assets.Scripts.Models;
using Game.Instances.Actor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ActorInstance : MonoBehaviour
{

    //Properties
    protected GameManager gameManager => GameManager.instance;
    protected AudioManager audioManager => gameManager.audioManager;
    protected TileManager tileManager => gameManager.tileManager;
    protected ResourceManager resourceManager => gameManager.resourceManager;
    protected TurnManager turnManager => gameManager.turnManager;
    protected VFXManager vfxManager => gameManager.vfxManager;
    protected DebugManager debugManager => gameManager.debugManager;
    protected DamageTextManager damageTextManager => gameManager.damageTextManager;
    protected PortraitManager portraitManager => gameManager.portraitManager;
    protected CoinManager coinManager => gameManager.coinManager;
    protected BoardInstance board => gameManager.board;
    protected float gameSpeed => gameManager.gameSpeed;
    protected float snapDistance => gameManager.snapDistance;
    protected float tileSize => gameManager.tileSize;
    protected Vector3 tileScale => gameManager.tileScale;
    protected float moveSpeed => gameManager.moveSpeed;
    protected ActorInstance focusedActor => gameManager.focusedActor;
    protected ActorInstance selectedPlayer => gameManager.selectedPlayer;
    protected bool HasFocusedActor => focusedActor != null;
    protected bool HasSelectedPlayer => selectedPlayer != null;
    protected List<ActorInstance> actors => gameManager.actors;
    protected IQueryable<ActorInstance> players => gameManager.players;

    //Variables
    public Character character;
    public Vector2Int location;
    public Vector2Int previousLocation;
    public Vector3 destination;
    public Team team = Team.Independant;
    public Quality quality = Rarity.Common;
    public int spawnDelay = -1;
    //public int turnDelay = 0;
    public int attackingPairCount = 0;
    public int supportingPairCount = 0;
    public float wiggleSpeed;
    public float wiggleAmplitude;
    public float glowIntensity;

    //Modules
    public ActorRenderers render = new ActorRenderers();
    public ActorStats stats = new ActorStats();
    public ActorFlags flags = new ActorFlags();
    public ActorAbilities abilities = new ActorAbilities();
    public ActorVFX vfx = new ActorVFX();
    public ActorWeapon weapon = new ActorWeapon();
    public ActorActions action = new ActorActions();
    public ActorMovement move = new ActorMovement();
    public ActorHealthBar healthBar = new ActorHealthBar();
    public ActorActionBar actionBar = new ActorActionBar();

    //Miscellaneous
    ActorSprite sprites;
    [SerializeField] public AnimationCurve glowCurve;
    //public VisualEffect attack;

    private void Awake()
    {
        render.opaque = gameObject.transform.GetChild(ActorLayer.Name.Opaque).GetComponent<SpriteRenderer>();
        render.quality = gameObject.transform.GetChild(ActorLayer.Name.Quality).GetComponent<SpriteRenderer>();
        render.glow = gameObject.transform.GetChild(ActorLayer.Name.Glow).GetComponent<SpriteRenderer>();
        render.parallax = gameObject.transform.GetChild(ActorLayer.Name.Parallax).GetComponent<SpriteRenderer>();
        render.thumbnail = gameObject.transform.GetChild(ActorLayer.Name.Thumbnail).GetComponent<SpriteRenderer>();
        render.frame = gameObject.transform.GetChild(ActorLayer.Name.Frame).GetComponent<SpriteRenderer>();
        render.statusIcon = gameObject.transform.GetChild(ActorLayer.Name.StatusIcon).GetComponent<SpriteRenderer>();
        render.healthBarBack = gameObject.transform.GetChild(ActorLayer.Name.HealthBar.Root).GetChild(ActorLayer.Name.HealthBar.Back).GetComponent<SpriteRenderer>() ?? throw new UnityException($"{ActorLayer.Name.HealthBar.Back} is null");
        render.healthBarDrain = gameObject.transform.GetChild(ActorLayer.Name.HealthBar.Root).GetChild(ActorLayer.Name.HealthBar.Drain).GetComponent<SpriteRenderer>() ?? throw new UnityException($"{ActorLayer.Name.HealthBar.Drain} is null");
        render.healthBarFill = gameObject.transform.GetChild(ActorLayer.Name.HealthBar.Root).GetChild(ActorLayer.Name.HealthBar.Fill).GetComponent<SpriteRenderer>() ?? throw new UnityException($"{ActorLayer.Name.HealthBar.Fill} is null");
        render.healthBarText = gameObject.transform.GetChild(ActorLayer.Name.HealthBar.Root).GetChild(ActorLayer.Name.HealthBar.Text).GetComponent<TextMeshPro>() ?? throw new UnityException($"{ActorLayer.Name.HealthBar.Text} is null");
        render.actionBarBack = gameObject.transform.GetChild(ActorLayer.Name.ActionBar.Root).GetChild(ActorLayer.Name.ActionBar.Back).GetComponent<SpriteRenderer>() ?? throw new UnityException($"{ActorLayer.Name.ActionBar.Back} is null");
        render.actionBarDrain = gameObject.transform.GetChild(ActorLayer.Name.ActionBar.Root).GetChild(ActorLayer.Name.ActionBar.Drain).GetComponent<SpriteRenderer>() ?? throw new UnityException($"{ActorLayer.Name.ActionBar.Drain} is null");
        render.actionBarFill = gameObject.transform.GetChild(ActorLayer.Name.ActionBar.Root).GetChild(ActorLayer.Name.ActionBar.Fill).GetComponent<SpriteRenderer>() ?? throw new UnityException($"{ActorLayer.Name.ActionBar.Fill} is null");
        render.actionBarText = gameObject.transform.GetChild(ActorLayer.Name.ActionBar.Root).GetChild(ActorLayer.Name.ActionBar.Text).GetComponent<TextMeshPro>() ?? throw new UnityException($"{ActorLayer.Name.ActionBar.Text} is null");
        render.mask = gameObject.transform.GetChild(ActorLayer.Name.Mask).GetComponent<SpriteMask>();
        render.radialBack = gameObject.transform.GetChild(ActorLayer.Name.RadialBack).GetComponent<SpriteRenderer>();
        render.radial = gameObject.transform.GetChild(ActorLayer.Name.RadialFill).GetComponent<SpriteRenderer>();
        render.radialText = gameObject.transform.GetChild(ActorLayer.Name.RadialText).GetComponent<TextMeshPro>();
        render.turnDelayText = gameObject.transform.GetChild(ActorLayer.Name.TurnDelayText).GetComponent<TextMeshPro>();
        render.nameTagText = gameObject.transform.GetChild(ActorLayer.Name.NameTagText).GetComponent<TextMeshPro>();
        render.weaponIcon = gameObject.transform.GetChild(ActorLayer.Name.WeaponIcon).GetComponent<SpriteRenderer>();
        render.selectionBox = gameObject.transform.GetChild(ActorLayer.Name.SelectionBox).GetComponent<SpriteRenderer>();

        action.Initialize(this);
        move.Initialize(this);
        healthBar.Initialize(this);
        actionBar.Initialize(this);


        wiggleSpeed = tileSize * 24f;
        wiggleAmplitude = 15f;  // Amplitude (difference from -45 degrees)

        glowIntensity = 1.3333f;

        //glowCurve = new AnimationCurve(
        //    new Keyframe(0f, 0f, 0f, 0f),      // First keyframe at time 0, value 0
        //    new Keyframe(1f, 0.25f, 0f, 0f)    // Second keyframe at time 1, value 0.25
        //);
        //glowCurve.preWrapMode = WrapMode.Loop;
        //glowCurve.postWrapMode = WrapMode.Loop;



    }

    private void Start()
    {

    }

    //Helpers
    public TileInstance CurrentTile => board.tileMap.GetTile(location); //tiles.First(x => x.location.Equals(location));
    public bool IsPlayer => team.Equals(Team.Player);
    public bool IsEnemy => team.Equals(Team.Enemy);
    public bool IsFocusedPlayer => HasFocusedActor && Equals(focusedActor);
    public bool IsSelectedPlayer => HasSelectedPlayer && Equals(selectedPlayer);
    public bool HasReachedDestination => position == destination;
    public bool OnNorthEdge => location.y == 1;
    public bool OnEastEdge => location.x == board.columnCount;
    public bool OnSouthEdge => location.y == board.rowCount;
    public bool OnWestEdge => location.x == 1;
    public bool IsActive => isActiveAndEnabled;
    public bool IsAlive => IsActive && stats.HP > 0;
    public bool IsDying => IsActive && stats.HP < 1;
    public bool IsDead => !IsActive && stats.HP < 1;
    public bool IsSpawnable => !IsActive && IsAlive && spawnDelay <= turnManager.currentTurn;
    public bool HasMaxAP => IsActive && IsAlive && stats.AP == stats.MaxAP;

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
        get => gameObject.transform.GetChild("Thumbnail").gameObject.transform.position;
        set => gameObject.transform.GetChild("Thumbnail").gameObject.transform.position = value;
    }

    public Quaternion rotation
    {
        get => gameObject.transform.rotation;
        set => gameObject.transform.rotation = value;
    }

    public Vector3 scale
    {
        get => gameObject.transform.localScale;
        set => gameObject.transform.localScale = value;
    }

    public Sprite thumbnail
    {
        get => render.thumbnail.sprite;
        set => render.thumbnail.sprite = value;
    }

    public int sortingOrder
    {
        get
        {
            return render.opaque.sortingOrder;
        }
        set
        {
            render.opaque.sortingOrder = value + ActorLayer.Value.Opaque;
            render.quality.sortingOrder = value + ActorLayer.Value.Quality;
            render.parallax.sortingOrder = value + ActorLayer.Value.Parallax;
            render.glow.sortingOrder = value + ActorLayer.Value.Glow;
            render.thumbnail.sortingOrder = value + ActorLayer.Value.Thumbnail;
            render.frame.sortingOrder = value + ActorLayer.Value.Frame;
            render.statusIcon.sortingOrder = value + ActorLayer.Value.StatusIcon;
            render.healthBarBack.sortingOrder = value + ActorLayer.Value.HealthBar.Back;
            render.healthBarDrain.sortingOrder = value + ActorLayer.Value.HealthBar.Drain;
            render.healthBarFill.sortingOrder = value + ActorLayer.Value.HealthBar.Fill;
            render.healthBarText.sortingOrder = value + ActorLayer.Value.HealthBar.Text;
            render.actionBarBack.sortingOrder = value + ActorLayer.Value.ActionBar.Back;
            render.actionBarFill.sortingOrder = value + ActorLayer.Value.ActionBar.Fill;
            render.actionBarText.sortingOrder = value + ActorLayer.Value.ActionBar.Text;
            render.mask.sortingOrder = value + ActorLayer.Value.Mask;
            render.radialBack.sortingOrder = value + ActorLayer.Value.RadialBack;
            render.radial.sortingOrder = value + ActorLayer.Value.RadialFill;
            render.radialText.sortingOrder = value + ActorLayer.Value.RadialText;
            render.turnDelayText.sortingOrder = value + ActorLayer.Value.TurnDelayText;
            render.nameTagText.sortingOrder = value + ActorLayer.Value.NameTagText;
            render.weaponIcon.sortingOrder = value + ActorLayer.Value.WeaponIcon;
            render.selectionBox.sortingOrder = value + ActorLayer.Value.SelectionBox;
        }
    }

    public bool IsSameColumn(Vector2Int other) => location.x == other.x;
    public bool IsSameRow(Vector2Int other) => location.y == other.y;
    public bool IsNorthOf(Vector2Int other) => IsSameColumn(other) && location.y == other.y - 1;
    public bool IsEastOf(Vector2Int other) => IsSameRow(other) && location.x == other.x + 1;
    public bool IsSouthOf(Vector2Int other) => IsSameColumn(other) && location.y == other.y + 1;
    public bool IsWestOf(Vector2Int other) => IsSameRow(other) && location.x == other.x - 1;
    public bool IsNorthWestOf(Vector2Int other) => location.x == other.x - 1 && location.y == other.y - 1;
    public bool IsNorthEastOf(Vector2Int other) => location.x == other.x + 1 && location.y == other.y - 1;
    public bool IsSouthWestOf(Vector2Int other) => location.x == other.x - 1 && location.y == other.y + 1;
    public bool IsSouthEastOf(Vector2Int other) => location.x == other.x + 1 && location.y == other.y + 1;
    public bool IsAdjacentTo(Vector2Int other) => (IsSameColumn(other) || IsSameRow(other)) && Vector2Int.Distance(location, other).Equals(1);

    public Direction GetAdjacentDirectionTo(ActorInstance other)
    {
        if (!IsAdjacentTo(other.location)) return Direction.None;
        if (IsNorthOf(other.location)) return Direction.South;
        if (IsEastOf(other.location)) return Direction.West;
        if (IsSouthOf(other.location)) return Direction.North;
        if (IsWestOf(other.location)) return Direction.East;

        return Direction.None;
    }

    public void Spawn(Vector2Int startLocation)
    {
        gameObject.SetActive(true);

        location = startLocation;
        position = Geometry.GetPositionByLocation(location);
        destination = position;
        sprites = resourceManager.ActorSprite(this.character.ToString());

        //TODO: Equip actor at stagemaanger load based on save file: party.json
        weapon.Type = Random.WeaponType();
        weapon.Attack = Random.Float(10, 15);
        weapon.Defense = Random.Float(0, 5);
        weapon.Name = $"{weapon.Type}";
        render.weaponIcon.sprite = resourceManager.WeaponType(weapon.Type).sprite;

        if (IsPlayer)
        {
            render.SetQualityColor(quality.Color);
            render.SetGlowColor(quality.Color);
            render.SetParallaxSprite(resourceManager.Seamless("WhiteFire"));
            render.SetParallaxMaterial(resourceManager.Material("PlayerParallax", thumbnail.texture));
            render.SetParallaxAlpha(0);
            render.SetThumbnailMaterial(resourceManager.Material("Sprites-Default", thumbnail.texture));
            render.SetFrameColor(quality.Color);
            render.SetHealthBarColor(ColorHelper.HealthBar.Green);
            render.SetActionBarColor(ColorHelper.ActionBar.Blue);
            render.SetSelectionBoxEnabled(isEnabled: false);
            vfx.Attack = resourceManager.VisualEffect("Blue_Slash_01");
        }
        else if (IsEnemy)
        {
            render.SetQualityColor(ColorHelper.Solid.Black);
            render.SetGlowColor(ColorHelper.Solid.Red);
            render.SetParallaxSprite(resourceManager.Seamless("RedFire"));
            render.SetParallaxMaterial(resourceManager.Material("EnemyParallax", thumbnail.texture));
            render.SetParallaxAlpha(0);
            render.SetThumbnailMaterial(resourceManager.Material("Sprites-Default", thumbnail.texture));
            render.SetFrameColor(ColorHelper.Solid.Red);
            render.SetHealthBarColor(ColorHelper.HealthBar.Green);
            render.SetActionBarColor(ColorHelper.ActionBar.Blue);
            render.SetSelectionBoxEnabled(isEnabled: false);
            vfx.Attack = resourceManager.VisualEffect("Double_Claw");
        }

        render.SetNameTagText(name);
        render.SetNameTagEnabled(isEnabled: debugManager.showActorNameTag);

        healthBar.Refresh();
        actionBar.Reset();
        action.ExecuteFadeIn();
        action.ExecuteSpin360();
    }

    void FixedUpdate()
    {
        //Check abort conditions
        if (!IsActive || !IsAlive || IsFocusedPlayer || IsSelectedPlayer)
            return;

        UpdateGlow();
    }



    public IEnumerator Attack(AttackResult attack)
    {
        if (attack.IsHit)
        {
            if (attack.IsCriticalHit)
            {
                var critVFX = resourceManager.VisualEffect("Yellow_Hit");
                vfxManager.Spawn(
                    critVFX,
                    attack.Opponent.position);
            }

            return vfxManager._Spawn(
                vfx.Attack,
                attack.Opponent.position,
                attack.Opponent._TakeDamage(attack));

        }
        else
        {
            return attack.Opponent.AttackMiss();
        }
    }


    public void CalculateAttackStrategy()
    {
        //Randomly select an Strength attackStrategy
        int[] ratios = { 50, 20, 15, 10, 5 };
        var attackStrategy = Random.Strategy(ratios);

        ActorInstance targetPlayer = null;

        switch (attackStrategy)
        {
            case AttackStrategy.AttackClosest:
                targetPlayer = players.Where(x => x.IsActive && x.IsAlive).OrderBy(x => Vector3.Distance(x.position, position)).FirstOrDefault();
                destination = Geometry.GetClosestAttackPosition(this, targetPlayer);
                break;

            case AttackStrategy.AttackWeakest:
                targetPlayer = players.Where(x => x.IsActive && x.IsAlive).OrderBy(x => x.stats.HP).FirstOrDefault();
                destination = Geometry.GetClosestAttackPosition(this, targetPlayer);
                break;

            case AttackStrategy.AttackStrongest:
                targetPlayer = players.Where(x => x.IsActive && x.IsAlive).OrderByDescending(x => x.stats.HP).FirstOrDefault();
                destination = Geometry.GetClosestAttackPosition(this, targetPlayer);
                break;

            case AttackStrategy.AttackRandom:
                targetPlayer = Random.Player;
                destination = Geometry.GetClosestAttackPosition(this, targetPlayer);
                break;

            case AttackStrategy.MoveAnywhere:
                var location = Random.Location;
                targetPlayer = null;
                destination = Geometry.GetPositionByLocation(location);
                break;
        }
    }


    public void ApplyTilt(Vector3 velocity, float tiltFactor, float rotationSpeed, float resetSpeed, Vector3 baseRotation)
    {
        if (velocity.magnitude > 0.01f) //Apply tilt if there is noticeable movement
        {
            // Determine if the movement is primarily vertical or horizontal

            bool isMovingVertical = Mathf.Abs(velocity.y) > Mathf.Abs(velocity.x);
            float velocityFactor = isMovingVertical ? velocity.y : velocity.x;
            float tiltZ = velocityFactor * tiltFactor; // Tilt on Z-axis based on velocity
            transform.localRotation = Quaternion.Slerp(
                transform.localRotation,
                Quaternion.Euler(0, 0, tiltZ),
                Time.deltaTime * rotationSpeed * gameSpeed
            );
        }
        else
        {
            //Reset rotation smoothly when velocity is minimal
            transform.localRotation = Quaternion.Slerp(
                transform.localRotation,
                Quaternion.Euler(baseRotation),
                Time.deltaTime * resetSpeed
            );
        }
    }

    private void UpdateGlow()
    {
        //Check abort conditions
        if (!IsActive || !IsAlive || !turnManager.IsStartPhase || (turnManager.IsPlayerTurn && !IsPlayer) || (turnManager.IsEnemyTurn && !IsEnemy))
            return;

        //Source: https://forum.unity.com/threads/how-to-make-an-object-move-up-and-down-on-a-loop.380159/
        var scale = new Vector3(
            glowIntensity + glowCurve.Evaluate(Time.time % glowCurve.length) * gameSpeed,
            glowIntensity + glowCurve.Evaluate(Time.time % glowCurve.length) * gameSpeed,
            1.0f);
        render.SetGlowScale(scale);
    }


    public void TakeDamage(AttackResult attack)
    {
        if (IsActive && IsAlive)
            StartCoroutine(_TakeDamage(attack));
    }

    public IEnumerator _TakeDamage(AttackResult attack)
    {
        //Check abort conditions
        if (!IsActive || !IsAlive)
            yield break;

        //Before:
        float ticks = 0f;
        float duration = Interval.TenTicks;

        bool isInvincible = (IsEnemy && debugManager.isEnemyInvincible) || (IsPlayer && debugManager.isPlayerInvincible);
        if (!isInvincible)
        {
            stats.PreviousHP = stats.HP;
            stats.HP -= attack.Damage;
            stats.HP = Mathf.Clamp(stats.HP, 0, stats.MaxHP);
            healthBar.Refresh();
        }

        damageTextManager.Spawn(attack.Damage.ToString(), position);
        audioManager.Play($"Slash{Random.Int(1, 7)}");

        //During:
        while (ticks < duration)
        {
            action.Grow();
            if (attack.IsCriticalHit)
                action.Shake(ShakeIntensity.Medium);

            ticks += Interval.OneTick;
            yield return Wait.For(Interval.OneTick);
        }

        //After:
        action.Shrink();
        action.Shake(ShakeIntensity.Stop);
    }

    public IEnumerator AttackMiss()
    {
        damageTextManager.Spawn("Miss", position);
        yield return action._Dodge();
    }

    public void Die()
    {
        StartCoroutine(_Die());
    }

    public IEnumerator _Die()
    {
        //Before:
        var alpha = 1f;
        render.SetAlpha(alpha);
        portraitManager.Dissolve(this);
        audioManager.Play("Death");
        sortingOrder = SortingOrder.Max;

        //During:
        var hasSpawnedCoins = false;
        while (alpha > 0f)
        {
            alpha -= Increment.OnePercent;
            alpha = Mathf.Clamp(alpha, Opacity.Transparent, Opacity.Opaque);
            render.SetAlpha(alpha);

            if (IsEnemy && !hasSpawnedCoins && alpha < Opacity.Percent10)
            {
                hasSpawnedCoins = true;
                var amount = 10;
                SpawnCoins(amount);
            }

            yield return Interval.FiveTicks;
        }

        //After:       
        location = board.NowhereLocation;
        destination = board.NowherePosition;
        position = destination;
        gameObject.SetActive(false);
    }

    private void SpawnCoins(int amount)
    {
        if (IsActive && IsAlive)
            StartCoroutine(_SpawnCoins(10)); //TODO: Spawn coins based on enemy stats...
    }

    IEnumerator _SpawnCoins(int amount)
    {
        var i = 0;
        do
        {
            coinManager.Spawn(position);
            i++;
        } while (i < amount);

        yield return true;
    }



    public void Teleport(Vector2Int location)
    {

        this.location = location;
        transform.position = Geometry.GetPositionByLocation(this.location);
    }

    public void ExecuteAngry()
    {
        if (IsActive && IsAlive)
            StartCoroutine(Angry());
    }

    private IEnumerator Angry()
    {
        //Check abort conditions
        if (!HasMaxAP || flags.isAngry)
            yield break;

        //Before:
        flags.isAngry = true;
        bool isDone = false;
        bool hasFlipped = false;
        var rotY = 0f;
        var speed = tileSize * 24f;
        render.turnDelayText.gameObject.transform.rotation = Geometry.Rotation(0, rotY, 0);

        //During:
        while (!isDone)
        {
            rotY += !hasFlipped ? speed : -speed;

            if (!hasFlipped && rotY >= 90f)
            {
                rotY = 90f;
                hasFlipped = true;
                //turnDelay--;
                //turnDelay = Math.Clamp(turnDelay, 0, 9);

                //actionBar.Refresh();
                //UpdateTurnDelayText();
            }

            isDone = hasFlipped && rotY <= 0f;
            if (isDone)
            {
                rotY = 0f;
            }

            //render.turnDelayText.gameObject.transform.rotation = Geometry.Rotation(0, rotY, 0);
            yield return Wait.OneTick();
        }

        //After:
        //render.turnDelayText.gameObject.transform.rotation = Geometry.Rotation(0, 0, 0);
        //if (turnDelay > 0)
        //{
        //    ExecuteTurnDelayWiggle();
        //}


        IEnumerator _()
        {
            render.SetThumbnailSprite(sprites.attack);
            yield return null;
        }

        if (IsActive && IsAlive)
            action.Spin90(_());

    }

    public void SetAttacking()
    {
        flags.IsAttacking = true;
        attackingPairCount++;
        sortingOrder = SortingOrder.Attacker;
    }

    public void SetDefending()
    {
        flags.IsDefending = true;
        sortingOrder = SortingOrder.Defender;
    }

    public void SetSupporting()
    {
        flags.IsSupporting = true;
        supportingPairCount++;
        sortingOrder = SortingOrder.Supporter;
    }

    public void SetDefault()
    {
        flags.IsAttacking = false;
        flags.IsDefending = false;
        flags.IsSupporting = false;
        sortingOrder = SortingOrder.Default;
    }


    public void SetReady()
    {
        //Check abort conditions
        if (!IsActive || !IsAlive || !IsEnemy)
            return;

        stats.AP = stats.MaxAP;
        stats.PreviousAP = stats.MaxAP;

        actionBar.Refresh();
    }
    //public IEnumerator _Bump(Direction direction)
    //{

    //    //Before:
    //    BumpStage stage = BumpStage.Start;
    //    var targetPosition = position;
    //    var range = tileSize * percent33;
    //    sortingOrder = SortingOrder.Default;

    //    //During:
    //    while (stage != BumpStage.End)
    //    {
    //        switch (stage)
    //        {
    //            case BumpStage.Start:
    //                {
    //                    sortingOrder = SortingOrder.Max;
    //                    position = CurrentTile.position;
    //                    targetPosition = Geometry.GetDirectionalPosition(position, direction, range);
    //                    stage = BumpStage.MoveToward;
    //                }
    //                break;

    //            case BumpStage.MoveToward:
    //                {
    //                    var delta = targetPosition - position;
    //                    if (Mathf.Abs(delta.x) > bumpSpeed)
    //                        position = Vector2.MoveTowards(position, new Vector3(targetPosition.x, position.y, position.z), bumpSpeed);
    //                    else if (Mathf.Abs(delta.y) > bumpSpeed)
    //                        position = Vector2.MoveTowards(position, new Vector3(position.x, targetPosition.y, position.z), bumpSpeed);

    //                    var isSnapDistance = Vector2.Distance(position, targetPosition) <= bumpSpeed;
    //                    if (isSnapDistance)
    //                    {
    //                        position = targetPosition;
    //                        targetPosition = CurrentTile.position;
    //                        stage = BumpStage.MoveAway;
    //                    }
    //                }
    //                break;

    //            case BumpStage.MoveAway:
    //                {
    //                    var delta = targetPosition - position;
    //                    if (Mathf.Abs(delta.x) > bumpSpeed)
    //                        position = Vector2.MoveTowards(position, new Vector3(targetPosition.x, position.y, position.z), bumpSpeed);
    //                    else if (Mathf.Abs(delta.y) > bumpSpeed)
    //                        position = Vector2.MoveTowards(position, new Vector3(position.x, targetPosition.y, position.z), bumpSpeed);

    //                    var isSnapDistance = Vector2.Distance(position, targetPosition) <= bumpSpeed;
    //                    if (isSnapDistance)
    //                    {
    //                        position = targetPosition;
    //                        targetPosition = CurrentTile.position;
    //                        stage = BumpStage.End;
    //                    }
    //                }
    //                break;

    //            case BumpStage.End:
    //                {
    //                    sortingOrder = SortingOrder.Default;
    //                    position = targetPosition;
    //                }
    //                break;
    //        }

    //        yield return Wait.OneTick();
    //    }

    //    //After:
    //    sortingOrder = SortingOrder.Default;
    //    position = targetPosition;
    //}

    //public void ParallaxFadeOutAsync()
    //{
    //    if (!IsActive && IsAlive)
    //        return;
    //    _FadeOut(render.parallax, Fill.FivePercent, Interval.OneTick, startAlpha: 0.5f, endAlpha: 0f);
    //}


    //public IEnumerator FadeIn(
    //     SpriteRenderer spriteRenderer,
    //     float increment,
    //     float interval,
    //     float startAlpha = 0f,
    //     float endAlpha = 1f)
    //{
    //    //Before:
    //    var alpha = startAlpha;
    //    spriteRenderer.color = new Color(1, 1, 1, alpha);

    //    //During:
    //    while (alpha < endAlpha)
    //    {
    //        alpha += increment;
    //        alpha = Mathf.Clamp(alpha, 0, endAlpha);
    //        spriteRenderer.color = new Color(1, 1, 1, alpha);
    //        yield return interval;
    //    }

    //    //After:
    //    spriteRenderer.color = new Color(1, 1, 1, endAlpha);
    //}

    //public void FadeIn(
    //   SpriteRenderer spriteRenderer,
    //   float increment,
    //   float interval,
    //   float startAlpha = 0f,
    //   float endAlpha = 1f)
    //{
    //    StartCoroutine(FadeIn(spriteRenderer, increment, interval, startAlpha, endAlpha));
    //}


    //public IEnumerator FillRadial()
    //{
    // Before:
    //flags.IsWaiting = true;

    // During:
    //while (HasSelectedPlayer)
    //{
    //if (ap < apMax)
    //{
    //    ap += stats.Speed;
    //    ap = Math.Clamp(ap, 0, apMax);
    //}

    //Fill ring
    //var fill = 360 - (360 * (ap / apMax));
    //render.radial.material.SetFloat("_Arc2", fill);

    //_Drain ring
    //var fill = (360 * (ap / apMax));
    //render.radial.material.SetFloat("_Arc1", fill);

    //Write text
    //render.radialText.text = ap < apMax ? $"{Math.Round(ap / apMax * 100)}" : "100";

    //yield return Wait.OneTick();
    //}




    //}


    // void Update()
    //{
    //if (!isTurnDelayWiggling)
    //{
    //    isTurnDelayWiggling = Random.Int(1, 20) == 1 ? true : false;
    //    StartCoroutine(TurnDelayWiggle());
    //}


    //Check abort status
    //if (!IsActive && IsAlive || isMoving)
    //    return;

    //var closestTile = Geometry.GetClosestTile(boardPosition);
    //if (boardLocation != closestTile.boardLocation)
    //{
    //    previousLocation = boardLocation;


    //    audioManager.Play($"Move{Random.Int(1, 6)}");

    //    var overlappingActor = FindOverlappingActor(closestTile);

    //    //Assign overlapping actors boardLocation to currentFps actor's boardLocation
    //    if (overlappingActor != null)
    //    {
    //        overlappingActor.boardLocation = boardLocation;
    //        overlappingActor.destination = Geometry.GetPositionByLocation(overlappingActor.boardLocation);
    //        overlappingActor.isMoving = true;
    //        StartCoroutine(overlappingActor.TowardDestination());
    //    }

    //    //Assign currentFps actor's boardLocation to closest tile boardLocation
    //    boardLocation = closestTile.boardLocation;
    //    StartCoroutine(TowardDestination());
    //}
    //}


    //public void AssignSkillWait()
    //{
    //    //Check abort conditions
    //    if (!IsActive && IsAlive)
    //        return;

    //    //TODO: Calculate based on stats....
    //    float min = (Interval.OneSecond * 20) - stats.Agility * Formulas.LuckModifier(stats);
    //    float max = (Interval.OneSecond * 40) - stats.Agility * Formulas.LuckModifier(stats);

    //    sp = 0;
    //    apMax = Random.Float(min, max);
    //}

    //private void UpdateTurnDelayText()
    //{
    //    render.SetTurnDelayTextEnabled(turnDelay > 0);
    //    render.SetTurnDelayText($"{turnDelay}");
    //}


    //public IEnumerator RadialBackFadeIn()
    //{
    //    //Before:
    //    var maxAlpha = 0.5f;
    //    var alpha = 0f;
    //    render.radialBack.color = new color(0, 0, 0, alpha);

    //    //During:
    //    while (alpha < maxAlpha)
    //    {
    //        alpha += Fill.OnePercent;
    //        alpha = Mathf.Clamp(alpha, 0, maxAlpha);
    //        render.radialBack.color = new color(0, 0, 0, alpha);
    //        yield return global::Destroy.OneTick();
    //    }

    //    //After:
    //    render.radialBack.color = new color(0, 0, 0, maxAlpha);
    //}

    //public IEnumerator RadialBackFadeOut()
    //{
    //    //Before:
    //    var maxAlpha = 0.5f;
    //    var alpha = maxAlpha;
    //    render.radialBack.color = new color(0, 0, 0, maxAlpha);

    //    //During:
    //    while (alpha > 0)
    //    {
    //        alpha -= Fill.OnePercent;
    //        alpha = Mathf.Clamp(alpha, 0, maxAlpha);
    //        render.radialBack.color = new color(0, 0, 0, alpha);
    //        yield return Destroy.OneTick();
    //    }

    //    //After:
    //    render.radialBack.color = new color(0, 0, 0, 0);
    //}

    //public void CalculateTurnDelay()
    //{
    //    IEnumerator _()
    //    {
    //        render.SetThumbnailSprite(sprites.idle);

    //        //render.SetThumbnailMaterial(resourceManager.Material("Sprites-Default", idle.texture));
    //        yield return null;
    //    }
    //    Spin90(_());

    //    //turnDelay = Formulas.CalculateTurnDelay(stats);
    //    //UpdateTurnDelayText();

    //    UpdateActionBar();
    //}


    //private void Swap(Vector2Int newLocation)
    //{
    //    boardLocation = newLocation;
    //    destination = Geometry.GetPositionByLocation(boardLocation);
    //    isMoving = true;
    //    StartCoroutine(TowardDestination());
    //}



    //public void AssignActionWait()
    //{
    //    //Check abort conditions
    //    if (!IsActive && IsAlive)
    //        return;

    //    //TODO: Calculate based on stats....
    //    float min = (Interval.OneSecond * 10) - amplitude * LuckModifier;
    //    float max = (Interval.OneSecond * 20) - amplitude * LuckModifier;

    //    ap = 0;
    //    maxAp = Random.Float(min, max);

    //    render.SetActionBarColor(Colors.ActionBarFill.Blue);
    //}

    //public void CheckActionBar()
    //{
    //Check abort conditions
    //if (!IsActive && IsAlive || turnManager.IsEnemyTurn || (!turnManager.IsStartPhase && !turnManager.IsMovePhase))
    //    return;


    //if (ap < maxAp)
    //{
    //    ap += Time.deltaTime;
    //    ap = Math.Clamp(ap, 0, maxAp);
    //}
    //else
    //{

    //    render.CycleActionBarColor();
    //}

    //UpdateActionBar();
    //}

    //public void UpdateActionBar()
    //{
    //    var scale = render.actionBarBack.transform.localScale;
    //    var x = Mathf.Clamp(scale.x * (ap / maxAp), 0, scale.x);
    //    render.actionBarFill.transform.localScale = new Vector3(x, scale.y, scale.z);

    //    Percent complete
    //    render.actionBarText.text = ap < maxAp ? $@"{Math.Round(ap / maxAp * 100)}" : "";

    //    Seconds remaining
    //    render.radialText.text = ap < maxAp ? $"{Math.Round(maxAp - ap)}" : "";



    //}

    //public void SetStatus(Status icon)
    //{
    //    //Check abort conditions
    //    if (!IsActive)
    //        return;

    //    StartCoroutine(SetStatusIcon(icon));
    //}

    //private IEnumerator SetStatusIcon(Status status)
    //{
    //    //Before:
    //    float increment = Fill.FivePercent;
    //    float alpha = render.statusIcon.color.a;
    //    render.statusIcon.color = new Color(1, 1, 1, alpha);

    //    //During:
    //    while (alpha > 0)
    //    {
    //        alpha -= increment;
    //        alpha = Mathf.Clamp(alpha, 0, 1);
    //        render.statusIcon.color = new Color(1, 1, 1, alpha);
    //        yield return Wait.OneTick();
    //    }

    //    //Before:
    //    render.statusIcon.sprite = resourceManager.statusSprites.First(x => x.id.Equals(status.ToString())).thumbnail;
    //    alpha = 0;
    //    render.statusIcon.color = new Color(1, 1, 1, alpha);

    //    //During:
    //    while (alpha < 1)
    //    {
    //        alpha += increment;
    //        alpha = Mathf.Clamp(alpha, 0, 1);
    //        render.statusIcon.color = new Color(1, 1, 1, alpha);

    //        yield return Wait.OneTick();
    //    }
    //}

    //private void CheckBobbing()
    //{
    //    //Check abort conditions
    //    if (!IsActive && IsAlive || !turnManager.IsStartPhase)
    //        return;


    //    //Source: https://forum.unity.com/threads/how-to-make-an-object-move-up-and-down-on-a-loop.380159/
    //    var pos = new Vector3(
    //        transform.boardPosition.x,
    //        transform.boardPosition.y + (glowCurve.Evaluate(Time.time % glowCurve.length) * (tileSize / 64)),
    //        transform.boardPosition.z);

    //    var rot = new Vector3(
    //       transform.angularRotation.x,
    //       transform.angularRotation.y,
    //       transform.angularRotation.z + (glowCurve.Evaluate(Time.time % glowCurve.length) * (tileSize / 128)));

    //    render.idle.transform.Rotate(Vector3.up * glowCurve.Evaluate(Time.time % glowCurve.length) * (tileSize / 3));

    //    render.glow.transform.boardPosition = pos;
    //    render.idle.transform.boardPosition = pos;
    //    render.frame.transform.boardPosition = pos;
    //    render.idle.transform.boardPosition = pos;
    //    render.idle.transform.angularRotation = rot;
    //}

}
