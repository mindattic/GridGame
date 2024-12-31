using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class TileInstance : MonoBehaviour
{
    protected Vector3 tileScale => GameManager.instance.tileScale;
    protected List<ActorInstance> actors => GameManager.instance.actors;

    //Variables
    public Vector2Int location;

    //public Vector3 position
    //{
    //    get => gameObject.transform.position;
    //    set => gameObject.transform.position = value;
    //}

    public bool IsOccupied => actors.Any(x => x != null && x.IsActive && x.IsAlive && x.location == location);

    #region Components

    public string Name
    {
        get => name;
        set => Name = value;
    }

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

    public Quaternion rotation
    {
        get => gameObject.transform.rotation;
        set => gameObject.transform.rotation = value;
    }

    public Vector3 scale
    {
        get => gameObject.transform.localScale;
        set => gameObject.transform.localScale = value;
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
    public bool IsSameColumn(Vector2Int other) => this.location.x == other.x;
    public bool IsSameRow(Vector2Int other) => this.location.y == other.y;
    public bool IsNorthOf(Vector2Int other) => IsSameColumn(other) && this.location.y == other.y - 1;
    public bool IsEastOf(Vector2Int other) => IsSameRow(other) && this.location.x == other.x + 1;
    public bool IsSouthOf(Vector2Int other) => IsSameColumn(other) && this.location.y == other.y + 1;
    public bool IsWestOf(Vector2Int other) => IsSameRow(other) && this.location.x == other.x - 1;
    public bool IsNorthWestOf(Vector2Int other) => this.location.x == other.x - 1 && this.location.y == other.y - 1;
    public bool IsNorthEastOf(Vector2Int other) => this.location.x == other.x + 1 && this.location.y == other.y - 1;
    public bool IsSouthWestOf(Vector2Int other) => this.location.x == other.x - 1 && this.location.y == other.y + 1;
    public bool IsSouthEastOf(Vector2Int other) => this.location.x == other.x + 1 && this.location.y == other.y + 1;
    public bool IsAdjacentTo(Vector2Int other) => (IsSameColumn(other) || IsSameRow(other)) && Vector2Int.Distance(this.location, other) == 1;

    #endregion

    public void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void Initialize(int col, int row)
    {
        location = new Vector2Int(col, row);
        position = Geometry.CalculatePositionByLocation(location);
        transform.localScale = tileScale;
    }

}
