using UnityEngine;
using UnityEngine.U2D;

public class LineBehavior : ExtendedMonoBehavior
{

    [SerializeField] public Vector3 start;
    [SerializeField] public Vector3 end;
    [SerializeField] public float width;


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

    public LineRenderer lineRenderer;

    #endregion

    #region Methods

    public void Set(Vector3 start, Vector3 end)
    {
        this.start = start;
        this.end = end;

        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        Show();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }



    #endregion

    private void Awake()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
    }

    void Start()
    {
        lineRenderer.positionCount = 2;
        var width = tileSize * percent50;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
    }

    void Update()
    {

    }
}
