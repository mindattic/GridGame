using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : ExtendedMonoBehavior
{
    // Start is called before the first Frame update
    void Start()
    {

    }

    // Update is called once per Frame
    void Update()
    {

    }


    public void Reset()
    {
        foreach (var tile in Tiles)
        {
            tile.spriteRenderer.color = Colors.Translucent.White;
        }
    }
}
