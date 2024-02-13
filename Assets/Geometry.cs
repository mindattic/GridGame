/*
Adapted from Geometry.cs 
www: https://www.kodeco.com/5441-how-to-make-a-chess-game-with-unity
zip: https://koenig-media.raywenderlich.com/uploads/2018/03/ChessGameInUnity-Project-Materials.zip
*/

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

    static public Vector3 PositionFromLocation(Vector2Int location)
    {
        float x = board.offset.x + (tileSize * location.x);
        float y = board.offset.y + -(tileSize * location.y);
        return new Vector3(x, y, 0);
    }

    ///DEBUG: Does this work?
    static public Vector2Int LocationFromPosition(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x / tileSize - board.offset.x);
        int y = Mathf.FloorToInt(position.y / tileSize - board.offset.y);
        return new Vector2Int(x, y);
    }

    static public TileBehavior ClosestTileByPosition(Vector2 position)
    {
        return tiles
            .OrderBy(x => Vector3.Distance(x.transform.position, position))
            .First();
    }

    static public TileBehavior ClosestTileByLocation(Vector2Int location)
    {
        return tiles
            .First(x => x.location == location);
    }

    //DEBUG: Does this work?...
    static public TileBehavior ClosestUnoccupiedTileByPosition(Vector2 position)
    {
        var unoccupiedLocations = UnoccupiedLocations();
        return tiles
            .Where(x => unoccupiedLocations.Contains(x.location))
            .OrderBy(x => Vector3.Distance(x.transform.position, position))
            .First();
    }

    //DEBUG: Does this work?...
    static public TileBehavior ClosestUnoccupiedTileByLocation(Vector2Int location)
    {
        var unoccupiedLocations = UnoccupiedLocations();
        return tiles
            .Where(x => unoccupiedLocations.Contains(x.location))
            .OrderBy(x => Vector2.Distance(x.location, location))
            .First();
    }

    /// <summary>
    /// Method which is used to retrieve all unoccupied locations
    /// via: https://stackoverflow.com/questions/5620266/the-opposite-of-intersect
    /// </summary>
    static public List<Vector2Int> UnoccupiedLocations()
    {
        List<Vector2Int> occupiedLocations = actors.Select(x => x.location).ToList();
        List<Vector2Int> tileLocations = tiles.Select(x => x.location).ToList();
        List<Vector2Int> unoccupiedLocations 
            = occupiedLocations.Except(tileLocations).Union(tileLocations.Except(occupiedLocations)).ToList();

        return unoccupiedLocations;
    }

    static public ActorBehavior GetActorAtLocation(Vector2Int location)
    {
        return actors.FirstOrDefault(x => x.location == location);
    }

}
