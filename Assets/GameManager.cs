using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Vector3 mousePosition2D;
    public Vector3 mousePosition3D;

    public Vector2 screenSize;

    public float tileSize;
    public Vector2 tileScale;
    public Vector2 spriteScale;


    public Vector2 boardOffset;

    public string selectedPlayerName;
    public string selectedPlayerCell;

    public string targetCellName;
    public string targetPlayerName;

    public Rigidbody2D selectedRigidBody;

    public GameObject selectedPlayer;


    public Dictionary<string, GameObject> gridMap = new Dictionary<string, GameObject>();

    public List<GameObject> actors;

    public Vector2 size33 = new Vector2(0.333333f, 0.333333f);
    public Vector2 size50 = new Vector2(0.5f, 0.5f);
    public Vector2 size66 = new Vector2(0.666666f, 0.666666f);
    public Vector2 size100 = new Vector2(1.0f, 1.0f);


    private void Awake()
    {
        instance.screenSize = new Vector2(Common.GetScreenToWorldWidth, Common.GetScreenToWorldHeight);
        instance.tileSize = instance.screenSize.x / 6.0f;
        instance.tileScale = new Vector2(instance.tileSize, instance.tileSize);
        instance.boardOffset = new Vector2(-2.25f, 4f);

    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
