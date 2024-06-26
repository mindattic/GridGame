using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public string DeviceType;

    //Settings
    public int TargetFramerate = 60;
    public int VSyncCount = 0;

    //Managers
    public ResourceManager ResourceManager;
    public InputManager InputManager;
    public StageManager StageManager;
    public TurnManager TurnManager; 
    public SupportLineManager SupportLineManager;
    public AttackLineManager AttackLineManager;
    public DamageTextManager DamageTextManager;
    public GhostManager GhostManager;
    public PortraitManager PortraitManager;
    public OverlayManager OverlayManager;
    public TitleManager TitleManager;
    public ConsoleManager ConsoleManager;
    public CardManager CardManager;
    public ActorManager ActorManager;
    public SelectedPlayerManager SelectedPlayerManager;
    public PlayerManager PlayerManager;
    public EnemyManager EnemyManager;
    public TileManager TileManager;
    public FootstepManager FootstepManager;

    //Audio
    public AudioSource SoundSource;
    public AudioSource MusicSource;

    //Scale
    public Vector2 ScreenSize;
    public float TileSize;
    public Vector2 TileScale;
    public Vector2 SpriteScale;


    public Canvas Canvas2D;
    public Canvas Canvas3D;

    //Mouse
    public Vector3 MousePosition2D;
    public Vector3 MousePosition3D;
    public Vector3 MouseOffset;

    public float CursorSpeed;
    public float SlideSpeed;
    public float SnapDistance;
    public float BumpSpeed;

    //Selection
    public ActorBehavior FocusedPlayer;
    public ActorBehavior SelectedPlayer;

    //Behaviors
    public BoardBehavior Board;
    public TimerBehavior Timer;
    public List<ActorBehavior> Actors;
    public List<TileBehavior> Tiles;
    public List<SupportLineBehavior> Lines;
 
    public PortraitBehavior PlayerArt;


    public HashSet<Vector2Int> BoardLocations;

    public AttackParticipants AttackParticipants;

    public ShakeIntensity ShakeIntensity;


    private void Awake()
    {

        ScreenSize = Common.ScreenToWorldSize;
        TileSize = ScreenSize.x / Constants.Percent666;
        TileScale = new Vector2(TileSize, TileSize);

        CursorSpeed = TileSize / 2;
        SlideSpeed = TileSize / 4;
        BumpSpeed = TileSize / 14;
        SnapDistance = TileSize / 8;
        ShakeIntensity = new ShakeIntensity(TileSize);

        Board = GameObject.Find(Constants.Board).GetComponent<BoardBehavior>();
        
        Canvas2D = GameObject.Find(Constants.Canvas2D).GetComponent<Canvas>();
        Canvas3D = GameObject.Find(Constants.Canvas3D).GetComponent<Canvas>();
        CardManager = GameObject.Find(Constants.Card).GetComponent<CardManager>();

        ResourceManager = GameObject.Find(Constants.Game).GetComponent<ResourceManager>();
        StageManager = GameObject.Find(Constants.Game).GetComponent<StageManager>();
        TurnManager = GameObject.Find(Constants.Game).GetComponent<TurnManager>();
        InputManager = GameObject.Find(Constants.Game).GetComponent<InputManager>();
        ActorManager = GameObject.Find(Constants.Game).GetComponent<ActorManager>();
        SupportLineManager = GameObject.Find(Constants.Game).GetComponent<SupportLineManager>();
        AttackLineManager = GameObject.Find(Constants.Game).GetComponent<AttackLineManager>();
        DamageTextManager = GameObject.Find(Constants.Game).GetComponent<DamageTextManager>();
        GhostManager = GameObject.Find(Constants.Game).GetComponent<GhostManager>();
        PortraitManager = GameObject.Find(Constants.Game).GetComponent<PortraitManager>();
        ConsoleManager = GameObject.Find(Constants.Console).GetComponent<ConsoleManager>();
        OverlayManager = GameObject.Find(Constants.Overlay).GetComponent<OverlayManager>();
        TitleManager = GameObject.Find(Constants.Title).GetComponent<TitleManager>();     
        ActorManager = GameObject.Find(Constants.Game).GetComponent<ActorManager>();
        SelectedPlayerManager = GameObject.Find(Constants.Game).GetComponent<SelectedPlayerManager>();
        PlayerManager = GameObject.Find(Constants.Game).GetComponent<PlayerManager>();
        EnemyManager = GameObject.Find(Constants.Game).GetComponent<EnemyManager>();
        TileManager = GameObject.Find(Constants.Game).GetComponent<TileManager>();
        FootstepManager = GameObject.Find(Constants.Game).GetComponent<FootstepManager>();

        Timer = GameObject.Find(Constants.Game).GetComponent<TimerBehavior>();

        const int SoundSourceIndex = 0;
        const int MusicSourceIndex = 1;
        SoundSource = GameObject.Find(Constants.Game).GetComponents<AudioSource>()[SoundSourceIndex];
        MusicSource = GameObject.Find(Constants.Game).GetComponents<AudioSource>()[MusicSourceIndex];

        AttackParticipants = new AttackParticipants();

        BoardLocations = new HashSet<Vector2Int>()
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
        Application.targetFrameRate = TargetFramerate;
        QualitySettings.vSyncCount = VSyncCount;
    }

    void Update()
    {

    }

    void FixedUpdate()
    {

    }



}
