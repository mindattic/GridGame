using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject cellPrefab;
    public GameObject playerPrefab;

    private float cellSize => Global.instance.cellSize;
    private Vector2 cellScale => Global.instance.cellScale;

    private Dictionary<Coordinates, Vector2> gridMap
    {
        get { return Global.instance.gridMap; }
        set { Global.instance.gridMap = value; }
    }  
  

    void Start()
    {
        GenerateGrid();
        GeneratePlayer();
    }

    void GenerateGrid()
    {
        var instance = Instantiate(cellPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        int cols = 5;
        int rows = 8;

        var cellScale = new Vector2(cellSize, cellSize);

        var start = new Vector2(transform.position.x, transform.position.y);
        var offset = new Vector2(cellSize, 0);

        for (int row = 1; row <= rows; row++)
        {
            for (int col = 1; col <= cols; col++)
            {
                var cell = Instantiate(instance, transform);
                cell.transform.SetParent(transform, true);
                cell.transform.localScale = cellScale;
                cell.GetComponent<CellManager>().coodinates = new Coordinates(col, row);
                float x = start.x + offset.x + (col * cellSize);
                float y = start.y + offset.y + (row * -cellSize);
                cell.transform.position = new Vector3(x, y, 0);


                //Assign grid map entry
                gridMap.Add(new Coordinates(col, row), new Vector2(x, y));
            }
        }

        Destroy(instance);
    }

    void GeneratePlayer()
    {

        var player1 = Instantiate(playerPrefab, transform);
        player1.transform.SetParent(transform, true);
        player1.transform.localScale = cellScale;

        //Set initial position and coordinates
        player1.GetComponent<Rigidbody2D>().MovePosition(new Vector2(2, -2));
        player1.GetComponent<PlayerManager>().DropPlayer();

        //player1.transform.position = new Vector3(-1, 4, 0);

        //var player2 = Instantiate(playerPrefab, transform);
        //player2.transform.SetParent(transform, true);
        //player2.transform.localScale = cellScale;
        //player2.GetComponent<BoxCollider2D>().size = cellScale;
        //player2.transform.position = new Vector3(3, 4, 0);
    }




    //public Transform GetTransformByCoordinates(Coordinates coordinates)
    //{

    //}
}