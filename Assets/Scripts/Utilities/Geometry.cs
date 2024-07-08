using Game.Behaviors.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Geometry
{
    private static BoardBehavior board => GameManager.instance.board;
    private static float tileSize => GameManager.instance.tileSize;
    private static List<ActorBehavior> actors => GameManager.instance.actors;
    private static List<TileBehavior> tiles => GameManager.instance.tiles;

    public static Vector3 PositionFromLocation(Vector2Int location)
    {
        float x = board.offset.x + (tileSize * location.x);
        float y = board.offset.y + -(tileSize * location.y);
        return new Vector3(x, y, 0);
    }

    public static Vector2Int GetLocation(int col, int row)
    {
        col = Math.Clamp(col, 1, board.columnCount);
        row = Math.Clamp(row, 1, board.rowCount);
        return new Vector2Int(col, row);
    }

    //public static Vector2Int LocationFromPosition(Vector3 location)
    //{
    //    int x = Mathf.FloorToInt(location.x / tileSize - board.offset.x);
    //    int y = Mathf.FloorToInt(location.y / tileSize - board.offset.y);
    //    return new Vector2Int(x, y);
    //}

    public static TileBehavior ClosestTile(Vector2 other)
    {
        return tiles.OrderBy(x => Vector3.Distance(x.transform.position, other)).First();
    }

    public static TileBehavior ClosestTile(Vector2Int location)
    {
        return tiles.OrderBy(x => Vector2Int.Distance(x.location, location)).First();
    }


    public static Vector3 ClosestTilePosition(Vector2Int location)
    {
        return tiles.OrderBy(x => Vector2Int.Distance(x.location, location)).First().position;
    }





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

    public static Vector3 ClosestAttackPosition(ActorBehavior attacker, ActorBehavior other)
    {
        //Determine if already adjacent to player...
        if (IsAdjacentTo(attacker, other))
            return attacker.position;

        //Swap position with target
        return other.position;



        /*


        //...Otherwise, Find closest unoccupied tile adjacent to player...
        var closestUnoccupiedAdjacentTile = ClosestUnoccupiedAdjacentTileByLocation(other.location);
        if (closestUnoccupiedAdjacentTile != null)
            return closestUnoccupiedAdjacentTile.position;

        //...Otherwise, Find closest tile adjacent to player...
        var closestAdjacentTile = ClosestAdjacentTileByLocation(other.location);
        if (closestAdjacentTile != null)
            return closestAdjacentTile.position;

        //...Otherwise, find closest unoccupied tile to player...
        var closestUnoccupiedTile = ClosestUnoccupiedTileByLocation(other.location);
        if (closestUnoccupiedTile != null)
            return closestUnoccupiedTile.position;

        //...Otherwise, find closest tile to player
        var closestTile = ClosestTile(other.location);
        if (closestTile != null)
            return closestTile.position;

        return attacker.position;
        */
    }

    public static TileBehavior ClosestUnoccupiedTileByLocation(Vector2Int other)
    {
        return tiles.FirstOrDefault(x => !x.IsOccupied && Vector2Int.Distance(x.location, other) == 1);
    }

    public static TileBehavior ClosestUnoccupiedAdjacentTileByLocation(Vector2Int other)
    {
        return tiles.FirstOrDefault(x => !x.IsOccupied && x.IsAdjacentTo(other));
    }

    public static TileBehavior ClosestAdjacentTileByLocation(Vector2Int other)
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
  


}
