using Assets.Scripts.Behaviors.Actor;
using Assets.Scripts.Instances.Actor;
using Assets.Scripts.Utilities;
using Game.Instances.Actor;
using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

//Layers

public class ActorInstance : ExtendedMonoBehavior
{

    //Variables
    public Character character;
    public Vector2Int location;
    public Vector2Int previousLocation;
    //public bool isMoving = false;


    public Vector3 destination;
    public Team team = Team.Independant;
    public Quality quality = Rarity.Common;

    public ActorRenderers renderers = new ActorRenderers();
    public ActorStats stats = new ActorStats();
    public ActorFlags flags = new ActorFlags();
    public ActorVFX vfx = new ActorVFX();
    public ActorWeapon weapon = new ActorWeapon();

    public float ap = 0;
    public float maxAp = 100;
    public float sp = 0;
    public float spMax = -1;

    public int spawnDelay = -1;
    public int turnDelay = 0;


    public float wiggleSpeed;
    public float wiggleAmplitude = 15f; // Amplitude (difference from -45 degrees)
    public float healthDrainDelay = 2f;
    public float healthDrainAmount = 0.5f;
    public Vector3 initialHealthBarScale;

    ActorSprite sprites;

    [SerializeField] public AnimationCurve glowCurve;


    [SerializeField] public AnimationCurve slideRotationCurve;
    [SerializeField] public AnimationCurve slideScaleCurve;

    //public ActorHealthBar HealthBar;
    //public ActorThumbnail Thumbnail;

    //public VisualEffect attack;

    private void Awake()
    {
        renderers.opaque = gameObject.transform.GetChild(ActorLayer.Opaque).GetComponent<SpriteRenderer>();
        renderers.quality = gameObject.transform.GetChild(ActorLayer.Quality).GetComponent<SpriteRenderer>();
        renderers.glow = gameObject.transform.GetChild(ActorLayer.Glow).GetComponent<SpriteRenderer>();
        renderers.parallax = gameObject.transform.GetChild(ActorLayer.Parallax).GetComponent<SpriteRenderer>();
        renderers.thumbnail = gameObject.transform.GetChild(ActorLayer.Thumbnail).GetComponent<SpriteRenderer>();
        renderers.frame = gameObject.transform.GetChild(ActorLayer.Frame).GetComponent<SpriteRenderer>();
        renderers.statusIcon = gameObject.transform.GetChild(ActorLayer.StatusIcon).GetComponent<SpriteRenderer>();
        renderers.healthBarBack = gameObject.transform.GetChild(ActorLayer.HealthBarBack).GetComponent<SpriteRenderer>();
        renderers.healthBarDrain = gameObject.transform.GetChild(ActorLayer.HealthBarDrain).GetComponent<SpriteRenderer>();
        renderers.healthBar = gameObject.transform.GetChild(ActorLayer.HealthBar).GetComponent<SpriteRenderer>();
        renderers.healthText = gameObject.transform.GetChild(ActorLayer.HealthText).GetComponent<TextMeshPro>();
        renderers.actionBarBack = gameObject.transform.GetChild(ActorLayer.ActionBarBack).GetComponent<SpriteRenderer>();
        renderers.actionBar = gameObject.transform.GetChild(ActorLayer.ActionBar).GetComponent<SpriteRenderer>();
        renderers.actionText = gameObject.transform.GetChild(ActorLayer.ActionText).GetComponent<TextMeshPro>();
        renderers.skillRadialBack = gameObject.transform.GetChild(ActorLayer.RadialBack).GetComponent<SpriteRenderer>();
        renderers.skillRadial = gameObject.transform.GetChild(ActorLayer.RadialFill).GetComponent<SpriteRenderer>();
        renderers.skillRadialText = gameObject.transform.GetChild(ActorLayer.RadialText).GetComponent<TextMeshPro>();
        renderers.mask = gameObject.transform.GetChild(ActorLayer.Mask).GetComponent<SpriteMask>();
        renderers.turnDelayText = gameObject.transform.GetChild(ActorLayer.TurnDelayText).GetComponent<TextMeshPro>();
        renderers.nameTagText = gameObject.transform.GetChild(ActorLayer.NameTagText).GetComponent<TextMeshPro>();
        renderers.weaponIcon = gameObject.transform.GetChild(ActorLayer.WeaponIcon).GetComponent<SpriteRenderer>();
        renderers.selectionBox = gameObject.transform.GetChild(ActorLayer.SelectionBox).GetComponent<SpriteRenderer>();

        //HealthBar = new ActorHealthBar(GetGameObjectByLayer(ActorLayer.HealthBarBack), GetGameObjectByLayer(ActorLayer.HealthBar));
        //Thumbnail = new ActorThumbnail(GetGameObjectByLayer(ActorLayer.Thumbnail));

        initialHealthBarScale = renderers.healthBar.transform.localScale;
        wiggleSpeed = tileSize * 24f;
    }

    private void Start()
    {

    }

    #region Properties

    public float Level { get => stats.Level; set => stats.Level = value; }
    public float HP { get => stats.HP; set => stats.HP = value; }
    public float MaxHP { get => stats.MaxHP; set => stats.MaxHP = value; }
    public float Strength { get => stats.Strength; set => stats.Strength = value; }
    public float Vitality { get => stats.Vitality; set => stats.Vitality = value; }
    public float Agility { get => stats.Agility; set => stats.Agility = value; }
    public float Speed { get => stats.Speed; set => stats.Speed = value; }
    public float Luck { get => stats.Luck; set => stats.Luck = value; }


    public TileInstance currentTile => tiles.First(x => x.location.Equals(location));
    public bool IsPlayer => team.Equals(Team.Player);
    public bool IsEnemy => team.Equals(Team.Enemy);
    public bool IsFocusedPlayer => HasFocusedActor && Equals(focusedActor);
    public bool IsSelectedPlayer => HasSelectedPlayer && Equals(selectedPlayer);
    public bool IsAttacking => combatParticipants.attackingPairs.Any(x => x.actor1 == this || x.actor2 == this);
    public bool HasLocation => location != board.NowhereLocation;
    public bool HasReachedDestination => position == destination;
    public bool IsNorthEdge => location.y == 1;
    public bool IsEastEdge => location.x == board.columnCount;
    public bool IsSouthEdge => location.y == board.rowCount;
    public bool IsWestEdge => location.x == 1;
    public bool IsAlive => IsActive && stats.HP > 0;
    public bool IsDying => IsActive && stats.HP < 1;
    public bool IsDead => !IsActive && stats.HP < 1;

