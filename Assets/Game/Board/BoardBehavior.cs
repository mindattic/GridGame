using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardBehavior : ExtendedMonoBehavior
{
    public Vector2 offset;
    public int columns = 6;
    public int rows = 8;
    public float top;
    public float right;
    public float bottom;
    public float left;

    void Awake()
    {


        offset = new Vector2(-2.44f, 4f); //TODO: Calculate mathematically...
        top = offset.y - tileSize / 2;
        right = offset.x + (tileSize * columns) + tileSize / 2;
        bottom = offset.y - (tileSize * rows) - tileSize / 2;
        left = offset.x + tileSize / 2;
    }

    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void Set()
    {

    }

    public void ResetBoard()
    {
        selectedPlayer = null;

        tiles.ForEach(x => x.isOccupied = false);

        int i = 0;
        var randomLocation = Common.RandomLocations();     
        actors.ForEach(x => x.Init(randomLocation[i++]));

        lineManager.Reset();

        timer.Set(scale: 1f, start: false);

    }




}
