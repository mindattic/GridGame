using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class ActorBehavior : ExtendedMonoBehavior
{

    //Variables
    [SerializeField] public Archetype archetype;
    [SerializeField] public Vector2Int location = Locations.Nowhere;
    [SerializeField] public Vector3? destination = null;
    [SerializeField] public Team team = Team.Independant;
    [SerializeField] public Quality quality = Colors.Common;
    [SerializeField] public float level;
    [SerializeField] public float hp;
    [SerializeField] public float maxHP;
    [SerializeField] public float attack;
    [SerializeField] public float defense;
    [SerializeField] public float accuracy;
    [SerializeField] public float evasion;
    [SerializeField] public float speed;
    [SerializeField] public float luck;

    [SerializeField] public float actionWait = 0;
    [SerializeField] public float actionWaitMax = -1;


    [SerializeField] public float skillWait = 0;
    [SerializeField] public float skillWaitMax = -1;


    [SerializeField] public int spawnTurn = -1;

    [SerializeField] public AnimationCurve flickeringCurve;
    [SerializeField] public AnimationCurve bobbingCurve;
    [SerializeField] public Guid guid;


    [SerializeField] public float backScale = 1.4f;

    public float RandomBob = Random.Float(0.5f, 1f);
    public ActorBehavior targetPlayer = null;

    public ActorHealthBar HealthBar;
    public ActorThumbnail Thumbnail;
    public ActorRenderers Renderers = new ActorRenderers();


    public bool IsAttacking => attackParticipants.attackingPairs.Any(x => x.actor1 == this || x.actor2 == this);

    private void Awake()
    {

        Renderers.back = gameObject.transform.GetChild(ActorLayer.Back).GetComponent<SpriteRenderer>();
        Renderers.parallax = gameObject.transform.GetChild(ActorLayer.Parallax).GetComponent<SpriteRenderer>();
        Renderers.glow = gameObject.transform.GetChild(ActorLayer.Glow).GetComponent<SpriteRenderer>();
        Renderers.thumbnail = gameObject.transform.GetChild(ActorLayer.Thumbnail).GetComponent<SpriteRenderer>();
        Renderers.frame = gameObject.transform.GetChild(ActorLayer.Frame).GetComponent<SpriteRenderer>();
        Renderers.statusIcon = gameObject.transform.GetChild(ActorLayer.StatusIcon).GetComponent<SpriteRenderer>();
        Renderers.healthBarBack = gameObject.transform.GetChild(ActorLayer.HealthBarBack).GetComponent<SpriteRenderer>();
        Renderers.healthBar = gameObject.transform.GetChild(ActorLayer.HealthBar).GetComponent<SpriteRenderer>();
        Renderers.healthBarFront = gameObject.transform.GetChild(ActorLayer.HealthBarFront).GetComponent<SpriteRenderer>();
        Renderers.healthText = gameObject.transform.GetChild(ActorLayer.HealthText).GetComponent<TextMeshPro>();
        Renderers.actionBarBack = gameObject.transform.GetChild(ActorLayer.ActionBarBack).GetComponent<SpriteRenderer>();
        Renderers.actionBar = gameObject.transform.GetChild(ActorLayer.ActionBar).GetComponent<SpriteRenderer>();
        Renderers.actionText = gameObject.transform.GetChild(ActorLayer.ActionText).GetComponent<TextMeshPro>();
        Renderers.skillRadialBack = gameObject.transform.GetChild(ActorLayer.RadialBack).GetComponent<SpriteRenderer>();
        Renderers.skillRadial = gameObject.transform.GetChild(ActorLayer.RadialFill).GetComponent<SpriteRenderer>();
        Renderers.skillRadialText = gameObject.transform.GetChild(ActorLayer.RadialText).GetComponent<TextMeshPro>();
        Renderers.selection = gameObject.transform.GetChild(ActorLayer.Selection).GetComponent<SpriteRenderer>();
        Renderers.mask = gameObject.transform.GetChild(ActorLayer.Mask).GetComponent<SpriteMask>();

        HealthBar = new ActorHealthBar(GameObjectByLayer(ActorLayer.HealthBarBack), GameObjectByLayer(ActorLayer.HealthBar));
        Thumbnail = new ActorThumbnail(GameObjectByLayer(ActorLayer.Thumbnail));
    }

    private void Start()
    {

    }


    void Update()
    {
        //Check abort state
        if (!IsPlaying)
            return;

        if (IsFocusedPlayer || IsSelectedPlayer)
            MoveTowardCursor();

        var closestTile = Geometry.ClosestTile(position);
        if (closestTile.location.Equals(location))
            return;

        soundSource.PlayOneShot(resourceManager.SoundEffect($"Move{Random.Int(1, 6)}"));

        //Determine if two actors are occupying same location
        var actor = actors.FirstOrDefault(x => x.IsPlaying && !x.Equals(this) && !x.Equals(selectedPlayer) && x.location.Equals(closestTile.location));
        if (actor != null)
        {
            actor.SwapLocation(this);
        }

        location = closestTile.location;

    }


    //private float minRot = -1f;
    //private float maxRot = 1f;
    //private float rotSpeed = 0.05f;
    //private bool isRising = true;

    
    void FixedUpdate()
    {
        //Check abort state
        if (!IsPlaying || IsFocusedPlayer || IsSelectedPlayer)
            return;

        CheckMovement();
        CheckBobbing();
        CheckThrobbing();
        //CheckFlicker();

        CheckActionBar();
        CheckSkillRadial();



        //if (isRising && Renderers.thumbnail.transform.rotation.z < maxRot)
        //{
        //    Renderers.thumbnail.transform.Rotate(new Vector3(0, 0, rotSpeed));
        //}
        //else
        //{
        //    rotSpeed = Random.Int(2, 5) * 0.01f;
        //    minRot = -1f + (-1f * Random.Percent);
        //    isRising = false;
        //}

        //if (!isRising && Renderers.thumbnail.transform.rotation.z > minRot)
        //{
        //    Renderers.thumbnail.transform.Rotate(new Vector3(0, 0, -rotSpeed));

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
        get => Renderers.thumbnail.sprite;
        set => Renderers.thumbnail.sprite = value;
    }

    public int sortingOrder
    {
        get
        {
            return Renderers.back.sortingOrder;
        }
        set
        {
            Renderers.back.sortingOrder = value + ActorLayer.Back;
            Renderers.parallax.sortingOrder = value + ActorLayer.Parallax;
            Renderers.glow.sortingOrder = value;
            Renderers.thumbnail.sortingOrder = value + ActorLayer.Thumbnail;
            Renderers.frame.sortingOrder = value + ActorLayer.Frame;
            Renderers.statusIcon.sortingOrder = value + ActorLayer.StatusIcon;
            Renderers.healthBarBack.sortingOrder = value + ActorLayer.HealthBarBack;
            Renderers.healthBar.sortingOrder = value + ActorLayer.HealthBar;
            Renderers.healthBarFront.sortingOrder = value + ActorLayer.HealthBarFront;
            Renderers.healthText.sortingOrder = value + ActorLayer.HealthText;
            Renderers.actionBarBack.sortingOrder = value + ActorLayer.ActionBarBack;
            Renderers.actionBar.sortingOrder = value + ActorLayer.ActionBar;
            Renderers.actionText.sortingOrder = value + ActorLayer.ActionText;
            Renderers.skillRadialBack.sortingOrder = value + ActorLayer.RadialBack;
            Renderers.skillRadial.sortingOrder = value + ActorLayer.RadialFill;
            Renderers.skillRadialText.sortingOrder = value + ActorLayer.RadialText;
            Renderers.selection.sortingOrder = value + ActorLayer.Selection;
            Renderers.mask.sortingOrder = value + ActorLayer.Mask;
        }
    }

    public TileBehavior CurrentTile => tiles.First(x => x.location.Equals(location));
    public bool IsPlayer => team.Equals(Team.Player);
    public bool IsEnemy => team.Equals(Team.Enemy);
    public bool IsFocusedPlayer => HasFocusedPlayer && Equals(focusedPlayer);
    public bool IsSelectedPlayer => HasSelectedPlayer && Equals(selectedPlayer);
    public bool HasLocation => location != Locations.Nowhere;
    public bool IsMoving => destination.HasValue;
    public bool IsNorthEdge => location.y == 1;
    public bool IsEastEdge => location.x == board.columnCount;
    public bool IsSouthEdge => location.y == board.rowCount;
    public bool IsWestEdge => location.x == 1;
    public bool IsAlive => hp > 0;
    public bool IsDead => hp < 1;
    public bool IsActive => this != null && isActiveAndEnabled;
    public bool IsInactive => this == null || !isActiveAndEnabled;
    public bool IsSpawnable => !IsActive && IsAlive && spawnTurn <= turnManager.currentTurn;
    public bool IsPlaying => IsAlive && IsActive;
    public bool IsReady => actionWait == actionWaitMax;
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


    #endregion


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

        if (IsPlayer)
        {
            Renderers.SetBackColor(quality.Color);
            Renderers.SetParallaxSprite(resourceManager.Seamless("WhiteFire"));
            Renderers.SetParallaxMaterial(resourceManager.ActorMaterial("PlayerParallax"));
            Renderers.SetGlowColor(quality.Color);
            Renderers.SetFrameColor(Colors.Solid.White);
            Renderers.SetAlpha(0);
            Renderers.actionBarBack.enabled = false;
            Renderers.actionBar.enabled = false;
        }
        else if (IsEnemy)
        {
            Renderers.back.enabled = false;
            //Renderers.SetBackColor(Colors.Solid.White);
            Renderers.SetParallaxSprite(resourceManager.Seamless("BlackFire"));
            Renderers.SetParallaxMaterial(resourceManager.ActorMaterial("EnemyParallax"));
            Renderers.SetGlowColor(Colors.Solid.White);
            Renderers.SetGlowAlpha(0);
            Renderers.SetFrameColor(Colors.Solid.Red);
            Renderers.SetAlpha(0);
            CalculateActionWait();
        }

        CalculateSkillWait();
        Renderers.back.transform.localScale = new Vector3(backScale, backScale, 1);

        UpdateHealthBar();


        float delay = turnManager.currentTurn == 1 ? 0 : Random.Float(0f, 2f);




        IEnumerator SpawnIn(float delay = 0)
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









        StartCoroutine(SpawnIn(delay));

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

        var closestTile = Geometry.ClosestTile(location);
        destination = closestTile.position;



        soundSource.PlayOneShot(resourceManager.SoundEffect("Slide"));
    }


    public void SetAttackStrategy()
    {
        //Randomly select an attack attackStrategy
        int[] ratios = { 50, 20, 15, 10, 5 };
        var attackStrategy = Random.Strategy(ratios);

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
                destination = Geometry.ClosestTile(location).position;
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
        //position = Vector2.MoveTowards(position, cursorPosition, CursorSpeed);

        //Snap selected player to cursor
        position = cursorPosition;
    }

    private void CheckMovement()
    {
        //Check abort state
        if (!IsMoving)
            return;

        var delta = destination.Value - position;
        if (Mathf.Abs(delta.x) >= SnapDistance)
        {
            position = Vector2.MoveTowards(position, new Vector3(destination.Value.x, position.y, position.z), SlideSpeed);
        }
        else if (Mathf.Abs(delta.y) >= SnapDistance)
        {
            position = Vector2.MoveTowards(position, new Vector3(position.x, destination.Value.y, position.z), SlideSpeed);
        }

        //Determine if Actor is close to destination
        bool isSnapDistance = Vector2.Distance(position, destination.Value) <= SnapDistance;
        if (isSnapDistance)
        {
            //Snap to destination, clear destination, and set Actor MoveState: "Idle"
            transform.position = destination.Value;
            destination = null;
        }
    }


    public void CheckActionBar()
    {
        //Check abort state
        if (turnManager.IsEnemyTurn || (!turnManager.IsStartPhase && !turnManager.IsMovePhase) || !IsPlaying || actionWait == actionWaitMax)
            return;

        if (actionWait < actionWaitMax)
        {
            actionWait += Time.deltaTime;
            actionWait = Math.Clamp(actionWait, 0, actionWaitMax);
        }

        UpdateActionBar();
    }

    public void UpdateActionBar()
    {
        var scale = Renderers.actionBarBack.transform.localScale;
        var x = Mathf.Clamp(scale.x * (actionWait / actionWaitMax), 0, scale.x);
        Renderers.actionBar.transform.localScale = new Vector3(x, scale.y, scale.z);

        //Percent complete
        Renderers.actionText.text = actionWait < actionWaitMax ? $@"{Math.Round(actionWait / actionWaitMax * 100)}" : "";

        //Seconds remaining
        //Renderers.skillRadialText.Text = actionWait < actionWaitMax ? $"{Math.Round(actionWaitMax - actionWait)}" : "";

    }

    public void CheckSkillRadial()
    {
        //if (skillWait < skillWaitMax)
        //{
        //    skillWait += Time.deltaTime;
        //    skillWait = Math.Clamp(actionWait, 0, skillWaitMax);
        //}

        UpdateSkillRadial();
    }

    public void UpdateSkillRadial()
    {
        var fill = 360 - (360 * (skillWait / skillWaitMax));
        Renderers.skillRadial.material.SetFloat("_Arc2", fill);
        Renderers.skillRadialText.text = skillWait < skillWaitMax ? $"{Math.Round(skillWait / skillWaitMax * 100)}%" : "100%";
    }


    private void CheckBobbing()
    {
        //Check abort state
        if (!IsPlaying || !turnManager.IsStartPhase)
            return;


        //Source: https://forum.unity.com/threads/how-to-make-an-object-move-up-and-down-on-a-loop.380159/
        //var pos = new Vector3(
        //    transform.position.x,
        //    transform.position.y + (bobbingCurve.Evaluate(Time.time % bobbingCurve.length) * (tileSize / 64)),
        //    transform.position.z);

        //var rot = new Vector3(
        //   transform.Rotation.x,
        //   transform.Rotation.y ,
        //   transform.Rotation.z + (bobbingCurve.Evaluate(Time.time % bobbingCurve.length) * (tileSize / 128)));

        //Renderers.thumbnail.transform.Rotate(Vector3.up * bobbingCurve.Evaluate(Time.time % bobbingCurve.length) * (tileSize / 3));

        //Renderers.glow.transform.position = pos;
        //Renderers.thumbnail.transform.position = pos;
        //Renderers.frame.transform.position = pos;
        //Renderers.thumbnail.transform.position = pos;
        //Renderers.thumbnail.transform.Rotation = rot;
    }


    private void CheckThrobbing()
    {
        //Check abort state
        if (!IsPlaying || !turnManager.IsStartPhase || (turnManager.IsPlayerTurn && !IsPlayer) || (turnManager.IsEnemyTurn && !IsEnemy))
            return;

        //Source: https://forum.unity.com/threads/how-to-make-an-object-move-up-and-down-on-a-loop.380159/
        var scale = new Vector3(
            backScale + (bobbingCurve.Evaluate(Time.time % bobbingCurve.length) * (tileSize / 24)),
            backScale + (bobbingCurve.Evaluate(Time.time % bobbingCurve.length) * (tileSize / 24)),
            1);
        Renderers.SetBackScale(scale);
        Renderers.SetBackColor(IsPlayer ? quality.Color : Colors.Translucent.Black);




    }


    private void CheckFlicker()
    {
        //Check abort state
        if (!IsPlaying || !turnManager.IsStartPhase || (turnManager.IsPlayerTurn && !IsPlayer) || (turnManager.IsEnemyTurn && !IsEnemy))
            return;

        var alpha = 0.5f + (bobbingCurve.Evaluate(Time.time % bobbingCurve.length) * (tileSize / 24));
        Renderers.SetGlowAlpha(alpha);
    }

    private void Shake(float intensity)
    {
        gameObject.transform.GetChild(ActorLayer.Thumbnail).gameObject.transform.position = CurrentTile.position;

        if (intensity > 0)
        {
            var amount = new Vector3(Random.Range(-intensity), Random.Range(intensity), 1);
            gameObject.transform.GetChild(ActorLayer.Thumbnail).gameObject.transform.position += amount;
        }

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
                        position = CurrentTile.position;
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
                            destination = CurrentTile.position;
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
                            destination = CurrentTile.position;
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
        var remainingHP = Mathf.Clamp(hp - damage, 0, maxHP);

        while (hp > remainingHP)
        {

            //Decrease hp
            var min = (int)Math.Max(1, damage * 0.1f);
            var max = (int)Math.Max(1, damage * 0.3f);
            var hit = Random.Int(min, max);
            hp -= hit;
            hp = Mathf.Clamp(hp, remainingHP, maxHP);


            damageTextManager.Spawn(hit.ToString(), position);

            //Shake Actor
            Shake(shakeIntensity.Medium);

            UpdateHealthBar();

            //SlideIn sfx
            soundSource.PlayOneShot(resourceManager.SoundEffect($"Slash{Random.Int(1, 7)}"));

            yield return Wait.For(Interval.FiveTicks);
        }

        Shake(shakeIntensity.Stop);
        hp = remainingHP;
        position = CurrentTile.position;
    }

    public IEnumerator TakeDamage(float damage)
    {
        var remainingHP = Mathf.Clamp(hp - damage, 0, maxHP);

        while (hp > remainingHP)
        {

            //Decrease hp
            var min = (int)Math.Max(1, damage * 0.1f);
            var max = (int)Math.Max(1, damage * 0.3f);
            var hit = Random.Int(min, max);
            hp -= hit;
            hp = Mathf.Clamp(hp, remainingHP, maxHP);

            skillWait += hit * 0.1f;

            damageTextManager.Spawn(hit.ToString(), position);

            //Shake Actor
            Shake(shakeIntensity.Medium);

            UpdateHealthBar();

            //SlideIn sfx
            soundSource.PlayOneShot(resourceManager.SoundEffect($"Slash{Random.Int(1, 7)}"));

            yield return Wait.For(Interval.FiveTicks);
        }

        Shake(shakeIntensity.Stop);
        hp = remainingHP;
        position = CurrentTile.position;
    }


    public void GainSkill(float amount)
    {
        IEnumerator _()
        {
            float ticks = 0f;
            float duration = Interval.OneSecond;

            while (ticks < duration)
            {
                ticks += Interval.OneTick;
                skillWait += Interval.OneTick * amount * 0.1f;
                yield return Wait.OneTick();
            }
        };

        StartCoroutine(_());
    }


    public IEnumerator MissAttack()
    {
        //yield return Wait.For(Interval.QuarterSecond);

        float ticks = 0;
        float duration = Interval.QuarterSecond;

        damageTextManager.Spawn("Miss", position);

        while (ticks < duration)
        {
            ticks += Interval.OneTick;
            Shake(shakeIntensity.Low);
            yield return Wait.OneTick();
        }

        Shake(shakeIntensity.Stop);
    }



    private void UpdateHealthBar()
    {
        var scale = Renderers.healthBarBack.transform.localScale;
        var x = Mathf.Clamp(scale.x * (hp / maxHP), 0, scale.x);
        Renderers.healthBar.transform.localScale = new Vector3(x, scale.y, scale.z);
        Renderers.healthText.text = $@"{hp}/{maxHP}";
    }


    public IEnumerator RadialBackFadeIn()
    {
        var maxAlpha = 0.5f;
        var alpha = 0f;
        Renderers.skillRadialBack.color = new Color(0, 0, 0, alpha);

        while (alpha < maxAlpha)
        {
            alpha += Increment.OnePercent;
            alpha = Mathf.Clamp(alpha, 0, maxAlpha);
            Renderers.skillRadialBack.color = new Color(0, 0, 0, alpha);
            yield return global::Wait.OneTick();
        }

        Renderers.skillRadialBack.color = new Color(0, 0, 0, maxAlpha);
    }

    public IEnumerator RadialBackFadeOut()
    {
        var maxAlpha = 0.5f;
        var alpha = maxAlpha;
        Renderers.skillRadialBack.color = new Color(0, 0, 0, maxAlpha);

        while (alpha > 0)
        {
            alpha -= Increment.OnePercent;
            alpha = Mathf.Clamp(alpha, 0, maxAlpha);
            Renderers.skillRadialBack.color = new Color(0, 0, 0, alpha);
            yield return global::Wait.OneTick();
        }

        Renderers.skillRadialBack.color = new Color(0, 0, 0, 0);
    }

    public IEnumerator Dissolve()
    {
        var alpha = 1f;
        Renderers.SetAlpha(alpha);

        portraitManager.Dissolve(this);
        soundSource.PlayOneShot(resourceManager.SoundEffect("Destroy"));
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
        float alpha = Renderers.statusIcon.color.a;

        Renderers.statusIcon.color = new Color(1, 1, 1, alpha);

        //Fade out
        while (alpha > 0)
        {
            alpha -= increment;
            alpha = Mathf.Clamp(alpha, 0, 1);
            Renderers.statusIcon.color = new Color(1, 1, 1, alpha);
            yield return global::Wait.OneTick();
        }

        //Switch status status Sprite
        Renderers.statusIcon.sprite = resourceManager.statusSprites.First(x => x.id.Equals(status.ToString())).thumbnail;

        //Fade in
        alpha = 0;
        Renderers.statusIcon.color = new Color(1, 1, 1, alpha);

        while (alpha < 1)
        {
            alpha += increment;
            alpha = Mathf.Clamp(alpha, 0, 1);
            Renderers.statusIcon.color = new Color(1, 1, 1, alpha);

            yield return global::Wait.OneTick();
        }

        Renderers.statusIcon.color = new Color(1, 1, 1, 1);

    }

    public void StartGlow()
    {
        //Check abort state
        if (!IsPlaying)
            return;

        Renderers.SetGlowColor(IsPlayer ? quality.Color : Colors.Solid.Black);

        IEnumerator _()
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


        StartCoroutine(_());
    }

    public void StopGlow()
    {
        //Check abort state
        if (!IsPlaying)
            return;

        IEnumerator _()
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

        StartCoroutine(_());
    }


    public void Destroy()
    {
        //Check abort state
        if (!IsActive)
            return;


        IEnumerator _()
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
            actors.Remove(this);
        }

        StartCoroutine(_());
    }

  

   

    public void CalculateActionWait()
    {
        //Check abort state
        if (!IsPlaying)
            return;

        //TODO: Calculate based on stats....
        float min = (Interval.OneSecond * 20) - speed * LuckModifier;
        float max = (Interval.OneSecond * 40) - speed * LuckModifier;

        actionWait = 0;
        actionWaitMax = Random.Float(min, max);
        StartCoroutine(RadialBackFadeIn());
    }

    public void CalculateSkillWait()
    {
        //Check abort state
        if (!IsPlaying)
            return;

        //TODO: Calculate based on stats....
        float min = (Interval.OneSecond * 20) - speed * LuckModifier;
        float max = (Interval.OneSecond * 40) - speed * LuckModifier;

        skillWait = 0;
        skillWaitMax = Random.Float(min, max);
    }


    public void ReadyUp()
    {
        //Check abort state
        if (!IsAlive || !IsActive || !IsEnemy)
            return;

        actionWait = actionWaitMax;
        UpdateActionBar();
    }

  

    private GameObject GameObjectByLayer(int layer)
    {
        return gameObject.transform.GetChild(layer).gameObject;
    }

}
