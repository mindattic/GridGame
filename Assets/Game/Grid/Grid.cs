using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public GameObject gridCell;

    void Start()
    {
        var refCell = Instantiate(gridCell, new Vector3(0, 0, 0), Quaternion.identity);

        int cols = 3;
        int rows = 3;
        float size = 0.64f;

        var start = new Vector2(transform.position.x, transform.position.y);
        var offset = new Vector2(size, 0);

        for (int row = 1; row <= rows; row++)
        {
            for (int col = 1; col <= cols; col++)
            {
                var cell = Instantiate(gridCell, transform);
                cell.transform.SetParent(transform, true);
                float x = start.x + offset.x + (col * size);
                float y = start.y + offset.y +(row * -size);
                cell.transform.position = new Vector3(x, y, 0);
            }
        }

        Destroy(refCell);

    }
}