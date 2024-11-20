using Game.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class BoardLocation
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

public class BoardPosition
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

public class BoardInstance : ExtendedMonoBehavior
{
    [SerializeField] public GameObject TilePrefab;
    [HideInInspector] public int columnCount = 6;
    [HideInInspector] public int rowCount = 8;

    [HideInInspector] public Vector2 offset;

    [HideInInspector] public RectFloat bounds;

    [HideInInspector] public BoardLocation boardLocation;
    [HideInInspector] public BoardPosition boardPosition;
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

        boardLocation = new BoardLocation()
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

        boardPosition = new BoardPosition()
        {
            Nowhere = new Vector3(-1000, -1000),

            A1 = Geometry.CalculatePositionByLocation(boardLocation.A1),
            A2 = Geometry.CalculatePositionByLocation(boardLocation.A2),
            A3 = Geometry.CalculatePositionByLocation(boardLocation.A3),
            A4 = Geometry.CalculatePositionByLocation(boardLocation.A4),
            A5 = Geometry.CalculatePositionByLocation(boardLocation.A5),
            A6 = Geometry.CalculatePositionByLocation(boardLocation.A6),

            B1 = Geometry.CalculatePositionByLocation(boardLocation.B1),
            B2 = Geometry.CalculatePositionByLocation(boardLocation.B2),
            B3 = Geometry.CalculatePositionByLocation(boardLocation.B3),
            B4 = Geometry.CalculatePositionByLocation(boardLocation.B4),
            B5 = Geometry.CalculatePositionByLocation(boardLocation.B5),
            B6 = Geometry.CalculatePositionByLocation(boardLocation.B6),

            C1 = Geometry.CalculatePositionByLocation(boardLocation.C1),
            C2 = Geometry.CalculatePositionByLocation(boardLocation.C2),
            C3 = Geometry.CalculatePositionByLocation(boardLocation.C3),
            C4 = Geometry.CalculatePositionByLocation(boardLocation.C4),
            C5 = Geometry.CalculatePositionByLocation(boardLocation.C5),
            C6 = Geometry.CalculatePositionByLocation(boardLocation.C6),

            D1 = Geometry.CalculatePositionByLocation(boardLocation.D1),
            D2 = Geometry.CalculatePositionByLocation(boardLocation.D2),
            D3 = Geometry.CalculatePositionByLocation(boardLocation.D3),
            D4 = Geometry.CalculatePositionByLocation(boardLocation.D4),
            D5 = Geometry.CalculatePositionByLocation(boardLocation.D5),
            D6 = Geometry.CalculatePositionByLocation(boardLocation.D6),

            E1 = Geometry.CalculatePositionByLocation(boardLocation.E1),
            E2 = Geometry.CalculatePositionByLocation(boardLocation.E2),
            E3 = Geometry.CalculatePositionByLocation(boardLocation.E3),
            E4 = Geometry.CalculatePositionByLocation(boardLocation.E4),
            E5 = Geometry.CalculatePositionByLocation(boardLocation.E5),
            E6 = Geometry.CalculatePositionByLocation(boardLocation.E6),

            F1 = Geometry.CalculatePositionByLocation(boardLocation.F1),
            F2 = Geometry.CalculatePositionByLocation(boardLocation.F2),
            F3 = Geometry.CalculatePositionByLocation(boardLocation.F3),
            F4 = Geometry.CalculatePositionByLocation(boardLocation.F4),
            F5 = Geometry.CalculatePositionByLocation(boardLocation.F5),
            F6 = Geometry.CalculatePositionByLocation(boardLocation.F6),

            G1 = Geometry.CalculatePositionByLocation(boardLocation.G1),
            G2 = Geometry.CalculatePositionByLocation(boardLocation.G2),
            G3 = Geometry.CalculatePositionByLocation(boardLocation.G3),
            G4 = Geometry.CalculatePositionByLocation(boardLocation.G4),
            G5 = Geometry.CalculatePositionByLocation(boardLocation.G5),
            G6 = Geometry.CalculatePositionByLocation(boardLocation.G6),

            H1 = Geometry.CalculatePositionByLocation(boardLocation.H1),
            H2 = Geometry.CalculatePositionByLocation(boardLocation.H2),
            H3 = Geometry.CalculatePositionByLocation(boardLocation.H3),
            H4 = Geometry.CalculatePositionByLocation(boardLocation.H4),
            H5 = Geometry.CalculatePositionByLocation(boardLocation.H5),
            H6 = Geometry.CalculatePositionByLocation(boardLocation.H6)
        };

