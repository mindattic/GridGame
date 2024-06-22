using System.Collections;
using UnityEngine;
using UnityEngine.U2D;

public class SupportLineBehavior : ExtendedMonoBehavior
{
    //Variables
    public Vector3 start;
    public Vector3 end;
    [SerializeField] public float width;
    float maxAlpha = 0.5f;
    public Color baseColor = Colors.RGBA(48, 161, 49, 0);

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

  
    public void Spawn(Vector3 start, Vector3 end)
    {
        this.start = start;
        this.end = end;

        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);

        IEnumerator FadeIn()
        {
            float alpha = 0f;
            Color color = baseColor;

            lineRenderer.startColor = new Color(color.r, color.g, color.b, alpha);
            lineRenderer.endColor = new Color(color.r, color.g, color.b, alpha);

            while (alpha < maxAlpha)
            {
                alpha += Increment.OnePercent;
                alpha = Mathf.Clamp(alpha, 0, 1);

                color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
                lineRenderer.startColor = new Color(color.r, color.g, color.b, alpha);
                lineRenderer.endColor = new Color(color.r, color.g, color.b, alpha);

                yield return Wait.Tick();
            }

            color = baseColor;
            lineRenderer.startColor = new Color(color.r, color.g, color.b, alpha);
            lineRenderer.endColor = new Color(color.r, color.g, color.b, alpha);
        };

        StartCoroutine(FadeIn());
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
