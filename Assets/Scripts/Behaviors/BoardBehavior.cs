using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardBehavior : ExtendedMonoBehavior
{
    public Vector2 Offset;
    public int ColumnCount = 6;
    public int RowCount = 8;
    public float Top;
    public float Right;
    public float Bottom;
    public float Left;


    #region Components

    public Vector3 Position
    {
        get => gameObject.transform.position;
        set => gameObject.transform.position = value;
    }

    #endregion




    void Awake()
    {
        //backgroundRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        Offset = new Vector2(-(TileSize * 3) - TileSize / 2, (TileSize * ColumnCount));
        Top = Offset.y - TileSize / 2;
        Right = Offset.x + (TileSize * ColumnCount) + TileSize / 2;
        Bottom = Offset.y - (TileSize * RowCount) - TileSize / 2;
        Left = Offset.x + TileSize / 2;

        Position = Offset;

        //backgroundRenderer.transform.localScale = new Vector3(TileSize * ColumnCount, TileSize * RowCount, 1);

    }

    // Update is called once per Frame
    void Update()
    {

    }

}
