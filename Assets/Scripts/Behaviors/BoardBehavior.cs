using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardBehavior : ExtendedMonoBehavior
{
    public Vector2 upperLeftOffset;
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
        upperLeftOffset = new Vector2(-(tileSize * 3) - tileSize / 2, (tileSize * columnCount));
        top = upperLeftOffset.y - tileSize / 2;
        right = upperLeftOffset.x + (tileSize * columnCount) + tileSize / 2;
        bottom = upperLeftOffset.y - (tileSize * rowCount) - tileSize / 2;
        left = upperLeftOffset.x + tileSize / 2;

        Position = upperLeftOffset;

        //backgroundRenderer.transform.localScale = new Vector3(tileSize * columnCount, tileSize * rowCount, 1);

    }

    // Update is called once per frame
    void Update()
    {

    }

}
