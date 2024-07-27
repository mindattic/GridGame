using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class ActorBehavior : ExtendedMonoBehavior
{

    //Variables
    public Archetype archetype;
    public Vector2Int location = Locations.Nowhere;
    public Vector3? destination = null;
    public Team team = Team.Independant;
    public Quality quality = Colors.Common;
    public float level;
    public float hp;
    public float maxHp;
    public float attack;
    public float defense;
    public float accuracy;
    public float evasion;
    public float speed;
    public float luck;
    public float ap = 0;
    public float maxAp = 100;
    public float sp = 0;
    public float spMax = -1;
    public int spawnTurn = -1;

    [SerializeField] public AnimationCurve glowCurve;

    public ActorHealthBar HealthBar;
    public ActorThumbnail Thumbnail;
    public ActorRenderers SpriteRenderer = new ActorRenderers();

    public bool IsAttacking => combatParticipants.attackingPairs.Any(x => x.actor1 == this || x.actor2 == this);

    private void Awake()
    {

        SpriteRenderer.quality = gameObject.transform.GetChild(ActorLayer.Quality).GetComponent<SpriteRenderer>();
        SpriteRenderer.glow = gameObject.transform.GetChild(ActorLayer.Glow).GetComponent<SpriteRenderer>();
        SpriteRenderer.parallax = gameObject.transform.GetChild(ActorLayer.Parallax).GetComponent<SpriteRenderer>();
        SpriteRenderer.thumbnail = gameObject.transform.GetChild(ActorLayer.Thumbnail).GetComponent<SpriteRenderer>();
        SpriteRenderer.frame = gameObject.transform.GetChild(ActorLayer.Frame).GetComponent<SpriteRenderer>();
        SpriteRenderer.statusIcon = gameObject.transform.GetChild(ActorLayer.StatusIcon).GetComponent<SpriteRenderer>();
        SpriteRenderer.healthBarBack = gameObject.transform.GetChild(ActorLayer.HealthBarBack).GetComponent<SpriteRenderer>();
        SpriteRenderer.healthBar = gameObject.transform.GetChild(ActorLayer.HealthBar).GetComponent<SpriteRenderer>();
        SpriteRenderer.healthBarFront = gameObject.transform.GetChild(ActorLayer.HealthBarFront).GetComponent<SpriteRenderer>();
        SpriteRenderer.healthText = gameObject.transform.GetChild(ActorLayer.HealthText).GetComponent<TextMeshPro>();
        SpriteRenderer.actionBarBack = gameObject.transform.GetChild(ActorLayer.ActionBarBack).GetComponent<SpriteRenderer>();
        SpriteRenderer.actionBar = gameObject.transform.GetChild(ActorLayer.ActionBar).GetComponent<SpriteRenderer>();
        SpriteRenderer.actionText = gameObject.transform.GetChild(ActorLayer.ActionText).GetComponent<TextMeshPro>();
        SpriteRenderer.skillRadialBack = gameObject.transform.GetChild(ActorLayer.RadialBack).GetComponent<SpriteRenderer>();
        SpriteRenderer.skillRadial = gameObject.transform.GetChild(ActorLayer.RadialFill).GetComponent<SpriteRenderer>();
        SpriteRenderer.skillRadialText = gameObject.transform.GetChild(ActorLayer.RadialText).GetComponent<TextMeshPro>();
        SpriteRenderer.selection = gameObject.transform.GetChild(ActorLayer.Selection).GetComponent<SpriteRenderer>();
        SpriteRenderer.mask = gameObject.transform.GetChild(ActorLayer.Mask).GetComponent<SpriteMask>();

        HealthBar = new ActorHealthBar(GetGameObjectByLayer(ActorLayer.HealthBarBack), GetGameObjectByLayer(ActorLayer.HealthBar));
        Thumbnail = new ActorThumbnail(GetGameObjectByLayer(ActorLayer.Thumbnail));
    }

    private void Start()
    {

    }


    void Update()
    {
        //Check abort stateub
        if (!IsPlaying)
            return;

        var closestTile = Geometry.ClosestTile(position);
        if (location == closestTile.location)
            return;

        audioManager.Play($"Move{Random.Int(1, 6)}");

        //Determine if two actors are overlapping the same location
        var overlappingActor = actors.FirstOrDefault(x => x != null
                                            && !x.Equals(this)
                                            && x.IsPlaying
                                            && !x.HasDestination
                                            && !x.Equals(focusedPlayer)
                                            && !x.Equals(selectedPlayer)
                                            && x.location.Equals(closestTile.location));

        //Assign overlapping actors location to current actor's location
        if (overlappingActor != null)
            overlappingActor.SwapLocation(location);

        //Assign current actor's location to closest tile location
        location = closestTile.location;
    }

    void FixedUpdate()
    {
        //Check abort state
        if (!IsPlaying || IsFocusedPlayer || IsSelectedPlayer)
            return;

        //CheckMovement();
        CheckBobbing();
        CheckThrobbing();
        //CheckFlicker();

        //CheckActionBar();
        CheckSkillRadial();



        //if (isRising && SpriteRenderer.thumbnail.transform.rotation.z < maxRot)
        //{
        //    SpriteRenderer.thumbnail.transform.Rotate(new Vector3(0, 0, rotSpeed));
        //}
        //else
        //{
        //    rotSpeed = Random.Int(2, 5) * 0.01f;
        //    minRot = -1f + (-1f * Random.Percent);
        //    isRising = false;
        //}

        //if (!isRising && SpriteRenderer.thumbnail.transform.rotation.z > minRot)
        //{
        //    SpriteRenderer.thumbnail.transform.Rotate(new Vector3(0, 0, -rotSpeed));

        //}
        //else
        //{
        //    rotSpeed = Random.Int(2, 5) * 0.01f;
        //    maxRot = 1f + (1f * Random.Percent);
        //    isRising = true;
        //}


    }

    #region Properties

    public string Name
    {
        get => name;
        set => Name = value;
    }

    public Transform Parent
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
        get => SpriteRenderer.thumbnail.sprite;
        set => SpriteRenderer.thumbnail.sprite = value;
    }



    public int sortingOrder
    {
        get
        {
            return SpriteRenderer.quality.sortingOrder;
        }
        set
        {
            SpriteRenderer.quality.sortingOrder = value + ActorLayer.Quality;
            SpriteRenderer.parallax.sortingOrder = value + ActorLayer.Parallax;
            SpriteRenderer.glow.sortingOrder = value;
            SpriteRenderer.thumbnail.sortingOrder = value + ActorLayer.Thumbnail;
            SpriteRenderer.frame.sortingOrder = value + ActorLayer.Frame;
            SpriteRenderer.statusIcon.sortingOrder = value + ActorLayer.StatusIcon;
            SpriteRenderer.healthBarBack.sortingOrder = value + ActorLayer.HealthBarBack;
            SpriteRenderer.healthBar.sortingOrder = value + ActorLayer.HealthBar;
            SpriteRenderer.healthBarFront.sortingOrder = value + ActorLayer.HealthBarFront;
            SpriteRenderer.healthText.sortingOrder = value + ActorLayer.HealthText;
            SpriteRenderer.actionBarBack.sortingOrder = value + ActorLayer.ActionBarBack;
            SpriteRenderer.actionBar.sortingOrder = value + ActorLayer.ActionBar;
            SpriteRenderer.actionText.sortingOrder = value + ActorLayer.ActionText;
            SpriteRenderer.skillRadialBack.sortingOrder = value + ActorLayer.RadialBack;
            SpriteRenderer.skillRadial.sortingOrder = value + ActorLayer.RadialFill;
            SpriteRenderer.skillRadialText.sortingOrder = value + ActorLayer.RadialText;
            SpriteRenderer.selection.sortingOrder = value + ActorLayer.Selection;
            SpriteRenderer.mask.sortingOrder = value + ActorLayer.Mask;
        }
    }

    public TileBehavior currentTile => tiles.First(x => x.location.Equals(location));
    public bool IsPlayer => team.Equals(Team.Player);
    public bool IsEnemy => team.Equals(Team.Enemy);
    public bool IsFocusedPlayer => HasFocusedPlayer && Equals(focusedPlayer);
    public bool IsSelectedPlayer => HasSelectedPlayer && Equals(selectedPlayer);
    public bool HasLocation => location != Locations.Nowhere;
    public bool HasDestination => destination.HasValue;
    public bool IsNorthEdge => location.y == 1;
    public bool IsEastEdge => location.x == board.columnCount;
    public bool IsSouthEdge => location.y == board.rowCount;
    public bool IsWestEdge => location.x == 1;
    public bool IsAlive => IsActive && hp > 0;
    public bool IsDying => IsActive && hp < 1;
    public bool IsDead => !IsActive && hp < 1;

    public bool IsActive => this != null && isActiveAndEnabled;
    public bool IsInactive => this == null || !isActiveAndEnabled;
    public bool IsSpawnable => !IsActive && IsAlive && spawnTurn <= turnManager.currentTurn;
    public bool IsPlaying => IsActive && IsAlive;
    public bool IsReady => ap == maxAp;
    public float LevelModifier => 1.0f + Random.Float(0, level * 0.01f);
    public float AttackModifier => 1.0f + Random.Float(0, attack * 0.01f);
    public float DefenseModifier => 1.0f + Random.Float(0, defense * 0.01f);
    public float AccuracyModifier => 1.0f + Random.Float(0, accuracy * 0.01f);
    public float EvasionModifier => 1.0f + Random.Float(0, evasion * 0.01f);
    public float LuckModifier => 1.0f + Random.Float(0, luck * 0.01f);

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

    public Direction GetAdjacentDirectionTo(ActorBehavior other)
    {
        if (!IsAdjacentTo(other.location)) return Direction.None;
        if (IsNorthOf(other.location)) return Direction.South;
        if (IsEastOf(other.location)) return Direction.West;
        if (IsSouthOf(other.location)) return Direction.North;
        if (IsWestOf(other.location)) return Direction.East;

        return Direction.None;
    }

    private Vector2Int SetLocation(Direction direction)
    {
        switch (direction)
        {
            case Direction.North: location += new Vector2Int(0, -1); break;
            case Direction.East: location += new Vector2Int(1, 0); break;
            case Direction.South: location += new Vector2Int(0, 1); break;
            case Direction.West: location += new Vector2Int(-1, 0); break;
        }

        return location;
    }

    public void SetSortingOrder(int sortingOrder) => this.sortingOrder = sortingOrder;


    #endregion

    public void Spawn(Vector2Int startLocation)
    {
        gameObject.SetActive(true);

        location = startLocation;
        position = Geometry.GetPosition(location);

        if (IsPlayer)
        {
            SpriteRenderer.SetQualityColor(quality.Color);
            SpriteRenderer.SetGlowColor(quality.Color);
            SpriteRenderer.SetParallaxSprite(resourceManager.Seamless("WhiteFire"));
            SpriteRenderer.SetParallaxMaterial(resourceManager.ActorMaterial("PlayerParallax"));
            SpriteRenderer.SetFrameColor(Colors.Solid.White);
            SpriteRenderer.SetHealthBarColor(Colors.HealthBar.Green);
            SpriteRenderer.SetActionBarEnabled(isEnabled: false);
            SpriteRenderer.SetSelectionActive(false);
            AssignSkillWait();
        }
        else if (IsEnemy)
        {
            SpriteRenderer.SetQualityColor(Colors.Solid.Red);
            SpriteRenderer.SetGlowColor(Colors.Solid.Red);
            SpriteRenderer.SetParallaxSprite(resourceManager.Seamless("BlackFire"));
            SpriteRenderer.SetParallaxMaterial(resourceManager.ActorMaterial("EnemyParallax"));
            SpriteRenderer.SetFrameColor(Colors.Solid.Red);
            SpriteRenderer.SetHealthBarColor(Colors.HealthBar.Green);
            SpriteRenderer.SetActionBarEnabled(isEnabled: true);
            SpriteRenderer.SetSelectionActive(false);
            //AssignActionWait();
            AssignSkillWait();
        }


        UpdateHealthBar();
        UpdateActionBar();

        IEnumerator _()
        {
            //Before...
            float alpha = 0;
            SpriteRenderer.SetAlpha(alpha);
            float delay = Random.Float(0f, 2f);
            yield return Wait.For(delay);

            //During...
            while (alpha < 1)
            {
                alpha += Increment.OnePercent;
                alpha = Mathf.Clamp(alpha, 0, 1);
                SpriteRenderer.SetAlpha(alpha);
                yield return Wait.OneTick();
            }

            //After...
            alpha = 1;
            SpriteRenderer.SetAlpha(alpha);
        }

        StartCoroutine(_());
    }

    public void SwapLocation(Vector2Int other)
    {
        //Check abort state
        if (HasDestination)
            return;

        //Assign location based on relative direction
        if (IsNorthOf(other) || IsNorthWestOf(other) || IsNorthEastOf(other))
            SetLocation(Direction.South);
        else if (IsEastOf(other))
            SetLocation(Direction.West);
        else if (IsSouthOf(other) || IsSouthWestOf(other) || IsSouthEastOf(other))
            SetLocation(Direction.North);
        else if (IsWestOf(other))
            SetLocation(Direction.East);

        //Assign targetPosition based on new location
        destination = Geometry.GetPosition(location);

        //Move actor toward targetPosition
        StartCoroutine(MoveTowardDestination());
    }


    public void SetAttackStrategy()
    {
        //Randomly select an attack attackStrategy
        int[] ratios = { 50, 20, 15, 10, 5 };
        var attackStrategy = Random.Strategy(ratios);

        ActorBehavior targetPlayer = null;

        switch (attackStrategy)
        {
            case AttackStrategy.AttackClosest:
                targetPlayer = players.Where(x => x.IsPlaying).OrderBy(x => Vector3.Distance(x.position, position)).FirstOrDefault();
                destination = Geometry.ClosestAttackPosition(this, targetPlayer);
                break;

            case AttackStrategy.AttackWeakest:
                targetPlayer = players.Where(x => x.IsPlaying).OrderBy(x => x.hp).FirstOrDefault();
                destination = Geometry.ClosestAttackPosition(this, targetPlayer);
                break;

            case AttackStrategy.AttackStrongest:
                targetPlayer = players.Where(x => x.IsPlaying).OrderBy(x => x.hp).FirstOrDefault();
                destination = Geometry.ClosestAttackPosition(this, targetPlayer);
                break;

            case AttackStrategy.AttackRandom:
                targetPlayer = Random.Player;
                destination = Geometry.ClosestAttackPosition(this, targetPlayer);
                break;

            case AttackStrategy.MoveAnywhere:
                var location = Random.Location;
                targetPlayer = null;
                destination = Geometry.GetPosition(location);
                break;
        }


    }


    public IEnumerator MoveTowardCursor()
    {
        //Before...

        //During...
        while (IsFocusedPlayer || IsSelectedPlayer)
        {
            var cursorPosition = mousePosition3D + mouseOffset;
            cursorPosition.x = Mathf.Clamp(cursorPosition.x, board.bounds.Left, board.bounds.Right);
            cursorPosition.y = Mathf.Clamp(cursorPosition.y, board.bounds.Bottom, board.bounds.Top);

            //Move selected player towards cursor
            //position = Vector2.MoveTowards(position, cursorPosition, cursorSpeed);

            //Snap selected player to cursor
            position = cursorPosition;

            yield return Wait.None();
        }

        //After...
    }


    public IEnumerator MoveTowardDestination()
    {
        //Before...
        audioManager.Play($"Slide");

        //During...

        while (HasDestination)
        {
            var delta = destination.Value - position;
            if (Mathf.Abs(delta.x) >= snapDistance)
            {
                position = Vector2.MoveTowards(position, new Vector3(destination.Value.x, position.y, position.z), moveSpeed);
            }
            else if (Mathf.Abs(delta.y) >= snapDistance)
            {
                position = Vector2.MoveTowards(position, new Vector3(position.x, destination.Value.y, position.z), moveSpeed);
            }

            //Determine if Actor is close to targetPosition
            bool isSnapDistance = Vector2.Distance(position, destination.Value) <= snapDistance;
            if (isSnapDistance)
            {
                //Snap to targetPosition, clear targetPosition, and set Actor MoveState: "Idle"
                transform.position = destination.Value;
                destination = null;
            }

            yield return Wait.OneTick();
        }

        //After...

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

        //    SpriteRenderer.CycleActionBarColor();
        //}

        //UpdateActionBar();
    }

    public void UpdateActionBar()
    {
        var scale = SpriteRenderer.actionBarBack.transform.localScale;
        var x = Mathf.Clamp(scale.x * (ap / maxAp), 0, scale.x);
        SpriteRenderer.actionBar.transform.localScale = new Vector3(x, scale.y, scale.z);

        //Percent complete
        SpriteRenderer.actionText.text = ap < maxAp ? $@"{Math.Round(ap / maxAp * 100)}" : "";

        //Seconds remaining
        //SpriteRenderer.skillRadialText.Text = ap < maxAp ? $"{Math.Round(maxAp - ap)}" : "";



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
        //SpriteRenderer.skillRadial.material.SetFloat("_Arc2", fill);

        var fill = (360 * (sp / spMax));
        SpriteRenderer.skillRadial.material.SetFloat("_Arc1", fill);
        SpriteRenderer.skillRadialText.text = sp < spMax ? $"{Math.Round(sp / spMax * 100)}%" : "100%";
    }


    private void CheckBobbing()
    {
        //Check abort state
        if (!IsPlaying || !turnManager.IsStartPhase)
            return;


        //Source: https://forum.unity.com/threads/how-to-make-an-object-move-up-and-down-on-a-loop.380159/
        //var pos = new Vector3(
        //    transform.position.x,
        //    transform.position.y + (glowCurve.Evaluate(Time.time % glowCurve.length) * (tileSize / 64)),
        //    transform.position.z);

        //var rot = new Vector3(
        //   transform.Rotation.x,
        //   transform.Rotation.y ,
        //   transform.Rotation.z + (glowCurve.Evaluate(Time.time % glowCurve.length) * (tileSize / 128)));

        //SpriteRenderer.thumbnail.transform.Rotate(Vector3.up * glowCurve.Evaluate(Time.time % glowCurve.length) * (tileSize / 3));

        //SpriteRenderer.glow.transform.position = pos;
        //SpriteRenderer.thumbnail.transform.position = pos;
        //SpriteRenderer.frame.transform.position = pos;
        //SpriteRenderer.thumbnail.transform.position = pos;
        //SpriteRenderer.thumbnail.transform.Rotation = rot;
    }


    private void CheckThrobbing()
    {
        //Check abort state
        if (!IsPlaying || !turnManager.IsStartPhase || (turnManager.IsPlayerTurn && !IsPlayer) || (turnManager.IsEnemyTurn && !IsEnemy))
            return;

        //Source: https://forum.unity.com/threads/how-to-make-an-object-move-up-and-down-on-a-loop.380159/
        var scale = new Vector3(
            1.5f + glowCurve.Evaluate(Time.time % glowCurve.length) * gameSpeed,
            1.5f + glowCurve.Evaluate(Time.time % glowCurve.length) * gameSpeed,
            1.0f);
        SpriteRenderer.SetGlowScale(scale);
    }

    private void Shake(float intensity)
    {
        gameObject.transform.GetChild(ActorLayer.Thumbnail).gameObject.transform.position = currentTile.position;

        if (intensity > 0)
        {
            var amount = new Vector3(Random.Range(-intensity), Random.Range(intensity), 1);
            gameObject.transform.GetChild(ActorLayer.Thumbnail).gameObject.transform.position += amount;
        }

    }


    public IEnumerator Bump(Direction direction)
    {

        //Before...
        BumpStage stage = BumpStage.Start;
        var targetPosition = position;
        var range = tileSize * percent33;

        //During...
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

            yield return Wait.None();
        }

        //After...
        sortingOrder = SortingOrder.Default;
        position = targetPosition;
    }

    
    public IEnumerator ChangeHp(float amount)
    {
        //Before...
        var remainingHP = Mathf.Clamp(hp + amount, 0, maxHp);

        //During...
        while (hp > remainingHP)
        {

            //Decrease hp
            var min = (int)Math.Max(1, amount * 0.1f);
            var max = (int)Math.Max(1, amount * 0.3f);
            var hit = Random.Int(min, max);
            hp -= hit;
            hp = Mathf.Clamp(hp, remainingHP, maxHp);

            sp += hit * 0.1f;

            damageTextManager.Spawn(hit.ToString(), position);

            //Shake Actor
            Shake(shakeIntensity.Medium);

            UpdateHealthBar();

            audioManager.Play($"Slash{Random.Int(1, 7)}");

            yield return Wait.For(Interval.FiveTicks);
        }

        //After...
        Shake(shakeIntensity.Stop);
        hp = remainingHP;
        UpdateHealthBar();
        position = currentTile.position;
    }

    public void ChangeHpAsync(float amount)
    {
        StartCoroutine(ChangeHp(amount));
    }


    public IEnumerator ChangeAp(float amount)
    {
        //Before...
        var targetAP = Mathf.Clamp(ap + amount, 0, maxAp);
        var increment = 1f;

        //During...
        while (ap < targetAP)
        {
            ap += increment;
            ap = Mathf.Clamp(ap, 0, maxAp);

            UpdateActionBar();
            yield return Wait.For(Interval.FiveTicks);
        }

        //After...
        ap = targetAP;
        UpdateActionBar();
    }



    public void ChangeApAsync(float amount)
    {
        StartCoroutine(ChangeAp(amount));
    }

    public IEnumerator ChangeSp(float amount)
    {
        //Before...
        float ticks = 0f;
        float duration = Interval.OneSecond;

        //During...
        while (ticks < duration)
        {
            ticks += Interval.OneTick;
            sp += Interval.OneTick * amount * 0.1f;
            yield return Wait.OneTick();
        }

        //After...
    }

    public void ChangeSpAsync(float amount)
    {
        StartCoroutine(ChangeSp(amount));
    }


    public IEnumerator MissAttack()
    {
        //Before...
        float ticks = 0;
        float duration = Interval.QuarterSecond;
        damageTextManager.Spawn("Miss", position);

        //During...
        while (ticks < duration)
        {
            ticks += Interval.OneTick;
            Shake(shakeIntensity.Low);
            yield return Wait.OneTick();
        }

        //After...
        Shake(shakeIntensity.Stop);
    }

    private void UpdateHealthBar()
    {
        var scale = SpriteRenderer.healthBarBack.transform.localScale;
        var x = Mathf.Clamp(scale.x * (hp / maxHp), 0, scale.x);
        SpriteRenderer.healthBar.transform.localScale = new Vector3(x, scale.y, scale.z);
        SpriteRenderer.healthText.text = $@"{hp}/{maxHp}";
    }

    public IEnumerator RadialBackFadeIn()
    {
        //Before...
        var maxAlpha = 0.5f;
        var alpha = 0f;
        SpriteRenderer.skillRadialBack.color = new Color(0, 0, 0, alpha);

        //During...
        while (alpha < maxAlpha)
        {
            alpha += Increment.OnePercent;
            alpha = Mathf.Clamp(alpha, 0, maxAlpha);
            SpriteRenderer.skillRadialBack.color = new Color(0, 0, 0, alpha);
            yield return global::Wait.OneTick();
        }

        //After...
        SpriteRenderer.skillRadialBack.color = new Color(0, 0, 0, maxAlpha);
    }

    public IEnumerator RadialBackFadeOut()
    {
        //Before...
        var maxAlpha = 0.5f;
        var alpha = maxAlpha;
        SpriteRenderer.skillRadialBack.color = new Color(0, 0, 0, maxAlpha);

        //During...
        while (alpha > 0)
        {
            alpha -= Increment.OnePercent;
            alpha = Mathf.Clamp(alpha, 0, maxAlpha);
            SpriteRenderer.skillRadialBack.color = new Color(0, 0, 0, alpha);
            yield return global::Wait.OneTick();
        }

        //After...
        SpriteRenderer.skillRadialBack.color = new Color(0, 0, 0, 0);
    }

    public IEnumerator Dissolve()
    {
        //Before...
        var alpha = 1f;
        SpriteRenderer.SetAlpha(alpha);
        portraitManager.Dissolve(this);
        audioManager.Play("Death");
        sortingOrder = SortingOrder.Max;

        //During...
        while (alpha > 0)
        {
            alpha -= Increment.OnePercent;
            alpha = Mathf.Clamp(alpha, 0, 1);
            SpriteRenderer.SetAlpha(alpha);
            yield return Wait.OneTick();
        }

        //After...
        gameObject.SetActive(false);
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
        //Before...
        float increment = Increment.FivePercent;
        float alpha = SpriteRenderer.statusIcon.color.a;
        SpriteRenderer.statusIcon.color = new Color(1, 1, 1, alpha);

        //During...
        while (alpha > 0)
        {
            alpha -= increment;
            alpha = Mathf.Clamp(alpha, 0, 1);
            SpriteRenderer.statusIcon.color = new Color(1, 1, 1, alpha);
            yield return Wait.OneTick();
        }

        //Before...
        SpriteRenderer.statusIcon.sprite = resourceManager.statusSprites.First(x => x.id.Equals(status.ToString())).thumbnail;
        alpha = 0;
        SpriteRenderer.statusIcon.color = new Color(1, 1, 1, alpha);

        //During...
        while (alpha < 1)
        {
            alpha += increment;
            alpha = Mathf.Clamp(alpha, 0, 1);
            SpriteRenderer.statusIcon.color = new Color(1, 1, 1, alpha);

            yield return Wait.OneTick();
        }
    }

    //public void AssignActionWait()
    //{
    //    //Check abort state
    //    if (!IsPlaying)
    //        return;

    //    //TODO: Calculate based on stats....
    //    float min = (Interval.OneSecond * 10) - speed * LuckModifier;
    //    float max = (Interval.OneSecond * 20) - speed * LuckModifier;

    //    ap = 0;
    //    maxAp = Random.Float(min, max);

    //    SpriteRenderer.SetActionBarColor(Colors.ActionBar.Blue);
    //}

    public void AssignSkillWait()
    {
        //Check abort state
        if (!IsPlaying)
            return;

        //TODO: Calculate based on stats....
        float min = (Interval.OneSecond * 20) - speed * LuckModifier;
        float max = (Interval.OneSecond * 40) - speed * LuckModifier;

        sp = 0;
        spMax = Random.Float(min, max);
    }


    public void SetApMax()
    {
        //Check abort state
        if (!IsAlive || !IsActive || !IsEnemy)
            return;

        ap = maxAp;
        UpdateActionBar();
    }

    private GameObject GetGameObjectByLayer(int layer)
    {
        return gameObject.transform.GetChild(layer).gameObject;
    }

}
