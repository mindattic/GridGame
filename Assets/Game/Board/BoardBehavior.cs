using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardBehavior : MonoBehaviour
{
    private BoardBehavior board => GameManager.instance.board;
    private TimerBehavior timer => GameManager.instance.timer;

    private List<ActorBehavior> actors => GameManager.instance.actors;


    public Vector2 offset = new Vector2(-2.44f, 4f);
    public int columns = 6;
    public int rows = 8;

    public float top;
    public float right;
    public float bottom;
    public float left;


    private float tileSize => GameManager.instance.tileSize;


    void Awake()
    {
       
    }

    void Start()
    {
        top = 0;
        right = offset.x + (tileSize * columns) - tileSize / 2;
        bottom = 0;
        left = offset.x + tileSize / 2;
    }

    // Update is called once per frame
    void Update()
    {

    }



    public void ResetBoard()
    {
        GameManager.instance.activeActor = null;
        actors.First(x => x.name == "Sentinel").Init(new Vector2Int(2, 3));
        actors.First(x => x.name == "Corsair").Init(new Vector2Int(4, 4));
        actors.First(x => x.name == "Oracle").Init(new Vector2Int(5, 6));
        actors.First(x => x.name == "Slime A").Init(new Vector2Int(5, 6));
        actors.First(x => x.name == "Slime B").Init(new Vector2Int(3, 3));
        timer.Set(scale: 1f, start: false);
    }


}
