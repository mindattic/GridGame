using System.Collections.Generic;
using UnityEngine;

public static class Tag
{
    public static string Board = "Board";
    public static string Tile = "Tile";
    public static string Actor = "Actor";
    public static string Line = "Line";
    public static string Trail = "Trail";
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



public class RectVector3
{
    public RectVector3() { }
}


public class RectFloat
{
    public RectFloat() { }
}


public class ActorPair
{
    public ActorPair() { }
    public ActorPair(ActorBehavior actor1, ActorBehavior actor2, Axis axis)
    {
        this.actor1 = actor1;
        this.actor2 = actor2;
        this.axis = axis;
    }

    public ActorBehavior actor1 { get; set; }
    public ActorBehavior actor2 { get; set; }
    public Axis axis { get; set; }
    public int gapCount { get; set; }
}