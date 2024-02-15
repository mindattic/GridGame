using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardBehavior : ExtendedMonoBehavior
{
    public Vector2 offset = new Vector2(-2.44f, 4f); //TODO: Calculate mathematically...
    public int columns = 6;
    public int rows = 8;
    public float top;
    public float right;
    public float bottom;
    public float left;

    void Awake()
    {

    }

    void Start()
    {
        top = offset.y - tileSize / 2;
        right = offset.x + (tileSize * columns) + tileSize / 2;
        bottom = offset.y - (tileSize * rows) - tileSize / 2;
        left = offset.x + tileSize / 2;
    }

    // Update is called once per frame
    void Update()
    {

    }



    public void ResetBoard()
    {
        selectedPlayer = null;

        tiles.ForEach(x => x.isOccupied = false);

        var locations = Common.GenerateUniqueLocations(10);
        actors.First(x => x.name == "Sentinel").Init(locations[0]);
        actors.First(x => x.name == "Corsair").Init(locations[1]);
        actors.First(x => x.name == "Oracle").Init(locations[2]);
        actors.First(x => x.name == "Mechanic").Init(locations[3]);
        actors.First(x => x.name == "Slime A").Init(locations[4]);
        actors.First(x => x.name == "Slime B").Init(locations[5]);
        actors.First(x => x.name == "Slime C").Init(locations[6]);
        actors.First(x => x.name == "Slime D").Init(locations[7]);
        actors.First(x => x.name == "Slime E").Init(locations[8]);
        actors.First(x => x.name == "Slime F").Init(locations[9]);

        timer.Set(scale: 1f, start: false);

    }

    private HashSet<Vector2Int> allLocations = new HashSet<Vector2Int>()
    {
        new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(1, 2), new Vector2Int(1, 3), new Vector2Int(1, 4), new Vector2Int(1, 5), new Vector2Int(1, 6),
        new Vector2Int(2, 0), new Vector2Int(2, 1), new Vector2Int(2, 2), new Vector2Int(2, 3), new Vector2Int(2, 4), new Vector2Int(2, 5), new Vector2Int(2, 6),
        new Vector2Int(3, 0), new Vector2Int(3, 1), new Vector2Int(3, 2), new Vector2Int(3, 3), new Vector2Int(3, 4), new Vector2Int(3, 5), new Vector2Int(3, 6),
        new Vector2Int(4, 0), new Vector2Int(4, 1), new Vector2Int(4, 2), new Vector2Int(4, 3), new Vector2Int(4, 4), new Vector2Int(4, 5), new Vector2Int(4, 6),
        new Vector2Int(5, 0), new Vector2Int(5, 1), new Vector2Int(5, 2), new Vector2Int(5, 3), new Vector2Int(5, 4), new Vector2Int(5, 5), new Vector2Int(5, 6),
        new Vector2Int(6, 0), new Vector2Int(6, 1), new Vector2Int(6, 2), new Vector2Int(6, 3), new Vector2Int(6, 4), new Vector2Int(6, 5), new Vector2Int(6, 6),
        new Vector2Int(7, 0), new Vector2Int(7, 1), new Vector2Int(7, 2), new Vector2Int(7, 3), new Vector2Int(7, 4), new Vector2Int(7, 5), new Vector2Int(7, 6),
        new Vector2Int(8, 0), new Vector2Int(8, 1), new Vector2Int(8, 2), new Vector2Int(8, 3), new Vector2Int(8, 4), new Vector2Int(8, 5), new Vector2Int(8, 6)
    };


}
