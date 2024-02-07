using UnityEngine;

public class Geometry
{
    static private Vector2 gridOffset => GameManager.instance.boardOffset;
    static private float tileSize => GameManager.instance.tileSize;


    static public Vector3 PointFromGrid(int col, int row)
    {
        Vector2Int point = new Vector2Int(col, row);
        float x = gridOffset.x + (tileSize * point.x);
        float y = gridOffset.y + -(tileSize * point.y);
        return new Vector3(x, y, 0);
    }

    static public Vector3 PointFromGrid(Vector2Int point)
    {
        float x = gridOffset.x + (tileSize * point.x);
        float y = gridOffset.y + -(tileSize * point.y);
        return new Vector3(x, y, 0);
    }

    static public Vector2Int GridPoint(int col, int row)
    {
        return new Vector2Int(col, row);
    }

    static public Vector2Int GridFromPoint(Vector3 point)
    {
        int col = Mathf.FloorToInt(point.x / tileSize - gridOffset.x);
        int row = Mathf.FloorToInt(-point.y / tileSize - gridOffset.y);
        return new Vector2Int(col, row);
    }
}
