using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardManager : ExtendedMonoBehavior
{
    [SerializeField] public GameObject TilePrefab;

    void Awake()
    {

    }

    void Start()
    {
        GenerateTiles();
        StageManager.Load();
    }

    void GenerateTiles()
    {
        GameObject prefab;
        TileBehavior tile;

        for (int col = 1; col <= Board.ColumnCount; col++)
        {
            for (int row = 1; row <= Board.RowCount; row++)
            {
                prefab = Instantiate(TilePrefab, Board.transform);
                tile = prefab.GetComponent<TileBehavior>();
                tile.name = $"{col}x{row}";
                tile.Location = new Vector2Int(col, row);
            }
        }

        //Assign Tiles list
        GameObject.FindGameObjectsWithTag(Tag.Tile).ToList()
            .ForEach(x => GameManager.instance.Tiles.Add(x.GetComponent<TileBehavior>()));


    }

    void Update()
    {

    }

}