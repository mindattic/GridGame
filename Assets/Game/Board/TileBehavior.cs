using UnityEngine;

public class TileBehavior : ExtendedMonoBehavior
{
    [SerializeField] public Vector2Int location { get; set; }
    [SerializeField] public bool isOccupied { get; set; }



    public BoxCollider2D boxCollider2D;


    #region Components

    public Transform parent
    {
        get => gameObject.transform.parent;
        set => gameObject.transform.SetParent(value, true);
    }

    public Vector3 position
    {
        get => gameObject.transform.position;
        set => gameObject.transform.position = value;
    }

    public SpriteRenderer spriteRenderer;

    public Sprite sprite
    {
        get => spriteRenderer.sprite;
        set => spriteRenderer.sprite = value;
    }

    #endregion



    public void Awake()
    {
        boxCollider2D = gameObject.GetComponent<BoxCollider2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void Start()
    {
        transform.position = Geometry.PositionFromLocation(location);
        transform.localScale = GameManager.instance.tileScale;
    }

    public void Update()
    {
        if (!HasSelectedPlayer)
            return;

        if (selectedPlayer.location.Equals(this.location))
        {
            spriteRenderer.color = Colors.Solid.Gold;
        }
        else
        {
            spriteRenderer.color = Colors.Transparent.White;
        }
    }

}
