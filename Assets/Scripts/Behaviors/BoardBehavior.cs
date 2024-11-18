using Game.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Location
{
    public Vector2Int Nowhere;

    public Vector2Int A1;
    public Vector2Int A2;
    public Vector2Int A3;
    public Vector2Int A4;
    public Vector2Int A5;
    public Vector2Int A6;

    public Vector2Int B1;
    public Vector2Int B2;
    public Vector2Int B3;
    public Vector2Int B4;
    public Vector2Int B5;
    public Vector2Int B6;

    public Vector2Int C1;
    public Vector2Int C2;
    public Vector2Int C3;
    public Vector2Int C4;
    public Vector2Int C5;
    public Vector2Int C6;

    public Vector2Int D1;
    public Vector2Int D2;
    public Vector2Int D3;
    public Vector2Int D4;
    public Vector2Int D5;
    public Vector2Int D6;

    public Vector2Int E1;
    public Vector2Int E2;
    public Vector2Int E3;
    public Vector2Int E4;
    public Vector2Int E5;
    public Vector2Int E6;

    public Vector2Int F1;
    public Vector2Int F2;
    public Vector2Int F3;
    public Vector2Int F4;
    public Vector2Int F5;
    public Vector2Int F6;

    public Vector2Int G1;
    public Vector2Int G2;
    public Vector2Int G3;
    public Vector2Int G4;
    public Vector2Int G5;
    public Vector2Int G6;

    public Vector2Int H1;
    public Vector2Int H2;
    public Vector2Int H3;
    public Vector2Int H4;
    public Vector2Int H5;
    public Vector2Int H6;
}

public class Position
{
    public Vector3 Nowhere;

    public Vector3 A1;
    public Vector3 A2;
    public Vector3 A3;
    public Vector3 A4;
    public Vector3 A5;
    public Vector3 A6;

    public Vector3 B1;
    public Vector3 B2;
    public Vector3 B3;
    public Vector3 B4;
    public Vector3 B5;
    public Vector3 B6;

    public Vector3 C1;
    public Vector3 C2;
    public Vector3 C3;
    public Vector3 C4;
    public Vector3 C5;
    public Vector3 C6;

    public Vector3 D1;
    public Vector3 D2;
    public Vector3 D3;
    public Vector3 D4;
    public Vector3 D5;
    public Vector3 D6;

    public Vector3 E1;
    public Vector3 E2;
    public Vector3 E3;
    public Vector3 E4;
    public Vector3 E5;
    public Vector3 E6;

    public Vector3 F1;
    public Vector3 F2;
    public Vector3 F3;
    public Vector3 F4;
    public Vector3 F5;
    public Vector3 F6;

    public Vector3 G1;
    public Vector3 G2;
    public Vector3 G3;
    public Vector3 G4;
    public Vector3 G5;
    public Vector3 G6;

    public Vector3 H1;
    public Vector3 H2;
    public Vector3 H3;
    public Vector3 H4;
    public Vector3 H5;
    public Vector3 H6;
}

public class BoardBehavior : ExtendedMonoBehavior
{
    [SerializeField] public GameObject TilePrefab;
    [HideInInspector] public int columnCount = 6;
    [HideInInspector] public int rowCount = 8;

    [HideInInspector] public Vector2 offset;

    [HideInInspector] public RectFloat bounds;

    [HideInInspector] public Location location;
    [HideInInspector] public Position position;
    [HideInInspector] public Dictionary<Vector2Int, Vector3> locationPosition = new Dictionary<Vector2Int, Vector3>();
    [HideInInspector] public Dictionary<Vector3, Vector2Int> positionLocation = new Dictionary<Vector3, Vector2Int>();

