using UnityEngine;

public class TileBehavior : MonoBehaviour
{
    [SerializeField] public Vector2Int location { get; set; }
    [SerializeField] public bool isOccupied { get; set; }

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

    private SpriteRenderer spriteRenderer;

    public Sprite sprite
    {
        get => spriteRenderer.sprite;
        set => spriteRenderer.sprite = value;
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
        spriteRenderer.color = isOccupied ? Color.yellow : Color.white;
    }
}
