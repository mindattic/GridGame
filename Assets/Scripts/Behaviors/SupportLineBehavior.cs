using System.Collections;
using UnityEngine;
using UnityEngine.U2D;

public class SupportLineBehavior : ExtendedMonoBehavior
{
    //Variables
    public Vector3 start;
    public Vector3 end;
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

  
    public void Add(Vector3 start, Vector3 end)
    {
        this.start = start;
        this.end = end;

        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);

        IEnumerator FadeIn()
        {
            var alpha = 0f;
            lineRenderer.startColor = new Color(1, 1, 1, alpha);
            lineRenderer.endColor = new Color(1, 1, 1, alpha);

            while (alpha < 1f)
            {
                alpha += Increment.OnePercent;
                alpha = Mathf.Clamp(alpha, 0, 1);

                lineRenderer.startColor = new Color(1, 1, 1, alpha);
                lineRenderer.endColor = new Color(1, 1, 1, alpha);
                yield return Wait.Tick();
            }

            lineRenderer.startColor = new Color(1, 1, 1, alpha);
            lineRenderer.endColor = new Color(1, 1, 1, alpha);
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

    public void Remove()
    {
        Destroy(this.gameObject);
    }
}
