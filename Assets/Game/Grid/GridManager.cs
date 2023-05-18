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
                cell.GetComponent<BoxCollider2D>().size = cellScale;
                cell.GetComponent<CellManager>().coodinates = new Coordinates(col, row);
                float x = start.x + offset.x + (col * cellSize);
                float y = start.y + offset.y + (row * -cellSize);
                cell.transform.position = new Vector3(x, y, 0);
            }
        }

        Destroy(instance);
    }

    void GeneratePlayer()
    {

        var player1 = Instantiate(playerPrefab, transform);
        player1.transform.SetParent(transform, true);
        player1.transform.localScale = cellScale;
        player1.GetComponent<BoxCollider2D>().size = cellScale;
        player1.transform.position = new Vector3(-1, 4, 0);

        //var player2 = Instantiate(playerPrefab, transform);
        //player2.transform.SetParent(transform, true);
        //player2.transform.localScale = cellScale;
        //player2.GetComponent<BoxCollider2D>().size = cellScale;
        //player2.transform.position = new Vector3(3, 4, 0);
    }

}