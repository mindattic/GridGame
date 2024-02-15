using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public System.Random rnd = new System.Random();

    //Settings
    public int targetFramerate = 60;
    public int vSyncCount = 1;

    //Managers
    public SpriteManager spriteManager;
    public InputManager inputManager;
    public ActorManager actorManager;

    //Scale
    public Vector2 screenSize;
    public float tileSize;
    public Vector2 tileScale;
    public Vector2 spriteScale;

   

    //Mouse
    public Vector3 mousePosition2D;
    public Vector3 mousePosition3D;
    public Vector3 mouseOffset;
 
    public float moveSpeed;
    public float snapDistance;

    //Selection
    public ActorBehavior selectedPlayer;

    //Behaviors
    public BoardBehavior board;
    public TimerBehavior timer;
    public List<ActorBehavior> actors;
    public List<TileBehavior> tiles;
    public List<LineBehavior> lines;

    private void Awake()
    {
        screenSize = new Vector2(Common.GetScreenToWorldWidth, Common.GetScreenToWorldHeight);
        tileSize = screenSize.x / Constants.percent666;
        tileScale = new Vector2(tileSize, tileSize);

        moveSpeed = tileSize / 2;
        snapDistance = moveSpeed / Constants.percent666;

        board = GameObject.Find(Constants.Board).GetComponent<BoardBehavior>();
        timer = GameObject.Find(Constants.Timer).GetComponent<TimerBehavior>();

        spriteManager = GameObject.Find(Constants.Game).GetComponent<SpriteManager>();
        inputManager = GameObject.Find(Constants.Game).GetComponent<InputManager>();
        actorManager = GameObject.Find(Constants.Game).GetComponent<ActorManager>();
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
