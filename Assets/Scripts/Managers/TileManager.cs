using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    protected List<TileInstance> tiles => GameManager.instance.tiles;



    public void Reset()
    {
        foreach (var tile in tiles)
        {
            tile.spriteRenderer.color = ColorHelper.Translucent.White;
        }
    }
}
