using UnityEngine;

public static class Tag
{
    public static string Board = "Board";
    public static string Tile = "Tile";
    public static string Actor = "Actor";
    public static string Wall = "Wall";
}

public class Destination
{

    public Destination() { }

    public Destination(Vector2Int location, Vector3 position, Direction direction)
    {

        this.location = location;
        this.position = position;
        this.direction = direction;
    }

    public Vector2Int? location { get; set; }
    public Vector3? position { get; set; }
    public Direction direction { get; set; }


    public bool IsValid => location != null && position != null && direction != Direction.None;

    public void Clear()
    {
        location = null;
        position = null;
        direction = Direction.None;
    }

}
