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
    public float followSpeed;
    public float snapDistance;

    //Board
    public Vector2 boardOffset;
    public int boardColumns = 5;
    public int boardRows = 8;

    //Selection
    public ActorBehavior activeActor;

    public Vector2Int nullLocation = new Vector2Int(-1, -1);
    public Vector3 nullPosition = Vector3.negativeInfinity;

    //Collections
    public List<ActorBehavior> actors;
    public List<TileBehavior> tiles;

    private void Awake()
    {
        instance.screenSize = new Vector2(Common.GetScreenToWorldWidth, Common.GetScreenToWorldHeight);
        instance.tileSize = instance.screenSize.x / 6.0f;
        instance.tileScale = new Vector2(instance.tileSize, instance.tileSize);

        size33 = new Vector2(0.333333f, 0.333333f);
        size50 = new Vector2(0.5f, 0.5f);
        size66 = new Vector2(0.666666f, 0.666666f);
        size100 = new Vector2(1.0f, 1.0f);

        followSpeed = tileSize / 2.5f; //TODO: Figure out mathematically
        snapDistance = followSpeed / 5f; //TODO: Figure out mathematically

        boardOffset = new Vector2(-2.25f, 4f); //TODO: Figure out mathematically
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
