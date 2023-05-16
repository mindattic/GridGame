using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public GameObject gridCell;

    void Start()
    {
        GenerateGrid();
    }




    void GenerateGrid()
    {
        var refCell = Instantiate(gridCell, new Vector3(0, 0, 0), Quaternion.identity);

        int cols = 5;
        int rows = 8;

        var screenSize = new Vector2(Common.GetScreenToWorldWidth, Common.GetScreenToWorldHeight);
        float size = screenSize.x / 6;

        var start = new Vector2(transform.position.x, transform.position.y);
        var offset = new Vector2(size, 0);

        for (int row = 1; row <= rows; row++)
        {
            for (int col = 1; col <= cols; col++)
            {
                var cell = Instantiate(gridCell, transform);
                cell.transform.SetParent(transform, true);
                cell.transform.localScale = Vector3.one * size;

                float x = start.x + offset.x + (col * size);
                float y = start.y + offset.y + (row * -size);
                cell.transform.position = new Vector3(x, y, 0);

            }
        }

        Destroy(refCell);
    }

}