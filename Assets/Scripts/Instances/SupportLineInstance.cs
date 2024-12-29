using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.U2D;

public class SupportLineInstance : ExtendedMonoBehavior
{
    private const string NameFormat = "SupportLine_{0}+{1}";

    //Variables
    public float alpha = 0;
    private Vector3 originActor;
    private Vector3 terminalActor;
    private float minAlpha = Opacity.Transparent;
    private float maxAlpha = Opacity.Percent50;
    private Color color = ColorHelper.RGBA(48, 161, 49, 0);
    private LineRenderer lineRenderer;


    #region Components

    public Transform parent
    {
        get => gameObject.transform.parent;
        set => gameObject.transform.SetParent(value, true);
    }


    #endregion


    private void Awake()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.sortingOrder = SortingOrder.SupportLine;
    }

    void Start()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = tileSize / 2;
        lineRenderer.endWidth = tileSize / 2;
    }

    public void Spawn(ActorPair pair)
    {
        parent = board.transform;
        name = NameFormat.Replace("{0}", pair.originActor.name).Replace("{1}", pair.terminalActor.name);

        originActor = pair.originActor.position;
        terminalActor = pair.terminalActor.position;

        //lineRenderer.sortingOrder = SortingOrder.SupportLine;
        lineRenderer.SetPosition(0, originActor);
        lineRenderer.SetPosition(1, terminalActor);

        IEnumerator _()
        {
            alpha = Opacity.Transparent;
            color = new Color(color.r, color.g, color.b, alpha);
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;

            while (alpha < maxAlpha)
            {
                alpha += Increment.OnePercent;
                alpha = Mathf.Clamp(alpha, minAlpha, maxAlpha);
                color = new Color(color.r, color.g, color.b, alpha);
                lineRenderer.startColor = new Color(color.r, color.g, color.b, alpha);
                lineRenderer.endColor = new Color(color.r, color.g, color.b, alpha);

                yield return Wait.OneTick();
            }
        };

        StartCoroutine(_());
    }

    public IEnumerator _Despawn()
    {
        while (alpha > minAlpha)
        {
            alpha -= Increment.OnePercent;
            alpha = Mathf.Clamp(alpha, minAlpha, maxAlpha);
            color = new Color(color.r, color.g, color.b, alpha);
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;

            yield return Wait.OneTick();
        }
    }

    public void Despawn()
    {
        StartCoroutine(_Despawn());
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }



}
