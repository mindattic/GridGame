using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardBehavior : ExtendedMonoBehavior
{
    public Vector2 cornerOffset;
    public int columnCount = 6;
    public int rowCount = 8;
    public float top;
    public float right;
    public float bottom;
    public float left;


    #region Components

    public Vector3 Position
    {
        get => gameObject.transform.position;
        set => gameObject.transform.position = value;
    }

    #endregion




    void Awake()
    {
        //backgroundRenderer = transform.GetChild(0).GetComponent<spriteRenderer>();
    }

    void Start()
    {
        cornerOffset = new Vector2(-(TileSize * 3) - TileSize / 2, (TileSize * columnCount));
        top = cornerOffset.y - TileSize / 2;
        right = cornerOffset.x + (TileSize * columnCount) + TileSize / 2;
        bottom = cornerOffset.y - (TileSize * rowCount) - TileSize / 2;
        left = cornerOffset.x + TileSize / 2;

        Position = cornerOffset;

        //backgroundRenderer.transform.localScale = new Vector3(TileSize * columnCount, TileSize * rowCount, 1);

    }

    // Update is called once per frame
    void Update()
    {

    }

}