    public bool IsActive => this != null && isActiveAndEnabled;
    public bool IsInactive => this == null || !isActiveAndEnabled;
    public bool IsSpawnable => !IsActive && IsAlive && spawnDelay <= turnManager.currentTurn;
    public bool IsPlaying => IsActive && IsAlive;
    public bool IsReady => turnDelay == 0;


    #endregion


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
        get => gameObject.transform.GetChild(ActorLayer.Thumbnail).gameObject.transform.position;
        set => gameObject.transform.GetChild(ActorLayer.Thumbnail).gameObject.transform.position = value;
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
        get => renderers.thumbnail.sprite;
        set => renderers.thumbnail.sprite = value;
    }

    public int sortingOrder
    {
        get
        {
            return renderers.quality.sortingOrder;
        }
        set
        {
            renderers.opaque.sortingOrder = value + ActorLayer.Opaque;
            renderers.quality.sortingOrder = value + ActorLayer.Quality;
            renderers.parallax.sortingOrder = value + ActorLayer.Parallax;
            renderers.glow.sortingOrder = value + ActorLayer.Glow;
            renderers.thumbnail.sortingOrder = value + ActorLayer.Thumbnail;
            renderers.frame.sortingOrder = value + ActorLayer.Frame;
            renderers.statusIcon.sortingOrder = value + ActorLayer.StatusIcon;
            renderers.healthBarBack.sortingOrder = value + ActorLayer.HealthBarBack;
            renderers.healthBarDrain.sortingOrder = value + ActorLayer.HealthBarDrain;
            renderers.healthBar.sortingOrder = value + ActorLayer.HealthBar;
            renderers.healthText.sortingOrder = value + ActorLayer.HealthText;
            renderers.actionBarBack.sortingOrder = value + ActorLayer.ActionBarBack;
            renderers.actionBar.sortingOrder = value + ActorLayer.ActionBar;
            renderers.actionText.sortingOrder = value + ActorLayer.ActionText;
            renderers.skillRadialBack.sortingOrder = value + ActorLayer.RadialBack;
            renderers.skillRadial.sortingOrder = value + ActorLayer.RadialFill;
            renderers.skillRadialText.sortingOrder = value + ActorLayer.RadialText;
            renderers.selectionBox.sortingOrder = value + ActorLayer.SelectionBox;
            renderers.mask.sortingOrder = value + ActorLayer.Mask;
            renderers.turnDelayText.sortingOrder = value + ActorLayer.TurnDelayText;
            renderers.nameTagText.sortingOrder = value + ActorLayer.NameTagText;
            renderers.weaponIcon.sortingOrder = value + ActorLayer.WeaponIcon;
        }
    }

    #endregion

    #region Helper Methods

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

    private void SetLocation(Direction direction)
    {
        switch (direction)
        {
            case Direction.North: location += new Vector2Int(0, -1); break;
            case Direction.East: location += new Vector2Int(1, 0); break;
            case Direction.South: location += new Vector2Int(0, 1); break;
            case Direction.West: location += new Vector2Int(-1, 0); break;
        }
    }


    #endregion

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
        renderers.weaponIcon.sprite = resourceManager.WeaponType(weapon.Type).sprite;

        if (IsPlayer)
        {
            renderers.SetQualityColor(quality.Color);
            renderers.SetGlowColor(quality.Color);
            renderers.SetParallaxSprite(resourceManager.Seamless("WhiteFire"));
            renderers.SetParallaxMaterial(resourceManager.Material("PlayerParallax", thumbnail.texture));
            renderers.SetParallaxAlpha(0);
            renderers.SetThumbnailMaterial(resourceManager.Material("Sprites-Default", thumbnail.texture));
            renderers.SetFrameColor(quality.Color);
            renderers.SetHealthBarColor(Colors.HealthBar.Green);
            renderers.SetActionBarEnabled(isEnabled: false);
            renderers.SetSelectionBoxEnabled(isEnabled: false);
            renderers.SetTurnDelayTextEnabled(isEnabled: false);
            vfx.Attack = resourceManager.VisualEffect("Blue_Slash_01");


        }
        else if (IsEnemy)
        {
            renderers.SetQualityColor(Colors.Solid.Black);
            renderers.SetGlowColor(Colors.Solid.Red);
            renderers.SetParallaxSprite(resourceManager.Seamless("RedFire"));
            renderers.SetParallaxMaterial(resourceManager.Material("EnemyParallax", thumbnail.texture));
            renderers.SetParallaxAlpha(0);
            renderers.SetThumbnailMaterial(resourceManager.Material("Sprites-Default", thumbnail.texture));
            renderers.SetFrameColor(Colors.Solid.Red);
            renderers.SetHealthBarColor(Colors.HealthBar.Green);
            renderers.SetActionBarEnabled(isEnabled: true);
            renderers.SetSelectionBoxEnabled(isEnabled: false);
            renderers.SetTurnDelayTextEnabled(isEnabled: true);
            renderers.SetTurnDelayTextColor(Colors.Solid.White);
            vfx.Attack = resourceManager.VisualEffect("Double_Claw");
            CalculateTurnDelay();
        }

        renderers.SetNameTagText(name);
        renderers.SetNameTagEnabled(isEnabled: showActorNameTag);

        AssignSkillWait();
        UpdateHealthBar();
        UpdateActionBar();


        if (turnManager.IsFirstTurn)
        {
            renderers.SetAlpha(1);
        }
        else
        {
            StartCoroutine(FadeIn());
        }

    }

    public void CalculateTurnDelay()
    {
        IEnumerator _()
        {
            renderers.SetThumbnailSprite(sprites.idle);

            //renderers.SetThumbnailMaterial(resourceManager.Material("Sprites-Default", idle.texture));
            yield return null;
        }
        Spin90Async(_());

        turnDelay = Formulas.CalculateTurnDelay(stats);
        UpdateTurnDelayText();
    }

    ActorInstance FindOverlappingActor(TileInstance closestTile)
    {
        //Determine if two actors are overlapping the same boardLocation
        var overlappingActor = actors.FirstOrDefault(x => x != null
                                            && !x.Equals(this)
                                            && x.IsPlaying
                                            && !x.Equals(focusedActor)
                                            && !x.Equals(selectedPlayer)
                                            && x.location.Equals(closestTile.location));

        return overlappingActor;
    }






    void Update()
    {


        //if (!isTurnDelayWiggling)
        //{
        //    isTurnDelayWiggling = Random.Int(1, 20) == 1 ? true : false;
        //    StartCoroutine(TurnDelayWiggle());
        //}


        //Check abort status
        //if (!IsPlaying || isMoving)
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
        //        StartCoroutine(overlappingActor.MoveTowardDestination());
        //    }

        //    //Assign currentFps actor's boardLocation to closest tile boardLocation
        //    boardLocation = closestTile.boardLocation;
        //    StartCoroutine(MoveTowardDestination());
        //}
    }


    //private void Swap(Vector2Int newLocation)
    //{
    //    boardLocation = newLocation;
    //    destination = Geometry.GetPositionByLocation(boardLocation);
    //    isMoving = true;
    //    StartCoroutine(MoveTowardDestination());
    //}

    void FixedUpdate()
    {
        //Check abort state
        if (!IsPlaying || IsFocusedPlayer || IsSelectedPlayer)
            return;

        //CheckMovement();
        //CheckBobbing();
        UpdateGlow();
        //CheckFlicker();

        //CheckActionBar();
        CheckSkillRadial();


        //if (isRising && renderers.idle.transform.angularRotation.z < maxRot)
        //{
        //    renderers.idle.transform.Rotate(new Vector3(0, 0, rotSpeed));
        //}
        //else
        //{
        //    rotSpeed = Random.Int(2, 5) * 0.01f;
        //    minRot = -1f + (-1f * Random.Percent);
        //    isRising = false;
        //}

        //if (!isRising && renderers.idle.transform.angularRotation.z > minRot)
        //{
        //    renderers.idle.transform.Rotate(new Vector3(0, 0, -rotSpeed));

        //}
        //else
        //{
        //    rotSpeed = Random.Int(2, 5) * 0.01f;
        //    maxRot = 1f + (1f * Random.Percent);
        //    isRising = true;
        //}


    }

    public IEnumerator Attack(ActorInstance opponent, int damage, bool isCriticalHit = false)
    {
        if (isCriticalHit)
        {
            var crit = resourceManager.VisualEffect("Yellow_Hit");
            vfxManager.SpawnAsync(crit, opponent.position);
        }

        return vfxManager.Spawn(vfx.Attack, opponent.position, opponent.TakeDamage(damage, isCriticalHit));
    }

    public IEnumerator FadeIn()
    {
        //Before:
        float alpha = 0;
        renderers.SetAlpha(alpha);
        float delay = Random.Float(0f, 2f);
        yield return Wait.For(delay);

        //During:
        while (alpha < 1)
        {
            alpha += Increment.OnePercent;
            alpha = Mathf.Clamp(alpha, 0, 1);
            renderers.SetAlpha(alpha);
            yield return Wait.OneTick();
        }

        //After:
        alpha = 1;
        renderers.SetAlpha(alpha);
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
                targetPlayer = players.Where(x => x.IsPlaying).OrderBy(x => Vector3.Distance(x.position, position)).FirstOrDefault();
                destination = Geometry.GetClosestAttackPosition(this, targetPlayer);
                break;

            case AttackStrategy.AttackWeakest:
                targetPlayer = players.Where(x => x.IsPlaying).OrderBy(x => x.stats.HP).FirstOrDefault();
                destination = Geometry.GetClosestAttackPosition(this, targetPlayer);
                break;

            case AttackStrategy.AttackStrongest:
                targetPlayer = players.Where(x => x.IsPlaying).OrderByDescending(x => x.stats.HP).FirstOrDefault();
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


    public void CheckLocationChanged()
    {
        //Check if currentFps actor is closer to another tile (i.e.: it has moved)
        var closestTile = Geometry.GetClosestTile(position);
        if (location == closestTile.location)
            return;

        previousLocation = location;

        //audioManager.Play($"Move{Random.Int(1, 6)}");

        CheckActorOverlapping(closestTile);

        //Assign currentFps actor's boardLocation to closest tile boardLocation
        location = closestTile.location;
    }

    public void CheckActorOverlapping(TileInstance closestTile)
    {
        var overlappingActor = FindOverlappingActor(closestTile);
        if (overlappingActor == null)
            return;

        overlappingActor.location = location;
        overlappingActor.destination = Geometry.GetPositionByLocation(overlappingActor.location);
        overlappingActor.flags.IsMoving = true;
        overlappingActor.flags.IsSwapping = true;
        StartCoroutine(overlappingActor.MoveTowardDestination());
    }

    private void ApplyTilt(Vector3 velocity, float tiltFactor, float rotationSpeed, float resetSpeed, Vector3 baseRotation)
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

            //if (isMovingVertical)
            //{
            //    //Tilt for vertical movement
            //    float tiltZ = velocity.y * tiltFactor; // Tilt on Z-axis based on X movement
            //    transform.localRotation = Quaternion.Slerp(
            //        transform.localRotation,
            //        Quaternion.Euler(0, 0, tiltZ),
            //        Time.deltaTime * rotationSpeed * gameSpeed
            //    );
            //}
            //else
            //{
            //    //Tilt for horizontal movement
            //    float tiltZ = velocity.x * tiltFactor; // Tilt on Z-axis based on X movement
            //    transform.localRotation = Quaternion.Slerp(
            //        transform.localRotation,
            //        Quaternion.Euler(0, 0, tiltZ),
            //        Time.deltaTime * rotationSpeed * gameSpeed
            //    );
            //}
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

    public IEnumerator MoveTowardCursor()
    {
        // Before:
        flags.IsMoving = true;
        sortingOrder = SortingOrder.Max;

        Vector3 prevPosition = position; // Store the initial position
        float tiltFactor = 25f; // How much tilt to apply based on movement
        float rotationSpeed = 10f; // Speed at which the tilt adjusts
        float resetSpeed = 5f; // Speed at which the rotation resets
        var baseRotation = Vector3.zero;

        // During:
        while (IsFocusedPlayer || IsSelectedPlayer)
        {
            sortingOrder = SortingOrder.Max;

            var cursorPosition = mousePosition3D + mouseOffset;
            cursorPosition.x = Mathf.Clamp(cursorPosition.x, board.bounds.Left, board.bounds.Right);
            cursorPosition.y = Mathf.Clamp(cursorPosition.y, board.bounds.Bottom, board.bounds.Top);

            //Snap selected player to cursor
            position = cursorPosition;

            //Calculate velocity
            Vector3 velocity = position - prevPosition;

            //Apply tilt effect
            ApplyTilt(velocity, tiltFactor, rotationSpeed, resetSpeed, baseRotation);

            // Update previous position for next frame
            prevPosition = position;

            CheckLocationChanged();

            destination = position;

            yield return Wait.None();
        }

        // After:
        flags.IsMoving = false;
        sortingOrder = SortingOrder.Default;

        // Reset rotation at the end
        transform.localRotation = Quaternion.Euler(baseRotation);
    }

    public IEnumerator MoveTowardDestination()
    {
        // Before:
        Vector3 initialPosition = position;
        Vector3 initialScale = tileScale;
        scale = tileScale;
        audioManager.Play($"Slide");
        sortingOrder = SortingOrder.Moving;

        // During:
        while (!HasReachedDestination)
        {
            sortingOrder = SortingOrder.Moving;

            var delta = destination - position;
            if (Mathf.Abs(delta.x) > snapDistance)
            {
                position = Vector2.MoveTowards(position, new Vector3(destination.x, position.y, position.z), moveSpeed);

                // Snap horizontal boardPosition (if applicable)
                if (Mathf.Abs(delta.x) <= snapDistance)
                    position = new Vector3(destination.x, position.y, position.z);
            }
            else if (Mathf.Abs(delta.y) > snapDistance)
            {
                position = Vector2.MoveTowards(position, new Vector3(position.x, destination.y, position.z), moveSpeed);

                // Snap vertical boardPosition (if applicable)
                if (Mathf.Abs(delta.y) <= snapDistance)
                    position = new Vector3(position.x, destination.y, position.z);
            }

            if (flags.IsSwapping)
            {
                //Calculate velocity
                Vector3 velocity = destination - position;

                //Apply tilt effect
                ApplyTilt(velocity, 25f, 10f, 5f, Vector3.zero);
            }

            CheckLocationChanged();

            //Determine whether to snap to destination
            bool isSnapDistance = Vector2.Distance(position, destination) <= snapDistance;
            if (isSnapDistance)
                position = destination;

            yield return Wait.OneTick();
        }

        //After:
        flags.IsMoving = false;
        flags.IsSwapping = false;
        scale = tileScale;
        transform.rotation = Quaternion.identity; //Reset rotation to default
        sortingOrder = SortingOrder.Default;
    }
    public void CheckActionBar()
    {
        //Check abort state
        //if (!IsPlaying || turnManager.IsEnemyTurn || (!turnManager.IsStartPhase && !turnManager.IsMovePhase))
        //    return;


        //if (ap < maxAp)
        //{
        //    ap += Time.deltaTime;
        //    ap = Math.Clamp(ap, 0, maxAp);
        //}
        //else
        //{

        //    renderers.CycleActionBarColor();
        //}

        //UpdateActionBar();
    }

    public void UpdateActionBar()
    {
        var scale = renderers.actionBarBack.transform.localScale;
        var x = Mathf.Clamp(scale.x * (ap / maxAp), 0, scale.x);
        renderers.actionBar.transform.localScale = new Vector3(x, scale.y, scale.z);

        //Percent complete
        renderers.actionText.text = ap < maxAp ? $@"{Math.Round(ap / maxAp * 100)}" : "";

        //Seconds remaining
        //renderers.skillRadialText.text = ap < maxAp ? $"{Math.Round(maxAp - ap)}" : "";



    }

    public void CheckSkillRadial()
    {
        //if (sp < spMax)
        //{
        //    sp += Time.deltaTime;
        //    sp = Math.Clamp(ap, 0, spMax);
        //}

        UpdateSkillRadial();
    }

    public void UpdateSkillRadial()
    {
        //var fill = 360 - (360 * (sp / spMax));
        //renderers.skillRadial.material.SetFloat("_Arc2", fill);

        var fill = (360 * (sp / spMax));
        renderers.skillRadial.material.SetFloat("_Arc1", fill);
        renderers.skillRadialText.text = sp < spMax ? $"{Math.Round(sp / spMax * 100)}%" : "100%";
    }


    private void CheckBobbing()
    {
        //Check abort state
        if (!IsPlaying || !turnManager.IsStartPhase)
            return;


        //Source: https://forum.unity.com/threads/how-to-make-an-object-move-up-and-down-on-a-loop.380159/
        //var pos = new Vector3(
        //    transform.boardPosition.x,
        //    transform.boardPosition.y + (glowCurve.Evaluate(Time.time % glowCurve.length) * (tileSize / 64)),
        //    transform.boardPosition.z);

        //var rot = new Vector3(
        //   transform.angularRotation.x,
        //   transform.angularRotation.y ,
        //   transform.angularRotation.z + (glowCurve.Evaluate(Time.time % glowCurve.length) * (tileSize / 128)));

        //renderers.idle.transform.Rotate(Vector3.up * glowCurve.Evaluate(Time.time % glowCurve.length) * (tileSize / 3));

        //renderers.glow.transform.boardPosition = pos;
        //renderers.idle.transform.boardPosition = pos;
        //renderers.frame.transform.boardPosition = pos;
        //renderers.idle.transform.boardPosition = pos;
        //renderers.idle.transform.angularRotation = rot;
    }

    private void UpdateGlow()
    {
        //Check abort state
        if (!IsPlaying || !turnManager.IsStartPhase || (turnManager.IsPlayerTurn && !IsPlayer) || (turnManager.IsEnemyTurn && !IsEnemy))
            return;

        //Source: https://forum.unity.com/threads/how-to-make-an-object-move-up-and-down-on-a-loop.380159/
        var scale = new Vector3(
            1.5f + glowCurve.Evaluate(Time.time % glowCurve.length) * gameSpeed,
            1.5f + glowCurve.Evaluate(Time.time % glowCurve.length) * gameSpeed,
            1.0f);
        renderers.SetGlowScale(scale);
    }

    private void Shake(float intensity)
    {
        thumbnailPosition = currentTile.position;

        if (intensity <= 0)
            return;

        var amount = new Vector3(Random.Range(-intensity), Random.Range(intensity), 1);
        thumbnailPosition += amount;
    }

    public IEnumerator Dodge()
    {
        //Before:
        DodgeStage stage = DodgeStage.Start;
        var targetRotation = new Vector3(
            15f,
            70f,
            15f);
        var currentRotation = Vector3.zero;
        var rotationSpeed = 12f;
        var minScale = 0.9f;
        float progress = 0f;
        var randomDirection = new Vector3Int(
            Random.Boolean ? -1 : 1,
            Random.Boolean ? -1 : 1,
            Random.Boolean ? -1 : 1);

        // During:
        while (stage != DodgeStage.End)
        {
            switch (stage)
            {
                case DodgeStage.Start:
                    {
                        currentRotation = Vector3.zero;
                        progress = 0f;
                        scale = tileScale;
                        rotation = Geometry.Rotation(currentRotation);

                        stage = DodgeStage.TwistForward;
                    }
                    break;

                case DodgeStage.TwistForward:
                    {
                        //SaveProfile forward progress and sync rotation/scaleMultiplier
                        progress += rotationSpeed / targetRotation.y; //Normalize progress
                        progress = Mathf.Clamp01(progress); //Clamp between 0 and 1

                        //Random twist direction on X, Y, and Z axes
                        currentRotation.x = Mathf.Lerp(0f, targetRotation.x, progress) * randomDirection.x;
                        currentRotation.y = Mathf.Lerp(0f, targetRotation.y, progress) * randomDirection.y;
                        currentRotation.z = Mathf.Lerp(0f, targetRotation.z, progress) * randomDirection.z;

                        //Calculate scaleMultiplier based on forward progress
                        float scaleFactor = Mathf.Lerp(1f, minScale, progress);
                        scale = tileScale * scaleFactor;

                        //Apply rotation (random X, Y, and Z axis twisting) and scaling
                        rotation = Geometry.Rotation(currentRotation.x, currentRotation.y, currentRotation.z);

                        //If fully twisted forward, move to TwistBackward
                        if (progress >= 1f)
                        {
                            progress = 0f; //Reset backward progress
                            stage = DodgeStage.TwistBackward;
                        }
                    }
                    break;

                case DodgeStage.TwistBackward:
                    {
                        //SaveProfile backward progress and sync rotation/scaleMultiplier
                        progress += rotationSpeed / targetRotation.y; //Normalize progress
                        progress = Mathf.Clamp01(progress); //Clamp between 0 and 1

                        //Reverse random twist direction on X, Y, and Z axes
                        currentRotation.x = Mathf.Lerp(targetRotation.x, 0f, progress) * randomDirection.x;
                        currentRotation.y = Mathf.Lerp(targetRotation.y, 0f, progress) * randomDirection.y;
                        currentRotation.z = Mathf.Lerp(targetRotation.z, 0f, progress) * randomDirection.z;

                        //Calculate scaleMultiplier based on backward progress
                        float scaleFactor = Mathf.Lerp(minScale, 1f, progress);
                        scale = tileScale * scaleFactor;

                        //Apply reverse rotation (random X, Y, and Z axis twisting) and scaling
                        rotation = Geometry.Rotation(currentRotation);

                        //If fully twisted back, move to End
                        if (progress >= 1f)
                        {
                            stage = DodgeStage.End;
                        }
                    }
                    break;

                case DodgeStage.End:
                    {
                        currentRotation = Vector3.zero;
                        scale = tileScale;
                        rotation = Geometry.Rotation(currentRotation);
                    }
                    break;
            }

            yield return Wait.OneTick();
        }

        //After:
        currentRotation = Vector3.zero;
        scale = tileScale;
        rotation = Geometry.Rotation(currentRotation);
    }



    public IEnumerator Bump(Direction direction)
    {

        //Before:
        BumpStage stage = BumpStage.Start;
        var targetPosition = position;
        var range = tileSize * percent33;
        sortingOrder = SortingOrder.Default;

        //During:
        while (stage != BumpStage.End)
        {
            switch (stage)
            {
                case BumpStage.Start:
                    {
                        sortingOrder = SortingOrder.Max;
                        position = currentTile.position;
                        targetPosition = Geometry.GetDirectionalPosition(position, direction, range);
                        stage = BumpStage.MoveToward;
                    }
                    break;

                case BumpStage.MoveToward:
                    {
                        var delta = targetPosition - position;
                        if (Mathf.Abs(delta.x) > bumpSpeed)
                            position = Vector2.MoveTowards(position, new Vector3(targetPosition.x, position.y, position.z), bumpSpeed);
                        else if (Mathf.Abs(delta.y) > bumpSpeed)
                            position = Vector2.MoveTowards(position, new Vector3(position.x, targetPosition.y, position.z), bumpSpeed);

                        var isSnapDistance = Vector2.Distance(position, targetPosition) <= bumpSpeed;
                        if (isSnapDistance)
                        {
                            position = targetPosition;
                            targetPosition = currentTile.position;
                            stage = BumpStage.MoveAway;
                        }
                    }
                    break;

                case BumpStage.MoveAway:
                    {
                        var delta = targetPosition - position;
                        if (Mathf.Abs(delta.x) > bumpSpeed)
                            position = Vector2.MoveTowards(position, new Vector3(targetPosition.x, position.y, position.z), bumpSpeed);
                        else if (Mathf.Abs(delta.y) > bumpSpeed)
                            position = Vector2.MoveTowards(position, new Vector3(position.x, targetPosition.y, position.z), bumpSpeed);

                        var isSnapDistance = Vector2.Distance(position, targetPosition) <= bumpSpeed;
                        if (isSnapDistance)
                        {
                            position = targetPosition;
                            targetPosition = currentTile.position;
                            stage = BumpStage.End;
                        }
                    }
                    break;

                case BumpStage.End:
                    {
                        sortingOrder = SortingOrder.Default;
                        position = targetPosition;
                    }
                    break;
            }

            yield return Wait.OneTick();
        }

        //After:
        sortingOrder = SortingOrder.Default;
        position = targetPosition;
    }


    public IEnumerator TakeDamage(int damage, bool isCriticalHit = false)
    {
        //Check abort state
        if (!IsPlaying)
            yield break;

        //Before:
        float ticks = 0f;
        float duration = Interval.TenTicks;


        stats.PreviousHP = stats.HP;
        stats.HP -= damage;
        stats.HP = Mathf.Clamp(stats.HP, 0, stats.MaxHP);
        UpdateHealthBar();

        var text = Math.Abs(damage).ToString();
        damageTextManager.Spawn(text, position);
        audioManager.Play($"Slash{Random.Int(1, 7)}");

        //During:
        while (ticks < duration)
        {
            GrowAsync();
            if (isCriticalHit)
                Shake(shakeIntensity.Medium);

            ticks += Interval.OneTick;
            yield return Wait.For(Interval.OneTick);
        }

        //After:
        ShrinkAsync();
        Shake(shakeIntensity.Stop);
    }

    public void TakeDamageAsync(int damage, bool isCriticalHit = false)
    {
        //Check abort state
        if (!IsPlaying)
            return;

        StartCoroutine(TakeDamage(damage, isCriticalHit));
    }

    public IEnumerator AddAp(float amount)
    {
        //Before:
        var targetAP = Mathf.Clamp(ap + amount, 0, maxAp);
        var increment = 1f;

        //During:
        while (ap < targetAP)
        {
            ap += increment;
            ap = Mathf.Clamp(ap, 0, maxAp);

            UpdateActionBar();
            yield return Wait.For(Interval.FiveTicks);
        }

        //After:
        ap = targetAP;
        UpdateActionBar();
    }



    public void AddApAsync(float amount)
    {
        StartCoroutine(AddAp(amount));
    }

    public IEnumerator AddSp(float amount)
    {
        //Before:
        float ticks = 0f;
        float duration = Interval.OneSecond;

        //During:
        while (ticks < duration)
        {
            ticks += Interval.OneTick;
            sp += Interval.OneTick * amount * 0.1f;
            yield return Wait.OneTick();
        }

        //After:
        Shake(shakeIntensity.Stop);
    }

    public void AddSpAsync(float amount)
    {
        StartCoroutine(AddSp(amount));
    }


    public IEnumerator AttackMiss()
    {
        damageTextManager.Spawn("Miss", position);
        yield return Dodge();
    }

    private void UpdateHealthBar()
    {
        var x = CalculateHealthBarScale(stats.HP);
        renderers.healthBar.transform.localScale = new Vector3(x, initialHealthBarScale.y, initialHealthBarScale.z);
        renderers.healthText.text = $@"{stats.HP}/{stats.MaxHP}";

        StartCoroutine(DrainHealthBar());
    }

    private float CalculateHealthBarScale(float x)
    {
        return Mathf.Clamp(initialHealthBarScale.x * (x / stats.MaxHP), 0, initialHealthBarScale.x);
    }

    public IEnumerator DrainHealthBar()
    {
        //Before:
        float x = 0;

        //During:
        yield return new WaitForSeconds(healthDrainDelay);

        while (stats.HP < stats.PreviousHP)
        {
            stats.PreviousHP -= healthDrainAmount;
            x = CalculateHealthBarScale(stats.PreviousHP);
            renderers.healthBarDrain.transform.localScale = new Vector3(x, initialHealthBarScale.y, initialHealthBarScale.z);
            yield return Wait.OneTick();
        }

        //After:
        stats.PreviousHP = stats.HP;
        x = CalculateHealthBarScale(stats.PreviousHP);
        renderers.healthBarDrain.transform.localScale = new Vector3(x, initialHealthBarScale.y, initialHealthBarScale.z);
    }

    private void UpdateTurnDelayText()
    {
        renderers.SetTurnDelayTextEnabled(turnDelay > 0);
        //renderers.SetTurnDelayTextColor(turnDelay > 0 ? Colors.Solid.White : Colors.Solid.Red);
        renderers.SetTurnDelayText($"{turnDelay}");
    }


    //public IEnumerator RadialBackFadeIn()
    //{
    //    //Before:
    //    var maxAlpha = 0.5f;
    //    var alpha = 0f;
    //    renderers.skillRadialBack.color = new color(0, 0, 0, alpha);

    //    //During:
    //    while (alpha < maxAlpha)
    //    {
    //        alpha += Increment.OnePercent;
    //        alpha = Mathf.Clamp(alpha, 0, maxAlpha);
    //        renderers.skillRadialBack.color = new color(0, 0, 0, alpha);
    //        yield return global::Destroy.OneTick();
    //    }

    //    //After:
    //    renderers.skillRadialBack.color = new color(0, 0, 0, maxAlpha);
    //}

    //public IEnumerator RadialBackFadeOut()
    //{
    //    //Before:
    //    var maxAlpha = 0.5f;
    //    var alpha = maxAlpha;
    //    renderers.skillRadialBack.color = new color(0, 0, 0, maxAlpha);

    //    //During:
    //    while (alpha > 0)
    //    {
    //        alpha -= Increment.OnePercent;
    //        alpha = Mathf.Clamp(alpha, 0, maxAlpha);
    //        renderers.skillRadialBack.color = new color(0, 0, 0, alpha);
    //        yield return Destroy.OneTick();
    //    }

    //    //After:
    //    renderers.skillRadialBack.color = new color(0, 0, 0, 0);
    //}

    public IEnumerator Die()
    {
        //Before:
        var alpha = 1f;
        renderers.SetAlpha(alpha);
        portraitManager.Dissolve(this);
        audioManager.Play("Death");
        sortingOrder = SortingOrder.Max;

        var hasSpawnedCoins = false;


        //During:
        while (alpha > 0f)
        {
            alpha -= Increment.OnePercent;
            alpha = Mathf.Clamp(alpha, 0f, 1f);
            renderers.SetAlpha(alpha);

            if (IsEnemy && !hasSpawnedCoins && alpha < 5f)
            {
                hasSpawnedCoins = true;

                IEnumerator spawnCoins(int amount)
                {
                    var i = 0;
                    do
                    {
                        coinManager.Spawn(position);
                        i++;
                    } while (i < amount);

                    yield return true;
                }
                StartCoroutine(spawnCoins(10)); //TODO: Spawn coins based on enemy stats...
            }

            yield return Interval.FiveTicks;
        }

        //After:       
        location = board.NowhereLocation;
        destination = board.NowherePosition;
        position = destination;
        gameObject.SetActive(false);
    }

    public void DieAsync()
    {
        StartCoroutine(Die());
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
        //Before:
        float increment = Increment.FivePercent;
        float alpha = renderers.statusIcon.color.a;
        renderers.statusIcon.color = new Color(1, 1, 1, alpha);

        //During:
        while (alpha > 0)
        {
            alpha -= increment;
            alpha = Mathf.Clamp(alpha, 0, 1);
            renderers.statusIcon.color = new Color(1, 1, 1, alpha);
            yield return Wait.OneTick();
        }

        //Before:
        renderers.statusIcon.sprite = resourceManager.statusSprites.First(x => x.id.Equals(status.ToString())).thumbnail;
        alpha = 0;
        renderers.statusIcon.color = new Color(1, 1, 1, alpha);

        //During:
        while (alpha < 1)
        {
            alpha += increment;
            alpha = Mathf.Clamp(alpha, 0, 1);
            renderers.statusIcon.color = new Color(1, 1, 1, alpha);

            yield return Wait.OneTick();
        }
    }

    //public void AssignActionWait()
    //{
    //    //Check abort state
    //    if (!IsPlaying)
    //        return;

    //    //TODO: Calculate based on stats....
    //    float min = (Interval.OneSecond * 10) - amplitude * LuckModifier;
    //    float max = (Interval.OneSecond * 20) - amplitude * LuckModifier;

    //    ap = 0;
    //    maxAp = Random.Float(min, max);

    //    renderers.SetActionBarColor(Colors.ActionBar.Blue);
    //}

    public void AssignSkillWait()
    {
        //Check abort state
        if (!IsPlaying)
            return;

        //TODO: Calculate based on stats....
        float min = (Interval.OneSecond * 20) - stats.Agility * Formulas.LuckModifier(stats);
        float max = (Interval.OneSecond * 40) - stats.Agility * Formulas.LuckModifier(stats);

        sp = 0;
        spMax = Random.Float(min, max);
    }


    public void SetReady()
    {
        //Check abort state
        if (!IsPlaying || !IsEnemy)
            return;

        turnDelay = 0;
        CheckWeaponWiggle();
        UpdateTurnDelayText();
    }

    public IEnumerator Grow(float maxSize = 0f)
    {
        //Before:
        if (maxSize == 0)
            maxSize = tileSize * 1.1f;
        sortingOrder = SortingOrder.Attacker;
        float minSize = scale.x;
        float increment = tileSize * 0.01f;
        float size = minSize;
        scale = new Vector3(size, size, 0);

        //During:
        while (size < maxSize)
        {
            size += increment;
            size = Mathf.Clamp(size, minSize, maxSize);
            scale = new Vector3(size, size, 0);
            yield return Wait.OneTick();
        }

        //After:
        scale = new Vector3(maxSize, maxSize, 0);
    }
    public void GrowAsync(float maxSize = 0f)
    {
        if (maxSize == 0)
            maxSize = tileSize * 1.1f;

        StartCoroutine(Grow(maxSize));
    }

    public IEnumerator Shrink(float minSize = 0f)
    {
        //Before:
        if (minSize == 0)
            minSize = tileSize;
        float maxSize = scale.x;
        float increment = tileSize * 0.01f;
        float size = maxSize;
        scale = new Vector3(size, size, 0);

        //During:
        while (size > minSize)
        {
            size -= increment;
            size = Mathf.Clamp(size, minSize, maxSize);
            scale = new Vector3(size, size, 0);
            yield return Wait.OneTick();
        }

        //After:
        scale = new Vector3(minSize, minSize, 0);
        sortingOrder = SortingOrder.Default;

    }

    public void ShrinkAsync(float minSize = 0f)
    {
        if (minSize == 0)
            minSize = tileSize;
        StartCoroutine(Shrink(minSize));
    }


    public void Spin90Async(IEnumerator triggeredEvent = null)
    {
        StartCoroutine(Spin90(triggeredEvent));
    }

    public IEnumerator Spin90(IEnumerator triggeredEvent = null)
    {
        //Before:
        bool isDone = false;
        bool is90Degrees = false;
        var rotY = 0f;
        var speed = tileSize * 24f;
        rotation = Geometry.Rotation(0, rotY, 0);

        //During:
        while (!isDone)
        {
            rotY += !is90Degrees ? speed : -speed;

            if (!is90Degrees && rotY >= 90f)
            {
                rotY = 90f;
                is90Degrees = true;
                yield return triggeredEvent;
            }

            isDone = is90Degrees && rotY <= 0f;
            if (isDone)
            {
                rotY = 0f;
            }

            rotation = Geometry.Rotation(0, rotY, 0);
            yield return Wait.OneTick();
        }

        //After:
        rotation = Geometry.Rotation(0, 0, 0);

    }

    public IEnumerator Spin360(IEnumerator triggeredEvent = null)
    {
        //Before:
        bool isDone = false;
        bool hasTriggered = false;
        var rotY = 0f;
        var speed = tileSize * 24f;
        rotation = Geometry.Rotation(0, rotY, 0);

        //During:
        while (!isDone)
        {
            rotY += speed;
            rotation = Geometry.Rotation(0, rotY, 0);

            //Trigger event and startDelay for it to finish (if applicable)
            if (!hasTriggered && rotY >= 240f)
            {
                hasTriggered = true;
                yield return triggeredEvent;
            }

            isDone = rotY >= 360f;
            yield return Wait.OneTick();
        }

        //After:
        rotation = Geometry.Rotation(0, 0, 0);



        //IEnumerator _()
        //{
        //    coinManager.Spawn(boardPosition);
        //    yield return true;
        //}
        //var vfx = resourceManager.VisualEffect("Yellow_Hit");
        //vfxManager.SpawnAsync(vfx, boardPosition, _());

    }

    public void Spin360Async(IEnumerator triggeredEvent = null)
    {
        StartCoroutine(Spin360(triggeredEvent));
    }

    public IEnumerator FadeIn(
         SpriteRenderer spriteRenderer,
         float increment,
         float interval,
         float startAlpha = 0f,
         float endAlpha = 1f)
    {
        //Before:
        var alpha = startAlpha;
        spriteRenderer.color = new Color(1, 1, 1, alpha);

        //During:
        while (alpha < endAlpha)
        {
            alpha += increment;
            alpha = Mathf.Clamp(alpha, 0, endAlpha);
            spriteRenderer.color = new Color(1, 1, 1, alpha);
            yield return interval;
        }

        //After:
        spriteRenderer.color = new Color(1, 1, 1, endAlpha);
    }

    public void FadeInAsync(
       SpriteRenderer spriteRenderer,
       float increment,
       float interval,
       float startAlpha = 0f,
       float endAlpha = 1f)
    {
        StartCoroutine(FadeIn(spriteRenderer, increment, interval, startAlpha, endAlpha));
    }

    public void ParallaxFadeInAsync()
    {
        FadeInAsync(renderers.parallax, Increment.TwoPercent, Interval.OneTick);
    }

    public static IEnumerator FadeOut(SpriteRenderer spriteRenderer, float increment, float interval, float startAlpha, float endAlpha)
    {
        //Before:
        var alpha = startAlpha;
        spriteRenderer.color = new Color(1, 1, 1, alpha);

        //During:
        while (alpha > 0)
        {
            alpha -= increment;
            alpha = Mathf.Clamp(alpha, 0, endAlpha);
            spriteRenderer.color = new Color(1, 1, 1, alpha);
            yield return interval;
        }

        //After:
        spriteRenderer.color = new Color(1, 1, 1, endAlpha);
    }

    public void FadeOutAsync(SpriteRenderer spriteRenderer, float increment, float interval, float startAlpha, float endAlpha)
    {
        StartCoroutine(FadeOut(spriteRenderer, increment, interval, startAlpha, endAlpha));
    }

    public void ParallaxFadeOutAsync()
    {
        if (!IsPlaying)
            return;
        FadeOutAsync(renderers.parallax, Increment.FivePercent, Interval.OneTick, startAlpha: 0.5f, endAlpha: 0f);
    }


    public void Relocate(Vector2Int location)
    {
        this.location = location;
        transform.position = Geometry.GetPositionByLocation(this.location);
    }


    public void CheckReady()
    {
        if (turnDelay == 0)
            return;

        IEnumerator _()
        {
            //Before:
            bool isDone = false;
            bool hasFlipped = false;
            var rotY = 0f;
            var speed = tileSize * 24f;
            renderers.turnDelayText.gameObject.transform.rotation = Geometry.Rotation(0, rotY, 0);

            //During:
            while (!isDone)
            {
                rotY += !hasFlipped ? speed : -speed;

                if (!hasFlipped && rotY >= 90f)
                {
                    rotY = 90f;
                    hasFlipped = true;
                    turnDelay--;
                    turnDelay = Math.Clamp(turnDelay, 0, 9);

                    CheckWeaponWiggle();
                    UpdateTurnDelayText();
                }

                isDone = hasFlipped && rotY <= 0f;
                if (isDone)
                {
                    rotY = 0f;
                }

                renderers.turnDelayText.gameObject.transform.rotation = Geometry.Rotation(0, rotY, 0);
                yield return Wait.OneTick();
            }

            //After:
            renderers.turnDelayText.gameObject.transform.rotation = Geometry.Rotation(0, 0, 0);
            if (turnDelay > 0)
            {
                TurnDelayWiggleAsync();
            }

            if (turnDelay == 0)
            {
                IEnumerator _()
                {
                    renderers.SetThumbnailSprite(sprites.attack);

                    //renderers.SetThumbnailMaterial(resourceManager.Material("Invert-color", idle.texture));
                    //ParallaxFadeInAsync();
                    yield return null;
                }
                Spin90Async(_());
            }
        }

        StartCoroutine(_());
    }

    public void CheckWeaponWiggle()
    {
        if (turnDelay > 0)
            return;

        IEnumerator _()
        {
            //Before:
            float start = -45f;
            float rotZ = start;
            renderers.weaponIcon.transform.rotation = Quaternion.Euler(0, 0, rotZ);

            //During:
            while (turnDelay == 0)
            {
                // Calculate rotation angle using sine wave
                rotZ = start + Mathf.Sin(Time.time * wiggleSpeed) * wiggleAmplitude;

                // Apply the rotation
                renderers.weaponIcon.transform.rotation = Quaternion.Euler(0, 0, rotZ);

                yield return Wait.OneTick();
            }

            //After:
            rotZ = start;
            renderers.weaponIcon.transform.rotation = Quaternion.Euler(0, 0, rotZ);
        }

        StartCoroutine(_());
    }

    public void TurnDelayWiggleAsync(bool isLooping = false)
    {
        StartCoroutine(TurnDelayWiggle(isLooping));
    }

    public IEnumerator TurnDelayWiggle(bool isLooping = false)
    {
        float timeElapsed = 0f;
        float amplitude = 10f;
        float speed = wiggleSpeed; // Wiggle speed
        float dampingRate = 0.99f; // Factor to reduce amplitude each cycle (closer to 1 = slower decay)
        float cutoff = 0.1f;

        renderers.turnDelayText.transform.rotation = Quaternion.Euler(0, 0, 0);

        while (amplitude > cutoff)
        {
            timeElapsed += Time.deltaTime;
            float rotZ = Mathf.Sin(timeElapsed * speed) * amplitude;
            renderers.turnDelayText.transform.rotation = Quaternion.Euler(0, 0, rotZ);
            amplitude *= dampingRate;

            yield return Wait.OneTick();
        }

        // Smoothly return to zero rotation after finishing
        float currentZ = renderers.turnDelayText.transform.rotation.eulerAngles.z;
        while (Mathf.Abs(Mathf.DeltaAngle(currentZ, 0f)) > cutoff)
        {
            timeElapsed += Time.deltaTime * speed;
            currentZ = Mathf.LerpAngle(currentZ, 0f, timeElapsed);
            renderers.turnDelayText.transform.rotation = Quaternion.Euler(0, 0, currentZ);
            yield return Wait.OneTick();
        }

        // Ensure rotation is exactly zero at the end
        renderers.turnDelayText.transform.rotation = Quaternion.Euler(0, 0, 0);

    }

}
