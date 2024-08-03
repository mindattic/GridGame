using Game.Behaviors;
using Game.Behaviors.Actor;
using Game.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExtendedMonoBehavior : MonoBehaviour
{

    //Variables
    protected float gameSpeed => GameManager.instance.gameSpeed;

    //Canvases
    protected Canvas canvas2D => GameManager.instance.canvas2D;
    protected Canvas canvas3D => GameManager.instance.canvas3D;

    //Managers
    protected CameraManager cameraManager => GameManager.instance.cameraManager;
    protected InputManager inputManager => GameManager.instance.inputManager;
    protected StageManager stageManager => GameManager.instance.stageManager;
    protected TurnManager turnManager => GameManager.instance.turnManager;
    protected ActorManager actorManager => GameManager.instance.actorManager;
    protected SupportLineManager supportLineManager => GameManager.instance.supportLineManager;
    protected AttackLineManager attackLineManager => GameManager.instance.attackLineManager;
    protected DamageTextManager damageTextManager => GameManager.instance.damageTextManager;
    protected ResourceManager resourceManager => GameManager.instance.resourceManager;
    protected GhostManager ghostManager => GameManager.instance.ghostManager;
    protected PortraitManager portraitManager => GameManager.instance.portraitManager;
    protected OverlayManager overlayManager => GameManager.instance.overlayManager;
    protected TitleManager titleManager => GameManager.instance.titleManager;
    protected CardManager cardManager => GameManager.instance.cardManager;
    protected SelectedPlayerManager selectedPlayerManager => GameManager.instance.selectedPlayerManager;
    protected PlayerManager playerManager => GameManager.instance.playerManager;
    protected EnemyManager enemyManager => GameManager.instance.enemyManager;
    protected TileManager tileManager => GameManager.instance.tileManager;
    protected FootstepManager footstepManager => GameManager.instance.footstepManager;
    protected AudioManager audioManager => GameManager.instance.audioManager;
 
    //UI
    protected ConsoleManager consoleManager => GameManager.instance.consoleManager;
    protected LogManager logManager => GameManager.instance.logManager;

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

    protected IQueryable<ActorBehavior> players => GameManager.instance.actors.Where(x => x.team.Equals(Team.Player)).AsQueryable();
    protected IQueryable<ActorBehavior> enemies => GameManager.instance.actors.Where(x => x.team.Equals(Team.Enemy)).AsQueryable();

    //Layers
    protected static class ActorLayer
    {
        public const int Quality = 0;
        public const int Glow = 1;
        public const int Parallax = 2;        
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
    protected float tileSize => GameManager.instance.tileSize;
    protected Vector2 tileScale => GameManager.instance.tileScale;

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

    protected float cursorSpeed => GameManager.instance.cursorSpeed;
    protected float swapSpeed => GameManager.instance.swapSpeed;
    protected float moveSpeed => GameManager.instance.moveSpeed;
    protected float bumpSpeed => GameManager.instance.bumpSpeed;
    protected float snapDistance => GameManager.instance.snapDistance;


    protected CombatParticipants combatParticipants
    {
        get { return GameManager.instance.combatParticipants; }
        set { GameManager.instance.combatParticipants = value; }
    }

    protected ActorBehavior focusedPlayer
    {
        get { return GameManager.instance.focusedPlayer; }
        set { GameManager.instance.focusedPlayer = value; }
    }

    protected ActorBehavior selectedPlayer
    {
        get { return GameManager.instance.selectedPlayer; }
        set { GameManager.instance.selectedPlayer = value; }
    }

   


  



}

