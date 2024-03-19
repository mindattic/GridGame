using System.Collections;
using UnityEngine;

public class AttackLineBehavior : ExtendedMonoBehavior
{
    //Variables
    ActorPair pair;
    float thickness = 0.9f;
    float maxAlpha = 0.5f;


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

    public void Spawn(ActorPair pair)
    {
        this.pair = pair;
        Vector3 start = pair.highest.position;
        Vector3 end = pair.lowest.position;

        //if (pair.axis == Axis.Vertical)
        //{
        //    start += new Vector3(0, -tileSize / 2 + -tileSize * 0.1f, 0);
        //    end += new Vector3(0, tileSize / 2 + tileSize * 0.1f, 0);

        //}
        //else if (pair.axis == Axis.Horizontal)
        //{
        //    start += new Vector3(tileSize / 2 + tileSize * 0.1f, 0, 0);
        //    end += new Vector3(-tileSize / 2 + -tileSize * 0.1f, 0, 0);
        //}

        if (pair.axis == Axis.Vertical)
        {
            start += new Vector3(0, -tileSize / 2, 0);
            end += new Vector3(0, tileSize / 2, 0);
        }
        else if (pair.axis == Axis.Horizontal)
        {
            start += new Vector3(tileSize / 2, 0, 0);
            end += new Vector3(-tileSize / 2, 0, 0);
        }

        lineRenderer.sortingOrder = ZAxis.Min;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        StartCoroutine(StartFadeIn());
    }


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
        var color = new Color(1, 1, 1, alpha);
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        while (alpha < maxAlpha)
        {
            alpha += Increment.OnePercent;
            alpha = Mathf.Clamp(alpha, 0, maxAlpha);
            color = new Color(color.r, color.g, color.b, alpha);
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;

            yield return Wait.Tick();
        }
    }

    public void FadeOut()
    {
        StartCoroutine(StartFadeOut());
    }

    private IEnumerator StartFadeOut()
    {
        float alpha = maxAlpha;
        var color = new Color(1, 1, 1, alpha);
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        while (alpha > 0)
        {
            alpha -= Increment.OnePercent;
            alpha = Mathf.Clamp(alpha, 0, maxAlpha);
            color = new Color(color.r, color.g, color.b, alpha);
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;

            yield return Wait.Tick();
        }

        Destroy(this.gameObject);
    }


}
