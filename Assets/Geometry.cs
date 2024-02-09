/*
Adapted from Geometry.cs 
www: https://www.kodeco.com/5441-how-to-make-a-chess-game-with-unity
zip: https://koenig-media.raywenderlich.com/uploads/2018/03/ChessGameInUnity-Project-Materials.zip
*/

using System.Linq;
using UnityEngine;

public class Geometry
{
    private static BoardBehavior board => GameManager.instance.board;
    private static float tileSize => GameManager.instance.tileSize;

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

        return GameManager.instance.tiles
            .OrderBy(x => Vector3.Distance(x.transform.position, position))
            .First();

        //var unoccupiedLocations = GetUnoccupiedLocations();
        //return GameManager.instance.tiles
        //    .Where(x => unoccupiedLocations.Contains(x.location))
        //    .OrderBy(x => Vector3.Distance(x.transform.position, position))
        //    .First();
    }

    static public TileBehavior ClosestTileByLocation(Vector2Int location)
    {
        return GameManager.instance.tiles
            .First(x => x.location == location);

        //var unoccupiedLocations = GetUnoccupiedLocations();
        //return GameManager.instance.tiles
        //    .Where(x => unoccupiedLocations.Contains(x.location))
        //    .First(x => x.location == location);
    }

}
