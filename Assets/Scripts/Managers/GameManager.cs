using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public string deviceType;

    //Settings
    public int targetFramerate = 60;
    public int vSyncCount = 0;

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
    public Vector2 spriteScale;


    public Canvas canvas2D;
    public Canvas canvas3D;

    //Mouse
    public Vector3 mousePosition2D;
    public Vector3 mousePosition3D;
    public Vector3 mouseOffset;

    public float cursorSpeed;
    public float slideSpeed;
    public float snapDistance;


    //Selection
    public ActorBehavior selectedPlayer;
    public ActorBehavior currentPlayer;

    //Behaviors
    public BoardBehavior board;
    public TimerBehavior timer;
    public List<ActorBehavior> actors;
    //public List<ActorBehavior> players;
    //public List<ActorBehavior> enemies;
    public List<TileBehavior> tiles;
    public List<SupportLineBehavior> lines;
 
    public PortraitBehavior playerArt;


    public HashSet<Vector2Int> boardLocations;

    public AttackParticipants attackParticipants;

    public ShakeIntensity shakeIntensity;


    private void Awake()
    {

        screenSize = Common.ScreenToWorldSize;
        tileSize = screenSize.x / Constants.percent666;
        tileScale = new Vector2(tileSize, tileSize);

        cursorSpeed = tileSize / 2;
        slideSpeed = tileSize / 4;
        snapDistance = tileSize / 8;
        shakeIntensity = new ShakeIntensity(tileSize);

        board = GameObject.Find(Constants.Board).GetComponent<BoardBehavior>();
        timer = GameObject.Find(Constants.Timer).GetComponent<TimerBehavior>();

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

        const int SoundSourceIndex = 0;
        const int MusicSourceIndex = 1;
        soundSource = GameObject.Find(Constants.Game).GetComponents<AudioSource>()[SoundSourceIndex];
        musicSource = GameObject.Find(Constants.Game).GetComponents<AudioSource>()[MusicSourceIndex];

        attackParticipants = new AttackParticipants();

        boardLocations = new HashSet<Vector2Int>()
        {
            new Vector2Int(1, 1), new Vector2Int(1, 2), new Vector2Int(1, 3), new Vector2Int(1, 4), new Vector2Int(1, 5), new Vector2Int(1, 6), new Vector2Int(1, 7), new Vector2Int(1, 8),
            new Vector2Int(2, 1), new Vector2Int(2, 2), new Vector2Int(2, 3), new Vector2Int(2, 4), new Vector2Int(2, 5), new Vector2Int(2, 6), new Vector2Int(2, 7), new Vector2Int(2, 8),
            new Vector2Int(3, 1), new Vector2Int(3, 2), new Vector2Int(3, 3), new Vector2Int(3, 4), new Vector2Int(3, 5), new Vector2Int(3, 6), new Vector2Int(3, 7), new Vector2Int(3, 8),
            new Vector2Int(4, 1), new Vector2Int(4, 2), new Vector2Int(4, 3), new Vector2Int(4, 4), new Vector2Int(4, 5), new Vector2Int(4, 6), new Vector2Int(4, 7), new Vector2Int(4, 8),
            new Vector2Int(5, 1), new Vector2Int(5, 2), new Vector2Int(5, 3), new Vector2Int(5, 4), new Vector2Int(5, 5), new Vector2Int(5, 6), new Vector2Int(5, 7), new Vector2Int(5, 8),
            new Vector2Int(6, 1), new Vector2Int(6, 2), new Vector2Int(6, 3), new Vector2Int(6, 4), new Vector2Int(6, 5), new Vector2Int(6, 6), new Vector2Int(6, 7), new Vector2Int(6, 8),
        };

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
