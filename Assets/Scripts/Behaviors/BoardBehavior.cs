using Game.Models;
using UnityEngine;

public class BoardBehavior : ExtendedMonoBehavior
{
    public Vector2 offset;
    public int columnCount = 6;
    public int rowCount = 8;
    public RectFloat bounds;

    #region Components

    public Vector3 position
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
        offset = new Vector2(-(tileSize * 3) - tileSize / 2, (tileSize * columnCount));
       
        bounds = new RectFloat();
        bounds.Top = offset.y - tileSize / 2;
        bounds.Right = offset.x + (tileSize * columnCount) + tileSize / 2;
        bounds.Bottom = offset.y - (tileSize * rowCount) - tileSize / 2;
        bounds.Left = offset.x + tileSize / 2;

        position = offset;

        //backgroundRenderer.transform.localScale = new Vector3(tileSize * columnCount, tileSize * rowCount, 1);

    }

    // Update is called once per frame
    void Update()
    {

    }

}
