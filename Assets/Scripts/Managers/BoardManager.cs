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
        stageManager.Load();
    }

    void GenerateTiles()
    {
        GameObject prefab;
        TileBehavior tile;

        for (int col = 1; col <= board.columnCount; col++)
        {
            for (int row = 1; row <= board.rowCount; row++)
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

    void Update()
    {

    }

}