using UnityEngine;

public static class Tag
{
    public static string Board = "Board";
    public static string Tile = "Tile";
    public static string Actor = "Actor";
    public static string Wall = "Wall";
}

public static class Vector3Direction
{
    public static Vector3 Up = new Vector3(0, GameManager.instance.tileSize);
    public static Vector3 Down = new Vector3(0, -GameManager.instance.tileSize);
    public static Vector3 Right = new Vector3(GameManager.instance.tileSize, 0);
    public static Vector3 Left = new Vector3(-GameManager.instance.tileSize, 0);
}

//public static class Direction2
//{
//    public static Vector2Int Up = new Vector2Int(0, 1);
//    public static Vector2Int Down = new Vector2Int(0, -1);
//    public static Vector2Int Right = new Vector2Int(1, 0);
//    public static Vector2Int Left = new Vector2Int(-1, 0);
//}