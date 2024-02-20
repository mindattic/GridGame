using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public string deviceType;

    //Settings
    public int targetFramerate = 60;
    public int vSyncCount = 0;

    //Managers
    public SpriteManager spriteManager;
    public InputManager inputManager;
    public ActorManager actorManager;
    public LineManager lineManager;

    //Scale
    public Vector2 screenSize;
    public float tileSize;
    public Vector2 tileScale;
    public Vector2 spriteScale;

    //Mouse
    public Vector3 mousePosition2D;
    public Vector3 mousePosition3D;
    public Vector3 mouseOffset;

    public float cursorSpeed;
    public float slideSpeed;
    public float snapDistance;

    //Selection
    public ActorBehavior selectedPlayer;

    //Behaviors
    public BoardBehavior board;
    public TimerBehavior timer;
    public List<ActorBehavior> actors;
    public List<ActorBehavior> players;
    public List<ActorBehavior> enemies;
    public List<TileBehavior> tiles;
    public List<LineBehavior> lines;


    public PlayerArtBehavior playerArt;


    public HashSet<Vector2Int> boardLocations;

    public BattleParticipants battle;

    private void Awake()
    {

        screenSize = Common.ViewportToWorldSize;
        tileSize = screenSize.x / Constants.percent666;
        tileScale = new Vector2(tileSize, tileSize);

        cursorSpeed = tileSize / 2;
        slideSpeed = tileSize / 4;
        snapDistance = tileSize / 8;

        board = GameObject.Find(Constants.Board).GetComponent<BoardBehavior>();
        timer = GameObject.Find(Constants.Timer).GetComponent<TimerBehavior>();

        spriteManager = GameObject.Find(Constants.Game).GetComponent<SpriteManager>();
        inputManager = GameObject.Find(Constants.Game).GetComponent<InputManager>();
        actorManager = GameObject.Find(Constants.Game).GetComponent<ActorManager>();
        lineManager = GameObject.Find(Constants.Game).GetComponent<LineManager>();
        playerArt = GameObject.Find("PlayerArt").GetComponent<PlayerArtBehavior>();

        battle = new BattleParticipants();

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

}
