using System.Linq;
using UnityEngine;

public class TileBehavior : ExtendedMonoBehavior
{

    //Variables
    [SerializeField] public Vector2Int Location { get; set; }
  
    public bool IsOccupied => Actors.Any(x => x != null && x.IsAlive && x.IsActive && x.Location.Equals(Location));

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

    #region Properties
    public bool IsSameColumn(Vector2Int other) => this.Location.x == other.x;
    public bool IsSameRow(Vector2Int other) => this.Location.y == other.y;
    public bool IsNorthOf(Vector2Int other) => IsSameColumn(other) && this.Location.y == other.y - 1;
    public bool IsEastOf(Vector2Int other) => IsSameRow(other) && this.Location.x == other.x + 1;
    public bool IsSouthOf(Vector2Int other) => IsSameColumn(other) && this.Location.y == other.y + 1;
    public bool IsWestOf(Vector2Int other) => IsSameRow(other) && this.Location.x == other.x - 1;
    public bool IsNorthWestOf(Vector2Int other) => this.Location.x == other.x - 1 && this.Location.y == other.y - 1;
    public bool IsNorthEastOf(Vector2Int other) => this.Location.x == other.x + 1 && this.Location.y == other.y - 1;
    public bool IsSouthWestOf(Vector2Int other) => this.Location.x == other.x - 1 && this.Location.y == other.y + 1;
    public bool IsSouthEastOf(Vector2Int other) => this.Location.x == other.x + 1 && this.Location.y == other.y + 1;
    public bool IsAdjacentTo(Vector2Int other) => (IsSameColumn(other) || IsSameRow(other)) && Vector2Int.Distance(this.Location, other) == 1;

    #endregion

    public void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void Start()
    {
        transform.position = Geometry.PositionFromLocation(Location);
        transform.localScale = GameManager.instance.TileScale;
    }

    public void Update()
    {
        //if (!HasSelectedPlayer)
        //    return;

        //if (FocusedPlayer.Location.Equals(this.Location))
        //{
        //    SpriteRenderer.Color = Colors.Solid.Gold;
        //}
        //else
        //{
        //    SpriteRenderer.Color = Colors.Translucent.White;
        //}
    }

}
