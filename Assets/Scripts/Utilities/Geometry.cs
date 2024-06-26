using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Geometry
{
    private static BoardBehavior board => GameManager.instance.Board;
    private static float tileSize => GameManager.instance.TileSize;
    private static List<ActorBehavior> actors => GameManager.instance.Actors;
    private static List<TileBehavior> tiles => GameManager.instance.Tiles;

    public static Vector3 PositionFromLocation(Vector2Int location)
    {
        float x = board.Offset.x + (tileSize * location.x);
        float y = board.Offset.y + -(tileSize * location.y);
        return new Vector3(x, y, 0);
    }


    //public static Vector2Int LocationFromPosition(Vector3 other)
    //{
    //    int x = Mathf.FloorToInt(other.x / TileSize - Board.Offset.x);
    //    int y = Mathf.FloorToInt(other.y / TileSize - Board.Offset.y);
    //    return new Vector2Int(x, y);
    //}

    public static TileBehavior ClosestTileByPosition(Vector2 other)
    {
        return tiles
            .OrderBy(x => Vector3.Distance(x.transform.position, other))
            .First();
    }

    public static TileBehavior ClosestTileByLocation(Vector2Int other)
    {
        return tiles
            .OrderBy(x => Vector2Int.Distance(x.Location, other))
            .First();
    }


    public static bool IsSameColumn(ActorBehavior a, ActorBehavior b) => a.Location.x == b.Location.x;
    public static bool IsSameRow(ActorBehavior a, ActorBehavior b) => a.Location.y == b.Location.y;
    public static bool IsNorthOf(ActorBehavior a, ActorBehavior b) => IsSameColumn(a, b) && a.Location.y == b.Location.y - 1;
    public static bool IsEastOf(ActorBehavior a, ActorBehavior b) => IsSameRow(a, b) && a.Location.x == b.Location.x + 1;
    public static bool IsSouthOf(ActorBehavior a, ActorBehavior b) => IsSameColumn(a, b) && a.Location.y == b.Location.y + 1;
    public static bool IsWestOf(ActorBehavior a, ActorBehavior b) => IsSameRow(a, b) && a.Location.x == b.Location.x - 1;
    public static bool IsNorthWestOf(ActorBehavior a, ActorBehavior b) => a.Location.x == b.Location.x - 1 && a.Location.y == b.Location.y - 1;
    public static bool IsNorthEastOf(ActorBehavior a, ActorBehavior b) => a.Location.x == b.Location.x + 1 && a.Location.y == b.Location.y - 1;
    public static bool IsSouthWestOf(ActorBehavior a, ActorBehavior b) => a.Location.x == b.Location.x - 1 && a.Location.y == b.Location.y + 1;
    public static bool IsSouthEastOf(ActorBehavior a, ActorBehavior b) => a.Location.x == b.Location.x + 1 && a.Location.y == b.Location.y + 1;
    public static bool IsAdjacentTo(ActorBehavior a, ActorBehavior b) => (IsSameColumn(a, b) || IsSameRow(a, b)) && Vector2Int.Distance(a.Location, a.Location).Equals(1);

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

        //...Otherwise, Find closest unoccupied tile adjacent to player...
        var closestUnoccupiedAdjacentTile = ClosestUnoccupiedAdjacentTileByLocation(other.Location);
        if (closestUnoccupiedAdjacentTile != null)
            return closestUnoccupiedAdjacentTile.position;

        //...Otherwise, Find closest tile adjacent to player...
        var closestAdjacentTile = ClosestAdjacentTileByLocation(other.Location);
        if (closestAdjacentTile != null)
            return closestAdjacentTile.position;

        //...Otherwise, find closest unoccupied tile to player...
        var closestUnoccupiedTile = ClosestUnoccupiedTileByLocation(other.Location);
        if (closestUnoccupiedTile != null)
            return closestUnoccupiedTile.position;

        //...Otherwise, find closest tile to player
        var closestTile = ClosestTileByLocation(other.Location);
        if (closestTile != null)
            return closestTile.position;

        return attacker.position;
    }


    public static TileBehavior RandomUnoccupiedTile()
    {
        var unoccupitedTiles = tiles.Where(x => !x.IsOccupied).ToList();
        var index = Random.Int(0, unoccupitedTiles.Count - 1);
        return unoccupitedTiles[index];
    }


    public static TileBehavior ClosestUnoccupiedTileByLocation(Vector2Int other)
    {
        return tiles
            .FirstOrDefault(x => !x.IsOccupied && Vector2Int.Distance(x.Location, other) == 1);
    }


    public static TileBehavior ClosestUnoccupiedAdjacentTileByLocation(Vector2Int other)
    {
        return tiles
            .FirstOrDefault(x => !x.IsOccupied && x.IsAdjacentTo(other));
    }

    public static TileBehavior ClosestAdjacentTileByLocation(Vector2Int other)
    {
        return tiles
            .FirstOrDefault(x => x.IsAdjacentTo(other));
    }

    public static ActorBehavior GetActorAtLocation(Vector2Int other)
    {
        return actors.FirstOrDefault(x => x.Location == other);
    }


    public static Vector2Int GetLocation(int col, int row)
    {
        col = Math.Clamp(col, 1, board.ColumnCount);
        row = Math.Clamp(row, 1, board.RowCount);
        return new Vector2Int(col, row);
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
