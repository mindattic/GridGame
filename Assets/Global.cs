using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Global : Singleton<Global> 
{
    public Vector3 mousePosition2D;
    public Vector3 mousePosition3D;

    public Vector2 screenSize;

    public float cellSize;
    public Vector2 cellScale;
    public Vector2 spriteScale;


    public string selectedPlayerName;
    public string selectedPlayerCell;

    public string targetCellName;
    public string targetPlayerName;

    public Rigidbody2D selectedRigidBody;

    public GameObject selectedPlayer;


    public Dictionary<string, GameObject> gridMap = new Dictionary<string, GameObject>();

    public List<GameObject> players;

    public Vector2 size33 = new Vector2(0.333333f, 0.333333f);
    public Vector2 size50 = new Vector2(0.5f, 0.5f);
    public Vector2 size66 = new Vector2(0.666666f, 0.666666f);
    public Vector2 size100 = new Vector2(1.0f, 1.0f);
    
    public Global()
    {
       //Calculate values of global variables
    }
}