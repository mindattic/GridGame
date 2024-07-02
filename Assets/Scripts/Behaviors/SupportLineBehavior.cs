using System.Collections;
using UnityEngine;
using UnityEngine.U2D;

public class SupportLineBehavior : ExtendedMonoBehavior
{
    //Variables
    public Vector3 PointA;
    public Vector3 PointB;
    [SerializeField] public float Width;
    float MaxAlpha = 0.5f;
    public Color BaseColor = Colors.RGBA(48, 161, 49, 0);

    #region Components

    public string Name
    {
        get => name;
        set => Name = value;
    }

    public Transform Parent
    {
        get => gameObject.transform.parent;
        set => gameObject.transform.SetParent(value, true);
    }

    public Vector3 Position
    {
        get => gameObject.transform.position;
        set => gameObject.transform.position = value;
    }

    public LineRenderer lineRenderer;

    #endregion

  
    public void Spawn(Vector3 start, Vector3 end)
    {
        this.PointA = start;
        this.PointB = end;

        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);

        IEnumerator _()
        {
            float alpha = 0f;
            Color color = BaseColor;

            lineRenderer.startColor = new Color(color.r, color.g, color.b, alpha);
            lineRenderer.endColor = new Color(color.r, color.g, color.b, alpha);

            while (alpha < MaxAlpha)
            {
                alpha += Increment.OnePercent;
                alpha = Mathf.Clamp(alpha, 0, 1);

                color = new Color(BaseColor.r, BaseColor.g, BaseColor.b, alpha);
                lineRenderer.startColor = new Color(color.r, color.g, color.b, alpha);
                lineRenderer.endColor = new Color(color.r, color.g, color.b, alpha);

                yield return Wait.OneTick();
            }

            color = BaseColor;
            lineRenderer.startColor = new Color(color.r, color.g, color.b, alpha);
            lineRenderer.endColor = new Color(color.r, color.g, color.b, alpha);
        };

        StartCoroutine(_());
    }


    private void Awake()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
    }

    void Start()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = tileSize / 2;
        lineRenderer.endWidth = tileSize / 2;
    }

    void Update()
    {

    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}
