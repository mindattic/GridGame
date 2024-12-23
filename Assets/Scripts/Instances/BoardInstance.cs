using Assets.Scripts.Models;
using Game.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardInstance : ExtendedMonoBehavior
{
    [SerializeField] public GameObject TilePrefab;

    [HideInInspector] public int columnCount = 6;
    [HideInInspector] public int rowCount = 8;

    [HideInInspector] public Vector2 offset;

    [HideInInspector] public RectFloat bounds;

    [HideInInspector] public Dictionary<Vector2Int, Vector3> locationToPosition = new Dictionary<Vector2Int, Vector3>();
    [HideInInspector] public Dictionary<Vector3, Vector2Int> positionToLocation = new Dictionary<Vector3, Vector2Int>();

    [HideInInspector] public Vector2Int NowhereLocation;
    [HideInInspector] public Vector3 NowherePosition;

    [HideInInspector] public Vector2Int A1;
    [HideInInspector] public Vector2Int A2;
    [HideInInspector] public Vector2Int A3;
    [HideInInspector] public Vector2Int A4;
    [HideInInspector] public Vector2Int A5;
    [HideInInspector] public Vector2Int A6;

    [HideInInspector] public Vector2Int B1;
    [HideInInspector] public Vector2Int B2;
    [HideInInspector] public Vector2Int B3;
    [HideInInspector] public Vector2Int B4;
    [HideInInspector] public Vector2Int B5;
    [HideInInspector] public Vector2Int B6;

    [HideInInspector] public Vector2Int C1;
    [HideInInspector] public Vector2Int C2;
    [HideInInspector] public Vector2Int C3;
    [HideInInspector] public Vector2Int C4;
    [HideInInspector] public Vector2Int C5;
    [HideInInspector] public Vector2Int C6;

    [HideInInspector] public Vector2Int D1;
    [HideInInspector] public Vector2Int D2;
    [HideInInspector] public Vector2Int D3;
    [HideInInspector] public Vector2Int D4;
    [HideInInspector] public Vector2Int D5;
    [HideInInspector] public Vector2Int D6;

    [HideInInspector] public Vector2Int E1;
    [HideInInspector] public Vector2Int E2;
    [HideInInspector] public Vector2Int E3;
    [HideInInspector] public Vector2Int E4;
    [HideInInspector] public Vector2Int E5;
    [HideInInspector] public Vector2Int E6;

    [HideInInspector] public Vector2Int F1;
    [HideInInspector] public Vector2Int F2;
    [HideInInspector] public Vector2Int F3;
    [HideInInspector] public Vector2Int F4;
    [HideInInspector] public Vector2Int F5;
    [HideInInspector] public Vector2Int F6;

    [HideInInspector] public Vector2Int G1;
    [HideInInspector] public Vector2Int G2;
    [HideInInspector] public Vector2Int G3;
    [HideInInspector] public Vector2Int G4;
    [HideInInspector] public Vector2Int G5;
    [HideInInspector] public Vector2Int G6;

    [HideInInspector] public Vector2Int H1;
    [HideInInspector] public Vector2Int H2;
    [HideInInspector] public Vector2Int H3;
    [HideInInspector] public Vector2Int H4;
    [HideInInspector] public Vector2Int H5;
    [HideInInspector] public Vector2Int H6;


    //public GameObject BoardOverlay => ScreenHelper.GetChildGameObjectByName(GameObject.Find(Constants.Board), Constants.BoardOverlay);


    private void Start()
    {
        offset = new Vector2(-(tileSize * 3) - tileSize / 2, (tileSize * columnCount));
        transform.position = offset;

        bounds = new RectFloat();
        bounds.Top = offset.y - tileSize / 2;
        bounds.Right = offset.x + (tileSize * columnCount) + tileSize / 2;
        bounds.Bottom = offset.y - (tileSize * rowCount) - tileSize / 2;
        bounds.Left = offset.x + tileSize / 2;

        InitializeBoard();
        GenerateTiles();

        //Order of Operations:
        profileManager.LoadProfiles();
        profileManager.Select(0); //TODO: Have user select profile, for now just use index 0   
        stageManager.Load();
    }

    private void InitializeBoard()
    {
        //Initialize "Nowhere" values
        NowhereLocation = new Vector2Int(-1, -1);
        NowherePosition = new Vector3(-1000, -1000, -1000);

        //Add "Nowhere" mappings
        locationToPosition[NowhereLocation] = NowherePosition;
        positionToLocation[NowherePosition] = NowhereLocation;

        //Dynamically create board locations and positions
        for (int y = 1; y <= 8; y++) // Rows A-H
        {
            for (int x = 1; x <= 6; x++) // Columns 1-6
            {
                var location = new Vector2Int(x, y);
                var position = Geometry.CalculatePositionByLocation(location);

                locationToPosition[location] = position;
                positionToLocation[position] = location;
            }
        }

        A1 = new Vector2Int(1, 1);
        A2 = new Vector2Int(2, 1);
        A3 = new Vector2Int(3, 1);
        A4 = new Vector2Int(4, 1);
        A5 = new Vector2Int(5, 1);
        A6 = new Vector2Int(6, 1);

        B1 = new Vector2Int(1, 2);
        B2 = new Vector2Int(2, 2);
        B3 = new Vector2Int(3, 2);
        B4 = new Vector2Int(4, 2);
        B5 = new Vector2Int(5, 2);
        B6 = new Vector2Int(6, 2);

        C1 = new Vector2Int(1, 3);
        C2 = new Vector2Int(2, 3);
        C3 = new Vector2Int(3, 3);
        C4 = new Vector2Int(4, 3);
        C5 = new Vector2Int(5, 3);
        C6 = new Vector2Int(6, 3);

        D1 = new Vector2Int(1, 4);
        D2 = new Vector2Int(2, 4);
        D3 = new Vector2Int(3, 4);
        D4 = new Vector2Int(4, 4);
        D5 = new Vector2Int(5, 4);
        D6 = new Vector2Int(6, 4);

        E1 = new Vector2Int(1, 5);
        E2 = new Vector2Int(2, 5);
        E3 = new Vector2Int(3, 5);
        E4 = new Vector2Int(4, 5);
        E5 = new Vector2Int(5, 5);
        E6 = new Vector2Int(6, 5);

        F1 = new Vector2Int(1, 6);
        F2 = new Vector2Int(2, 6);
        F3 = new Vector2Int(3, 6);
        F4 = new Vector2Int(4, 6);
        F5 = new Vector2Int(5, 6);
        F6 = new Vector2Int(6, 6);

        G1 = new Vector2Int(1, 7);
        G2 = new Vector2Int(2, 7);
        G3 = new Vector2Int(3, 7);
        G4 = new Vector2Int(4, 7);
        G5 = new Vector2Int(5, 7);
        G6 = new Vector2Int(6, 7);

        H1 = new Vector2Int(1, 8);
        H2 = new Vector2Int(2, 8);
        H3 = new Vector2Int(3, 8);
        H4 = new Vector2Int(4, 8);
        H5 = new Vector2Int(5, 8);
        H6 = new Vector2Int(6, 8);

    }

    //Retrieve position by location
    public Vector3 GetPosition(Vector2Int location)
    {
        return locationToPosition.TryGetValue(location, out var position) ? position : NowherePosition;
    }

    //Retrieve location by position
    public Vector2Int GetLocation(Vector3 position)
    {
        return positionToLocation.TryGetValue(position, out var location) ? location : NowhereLocation;
    }


    private void GenerateTiles()
    {
        for (int col = 1; col <= columnCount; col++)
        {
            for (int row = 1; row <= rowCount; row++)
            {
                var prefab = Instantiate(TilePrefab, board.transform);
                var instance = prefab.GetComponent<TileInstance>();
                instance.name = $"{col}x{row}";
                instance.location = new Vector2Int(col, row);
            }
        }

        //Assign tiles list
        GameObject.FindGameObjectsWithTag(Tag.Tile).ToList()
            .ForEach(x => GameManager.instance.tiles.Add(x.GetComponent<TileInstance>()));


    }
}
