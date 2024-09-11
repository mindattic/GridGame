using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class ActorBehavior : ExtendedMonoBehavior
{

    //Variables
    public Archetype archetype;
    public Vector2Int location = Location.Nowhere;
    public Vector3 destination;
    public Team team = Team.Independant;
    public Quality quality = Qualities.Common;
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
    [SerializeField] public AnimationCurve slideCurve;

    public ActorHealthBar HealthBar;
    public ActorThumbnail Thumbnail;
    public ActorRenderers renderers = new ActorRenderers();

    public bool IsAttacking => combatParticipants.attackingPairs.Any(x => x.actor1 == this || x.actor2 == this);

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
        renderers.healthBar = gameObject.transform.GetChild(ActorLayer.HealthBar).GetComponent<SpriteRenderer>();
        renderers.healthBarFront = gameObject.transform.GetChild(ActorLayer.HealthBarFront).GetComponent<SpriteRenderer>();
        renderers.healthText = gameObject.transform.GetChild(ActorLayer.HealthText).GetComponent<TextMeshPro>();
        renderers.actionBarBack = gameObject.transform.GetChild(ActorLayer.ActionBarBack).GetComponent<SpriteRenderer>();
        renderers.actionBar = gameObject.transform.GetChild(ActorLayer.ActionBar).GetComponent<SpriteRenderer>();
        renderers.actionText = gameObject.transform.GetChild(ActorLayer.ActionText).GetComponent<TextMeshPro>();
        renderers.skillRadialBack = gameObject.transform.GetChild(ActorLayer.RadialBack).GetComponent<SpriteRenderer>();
        renderers.skillRadial = gameObject.transform.GetChild(ActorLayer.RadialFill).GetComponent<SpriteRenderer>();
        renderers.skillRadialText = gameObject.transform.GetChild(ActorLayer.RadialText).GetComponent<TextMeshPro>();
        renderers.selection = gameObject.transform.GetChild(ActorLayer.Selection).GetComponent<SpriteRenderer>();
        renderers.mask = gameObject.transform.GetChild(ActorLayer.Mask).GetComponent<SpriteMask>();

        HealthBar = new ActorHealthBar(GetGameObjectByLayer(ActorLayer.HealthBarBack), GetGameObjectByLayer(ActorLayer.HealthBar));
        Thumbnail = new ActorThumbnail(GetGameObjectByLayer(ActorLayer.Thumbnail));
    }

    private void Start()
    {

    }


    void Update()
    {
        //Check abort status
        if (!IsPlaying)
            return;

        var closestTile = Geometry.GetClosestTileByPosition(position);
        if (location == closestTile.location)
            return;

        audioManager.Play($"Move{Random.Int(1, 6)}");

        //Determine if two actors are overlapping the same location
        var overlappingActor = actors.FirstOrDefault(x => x != null
                                            && !x.Equals(this)
                                            && x.IsPlaying
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
        //CheckBobbing();
        CheckThrobbing();
        //CheckFlicker();

        //CheckActionBar();
        CheckSkillRadial();



        //if (isRising && renderers.thumbnail.transform.angularRotation.z < maxRot)
        //{
        //    renderers.thumbnail.transform.Rotate(new Vector3(0, 0, rotSpeed));
        //}
        //else
        //{
        //    rotSpeed = Random.Int(2, 5) * 0.01f;
        //    minRot = -1f + (-1f * Random.Percent);
        //    isRising = false;
        //}

        //if (!isRising && renderers.thumbnail.transform.angularRotation.z > minRot)
        //{
        //    renderers.thumbnail.transform.Rotate(new Vector3(0, 0, -rotSpeed));

        //}
        //else
        //{
        //    rotSpeed = Random.Int(2, 5) * 0.01f;
        //    maxRot = 1f + (1f * Random.Percent);
        //    isRising = true;
        //}


    }

    #region Components

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

    public Vector3 thumbnailPosition
    {
        get => gameObject.transform.GetChild(ActorLayer.Thumbnail).gameObject.transform.position;
        set => gameObject.transform.GetChild(ActorLayer.Thumbnail).gameObject.transform.position = value;
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
            renderers.healthBar.sortingOrder = value + ActorLayer.HealthBar;
            renderers.healthBarFront.sortingOrder = value + ActorLayer.HealthBarFront;
            renderers.healthText.sortingOrder = value + ActorLayer.HealthText;
            renderers.actionBarBack.sortingOrder = value + ActorLayer.ActionBarBack;
            renderers.actionBar.sortingOrder = value + ActorLayer.ActionBar;
            renderers.actionText.sortingOrder = value + ActorLayer.ActionText;
            renderers.skillRadialBack.sortingOrder = value + ActorLayer.RadialBack;
            renderers.skillRadial.sortingOrder = value + ActorLayer.RadialFill;
            renderers.skillRadialText.sortingOrder = value + ActorLayer.RadialText;
            renderers.selection.sortingOrder = value + ActorLayer.Selection;
            renderers.mask.sortingOrder = value + ActorLayer.Mask;
        }
    }

    public TileBehavior currentTile => tiles.First(x => x.location.Equals(location));
    public bool IsPlayer => team.Equals(Team.Player);
    public bool IsEnemy => team.Equals(Team.Enemy);
    public bool IsFocusedPlayer => HasFocusedPlayer && Equals(focusedPlayer);
    public bool IsSelectedPlayer => HasSelectedPlayer && Equals(selectedPlayer);
    public bool HasLocation => location != Location.Nowhere;
    public bool HasReachedDestination => position == destination;
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

    public void SetSortingOrder(int sortingOrder) => this.sortingOrder = sortingOrder;


    #endregion

    public void Spawn(Vector2Int startLocation)
    {
        gameObject.SetActive(true);

        location = startLocation;
        position = Geometry.GetPositionByLocation(location);
        destination = position;

        if (IsPlayer)
        {
            renderers.SetQualityColor(quality.Color);
            renderers.SetGlowColor(quality.Color);
            renderers.SetParallaxSprite(resourceManager.Seamless("WhiteFire"));
            renderers.SetParallaxMaterial(resourceManager.ActorMaterial("PlayerParallax"));
            renderers.SetParallaxAlpha(0);
            renderers.SetFrameColor(Colors.Solid.White);
            renderers.SetHealthBarColor(Colors.HealthBar.Green);
            renderers.SetActionBarEnabled(isEnabled: false);
            renderers.SetSelectionActive(false);
        }
        else if (IsEnemy)
        {
            renderers.SetQualityColor(Colors.Solid.Black);
            renderers.SetGlowColor(Colors.Solid.Red);
            renderers.SetParallaxSprite(resourceManager.Seamless("BlackFire"));
            renderers.SetParallaxMaterial(resourceManager.ActorMaterial("EnemyParallax"));
            renderers.SetParallaxAlpha(0);
            renderers.SetFrameColor(Colors.Solid.White);
            renderers.SetHealthBarColor(Colors.HealthBar.Green);
            renderers.SetActionBarEnabled(isEnabled: true);
            renderers.SetSelectionActive(false);
        }

        AssignSkillWait();
        UpdateHealthBar();
        UpdateActionBar();


        if (turnManager.IsFirstTurn)
            renderers.SetAlpha(1);
        else
            StartCoroutine(FadeIn());
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

    public void SwapLocation(Vector2Int other)
    {
        //Assign location based on relative direction
        if (IsNorthOf(other) || IsNorthWestOf(other) || IsNorthEastOf(other))
            SetLocation(Direction.South);
        else if (IsEastOf(other))
            SetLocation(Direction.West);
        else if (IsSouthOf(other) || IsSouthWestOf(other) || IsSouthEastOf(other))
            SetLocation(Direction.North);
        else if (IsWestOf(other))
            SetLocation(Direction.East);

        //Assign destination based on new location
        var closetTile = Geometry.GetClosestTileByLocation(location);
        destination = closetTile.position;

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
                destination = Geometry.GetClosestAttackPosition(this, targetPlayer);
                break;

            case AttackStrategy.AttackWeakest:
                targetPlayer = players.Where(x => x.IsPlaying).OrderBy(x => x.hp).FirstOrDefault();
                destination = Geometry.GetClosestAttackPosition(this, targetPlayer);
                break;

            case AttackStrategy.AttackStrongest:
                targetPlayer = players.Where(x => x.IsPlaying).OrderBy(x => x.hp).FirstOrDefault();
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


    public IEnumerator MoveTowardCursor()
    {
        //Before:

        //During:
        while (IsFocusedPlayer || IsSelectedPlayer)
        {
            var cursorPosition = mousePosition3D + mouseOffset;
            cursorPosition.x = Mathf.Clamp(cursorPosition.x, board.bounds.Left, board.bounds.Right);
            cursorPosition.y = Mathf.Clamp(cursorPosition.y, board.bounds.Bottom, board.bounds.Top);

            //Move selected player towards cursor
            position = Vector2.MoveTowards(position, cursorPosition, cursorSpeed);

            //Snap selected player to cursor
            //position = cursorPosition;

            destination = position;

            yield return Wait.OneTick();
        }

        //After:
    }


    public IEnumerator MoveTowardDestination()
    {
        //Before:
        Vector3 initialPosition = position;
        Vector3 initialScale = tileScale;
        scale = tileScale;
        audioManager.Play($"Slide");

        //During:
        while (!HasReachedDestination)
        {
            var delta = destination - position;
            if (Mathf.Abs(delta.x) >= snapDistance)
            {
                position = Vector2.MoveTowards(position, new Vector3(destination.x, position.y, position.z), moveSpeed);
            }
            else if (Mathf.Abs(delta.y) >= snapDistance)
            {
                position = Vector2.MoveTowards(position, new Vector3(position.x, destination.y, position.z), moveSpeed);
            }

            float percentage = Geometry.GetPercentageBetween(initialPosition, destination, position);
            scale = initialScale * slideCurve.Evaluate(percentage);

            //Determine whether to snap to destination
            bool isSnapDistance = Vector2.Distance(position, destination) <= snapDistance;
            if (isSnapDistance)
            {
                position = destination;
            }

            yield return Wait.OneTick();
        }

        //After:
        scale = tileScale;
        position = destination;
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
        //renderers.skillRadialText.Text = ap < maxAp ? $"{Math.Round(maxAp - ap)}" : "";



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
        //    transform.position.x,
        //    transform.position.y + (glowCurve.Evaluate(Time.time % glowCurve.length) * (tileSize / 64)),
        //    transform.position.z);

        //var rot = new Vector3(
        //   transform.angularRotation.x,
        //   transform.angularRotation.y ,
        //   transform.angularRotation.z + (glowCurve.Evaluate(Time.time % glowCurve.length) * (tileSize / 128)));

        //renderers.thumbnail.transform.Rotate(Vector3.up * glowCurve.Evaluate(Time.time % glowCurve.length) * (tileSize / 3));

        //renderers.glow.transform.position = pos;
        //renderers.thumbnail.transform.position = pos;
        //renderers.frame.transform.position = pos;
        //renderers.thumbnail.transform.position = pos;
        //renderers.thumbnail.transform.angularRotation = rot;
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


    public IEnumerator Bump(Direction direction)
    {

        //Before:
        BumpStage stage = BumpStage.Start;
        var targetPosition = position;
        var range = tileSize * percent33;

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

            yield return Wait.None();
        }

        //After:
        sortingOrder = SortingOrder.Default;
        position = targetPosition;
    }


    public IEnumerator ChangeHp(float amount)
    {
        //Check abort state
        if (!IsAlive)
            yield break;
 
        //Before:
        float ticks = 0f;
        float duration = Interval.QuarterSecond;
        bool isDamage = amount < 0f;
        bool isHeal = amount >= 0f;
        bool isIneffective = amount == 0f;

        hp += amount;
        hp = Mathf.Clamp(hp, 0, maxHp);
        UpdateHealthBar();

        if (isDamage)
        {
            damageTextManager.Spawn(Math.Abs(amount).ToString(), position);
            audioManager.Play($"Slash{Random.Int(1, 7)}");
        }
        else if (isHeal)
        {

        }
        else if (isIneffective)
        {

        }

        //During:
        while (ticks < duration)
        {
            if (isDamage)
            {
                GrowAsync();
                Shake(shakeIntensity.Medium);
            }
            else if (isHeal)
            {

            }
            else if (isIneffective)
            {

            }

            ticks += Interval.OneTick;
            yield return Wait.For(Interval.OneTick);
        }

        //After:
        if (isDamage)
        {
            ShrinkAsync();
            Shake(shakeIntensity.Stop);

            if (IsDying)
                DieAsync();

        }
        else if (isHeal)
        {

        }
        else if (isIneffective)
        {

        }


    }

    //public IEnumerator ChangeHp(float amount)
    //{
    //    //Before:
    //    var remainingHP = Mathf.Clamp(hp + amount, 0, maxHp);

    //    //During:
    //    while (hp > remainingHP)
    //    {

    //        //Decrease hp
    //        var min = (int)Math.Max(1, amount * 0.1f);
    //        var max = (int)Math.Max(1, amount * 0.3f);
    //        var hit = Random.Int(min, max);
    //        hp -= hit;
    //        hp = Mathf.Clamp(hp, remainingHP, maxHp);

    //        sp += hit * 0.1f;

    //        damageTextManager.Spawn(hit.ToString(), position);

    //        //Shake Actor
    //        Shake(shakeIntensity.Medium);

    //        UpdateHealthBar();

    //        audioManager.Play($"Slash{Random.Int(1, 7)}");

    //        yield return Wait.For(Interval.FiveTicks);
    //    }

    //    //After:
    //    Shake(shakeIntensity.Stop);
    //    hp = remainingHP;
    //    UpdateHealthBar();
    //    position = currentTile.position;
    //}

    public void ChangeHpAsync(float amount)
    {
        //Check abort state
        if (!IsAlive)
            return;

        StartCoroutine(ChangeHp(amount));
    }


    public IEnumerator ChangeAp(float amount)
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



    public void ChangeApAsync(float amount)
    {
        StartCoroutine(ChangeAp(amount));
    }

    public IEnumerator ChangeSp(float amount)
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

    public void ChangeSpAsync(float amount)
    {
        StartCoroutine(ChangeSp(amount));
    }


    public IEnumerator MissAttack()
    {
        //Before:
        float ticks = 0;
        float duration = Interval.QuarterSecond;
        damageTextManager.Spawn("Miss", position);

        //During:
        while (ticks < duration)
        {
            ticks += Interval.OneTick;
            Shake(shakeIntensity.Low);
            yield return Wait.OneTick();
        }

        //After:
        Shake(shakeIntensity.Stop);
    }

    private void UpdateHealthBar()
    {
        var scale = renderers.healthBarBack.transform.localScale;
        var x = Mathf.Clamp(scale.x * (hp / maxHp), 0, scale.x);
        renderers.healthBar.transform.localScale = new Vector3(x, scale.y, scale.z);
        renderers.healthText.text = $@"{hp}/{maxHp}";
    }

    //public IEnumerator RadialBackFadeIn()
    //{
    //    //Before:
    //    var maxAlpha = 0.5f;
    //    var alpha = 0f;
    //    renderers.skillRadialBack.color = new Color(0, 0, 0, alpha);

    //    //During:
    //    while (alpha < maxAlpha)
    //    {
    //        alpha += Increment.OnePercent;
    //        alpha = Mathf.Clamp(alpha, 0, maxAlpha);
    //        renderers.skillRadialBack.color = new Color(0, 0, 0, alpha);
    //        yield return global::Wait.OneTick();
    //    }

    //    //After:
    //    renderers.skillRadialBack.color = new Color(0, 0, 0, maxAlpha);
    //}

    //public IEnumerator RadialBackFadeOut()
    //{
    //    //Before:
    //    var maxAlpha = 0.5f;
    //    var alpha = maxAlpha;
    //    renderers.skillRadialBack.color = new Color(0, 0, 0, maxAlpha);

    //    //During:
    //    while (alpha > 0)
    //    {
    //        alpha -= Increment.OnePercent;
    //        alpha = Mathf.Clamp(alpha, 0, maxAlpha);
    //        renderers.skillRadialBack.color = new Color(0, 0, 0, alpha);
    //        yield return Wait.OneTick();
    //    }

    //    //After:
    //    renderers.skillRadialBack.color = new Color(0, 0, 0, 0);
    //}

    public IEnumerator Die()
    {
        //Before:
        var alpha = 1f;
        renderers.SetAlpha(alpha);
        portraitManager.Dissolve(this);
        audioManager.Play("Death");
        sortingOrder = SortingOrder.Max;

        //During:
        while (alpha > 0f)
        {
            alpha -= Increment.OnePercent;
            alpha = Mathf.Clamp(alpha, 0f, 1f);
            renderers.SetAlpha(alpha);
            yield return Interval.FiveTicks;
        }

        //After:       
        location = Location.Nowhere;
        destination = new Vector3(-100, -100, 0);
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
    //    float min = (Interval.OneSecond * 10) - range * LuckModifier;
    //    float max = (Interval.OneSecond * 20) - range * LuckModifier;

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
        float min = (Interval.OneSecond * 20) - speed * LuckModifier;
        float max = (Interval.OneSecond * 40) - speed * LuckModifier;

        sp = 0;
        spMax = Random.Float(min, max);
    }


    public void SetReady()
    {
        //Check abort state
        if (!IsPlaying || !IsEnemy)
            return;

        ap = maxAp;
        UpdateActionBar();
    }

    private GameObject GetGameObjectByLayer(int layer)
    {
        return gameObject.transform.GetChild(layer).gameObject;
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


    public static IEnumerator FadeIn(
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
        FadeInAsync(renderers.parallax, Increment.FivePercent, Interval.OneTick, startAlpha: 0f, endAlpha: 0.5f);
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
        FadeOutAsync(renderers.parallax, Increment.FivePercent, Interval.OneTick, startAlpha: 0.5f, endAlpha: 0f);
    }



}