    private void Start()
    {
        offset = new Vector2(-(tileSize * 3) - tileSize / 2, (tileSize * board.columnCount));
        transform.position = offset;

        bounds = new RectFloat();
        bounds.Top = offset.y - tileSize / 2;
        bounds.Right = offset.x + (tileSize * columnCount) + tileSize / 2;
        bounds.Bottom = offset.y - (tileSize * rowCount) - tileSize / 2;
        bounds.Left = offset.x + tileSize / 2;

        location = new Location()
        {
            Nowhere = new Vector2Int(-1, -1),

            A1 = new Vector2Int(1, 1),
            A2 = new Vector2Int(2, 1),
            A3 = new Vector2Int(3, 1),
            A4 = new Vector2Int(4, 1),
            A5 = new Vector2Int(5, 1),
            A6 = new Vector2Int(6, 1),

            B1 = new Vector2Int(1, 2),
            B2 = new Vector2Int(2, 2),
            B3 = new Vector2Int(3, 2),
            B4 = new Vector2Int(4, 2),
            B5 = new Vector2Int(5, 2),
            B6 = new Vector2Int(6, 2),

            C1 = new Vector2Int(1, 3),
            C2 = new Vector2Int(2, 3),
            C3 = new Vector2Int(3, 3),
            C4 = new Vector2Int(4, 3),
            C5 = new Vector2Int(5, 3),
            C6 = new Vector2Int(6, 3),

            D1 = new Vector2Int(1, 4),
            D2 = new Vector2Int(2, 4),
            D3 = new Vector2Int(3, 4),
            D4 = new Vector2Int(4, 4),
            D5 = new Vector2Int(5, 4),
            D6 = new Vector2Int(6, 4),

            E1 = new Vector2Int(1, 5),
            E2 = new Vector2Int(2, 5),
            E3 = new Vector2Int(3, 5),
            E4 = new Vector2Int(4, 5),
            E5 = new Vector2Int(5, 5),
            E6 = new Vector2Int(6, 5),

            F1 = new Vector2Int(1, 6),
            F2 = new Vector2Int(2, 6),
            F3 = new Vector2Int(3, 6),
            F4 = new Vector2Int(4, 6),
            F5 = new Vector2Int(5, 6),
            F6 = new Vector2Int(6, 6),

            G1 = new Vector2Int(1, 7),
            G2 = new Vector2Int(2, 7),
            G3 = new Vector2Int(3, 7),
            G4 = new Vector2Int(4, 7),
            G5 = new Vector2Int(5, 7),
            G6 = new Vector2Int(6, 7),

            H1 = new Vector2Int(1, 8),
            H2 = new Vector2Int(2, 8),
            H3 = new Vector2Int(3, 8),
            H4 = new Vector2Int(4, 8),
            H5 = new Vector2Int(5, 8),
            H6 = new Vector2Int(6, 8)
        };

        position = new Position()
        {
            Nowhere = new Vector3(-1000, -1000),

            A1 = Geometry.CalculatePositionByLocation(location.A1),
            A2 = Geometry.CalculatePositionByLocation(location.A2),
            A3 = Geometry.CalculatePositionByLocation(location.A3),
            A4 = Geometry.CalculatePositionByLocation(location.A4),
            A5 = Geometry.CalculatePositionByLocation(location.A5),
            A6 = Geometry.CalculatePositionByLocation(location.A6),

            B1 = Geometry.CalculatePositionByLocation(location.B1),
            B2 = Geometry.CalculatePositionByLocation(location.B2),
            B3 = Geometry.CalculatePositionByLocation(location.B3),
            B4 = Geometry.CalculatePositionByLocation(location.B4),
            B5 = Geometry.CalculatePositionByLocation(location.B5),
            B6 = Geometry.CalculatePositionByLocation(location.B6),

            C1 = Geometry.CalculatePositionByLocation(location.C1),
            C2 = Geometry.CalculatePositionByLocation(location.C2),
            C3 = Geometry.CalculatePositionByLocation(location.C3),
            C4 = Geometry.CalculatePositionByLocation(location.C4),
            C5 = Geometry.CalculatePositionByLocation(location.C5),
            C6 = Geometry.CalculatePositionByLocation(location.C6),

            D1 = Geometry.CalculatePositionByLocation(location.D1),
            D2 = Geometry.CalculatePositionByLocation(location.D2),
            D3 = Geometry.CalculatePositionByLocation(location.D3),
            D4 = Geometry.CalculatePositionByLocation(location.D4),
            D5 = Geometry.CalculatePositionByLocation(location.D5),
            D6 = Geometry.CalculatePositionByLocation(location.D6),

            E1 = Geometry.CalculatePositionByLocation(location.E1),
            E2 = Geometry.CalculatePositionByLocation(location.E2),
            E3 = Geometry.CalculatePositionByLocation(location.E3),
            E4 = Geometry.CalculatePositionByLocation(location.E4),
            E5 = Geometry.CalculatePositionByLocation(location.E5),
            E6 = Geometry.CalculatePositionByLocation(location.E6),

            F1 = Geometry.CalculatePositionByLocation(location.F1),
            F2 = Geometry.CalculatePositionByLocation(location.F2),
            F3 = Geometry.CalculatePositionByLocation(location.F3),
            F4 = Geometry.CalculatePositionByLocation(location.F4),
            F5 = Geometry.CalculatePositionByLocation(location.F5),
            F6 = Geometry.CalculatePositionByLocation(location.F6),

            G1 = Geometry.CalculatePositionByLocation(location.G1),
            G2 = Geometry.CalculatePositionByLocation(location.G2),
            G3 = Geometry.CalculatePositionByLocation(location.G3),
            G4 = Geometry.CalculatePositionByLocation(location.G4),
            G5 = Geometry.CalculatePositionByLocation(location.G5),
            G6 = Geometry.CalculatePositionByLocation(location.G6),

            H1 = Geometry.CalculatePositionByLocation(location.H1),
            H2 = Geometry.CalculatePositionByLocation(location.H2),
            H3 = Geometry.CalculatePositionByLocation(location.H3),
            H4 = Geometry.CalculatePositionByLocation(location.H4),
            H5 = Geometry.CalculatePositionByLocation(location.H5),
            H6 = Geometry.CalculatePositionByLocation(location.H6)
        };

        locationPosition = new Dictionary<Vector2Int, Vector3>()
        {
            { location.Nowhere, position.Nowhere },

            { location.A1, position.A1 },
            { location.A2, position.A2 },
            { location.A3, position.A3 },
            { location.A4, position.A4 },
            { location.A5, position.A5 },
            { location.A6, position.A6 },

            { location.B1, position.B1 },
            { location.B2, position.B2 },
            { location.B3, position.B3 },
            { location.B4, position.B4 },
            { location.B5, position.B5 },
            { location.B6, position.B6 },

            { location.C1, position.C1 },
            { location.C2, position.C2 },
            { location.C3, position.C3 },
            { location.C4, position.C4 },
            { location.C5, position.C5 },
            { location.C6, position.C6 },

            { location.D1, position.D1 },
            { location.D2, position.D2 },
            { location.D3, position.D3 },
            { location.D4, position.D4 },
            { location.D5, position.D5 },
            { location.D6, position.D6 },

            { location.E1, position.E1 },
            { location.E2, position.E2 },
            { location.E3, position.E3 },
            { location.E4, position.E4 },
            { location.E5, position.E5 },
            { location.E6, position.E6 },

            { location.F1, position.F1 },
            { location.F2, position.F2 },
            { location.F3, position.F3 },
            { location.F4, position.F4 },
            { location.F5, position.F5 },
            { location.F6, position.F6 },

            { location.G1, position.G1 },
            { location.G2, position.G2 },
            { location.G3, position.G3 },
            { location.G4, position.G4 },
            { location.G5, position.G5 },
            { location.G6, position.G6 },

            { location.H1, position.H1 },
            { location.H2, position.H2 },
            { location.H3, position.H3 },
            { location.H4, position.H4 },
            { location.H5, position.H5 },
            { location.H6, position.H6 }
        };

        positionLocation = new Dictionary<Vector3, Vector2Int>()
        {
            { position.Nowhere, location.Nowhere },

            { position.A1, location.A1 },
            { position.A2, location.A2 },
            { position.A3, location.A3 },
            { position.A4, location.A4 },
            { position.A5, location.A5 },
            { position.A6, location.A6 },

            { position.B1, location.B1 },
            { position.B2, location.B2 },
            { position.B3, location.B3 },
            { position.B4, location.B4 },
            { position.B5, location.B5 },
            { position.B6, location.B6 },

            { position.C1, location.C1 },
            { position.C2, location.C2 },
            { position.C3, location.C3 },
            { position.C4, location.C4 },
            { position.C5, location.C5 },
            { position.C6, location.C6 },

            { position.D1, location.D1 },
            { position.D2, location.D2 },
            { position.D3, location.D3 },
            { position.D4, location.D4 },
            { position.D5, location.D5 },
            { position.D6, location.D6 },

            { position.E1, location.E1 },
            { position.E2, location.E2 },
            { position.E3, location.E3 },
            { position.E4, location.E4 },
            { position.E5, location.E5 },
            { position.E6, location.E6 },

            { position.F1, location.F1 },
            { position.F2, location.F2 },
            { position.F3, location.F3 },
            { position.F4, location.F4 },
            { position.F5, location.F5 },
            { position.F6, location.F6 },

            { position.G1, location.G1 },
            { position.G2, location.G2 },
            { position.G3, location.G3 },
            { position.G4, location.G4 },
            { position.G5, location.G5 },
            { position.G6, location.G6 },

            { position.H1, location.H1 },
            { position.H2, location.H2 },
            { position.H3, location.H3 },
            { position.H4, location.H4 },
            { position.H5, location.H5 },
            { position.H6, location.H6 }
        };

        GenerateTiles();

        //Order of Operations:
        //saveFileManager.Load() => stageManager.Load() 
        saveFileManager.Load();
        stageManager.Load();



    }

    void GenerateTiles()
    {
        GameObject prefab;
        TileBehavior tile;

        for (int col = 1; col <= columnCount; col++)
        {
            for (int row = 1; row <= rowCount; row++)
            {
                prefab = Instantiate(TilePrefab, board.transform);
                tile = prefab.GetComponent<TileBehavior>();
                tile.name = $"{col}x{row}";
                tile.location = new Vector2Int(col, row);
            }
        }

        //Assign tiles list
        GameObject.FindGameObjectsWithTag(Tag.Tile).ToList()
            .ForEach(x => GameManager.instance.tiles.Add(x.GetComponent<TileBehavior>()));


    }
}
