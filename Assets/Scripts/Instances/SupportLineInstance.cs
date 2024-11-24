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
    private float maxAlpha = 0.5f;
    private Color baseColor = Shared.RGBA(48, 161, 49, 0);
    private Color color;
    private LineRenderer lineRenderer;


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

    public int sortingOrder
    {
        get => lineRenderer.sortingOrder;
        set => lineRenderer.sortingOrder = value;
    }

    #endregion


    private void Awake()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.sortingOrder = SortingOrder.Min;
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

        lineRenderer.sortingOrder = SortingOrder.SupportLine;
        lineRenderer.SetPosition(0, originActor);
        lineRenderer.SetPosition(1, terminalActor);

        IEnumerator _()
        {
            alpha = 0f;
            color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;

            while (alpha < maxAlpha)
            {
                alpha += Increment.OnePercent;
                alpha = Mathf.Clamp(alpha, 0, maxAlpha);

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
        while (alpha > 0)
        {
            alpha -= Increment.OnePercent;
            alpha = Mathf.Clamp(alpha, 0, maxAlpha);
            color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;

            yield return Wait.OneTick();
        }

        //Clear(this.prefab);
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
