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

    //public static Vector3 PositionFromLocation(Vector2Int location)
    //{
    //    return tiles.FirstOrDefault(x => x.location.Equals(location)).position;
    //}


    //public static Vector2Int LocationFromPosition(Vector3 position)
    //{
    //    int x = Mathf.FloorToInt(position.x / tileSize - board.offset.x);
    //    int y = Mathf.FloorToInt(position.y / tileSize - board.offset.y);
    //    return new Vector2Int(x, y);
    //}

    public static TileBehavior ClosestTileByPosition(Vector2 position)
    {
        return tiles
            .OrderBy(x => Vector3.Distance(x.transform.position, position))
            .First();
    }

    public static TileBehavior ClosestTileByLocation(Vector2Int location)
    {
        return tiles
            .OrderBy(x => Vector2Int.Distance(x.location, location))
            .First();
    }


    public static TileBehavior RandomUnoccupiedTile()
    {
        var unoccupitedTiles = tiles.Where(x => !x.IsOccupied).ToList();
        var index = Random.Int(0, unoccupitedTiles.Count - 1);
        return unoccupitedTiles[index];
    }


    public static TileBehavior ClosestUnoccupiedTileByLocation(Vector2Int location)
    {
        return tiles
            .Where(x => !x.IsOccupied)
            .FirstOrDefault(x => Vector2Int.Distance(x.location, location) == 1);
    }

    public static ActorBehavior GetActorAtLocation(Vector2Int location)
    {
        return actors.FirstOrDefault(x => x.location.Equals(location));
    }


    public static Vector2Int GetLocation(int col, int row)
    {
        col = Math.Clamp(col, 1, board.columns);
        row = Math.Clamp(row, 1, board.rows);
        return new Vector2Int(col, row);
    }



}
