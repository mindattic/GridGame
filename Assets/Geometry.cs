/*
Adapted from Geometry.cs 
www: https://www.kodeco.com/5441-how-to-make-a-chess-game-with-unity
zip: https://koenig-media.raywenderlich.com/uploads/2018/03/ChessGameInUnity-Project-Materials.zip
*/

using UnityEngine;

public class Geometry
{
    static private Vector2 boardOffset => GameManager.instance.boardOffset;
    static private float tileSize => GameManager.instance.tileSize;

    static public Vector3 PositionFromLocation(Vector2Int location)
    {
        float x = boardOffset.x + (tileSize * location.x);
        float y = boardOffset.y + -(tileSize * location.y);
        return new Vector3(x, y, 0);
    }

    ///DEBUG: Does this work?
    static public Vector2Int LocationFromPosition(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x / tileSize - boardOffset.x);
        int y = Mathf.FloorToInt(position.y / tileSize - boardOffset.y);
        return new Vector2Int(x, y);
    }

}
