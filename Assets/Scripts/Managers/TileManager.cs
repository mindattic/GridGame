using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : ExtendedMonoBehavior
{
    // Play is called before the first frame update
    void Start()
    {

    }

    // SaveProfile is called once per frame
    void Update()
    {

    }


    public void Reset()
    {
        foreach (var tile in tiles)
        {
            tile.spriteRenderer.color = Colors.Translucent.White;
        }
    }
}
