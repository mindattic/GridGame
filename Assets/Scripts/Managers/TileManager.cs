using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : ExtendedMonoBehavior
{
    public Dictionary<Vector2Int, TileInstance> tileMap = new Dictionary<Vector2Int, TileInstance>();

    public void Reset()
    {
        foreach (var tile in tiles)
        {
            tile.spriteRenderer.color = ColorHelper.Translucent.White;
        }
    }
}
