using Game.Behaviors;
using Game.Manager;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : Singleton<GameManager>
{
    [HideInInspector] public string deviceType;

    //Settings
    [HideInInspector] public int targetFramerate = 60;
    [HideInInspector] public int vSyncCount = 0;
    [HideInInspector] public float gameSpeed = 1.0f;
    
    //Flags
    [HideInInspector] public bool showActorNameTag = false;
    [HideInInspector] public bool showActorFrame = false;

    //Managers
    [HideInInspector] public ResourceManager resourceManager;
    [HideInInspector] public InputManager inputManager;
    [HideInInspector] public CameraManager cameraManager;
    [HideInInspector] public StageManager stageManager;
    [HideInInspector] public BoardManager boardManager;
    [HideInInspector] public TurnManager turnManager;
    [HideInInspector] public SupportLineManager supportLineManager;
    [HideInInspector] public AttackLineManager attackLineManager;
    [HideInInspector] public DamageTextManager damageTextManager;
    [HideInInspector] public GhostManager ghostManager;
    [HideInInspector] public PortraitManager portraitManager;
    [HideInInspector] public OverlayManager overlayManager;
    [HideInInspector] public TitleManager titleManager;
    [HideInInspector] public CardManager cardManager;
    [HideInInspector] public ActorManager actorManager;
    [HideInInspector] public SelectedPlayerManager selectedPlayerManager;
    [HideInInspector] public PlayerManager playerManager;
    [HideInInspector] public EnemyManager enemyManager;
    [HideInInspector] public TileManager tileManager;
    [HideInInspector] public FootstepManager footstepManager;
    [HideInInspector] public TooltipManager tooltipManager;
    [HideInInspector] public AudioManager audioManager;
    [HideInInspector] public VFXManager vfxManager;
    [HideInInspector] public CoinManager coinManager;

    //UI Managers
    [HideInInspector] public DebugManager debugManager;
    [HideInInspector] public ConsoleManager consoleManager;
    [HideInInspector] public LogManager logManager;

    //Audio
    [HideInInspector] public AudioSource soundSource;
    [HideInInspector] public AudioSource musicSource;

    //relativeScale
    [HideInInspector] public Vector2 viewport;
    [HideInInspector] public float tileSize;
    [HideInInspector] public Vector3 tileScale;
    [HideInInspector] public float cardPortraitSize;

    [HideInInspector] public Canvas canvas2D;
    [HideInInspector] public Canvas canvas3D;

    //Mouse
    [HideInInspector] public Vector3 mousePosition2D;
    [HideInInspector] public Vector3 mousePosition3D;
    [HideInInspector] public Vector3 mouseOffset;

    [HideInInspector] public float cursorSpeed;
    [HideInInspector] public float swapSpeed;
    [HideInInspector] public float moveSpeed;
    [HideInInspector] public float snapDistance;
    [HideInInspector] public float bumpSpeed;

    //selection
    [HideInInspector] public ActorBehavior focusedPlayer;
    [HideInInspector] public ActorBehavior selectedPlayer;

    //Behaviors
    [HideInInspector] public TimerBarBehavior timerBar;
    [HideInInspector] public List<ActorBehavior> actors;
    [HideInInspector] public BoardBehavior board;
    [HideInInspector] public List<TileBehavior> tiles;
    [HideInInspector] public List<SupportLineBehavior> lines;

    [HideInInspector] public CombatParticipants combatParticipants;

    [HideInInspector] public ShakeIntensity shakeIntensity;


    [HideInInspector] public IQueryable<ActorBehavior> players => actors.Where(x => x.team.Equals(Team.Player)).AsQueryable();
    [HideInInspector] public IQueryable<ActorBehavior> enemies => actors.Where(x => x.team.Equals(Team.Enemy)).AsQueryable();

    [HideInInspector] public int coinCount;

    [HideInInspector] public CoinBarBehavior coinBar;

    private void Awake()
    {

        tileSize = Shared.ScreenInWorldUnits.Width / 6;
        tileScale = new Vector3(tileSize, tileSize, 1f);

        cardPortraitSize = Shared.ScreenInPixels.Width / 2;

        cursorSpeed = tileSize * 0.5f;
        swapSpeed = tileSize * 0.1666f;
        moveSpeed = tileSize * 0.125f;
        bumpSpeed = tileSize * 0.08f;
        snapDistance = tileSize * 0.125f;
        shakeIntensity = new ShakeIntensity(tileSize);

        board = GameObject.Find(Constants.Board).GetComponent<BoardBehavior>() ?? throw new UnityException("BoardBehavior is null");
        canvas2D = GameObject.Find(Constants.Canvas2D).GetComponent<Canvas>() ?? throw new UnityException("Canvas2D is null");
        canvas3D = GameObject.Find(Constants.Canvas3D).GetComponent<Canvas>() ?? throw new UnityException("Canvas3D is null");
        cardManager = GameObject.Find(Constants.Card).GetComponent<CardManager>() ?? throw new UnityException("CardManager is null");
        titleManager = GameObject.Find(Constants.Title).GetComponent<TitleManager>() ?? throw new UnityException("TitleManager is null");
        resourceManager = GameObject.Find(Constants.Resources).GetComponent<ResourceManager>() ?? throw new UnityException("ResourceManager is null");
        timerBar = GameObject.Find(Constants.TimerBar).GetComponent<TimerBarBehavior>() ?? throw new UnityException("TimerBarBehavior is null");
        coinBar = GameObject.Find(Constants.CoinBar).GetComponent<CoinBarBehavior>() ?? throw new UnityException("CoinBarBehavior is null");

        var game = GameObject.Find(Constants.Game);
        cameraManager = game.GetComponent<CameraManager>() ?? throw new UnityException("CameraManager is null");
        stageManager = game.GetComponent<StageManager>() ?? throw new UnityException("StageManager is null");
        boardManager = game.GetComponent<BoardManager>() ?? throw new UnityException("BoardManager is null");
        turnManager = game.GetComponent<TurnManager>() ?? throw new UnityException("TurnManager is null");
        inputManager = game.GetComponent<InputManager>() ?? throw new UnityException("InputManager is null");
        actorManager = game.GetComponent<ActorManager>() ?? throw new UnityException("ActorManager is null");
        supportLineManager = game.GetComponent<SupportLineManager>() ?? throw new UnityException("SupportLineManager is null");
        attackLineManager = game.GetComponent<AttackLineManager>() ?? throw new UnityException("AttackLineManager is null");
        damageTextManager = game.GetComponent<DamageTextManager>() ?? throw new UnityException("DamageTextManager is null");
        ghostManager = game.GetComponent<GhostManager>() ?? throw new UnityException("GhostManager is null");
        portraitManager = game.GetComponent<PortraitManager>() ?? throw new UnityException("PortraitManager is null");
        overlayManager = GameObject.Find(Constants.Overlay).GetComponent<OverlayManager>() ?? throw new UnityException("OverlayManager is null");
        actorManager = game.GetComponent<ActorManager>() ?? throw new UnityException("ActorManager is null");
        selectedPlayerManager = game.GetComponent<SelectedPlayerManager>() ?? throw new UnityException("SelectedPlayerManager is null");
        playerManager = game.GetComponent<PlayerManager>() ?? throw new UnityException("PlayerManager is null");
        enemyManager = game.GetComponent<EnemyManager>() ?? throw new UnityException("EnemyManager is null");
        tileManager = game.GetComponent<TileManager>() ?? throw new UnityException("TileManager is null");
        footstepManager = game.GetComponent<FootstepManager>() ?? throw new UnityException("FootstepManager is null");
        tooltipManager = game.GetComponent<TooltipManager>() ?? throw new UnityException("TooltipManager is null");
        audioManager = game.GetComponent<AudioManager>() ?? throw new UnityException("AudioManager is null");
        debugManager = game.GetComponent<DebugManager>() ?? throw new UnityException("DebugManager is null");
        consoleManager = game.GetComponent<ConsoleManager>() ?? throw new UnityException("ConsoleManager is null");
        logManager = game.GetComponent<LogManager>() ?? throw new UnityException("LogManager is null");
        vfxManager = game.GetComponent<VFXManager>() ?? throw new UnityException("VFXManager is null");
        coinManager = game.GetComponent<CoinManager>() ?? throw new UnityException("CoinManager is null");
        soundSource = game.GetComponents<AudioSource>()[Constants.SoundSourceIndex] ?? throw new UnityException("SoundSource is null");
        musicSource = game.GetComponents<AudioSource>()[Constants.MusicSourceIndex] ?? throw new UnityException("MusicSource is null");

        combatParticipants = new CombatParticipants();

        //TODO: Retrieve coin count from "save game json"
        coinCount = 0;

       
     
        #region Platform Dependent Compilation

        //https://docs.unity3d.com/520/Documentation/Manual/PlatformDependentCompilation.html
        //#if UNITY_STANDALONE_WIN
        //        deviceType = "UNITY_STANDALONE_WIN";
        //#elif UNITY_STANDALONE_LINUX
        //  deviceType = "UNITY_STANDALONE_LINUX";
        //#elif UNITY_IPHONE
        //        deviceType = "UNITY_IPHONE";
        //#elif UNITY_STANDALONE_OSX
        //    deviceType = "UNITY_STANDALONE_OSX"
        //#elif UNITY_WEBPLAYER
        //  deviceType = "UNITY_WEBPLAYER";
        //#elif UNITY_WEBGL
        //  deviceType = "UNITY_WEBGL";
        //#else
        //    deviceType = "Unknown";;
        //#endif
        //        Debug.Log($"Running on {deviceType}");

        //#if UNITY_EDITOR
        //        Debug.Log($"Emulated on UNITY_EDITOR");
        //#endif

        #endregion

    }

    void Start()
    {
        Application.targetFrameRate = targetFramerate;
        QualitySettings.vSyncCount = vSyncCount;
    }

    void Update()
    {

    }

    void FixedUpdate()
    {

    }



}
