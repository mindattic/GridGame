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

    public SpriteRenderer spriteRenderer
    {
        get => spriteRenderer;
        set => spriteRenderer = value;
    }

    public Sprite sprite
    {
        get => spriteRenderer.sprite;
        set => spriteRenderer.sprite = value;
    }

    #endregion



    public void Awake()
    {

    }

    public void Start()
    {
        transform.position = Geometry.PositionFromLocation(location);
        transform.localScale = GameManager.instance.tileScale;
    }
}
