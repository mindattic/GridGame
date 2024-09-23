using Game.Models;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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

    [HideInInspector] public Location Location;
    [HideInInspector] public Position Position;
    [HideInInspector] public Dictionary<Vector2Int, Vector3> LocationPosition = new Dictionary<Vector2Int, Vector3>();

    #region Components

    public Vector3 position
    {
        get => gameObject.transform.position;
        set => gameObject.transform.position = value;
    }

    #endregion

    private void Start()
    {
        offset = new Vector2(-(tileSize * 3) - tileSize / 2, (tileSize * board.columnCount));

        bounds = new RectFloat();
        bounds.Top = offset.y - tileSize / 2;
        bounds.Right = offset.x + (tileSize * columnCount) + tileSize / 2;
        bounds.Bottom = offset.y - (tileSize * rowCount) - tileSize / 2;
        bounds.Left = offset.x + tileSize / 2;

        transform.position = offset;

        Location = new Location()
        {
            Nowhere = new Vector2Int(-1, -1),

            A1 = new Vector2Int(1, 1),
            A2 = new Vector2Int(1, 2),
            A3 = new Vector2Int(1, 3),
            A4 = new Vector2Int(1, 4),
            A5 = new Vector2Int(1, 5),
            A6 = new Vector2Int(1, 6),

            B1 = new Vector2Int(2, 1),
            B2 = new Vector2Int(2, 2),
            B3 = new Vector2Int(2, 3),
            B4 = new Vector2Int(2, 4),
            B5 = new Vector2Int(2, 5),
            B6 = new Vector2Int(2, 6),

            C1 = new Vector2Int(3, 1),
            C2 = new Vector2Int(3, 2),
            C3 = new Vector2Int(3, 3),
            C4 = new Vector2Int(3, 4),
            C5 = new Vector2Int(3, 5),
            C6 = new Vector2Int(3, 6),

            D1 = new Vector2Int(4, 1),
            D2 = new Vector2Int(4, 2),
            D3 = new Vector2Int(4, 3),
            D4 = new Vector2Int(4, 4),
            D5 = new Vector2Int(4, 5),
            D6 = new Vector2Int(4, 6),

            E1 = new Vector2Int(5, 1),
            E2 = new Vector2Int(5, 2),
            E3 = new Vector2Int(5, 3),
            E4 = new Vector2Int(5, 4),
            E5 = new Vector2Int(5, 5),
            E6 = new Vector2Int(5, 6),

            F1 = new Vector2Int(6, 1),
            F2 = new Vector2Int(6, 2),
            F3 = new Vector2Int(6, 3),
            F4 = new Vector2Int(6, 4),
            F5 = new Vector2Int(6, 5),
            F6 = new Vector2Int(6, 6),

            G1 = new Vector2Int(7, 1),
            G2 = new Vector2Int(7, 2),
            G3 = new Vector2Int(7, 3),
            G4 = new Vector2Int(7, 4),
            G5 = new Vector2Int(7, 5),
            G6 = new Vector2Int(7, 6),

            H1 = new Vector2Int(8, 1),
            H2 = new Vector2Int(8, 2),
            H3 = new Vector2Int(8, 3),
            H4 = new Vector2Int(8, 4),
            H5 = new Vector2Int(8, 5),
            H6 = new Vector2Int(8, 6)
        };

        Position = new Position()
        {
            Nowhere = new Vector3(-1000, -1000),

            A1 = Geometry.CalculatePositionByLocation(Location.A1),
            A2 = Geometry.CalculatePositionByLocation(Location.A2),
            A3 = Geometry.CalculatePositionByLocation(Location.A3),
            A4 = Geometry.CalculatePositionByLocation(Location.A4),
            A5 = Geometry.CalculatePositionByLocation(Location.A5),
            A6 = Geometry.CalculatePositionByLocation(Location.A6),

            B1 = Geometry.CalculatePositionByLocation(Location.B1),
            B2 = Geometry.CalculatePositionByLocation(Location.B2),
            B3 = Geometry.CalculatePositionByLocation(Location.B3),
            B4 = Geometry.CalculatePositionByLocation(Location.B4),
            B5 = Geometry.CalculatePositionByLocation(Location.B5),
            B6 = Geometry.CalculatePositionByLocation(Location.B6),

            C1 = Geometry.CalculatePositionByLocation(Location.C1),
            C2 = Geometry.CalculatePositionByLocation(Location.C2),
            C3 = Geometry.CalculatePositionByLocation(Location.C3),
            C4 = Geometry.CalculatePositionByLocation(Location.C4),
            C5 = Geometry.CalculatePositionByLocation(Location.C5),
            C6 = Geometry.CalculatePositionByLocation(Location.C6),

            D1 = Geometry.CalculatePositionByLocation(Location.D1),
            D2 = Geometry.CalculatePositionByLocation(Location.D2),
            D3 = Geometry.CalculatePositionByLocation(Location.D3),
            D4 = Geometry.CalculatePositionByLocation(Location.D4),
            D5 = Geometry.CalculatePositionByLocation(Location.D5),
            D6 = Geometry.CalculatePositionByLocation(Location.D6),

            E1 = Geometry.CalculatePositionByLocation(Location.E1),
            E2 = Geometry.CalculatePositionByLocation(Location.E2),
            E3 = Geometry.CalculatePositionByLocation(Location.E3),
            E4 = Geometry.CalculatePositionByLocation(Location.E4),
            E5 = Geometry.CalculatePositionByLocation(Location.E5),
            E6 = Geometry.CalculatePositionByLocation(Location.E6),

            F1 = Geometry.CalculatePositionByLocation(Location.F1),
            F2 = Geometry.CalculatePositionByLocation(Location.F2),
            F3 = Geometry.CalculatePositionByLocation(Location.F3),
            F4 = Geometry.CalculatePositionByLocation(Location.F4),
            F5 = Geometry.CalculatePositionByLocation(Location.F5),
            F6 = Geometry.CalculatePositionByLocation(Location.F6),

            G1 = Geometry.CalculatePositionByLocation(Location.G1),
            G2 = Geometry.CalculatePositionByLocation(Location.G2),
            G3 = Geometry.CalculatePositionByLocation(Location.G3),
            G4 = Geometry.CalculatePositionByLocation(Location.G4),
            G5 = Geometry.CalculatePositionByLocation(Location.G5),
            G6 = Geometry.CalculatePositionByLocation(Location.G6),

            H1 = Geometry.CalculatePositionByLocation(Location.H1),
            H2 = Geometry.CalculatePositionByLocation(Location.H2),
            H3 = Geometry.CalculatePositionByLocation(Location.H3),
            H4 = Geometry.CalculatePositionByLocation(Location.H4),
            H5 = Geometry.CalculatePositionByLocation(Location.H5),
            H6 = Geometry.CalculatePositionByLocation(Location.H6)
        };

        LocationPosition = new Dictionary<Vector2Int, Vector3>()
        {
            { Location.Nowhere, Position.Nowhere },

            { Location.A1, Position.A1 },
            { Location.A2, Position.A2 },
            { Location.A3, Position.A3 },
            { Location.A4, Position.A4 },
            { Location.A5, Position.A5 },
            { Location.A6, Position.A6 },

            { Location.B1, Position.B1 },
            { Location.B2, Position.B2 },
            { Location.B3, Position.B3 },
            { Location.B4, Position.B4 },
            { Location.B5, Position.B5 },
            { Location.B6, Position.B6 },

            { Location.C1, Position.C1 },
            { Location.C2, Position.C2 },
            { Location.C3, Position.C3 },
            { Location.C4, Position.C4 },
            { Location.C5, Position.C5 },
            { Location.C6, Position.C6 },

            { Location.D1, Position.D1 },
            { Location.D2, Position.D2 },
            { Location.D3, Position.D3 },
            { Location.D4, Position.D4 },
            { Location.D5, Position.D5 },
            { Location.D6, Position.D6 },

            { Location.E1, Position.E1 },
            { Location.E2, Position.E2 },
            { Location.E3, Position.E3 },
            { Location.E4, Position.E4 },
            { Location.E5, Position.E5 },
            { Location.E6, Position.E6 },

            { Location.F1, Position.F1 },
            { Location.F2, Position.F2 },
            { Location.F3, Position.F3 },
            { Location.F4, Position.F4 },
            { Location.F5, Position.F5 },
            { Location.F6, Position.F6 },

            { Location.G1, Position.G1 },
            { Location.G2, Position.G2 },
            { Location.G3, Position.G3 },
            { Location.G4, Position.G4 },
            { Location.G5, Position.G5 },
            { Location.G6, Position.G6 },

            { Location.H1, Position.H1 },
            { Location.H2, Position.H2 },
            { Location.H3, Position.H3 },
            { Location.H4, Position.H4 },
            { Location.H5, Position.H5 },
            { Location.H6, Position.H6 }
        };


        GenerateTiles();

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
