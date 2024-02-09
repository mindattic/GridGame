using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    //Managers
    public SpriteManager spriteManager;
    public InputManager inputManager;
    public ActorManager actorManager;

    //Sizes
    public Vector2 screenSize;
    public float tileSize;
    public Vector2 tileScale;
    public Vector2 spriteScale;

    //Scales
    public Vector2 size33 = new Vector2(0.333333f, 0.333333f);
    public Vector2 size50 = new Vector2(0.5f, 0.5f);
    public Vector2 size66 = new Vector2(0.666666f, 0.666666f);
    public Vector2 size100 = new Vector2(1.0f, 1.0f);

    //Mouse
    public Vector3 mousePosition2D;
    public Vector3 mousePosition3D;
    public Vector3 mouseOffset;
    public float moveSpeed;
    public float snapDistance;

    //Selection
    public ActorBehavior activeActor;

    //Behaviors
    public BoardBehavior board;
    public TimerBehavior timer;
    public List<ActorBehavior> actors;
    public List<TileBehavior> tiles;


    private void Awake()
    {
        screenSize = new Vector2(Common.GetScreenToWorldWidth, Common.GetScreenToWorldHeight);
        tileSize = screenSize.x / 6.666666f;
        tileScale = new Vector2(tileSize, tileSize);

        size33 = new Vector2(0.333333f, 0.333333f);
        size50 = new Vector2(0.5f, 0.5f);
        size66 = new Vector2(0.666666f, 0.666666f);
        size100 = new Vector2(1.0f, 1.0f);

        moveSpeed = tileSize / 2.5f; //TODO: Figure out mathematically
        snapDistance = moveSpeed / 5f; //TODO: Figure out mathematically

        board = GameObject.Find(Constants.Board).GetComponent<BoardBehavior>();
        timer = GameObject.Find(Constants.Timer).GetComponent<TimerBehavior>();

        spriteManager = GameObject.Find(Constants.Game).GetComponent<SpriteManager>();
        inputManager = GameObject.Find(Constants.Game).GetComponent<InputManager>();
        actorManager = GameObject.Find(Constants.Game).GetComponent<ActorManager>();
    }

    void Start()
    {

    }

    void Update()
    {
      
    }

}
