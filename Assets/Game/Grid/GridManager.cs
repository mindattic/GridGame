using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject cellPrefab;
    public GameObject playerPrefab;

    SpriteManager spriteManager;

    private float cellSize => Global.instance.cellSize;
    private Vector2 cellScale => Global.instance.cellScale;
    private Vector2 spriteScale => Global.instance.spriteScale;

    private Dictionary<string, GameObject> gridMap
    {
        get { return Global.instance.gridMap; }
        set { Global.instance.gridMap = value; }
    }

    void Awake()
    {
        spriteManager = GameObject.Find("Sprite").GetComponent<SpriteManager>();

    }

    void Start()
    {
        GenerateGrid();
        GeneratePlayer();
    }

    void GenerateGrid()
    {
        var cells = GameObject.FindGameObjectsWithTag("Cell");
        if (cells == null)
            return;

        int columns = 4;
        int rows = 7;

        for (int c = 0; c <= columns; c++)
        {
            for (int r = 0; r <= rows; r++)
            {
                var name = $"Cell_{c}x{r}";
                var cell = cells.FirstOrDefault(x => string.Equals(x.name, name));
                if (cell == null)
                    return;

                var cellBehavior = cell.GetComponent<CellBehavior>();
                if (cellBehavior == null)
                    return;

                cellBehavior.X = c;
                cellBehavior.Y = r;
                gridMap.Add(name, cell);
            }
        }
    }

    void GeneratePlayer()
    {
        GameObject gameObject;

        gridMap.TryGetValue("Cell_2x2", out gameObject);
        if (gameObject == null)
            return;

        var player1 = Instantiate(playerPrefab, gameObject.transform);
        player1.name = "Sentinel";
        player1.GetComponent<SpriteRenderer>().sprite = spriteManager.sentinel;
        player1.transform.SetParent(transform, true);
        player1.transform.localScale = cellScale;

        gridMap.TryGetValue("Cell_4x4", out gameObject);
        if (gameObject == null)
            return;

        var player2 = Instantiate(playerPrefab, gameObject.transform);
        player2.name = "Corsair";
        player2.GetComponent<SpriteRenderer>().sprite = spriteManager.corsair;
        player2.transform.SetParent(transform, true);
        player2.transform.localScale = cellScale;

        gridMap.TryGetValue("Cell_1x6", out gameObject);
        if (gameObject == null)
            return;

        var player3 = Instantiate(playerPrefab, gameObject.transform);
        player3.name = "Oracle";
        player3.GetComponent<SpriteRenderer>().sprite = spriteManager.oracle;
        player3.transform.SetParent(transform, true);
        player3.transform.localScale = cellScale;

        //Assign players list
        Global.instance.players = GameObject.FindGameObjectsWithTag(Tag.Player).ToList();

    }

}