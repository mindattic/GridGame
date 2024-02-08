using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
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

    //Board
    public Vector2 boardOffset;
    public int boardColumns = 6;
    public int boardRows = 8;

    //Selection
    public ActorBehavior activeActor;

    //Behaviors
    public List<ActorBehavior> actors;
    public List<TileBehavior> tiles;
    public TimerBehavior timer;

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

        boardOffset = new Vector2(-2.44f, 4f); //TODO: Figure out mathematically

        timer = GameObject.Find("Timer").GetComponent<TimerBehavior>();
 

    }

    void Start()
    {

    }

    void Update()
    {

    }
}