        locationPosition = new Dictionary<Vector2Int, Vector3>()
        {
            { boardLocation.Nowhere, boardPosition.Nowhere },

            { boardLocation.A1, boardPosition.A1 },
            { boardLocation.A2, boardPosition.A2 },
            { boardLocation.A3, boardPosition.A3 },
            { boardLocation.A4, boardPosition.A4 },
            { boardLocation.A5, boardPosition.A5 },
            { boardLocation.A6, boardPosition.A6 },

            { boardLocation.B1, boardPosition.B1 },
            { boardLocation.B2, boardPosition.B2 },
            { boardLocation.B3, boardPosition.B3 },
            { boardLocation.B4, boardPosition.B4 },
            { boardLocation.B5, boardPosition.B5 },
            { boardLocation.B6, boardPosition.B6 },

            { boardLocation.C1, boardPosition.C1 },
            { boardLocation.C2, boardPosition.C2 },
            { boardLocation.C3, boardPosition.C3 },
            { boardLocation.C4, boardPosition.C4 },
            { boardLocation.C5, boardPosition.C5 },
            { boardLocation.C6, boardPosition.C6 },

            { boardLocation.D1, boardPosition.D1 },
            { boardLocation.D2, boardPosition.D2 },
            { boardLocation.D3, boardPosition.D3 },
            { boardLocation.D4, boardPosition.D4 },
            { boardLocation.D5, boardPosition.D5 },
            { boardLocation.D6, boardPosition.D6 },

            { boardLocation.E1, boardPosition.E1 },
            { boardLocation.E2, boardPosition.E2 },
            { boardLocation.E3, boardPosition.E3 },
            { boardLocation.E4, boardPosition.E4 },
            { boardLocation.E5, boardPosition.E5 },
            { boardLocation.E6, boardPosition.E6 },

            { boardLocation.F1, boardPosition.F1 },
            { boardLocation.F2, boardPosition.F2 },
            { boardLocation.F3, boardPosition.F3 },
            { boardLocation.F4, boardPosition.F4 },
            { boardLocation.F5, boardPosition.F5 },
            { boardLocation.F6, boardPosition.F6 },

            { boardLocation.G1, boardPosition.G1 },
            { boardLocation.G2, boardPosition.G2 },
            { boardLocation.G3, boardPosition.G3 },
            { boardLocation.G4, boardPosition.G4 },
            { boardLocation.G5, boardPosition.G5 },
            { boardLocation.G6, boardPosition.G6 },

            { boardLocation.H1, boardPosition.H1 },
            { boardLocation.H2, boardPosition.H2 },
            { boardLocation.H3, boardPosition.H3 },
            { boardLocation.H4, boardPosition.H4 },
            { boardLocation.H5, boardPosition.H5 },
            { boardLocation.H6, boardPosition.H6 }
        };

        positionLocation = new Dictionary<Vector3, Vector2Int>()
        {
            { boardPosition.Nowhere, boardLocation.Nowhere },

            { boardPosition.A1, boardLocation.A1 },
            { boardPosition.A2, boardLocation.A2 },
            { boardPosition.A3, boardLocation.A3 },
            { boardPosition.A4, boardLocation.A4 },
            { boardPosition.A5, boardLocation.A5 },
            { boardPosition.A6, boardLocation.A6 },

            { boardPosition.B1, boardLocation.B1 },
            { boardPosition.B2, boardLocation.B2 },
            { boardPosition.B3, boardLocation.B3 },
            { boardPosition.B4, boardLocation.B4 },
            { boardPosition.B5, boardLocation.B5 },
            { boardPosition.B6, boardLocation.B6 },

            { boardPosition.C1, boardLocation.C1 },
            { boardPosition.C2, boardLocation.C2 },
            { boardPosition.C3, boardLocation.C3 },
            { boardPosition.C4, boardLocation.C4 },
            { boardPosition.C5, boardLocation.C5 },
            { boardPosition.C6, boardLocation.C6 },

            { boardPosition.D1, boardLocation.D1 },
            { boardPosition.D2, boardLocation.D2 },
            { boardPosition.D3, boardLocation.D3 },
            { boardPosition.D4, boardLocation.D4 },
            { boardPosition.D5, boardLocation.D5 },
            { boardPosition.D6, boardLocation.D6 },

            { boardPosition.E1, boardLocation.E1 },
            { boardPosition.E2, boardLocation.E2 },
            { boardPosition.E3, boardLocation.E3 },
            { boardPosition.E4, boardLocation.E4 },
            { boardPosition.E5, boardLocation.E5 },
            { boardPosition.E6, boardLocation.E6 },

            { boardPosition.F1, boardLocation.F1 },
            { boardPosition.F2, boardLocation.F2 },
            { boardPosition.F3, boardLocation.F3 },
            { boardPosition.F4, boardLocation.F4 },
            { boardPosition.F5, boardLocation.F5 },
            { boardPosition.F6, boardLocation.F6 },

            { boardPosition.G1, boardLocation.G1 },
            { boardPosition.G2, boardLocation.G2 },
            { boardPosition.G3, boardLocation.G3 },
            { boardPosition.G4, boardLocation.G4 },
            { boardPosition.G5, boardLocation.G5 },
            { boardPosition.G6, boardLocation.G6 },

            { boardPosition.H1, boardLocation.H1 },
            { boardPosition.H2, boardLocation.H2 },
            { boardPosition.H3, boardLocation.H3 },
            { boardPosition.H4, boardLocation.H4 },
            { boardPosition.H5, boardLocation.H5 },
            { boardPosition.H6, boardLocation.H6 }
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

        for (int col = 1; col <= columnCount; col++)
        {
            for (int row = 1; row <= rowCount; row++)
            {
                prefab = Instantiate(TilePrefab, board.transform);
                var tile = prefab.GetComponent<TileInstance>();
                tile.name = $"{col}x{row}";
                tile.location = new Vector2Int(col, row);
            }
        }

        //Assign tiles list
        GameObject.FindGameObjectsWithTag(Tag.Tile).ToList()
            .ForEach(x => GameManager.instance.tiles.Add(x.GetComponent<TileInstance>()));


    }
}
