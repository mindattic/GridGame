using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardBehavior : ExtendedMonoBehavior
{
    public Vector2 offset;
    public int columns = 6;
    public int rows = 8;
    public float top;
    public float right;
    public float bottom;
    public float left;


    #region Components

    //SpriteRenderer backgroundRenderer;

    public Vector3 position
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
        offset = new Vector2(-(tileSize * 3) - tileSize / 2, (tileSize * columns));
        top = offset.y - tileSize / 2;
        right = offset.x + (tileSize * columns) + tileSize / 2;
        bottom = offset.y - (tileSize * rows) - tileSize / 2;
        left = offset.x + tileSize / 2;

        position = offset;

        //backgroundRenderer.transform.localScale = new Vector3(tileSize * columns, tileSize * rows, 1);

    }

    // Update is called once per Frame
    void Update()
    {

    }

}
