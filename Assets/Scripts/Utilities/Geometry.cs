using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

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


    //public static Vector2Int LocationFromPosition(Vector3 other)
    //{
    //    int x = Mathf.FloorToInt(other.x / tileSize - board.offset.x);
    //    int y = Mathf.FloorToInt(other.y / tileSize - board.offset.y);
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
            .OrderBy(x => Vector2Int.Distance(x.location, other))
            .First();
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
            .FirstOrDefault(x => !x.IsOccupied && Vector2Int.Distance(x.location, other) == 1);
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
        return actors.FirstOrDefault(x => x.location == other);
    }


    public static Vector2Int GetLocation(int col, int row)
    {
        col = Math.Clamp(col, 1, board.columns);
        row = Math.Clamp(row, 1, board.rows);
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
