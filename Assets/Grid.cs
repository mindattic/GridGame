using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Grid : MonoBehaviour
{
    // Reference to the Prefab. Drag a Prefab into this field in the Inspector.

    public GameObject canvas;
    public GameObject cell;
    GameObject grid;

    // This script will simply instantiate the Prefab when the game starts.
    void Start()
    {
        grid = GameObject.FindGameObjectWithTag("Grid");

        int columns = 6;
        int rows = 12;

        float cellSize = 150;

        
        Vector2 startPos = new Vector2(500, 1200);


        for (int r = 1; r <= rows; r++)
        {
            for (int c = 1; c <= columns; c++)
            {
                GameObject currentCell = Instantiate(cell, new Vector3(0, 0, 0), Quaternion.identity);
                currentCell.transform.SetParent(grid.transform, true);

                float x = grid.transform.position.x + startPos.x - (cellSize * c);
                float y = grid.transform.position.y + startPos.y - (cellSize * r);

                Vector3 pos = new Vector3((float)x, (float)y, 1);
                currentCell.transform.position = pos;
                currentCell.transform.localScale = Vector3.one;
            }

        }



        // Instantiate at position (0, 0, 0) and zero rotation.


    }
}