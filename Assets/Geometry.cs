using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Geometry
{
    private static BoardBehavior board => GameManager.instance.board;
    private static float tileSize => GameManager.instance.tileSize;
    private static List<ActorBehavior> actors => GameManager.instance.actors;
    private static List<TileBehavior> tiles => GameManager.instance.tiles;

    static public Vector3 PositionFromLocation(Vector2Int location)
    {
        float x = board.offset.x + (tileSize * location.x);
        float y = board.offset.y + -(tileSize * location.y);
        return new Vector3(x, y, 0);
    }

    //static public Vector3 PositionFromLocation(Vector2Int location)
    //{
    //    return tiles.FirstOrDefault(x => x.location.Equals(location)).position;
    //}

 
    //static public Vector2Int LocationFromPosition(Vector3 position)
    //{
    //    int x = Mathf.FloorToInt(position.x / tileSize - board.offset.x);
    //    int y = Mathf.FloorToInt(position.y / tileSize - board.offset.y);
    //    return new Vector2Int(x, y);
    //}

    static public TileBehavior ClosestTileByPosition(Vector2 position)
    {
        return tiles
            .OrderBy(x => Vector3.Distance(x.transform.position, position))
            .First();
    }

    static public TileBehavior ClosestTileByLocation(Vector2Int location)
    {
        return tiles
            .OrderBy(x => Vector2Int.Distance(x.location, location))
            .First();
    }

    static public TileBehavior ClosestUnoccupiedTileByLocation(Vector2Int location)
    {
        return tiles
            .Where(x => !x.isOccupied)
            .First(x => Vector2Int.Distance(x.location, location) == 1);
    }

    static public ActorBehavior GetActorAtLocation(Vector2Int location)
    {
        return actors.FirstOrDefault(x => x.location.Equals(location));
    }




}
