using Assets.Scripts.Models;
using Game.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class BoardInstance : MonoBehaviour
{
    #region Properties
    protected float tileSize => GameManager.instance.tileSize;
    protected ProfileManager profileManager => GameManager.instance.profileManager;
    protected StageManager stageManager => GameManager.instance.stageManager;
    protected BoardInstance board => GameManager.instance.board;
    #endregion

    //Variables
    [SerializeField] public GameObject TilePrefab;
    [HideInInspector] public int columnCount = 6;
    [HideInInspector] public int rowCount = 8;
    [HideInInspector] public Vector2 offset;
    [HideInInspector] public RectFloat bounds;
    [HideInInspector] public TileMap tileMap = new TileMap();
    [HideInInspector] public Vector2Int NowhereLocation = new Vector2Int(-1, -1);
    [HideInInspector] public Vector3 NowherePosition = new Vector3(-1000, -1000, -1000);

    //Method which is automatically called before the first frame update  
    private void Start()
    {
        offset = new Vector2(-(tileSize * 3) - tileSize / 2, (tileSize * columnCount));
        transform.position = offset;

        //Order of Operations:
        CalculateBounds();
        GenerateTiles();
        profileManager.LoadProfiles();
        profileManager.Select(0); //TODO: Have user select profile, for now just use index 0   
        stageManager.Load();
    }

    private void CalculateBounds()
    {
        bounds = new RectFloat();
        bounds.Top = offset.y - tileSize / 2;
        bounds.Right = offset.x + (tileSize * columnCount) + tileSize / 2;
        bounds.Bottom = offset.y - (tileSize * rowCount) - tileSize / 2;
        bounds.Left = offset.x + tileSize / 2;
    }

    private void GenerateTiles()
    {
        for (int col = 1; col <= columnCount; col++)
        {
            for (int row = 1; row <= rowCount; row++)
            {
                var prefab = Instantiate(TilePrefab, Vector2.zero, Quaternion.identity);
                var instance = prefab.GetComponent<TileInstance>();
                instance.parent = board.transform;
                instance.name = $"Tile_{col}x{row}";
                instance.Initialize(col, row);
                tileMap.Add(instance);
                //tiles.Add(instance);
            }
        }

        //TODO: Remove this an just use the tileMap for all lookups...
        //Assign tiles list
        GameObject.FindGameObjectsWithTag(Tag.Tile).ToList()
            .ForEach(x => GameManager.instance.tiles.Add(x.GetComponent<TileInstance>()));
    }
}
