using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Coordinates
{
    public Coordinates() { }
    public Coordinates(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public int x { get; set; }
    public int y { get; set; }
}

public static class Tag
{
    public static string Player = "Player";
    public static string Grid = "Grid";
    public static string Cell = "Cell";
    public static string Wall = "Wall";
}

public enum CollisionDirection
{
    None = 0,
    Top = 1,
    Right = 2,
    Bottom = 3,
    Left = 4
}

public class GameBehaviour : MonoBehaviour
{
    public Coordinates coordinates;


    public void SetCoordinates(Coordinates coordinates)
    {
        this.coordinates = coordinates;
    }
    public void SetCoordinates(int x, int y)
    {
        this.coordinates = new Coordinates(x, y);
    }

}

