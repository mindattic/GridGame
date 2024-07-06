using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.U2D;

public class SupportLineBehavior : ExtendedMonoBehavior
{
    //Variables
    public ActorPair pair;
    public float alpha = 0;
    private Vector3 start;
    private Vector3 end;
    private float maxAlpha = 0.5f;
    private Color baseColor = Colors.RGBA(48, 161, 49, 0);
    private LineRenderer lineRenderer;

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


    #endregion


    private void Awake()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
    }

    void Start()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = tileSize / 2;
        lineRenderer.endWidth = tileSize / 2;
    }


    public void Spawn()
    {
        if (pair == null)
            return;

        start = pair.highestActor.position;
        end = pair.lowestActor.position;

        lineRenderer.sortingOrder = ZAxis.Half;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);

        IEnumerator _()
        {
            alpha = 0f;
            var color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;

            while (alpha < maxAlpha)
            {
                alpha += Increment.OnePercent;
                alpha = Mathf.Clamp(alpha, 0, 1);

                color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
                lineRenderer.startColor = new Color(color.r, color.g, color.b, alpha);
                lineRenderer.endColor = new Color(color.r, color.g, color.b, alpha);

                yield return Wait.OneTick();
            }
        };

        StartCoroutine(_());
    }

    public IEnumerator Despawn()
    {
        alpha = maxAlpha;
        var color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        while (alpha > 0)
        {
            alpha -= Increment.OnePercent;
            alpha = Mathf.Clamp(alpha, 0, maxAlpha);
            color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;

            yield return Wait.OneTick();
        }
    }

    public void DespawnAsync()
    {
        StartCoroutine(Despawn());
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }



}
