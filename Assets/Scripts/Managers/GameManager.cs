using Game.Behaviors;
using Game.Behaviors.Actor;
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
    public StageManager stageManager;
    public TurnManager turnManager;
    public SupportLineManager supportLineManager;
    public AttackLineManager attackLineManager;
    public DamageTextManager damageTextManager;
    public GhostManager ghostManager;
    public PortraitManager portraitManager;
    public OverlayManager overlayManager;
    public TitleManager titleManager;
    public ConsoleManager consoleManager;
    public CardManager cardManager;
    public ActorManager actorManager;
    public SelectedPlayerManager selectedPlayerManager;
    public PlayerManager playerManager;
    public EnemyManager enemyManager;
    public TileManager tileManager;
    public FootstepManager footstepManager;

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
    public float slideSpeed;
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

    public AttackParticipants attackParticipants;

    public ShakeIntensity shakeIntensity;


    private void Awake()
    {

        screenSize = Common.ScreenToWorldSize;
        tileSize = screenSize.x / Constants.percent666;
        tileScale = new Vector2(tileSize, tileSize);

        cursorSpeed = tileSize / 2;
        slideSpeed = tileSize / 4;
        bumpSpeed = tileSize / 14;
        snapDistance = tileSize / 8;
        shakeIntensity = new ShakeIntensity(tileSize);

        board = GameObject.Find(Constants.Board).GetComponent<BoardBehavior>();

        canvas2D = GameObject.Find(Constants.Canvas2D).GetComponent<Canvas>();
        canvas3D = GameObject.Find(Constants.Canvas3D).GetComponent<Canvas>();
        cardManager = GameObject.Find(Constants.Card).GetComponent<CardManager>();

        resourceManager = GameObject.Find(Constants.Game).GetComponent<ResourceManager>();
        stageManager = GameObject.Find(Constants.Game).GetComponent<StageManager>();
        turnManager = GameObject.Find(Constants.Game).GetComponent<TurnManager>();
        inputManager = GameObject.Find(Constants.Game).GetComponent<InputManager>();
        actorManager = GameObject.Find(Constants.Game).GetComponent<ActorManager>();
        supportLineManager = GameObject.Find(Constants.Game).GetComponent<SupportLineManager>();
        attackLineManager = GameObject.Find(Constants.Game).GetComponent<AttackLineManager>();
        damageTextManager = GameObject.Find(Constants.Game).GetComponent<DamageTextManager>();
        ghostManager = GameObject.Find(Constants.Game).GetComponent<GhostManager>();
        portraitManager = GameObject.Find(Constants.Game).GetComponent<PortraitManager>();
        consoleManager = GameObject.Find(Constants.Console).GetComponent<ConsoleManager>();
        overlayManager = GameObject.Find(Constants.Overlay).GetComponent<OverlayManager>();
        titleManager = GameObject.Find(Constants.Title).GetComponent<TitleManager>();
        actorManager = GameObject.Find(Constants.Game).GetComponent<ActorManager>();
        selectedPlayerManager = GameObject.Find(Constants.Game).GetComponent<SelectedPlayerManager>();
        playerManager = GameObject.Find(Constants.Game).GetComponent<PlayerManager>();
        enemyManager = GameObject.Find(Constants.Game).GetComponent<EnemyManager>();
        tileManager = GameObject.Find(Constants.Game).GetComponent<TileManager>();
        footstepManager = GameObject.Find(Constants.Game).GetComponent<FootstepManager>();

        timer = GameObject.Find(Constants.Game).GetComponent<TimerBehavior>();

        soundSource = GameObject.Find(Constants.Game).GetComponents<AudioSource>()[Constants.SoundSourceIndex];
        musicSource = GameObject.Find(Constants.Game).GetComponents<AudioSource>()[Constants.MusicSourceIndex];

        attackParticipants = new AttackParticipants();



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
