using Game.Behaviors;
using Game.Behaviors.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Geometry
{
    private static BoardBehavior board => GameManager.instance.board;
    private static float tileSize => GameManager.instance.tileSize;
    private static Vector3 tileScale => GameManager.instance.tileScale;
    private static List<ActorBehavior> actors => GameManager.instance.actors;
    private static List<TileBehavior> tiles => GameManager.instance.tiles;

    //private static Dictionary<Vector2Int, Vector3> boardPositions = new Dictionary<Vector2Int, Vector3>();
    //private static Dictionary<Vector3, Vector2Int> boardLocations = new Dictionary<Vector3, Vector2Int>();


    public Geometry()
    {

        //Assign lookup dictionaries
        //tiles.ForEach(x => boardPositions.Add(x.location, x.position));
        //tiles.ForEach(x => boardLocations.Add(x.position, x.location));
    }


    public static Vector3 CalculatePositionByLocation(Vector2Int location)
    {
        //return boardPositions[location];
        float x = board.offset.x + (tileSize * location.x);
        float y = board.offset.y + -(tileSize * location.y);
        return new Vector3(x, y, 0);
    }

  


    public static Vector3 GetPositionByLocation(Vector2Int location)
    {
        return board.LocationPosition[location];
    }

    //public static Vector2Int GetLocation(int col, int row)
    //{
    //    col = Math.Clamp(col, 1, board.columnCount);
    //    row = Math.Clamp(row, 1, board.rowCount);
    //    return new Vector2Int(col, row);
    //}

    //public static Vector2Int LocationFromPosition(Vector3 location)
    //{
    //    int x = Mathf.FloorToInt(location.x / tileSize - board.relativeOffset.x);
    //    int y = Mathf.FloorToInt(location.y / tileSize - board.relativeOffset.y);
    //    return new Vector2Int(x, y);
    //}

    public static TileBehavior GetClosestTileByPosition(Vector3 position)
    {
        return tiles.OrderBy(x => Vector3.Distance(x.transform.position, position)).First();
    }

    public static TileBehavior GetClosestTileByLocation(Vector2Int location)
    {
        return tiles.OrderBy(x => Vector2Int.Distance(x.location, location)).First();
    }


    //public static Vector3 ClosestTilePosition(Vector2Int location)
    //{
    //    return tiles.OrderBy(x => Vector2Int.Distance(x.location, location)).First().position;
    //}





    public static bool IsSameColumn(ActorBehavior a, ActorBehavior b) => a.location.x == b.location.x;
    public static bool IsSameRow(ActorBehavior a, ActorBehavior b) => a.location.y == b.location.y;
    public static bool IsNorthOf(ActorBehavior a, ActorBehavior b) => IsSameColumn(a, b) && a.location.y == b.location.y - 1;
    public static bool IsEastOf(ActorBehavior a, ActorBehavior b) => IsSameRow(a, b) && a.location.x == b.location.x + 1;
    public static bool IsSouthOf(ActorBehavior a, ActorBehavior b) => IsSameColumn(a, b) && a.location.y == b.location.y + 1;
    public static bool IsWestOf(ActorBehavior a, ActorBehavior b) => IsSameRow(a, b) && a.location.x == b.location.x - 1;
    public static bool IsNorthWestOf(ActorBehavior a, ActorBehavior b) => a.location.x == b.location.x - 1 && a.location.y == b.location.y - 1;
    public static bool IsNorthEastOf(ActorBehavior a, ActorBehavior b) => a.location.x == b.location.x + 1 && a.location.y == b.location.y - 1;
    public static bool IsSouthWestOf(ActorBehavior a, ActorBehavior b) => a.location.x == b.location.x - 1 && a.location.y == b.location.y + 1;
    public static bool IsSouthEastOf(ActorBehavior a, ActorBehavior b) => a.location.x == b.location.x + 1 && a.location.y == b.location.y + 1;
    public static bool IsAdjacentTo(ActorBehavior a, ActorBehavior b) => (IsSameColumn(a, b) || IsSameRow(a, b)) && Vector2Int.Distance(a.location, a.location).Equals(1);

    public static Direction AdjacentDirectionTo(ActorBehavior a, ActorBehavior b)
    {
        if (!IsAdjacentTo(a, b)) return Direction.None;
        if (IsNorthOf(a, b)) return Direction.South;
        if (IsEastOf(a, b)) return Direction.West;
        if (IsSouthOf(a, b)) return Direction.North;
        if (IsWestOf(a, b)) return Direction.East;

        return Direction.None;
    }

    public static Vector3 GetClosestAttackPosition(ActorBehavior attacker, ActorBehavior other)
    {
        //Determine if already adjacent to player...
        if (IsAdjacentTo(attacker, other))
            return attacker.currentTile.position;

        //Swap position with target
        return other.currentTile.position;



        /*


        //...Otherwise, Find closest unoccupied tile adjacent to player...
        var closestUnoccupiedAdjacentTile = GetClosestUnoccupiedAdjacentTileByLocation(position.location);
        if (closestUnoccupiedAdjacentTile != null)
            return closestUnoccupiedAdjacentTile.position;

        //...Otherwise, Find closest tile adjacent to player...
        var closestAdjacentTile = GetClosestAdjacentTileByLocation(position.location);
        if (closestAdjacentTile != null)
            return closestAdjacentTile.position;

        //...Otherwise, find closest unoccupied tile to player...
        var closestUnoccupiedTile = GetClosestUnoccupiedTileByLocation(position.location);
        if (closestUnoccupiedTile != null)
            return closestUnoccupiedTile.position;

        //...Otherwise, find closest tile to player
        var closestTile = GetClosestTileByPosition(position.location);
        if (closestTile != null)
            return closestTile.position;

        return attacker.position;
        */
    }

    public static TileBehavior GetClosestUnoccupiedTileByLocation(Vector2Int other)
    {
        return tiles.FirstOrDefault(x => !x.IsOccupied && Vector2Int.Distance(x.location, other) == 1);
    }

    public static TileBehavior GetClosestUnoccupiedAdjacentTileByLocation(Vector2Int other)
    {
        return tiles.FirstOrDefault(x => !x.IsOccupied && x.IsAdjacentTo(other));
    }

    public static TileBehavior GetClosestAdjacentTileByLocation(Vector2Int other)
    {
        return tiles.FirstOrDefault(x => x.IsAdjacentTo(other));
    }

    public static Vector3 GetDirectionalPosition(Vector3 position, Direction direction, float amount)
    {
        return direction switch
        {
            Direction.North => new Vector3(position.x, position.y + amount, position.z),
            Direction.East => new Vector3(position.x + amount, position.y, position.z),
            Direction.South => new Vector3(position.x, position.y + -amount, position.z),
            Direction.West => new Vector3(position.x + -amount, position.y, position.z),
            _ => position,
        };
    }

    public static bool IsInCorner(Vector2Int location)
    {
        return location == board.Location.A1 
            || location == board.Location.A6 
            || location == board.Location.H1 
            || location == board.Location.H6;
    }


    public static float GetPercentageBetween(Vector3 start, Vector3 end, Vector3 current)
    {
        //Calculate the vectors
        Vector3 AB = end - start;
        Vector3 AC = current - start;

        //Check for division by zero; Handle the case where start and end are the same point
        if (AB.magnitude == 0)
            return 0;

        //Calculate the percentage along the line segment
        float percentage = AC.magnitude / AB.magnitude;
        return percentage;
    }

    /// <summary>
    /// Methods which calculate values relative to another unit
    /// (which is calculated based on current device aspect ratio, screen size, etc)
    /// </summary>
    public static class Tile
    {

        public static class Relative
        {
            public static Vector3 Translation(float x, float y, float z)
            {
                return new Vector3(
                    tileSize * (x / tileSize),
                    tileSize * (y / tileSize),
                    tileSize * (z / tileSize));
            }
            public static Vector3 Translation(Vector3 v) => Translation(v.x, v.y, v.z);

            public static Vector3 Scale(float x, float y, float z)
            {
                return new Vector3(
                    tileSize * (x / tileSize),
                    tileSize * (y / tileSize),
                    tileSize * (z / tileSize));
            }
            public static Vector3 Scale(Vector3 v) => Scale(v.x, v.y, v.z);

        }

    }

    public static Quaternion Rotation(float x, float y, float z)
    {
        return Quaternion.Euler(new Vector3(x, y, z));
    }
    public static Quaternion Rotation(Vector3 v) => Rotation(v.x, v.y, v.z);

}
