using Game.Behaviors;
using Game.Behaviors.Actor;
using Game.Manager;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public string DeviceType;

    //Settings
    public int targetFramerate = 60;
    public int vSyncCount = 0;
    public float gameSpeed = 1.0f;

    //Managers
    public ResourceManager resourceManager;
    public InputManager inputManager;
    public CameraManager cameraManager;
    public StageManager stageManager;
    public TurnManager turnManager;
    public SupportLineManager supportLineManager;
    public AttackLineManager attackLineManager;
    public DamageTextManager damageTextManager;
    public GhostManager ghostManager;
    public PortraitManager portraitManager;
    public OverlayManager overlayManager;
    public TitleManager titleManager; 
    public CardManager cardManager;
    public ActorManager actorManager;
    public SelectedPlayerManager selectedPlayerManager;
    public PlayerManager playerManager;
    public EnemyManager enemyManager;
    public TileManager tileManager;
    public FootstepManager footstepManager;
    public AudioManager audioManager;

    //UI Managers
    public ConsoleManager consoleManager;
    public LogManager logManager;

    //Audio
    public AudioSource soundSource;
    public AudioSource musicSource;

    //Scale
    public Vector2 screenSize;
    public float tileSize;
    public Vector2 tileScale;
    public Vector2 SpriteScale;


    public Canvas canvas2D;
    public Canvas canvas3D;

    //Mouse
    public Vector3 mousePosition2D;
    public Vector3 mousePosition3D;
    public Vector3 mouseOffset;

    public float cursorSpeed;
    public float swapSpeed;
    public float moveSpeed;
    public float snapDistance;
    public float bumpSpeed;

    //selection
    public ActorBehavior focusedPlayer;
    public ActorBehavior selectedPlayer;

    //Behaviors
    public BoardBehavior board;
    public TimerBehavior timer;
    public List<ActorBehavior> actors;
    public List<TileBehavior> tiles;
    public List<SupportLineBehavior> lines;

    public CombatParticipants combatParticipants;

    public ShakeIntensity shakeIntensity;


    private void Awake()
    {

        screenSize = Common.ScreenToWorldSize;
        tileSize = screenSize.x / Constants.percent666;
        tileScale = new Vector2(tileSize, tileSize);

        cursorSpeed = tileSize / 2;
        swapSpeed = tileSize / 4;
        moveSpeed = tileSize / 6;
        bumpSpeed = tileSize / 14;
        snapDistance = tileSize / 8;
        shakeIntensity = new ShakeIntensity(tileSize);


        board = GameObject.Find(Constants.Board).GetComponent<BoardBehavior>() ?? throw new UnityException("BoardBehavior is null");

        canvas2D = GameObject.Find(Constants.Canvas2D).GetComponent<Canvas>() ?? throw new UnityException("Canvas2D is null");
        canvas3D = GameObject.Find(Constants.Canvas3D).GetComponent<Canvas>() ?? throw new UnityException("Canvas3D is null");
        cardManager = GameObject.Find(Constants.Card).GetComponent<CardManager>() ?? throw new UnityException("CardManager is null");

        resourceManager = GameObject.Find(Constants.Game).GetComponent<ResourceManager>() ?? throw new UnityException("ResourceManager is null");
        cameraManager = GameObject.Find(Constants.Game).GetComponent<CameraManager>() ?? throw new UnityException("CameraManager is null");
        stageManager = GameObject.Find(Constants.Game).GetComponent<StageManager>() ?? throw new UnityException("StageManager is null");
        turnManager = GameObject.Find(Constants.Game).GetComponent<TurnManager>() ?? throw new UnityException("TurnManager is null");
        inputManager = GameObject.Find(Constants.Game).GetComponent<InputManager>() ?? throw new UnityException("InputManager is null");
        actorManager = GameObject.Find(Constants.Game).GetComponent<ActorManager>() ?? throw new UnityException("ActorManager is null");
        supportLineManager = GameObject.Find(Constants.Game).GetComponent<SupportLineManager>() ?? throw new UnityException("SupportLineManager is null");
        attackLineManager = GameObject.Find(Constants.Game).GetComponent<AttackLineManager>() ?? throw new UnityException("AttackLineManager is null");
        damageTextManager = GameObject.Find(Constants.Game).GetComponent<DamageTextManager>() ?? throw new UnityException("DamageTextManager is null");
        ghostManager = GameObject.Find(Constants.Game).GetComponent<GhostManager>() ?? throw new UnityException("GhostManager is null");
        portraitManager = GameObject.Find(Constants.Game).GetComponent<PortraitManager>() ?? throw new UnityException("PortraitManager is null");     
        overlayManager = GameObject.Find(Constants.Overlay).GetComponent<OverlayManager>() ?? throw new UnityException("OverlayManager is null");
        titleManager = GameObject.Find(Constants.Title).GetComponent<TitleManager>() ?? throw new UnityException("TitleManager is null");
        actorManager = GameObject.Find(Constants.Game).GetComponent<ActorManager>() ?? throw new UnityException("ActorManager is null");
        selectedPlayerManager = GameObject.Find(Constants.Game).GetComponent<SelectedPlayerManager>() ?? throw new UnityException("SelectedPlayerManager is null");
        playerManager = GameObject.Find(Constants.Game).GetComponent<PlayerManager>() ?? throw new UnityException("PlayerManager is null");
        enemyManager = GameObject.Find(Constants.Game).GetComponent<EnemyManager>() ?? throw new UnityException("EnemyManager is null");
        tileManager = GameObject.Find(Constants.Game).GetComponent<TileManager>() ?? throw new UnityException("TileManager is null");
        footstepManager = GameObject.Find(Constants.Game).GetComponent<FootstepManager>() ?? throw new UnityException("FootstepManager is null");
        audioManager = GameObject.Find(Constants.Game).GetComponent<AudioManager>() ?? throw new UnityException("AudioManager is null");
     
        consoleManager = GameObject.Find(Constants.Console).GetComponent<ConsoleManager>() ?? throw new UnityException("ConsoleManager is null");
        logManager = GameObject.Find(Constants.Log).GetComponent<LogManager>() ?? throw new UnityException("LogManager is null");

        timer = GameObject.Find(Constants.Game).GetComponent<TimerBehavior>() ?? throw new UnityException("TimerBehavior is null");

        soundSource = GameObject.Find(Constants.Game).GetComponents<AudioSource>()[Constants.SoundSourceIndex] ?? throw new UnityException("SoundSource is null");
        musicSource = GameObject.Find(Constants.Game).GetComponents<AudioSource>()[Constants.MusicSourceIndex] ?? throw new UnityException("MusicSource is null");

        combatParticipants = new CombatParticipants();



        #region Platform Dependent Compilation

        //https://docs.unity3d.com/520/Documentation/Manual/PlatformDependentCompilation.html
        //#if UNITY_STANDALONE_WIN
        //        DeviceType = "UNITY_STANDALONE_WIN";
        //#elif UNITY_STANDALONE_LINUX
        //  DeviceType = "UNITY_STANDALONE_LINUX";
        //#elif UNITY_IPHONE
        //        DeviceType = "UNITY_IPHONE";
        //#elif UNITY_STANDALONE_OSX
        //    DeviceType = "UNITY_STANDALONE_OSX"
        //#elif UNITY_WEBPLAYER
        //  DeviceType = "UNITY_WEBPLAYER";
        //#elif UNITY_WEBGL
        //  DeviceType = "UNITY_WEBGL";
        //#else
        //    DeviceType = "Unknown";;
        //#endif
        //        Debug.Log($"Running on {DeviceType}");

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
