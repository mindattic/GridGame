using System.Collections;
using UnityEngine;

public class AttackLineBehavior : ExtendedMonoBehavior
{
    //Variables
    ActorPair pair;
    float thickness = 1.2f;
    float maxAlpha = 0.5f;
    Color baseColor = Colors.RGBA(100, 195, 200, 0);

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


    private void Awake()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;

    }

    void Start()
    {
        lineRenderer.startWidth = tileSize * this.thickness;
        lineRenderer.endWidth = tileSize * this.thickness;
    }

    public void Spawn(ActorPair pair)
    {
        this.pair = pair;
        Vector3 start = pair.highest.position;
        Vector3 end = pair.lowest.position;

        if (pair.axis == Axis.Vertical)
        {
            start += new Vector3(0, -(tileSize / 2) + -(tileSize * 0.1f) , 0);
            end += new Vector3(0, tileSize / 2 + (tileSize * 0.1f), 0);
        }
        else if (pair.axis == Axis.Horizontal)
        {
            start += new Vector3(tileSize / 2 + (tileSize * 0.1f), 0, 0);
            end += new Vector3(-(tileSize / 2) + -(tileSize * 0.1f), 0, 0);
        }

        lineRenderer.sortingOrder = ZAxis.Min;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        StartCoroutine(StartFadeIn());
    }



    void Update()
    {

    }

    public void FadeIn()
    {
        StartCoroutine(StartFadeIn());
    }

    private IEnumerator StartFadeIn()
    {
        float alpha = 0f;
        Color color = baseColor;

        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        while (alpha < maxAlpha)
        {
            alpha += Increment.OnePercent;
            alpha = Mathf.Clamp(alpha, 0, maxAlpha);

            color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;

            yield return Wait.Tick();
        }

        color = baseColor;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
    }

    public void Destroy()
    {
        StartCoroutine(StartFadeOut());
    }

    private IEnumerator StartFadeOut()
    {
        float alpha = maxAlpha;
        var color = baseColor;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        while (alpha > 0)
        {
            alpha -= Increment.OnePercent;
            alpha = Mathf.Clamp(alpha, 0, maxAlpha);
            color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;

            yield return Wait.Tick();
        }

        Destroy(this.gameObject);
    }


}
