using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExtendedMonoBehavior : MonoBehaviour
{

    //Canvases
    protected Canvas canvas2D => GameManager.instance.Canvas2D;
    protected Canvas canvas3D => GameManager.instance.Canvas3D;

    //Managers
    protected InputManager inputManager => GameManager.instance.inputManager;
    protected StageManager stageManager => GameManager.instance.stageManager;
    protected TurnManager turnManager => GameManager.instance.TurnManager;
    protected ActorManager actorManager => GameManager.instance.actorManager;
    protected SupportLineManager supportLineManager => GameManager.instance.supportLineManager;
    protected AttackLineManager attackLineManager => GameManager.instance.attackLineManager;
    protected DamageTextManager damageTextManager => GameManager.instance.damageTextManager;
    protected ResourceManager resourceManager => GameManager.instance.resourceManager;
    protected GhostManager ghostManager => GameManager.instance.ghostManager;
    protected PortraitManager portraitManager => GameManager.instance.portraitManager;
    protected OverlayManager overlayManager => GameManager.instance.overlayManager;
    protected TitleManager titleManager => GameManager.instance.titleManager;
    protected ConsoleManager consoleManager => GameManager.instance.consoleManager;
    protected CardManager cardManager => GameManager.instance.cardManager;
    protected SelectedPlayerManager selectedPlayerManager => GameManager.instance.selectedPlayerManager;
    protected PlayerManager playerManager => GameManager.instance.playerManager;
    protected EnemyManager enemyManager => GameManager.instance.enemyManager;
    protected TileManager tileManager => GameManager.instance.tileManager;
    protected FootstepManager footstepManager => GameManager.instance.footstepManager;

    //Audio
    protected AudioSource soundSource => GameManager.instance.soundSource;
    protected AudioSource musicSource => GameManager.instance.musicSource;

    //Behaviors
    protected BoardBehavior board => GameManager.instance.board;
    protected TimerBehavior timer => GameManager.instance.timer;
    protected List<TileBehavior> tiles => GameManager.instance.tiles;
    protected List<ActorBehavior> actors
    {
        get => GameManager.instance.actors;
        set => GameManager.instance.actors = value;
    }

    protected List<ActorBehavior> players => GameManager.instance.actors.Where(x => x.team.Equals(Team.Player)).ToList();
    protected List<ActorBehavior> enemies => GameManager.instance.actors.Where(x => x.team.Equals(Team.Enemy)).ToList();

    //Layers
    protected static class ActorLayer
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

    //Actor
    protected bool HasFocusedPlayer => focusedPlayer != null;
    protected bool HasSelectedPlayer => selectedPlayer != null;

    //Scale
    protected float TileSize => GameManager.instance.TileSize;
    protected Vector2 tileScale => GameManager.instance.TileScale;

    protected ShakeIntensity shakeIntensity => GameManager.instance.shakeIntensity;

    //Percent
    protected float percent25 => Constants.percent25;
    protected float percent33 => Constants.percent33;
    protected float percent50 => Constants.percent50;
    protected float percent66 => Constants.percent66;
    protected float percent75 => Constants.percent75;
    protected float percent100 => Constants.percent100;
    protected float percent333 => Constants.percent333;
    protected float percent666 => Constants.percent666;

    //Size
    protected Vector2 size25 => Constants.size25;
    protected Vector2 size33 => Constants.size33;
    protected Vector2 size50 => Constants.size50;
    protected Vector2 size66 => Constants.size66;
    protected Vector2 size75 => Constants.size75;
    protected Vector2 size100 => Constants.size100;

    //Mouse
    protected Vector3 mousePosition2D => GameManager.instance.mousePosition2D;
    protected Vector3 mousePosition3D => GameManager.instance.mousePosition3D;
    protected Vector3 mouseOffset
    {
        get { return GameManager.instance.mouseOffset; }
        set { GameManager.instance.mouseOffset = value; }
    }

    protected float CursorSpeed => GameManager.instance.CursorSpeed;
    protected float SlideSpeed => GameManager.instance.SlideSpeed;
    protected float BumpSpeed => GameManager.instance.BumpSpeed;
    protected float SnapDistance => GameManager.instance.SnapDistance;


    protected AttackParticipants AttackParticipants
    {
        get { return GameManager.instance.AttackParticipants; }
        set { GameManager.instance.AttackParticipants = value; }
    }

    protected ActorBehavior focusedPlayer
    {
        get { return GameManager.instance.FocusedPlayer; }
        set { GameManager.instance.FocusedPlayer = value; }
    }

    protected ActorBehavior selectedPlayer
    {
        get { return GameManager.instance.SelectedPlayer; }
        set { GameManager.instance.SelectedPlayer = value; }
    }

   


    public void CheckAlignment(ActorPair pair, out bool hasEnemiesBetween, out bool hasPlayersBetween, out bool hasGapsBetween)
    {
        hasEnemiesBetween = false;
        hasPlayersBetween = false;
        hasGapsBetween = false;

        if (pair.Axis == Axis.Vertical)
        {
            pair.HighestActor = pair.Actor1.location.y > pair.Actor2.location.y ? pair.Actor1 : pair.Actor2;
            pair.LowestActor = pair.HighestActor == pair.Actor1 ? pair.Actor2 : pair.Actor1;
            pair.Enemies = enemies.Where(x => x != null && x.IsAlive && x.IsActive && x.IsSameColumn(pair.Actor1.location) && Common.IsBetween(x.location.y, pair.Floor, pair.Ceiling)).OrderBy(x => x.location.y).ToList();
            pair.Players = players.Where(x => x != null && x.IsAlive && x.IsActive && x.IsSameColumn(pair.Actor1.location) && Common.IsBetween(x.location.y, pair.Floor, pair.Ceiling)).OrderBy(x => x.location.y).ToList();
            pair.Gaps = tiles.Where(x => !x.IsOccupied && pair.Actor1.IsSameColumn(x.location) && Common.IsBetween(x.location.y, pair.Floor, pair.Ceiling)).OrderBy(x => x.location.y).ToList();
        }
        else if (pair.Axis == Axis.Horizontal)
        {
            pair.HighestActor = pair.Actor1.location.x > pair.Actor2.location.x ? pair.Actor1 : pair.Actor2;
            pair.LowestActor = pair.HighestActor == pair.Actor1 ? pair.Actor2 : pair.Actor1;
            pair.Enemies = enemies.Where(x => x != null && x.IsAlive && x.IsActive && x.IsSameRow(pair.Actor1.location) && Common.IsBetween(x.location.x, pair.Floor, pair.Ceiling)).OrderBy(x => x.location.x).ToList();
            pair.Players = players.Where(x => x != null && x.IsAlive && x.IsActive && x.IsSameRow(pair.Actor1.location) && Common.IsBetween(x.location.x, pair.Floor, pair.Ceiling)).OrderBy(x => x.location.x).ToList();
            pair.Gaps = tiles.Where(x => !x.IsOccupied && pair.Actor1.IsSameRow(x.location) && Common.IsBetween(x.location.x, pair.Floor, pair.Ceiling)).OrderBy(x => x.location.x).ToList();
        }

        hasEnemiesBetween = pair.Enemies.Any();
        hasPlayersBetween = pair.Players.Any();
        hasGapsBetween = pair.Gaps.Any();
        //var hasDuplicate = AttackParticipants.attackingPairs.Count(x => (x.Actor1 == pair.Actor1 && x.Actor2 == pair.Actor2) || (x.Actor1 == pair.Actor2 && x.Actor2 == pair.Actor1)) > 0;
    }




}

