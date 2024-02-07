using UnityEngine;

public static class Tag
{
    public static string Board = "Board";
    public static string Tile = "Tile";
    public static string Actor = "Actor";
    public static string Wall = "Wall";
}

public static class Direction
{
    public static Vector3 Up = new Vector3(0, GameManager.instance.tileSize);
    public static Vector3 Down = new Vector3(0, -GameManager.instance.tileSize);
    public static Vector3 Right = new Vector3(GameManager.instance.tileSize, 0);
    public static Vector3 Left = new Vector3(-GameManager.instance.tileSize, 0);
}