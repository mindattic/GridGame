using System.Linq;
using UnityEngine;

public class TileBehavior : ExtendedMonoBehavior
{

    //Variables
    [SerializeField] public Vector2Int location { get; set; }
  
    public bool IsOccupied => actors.Any(x => x != null && x.IsAlive && x.IsActive && x.location.Equals(location));

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

    public Color color
    {
        get => spriteRenderer.color;
        set => spriteRenderer.color = value;
    }

    #endregion

    public void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void Start()
    {
        transform.position = Geometry.PositionFromLocation(location);
        transform.localScale = GameManager.instance.tileScale;
    }

    public void Update()
    {
        //if (!HasCurrentPlayer)
        //    return;

        //if (currentPlayer.location.Equals(this.location))
        //{
        //    spriteRenderer.color = Colors.Solid.Gold;
        //}
        //else
        //{
        //    spriteRenderer.color = Colors.Translucent.White;
        //}
    }

}
