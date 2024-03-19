using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackLineBehavior : ExtendedMonoBehavior
{
    //Variables
    Color color;
    float width = 0.7f;
    float maxAlpha = 0.25f;


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

    public void Spawn(Vector3 start, Vector3 end)
    {
        color = lineRenderer.startColor;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);



        Show();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        StartCoroutine(FadeIn());
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

        var thickness = tileSize * width;
        lineRenderer.startWidth = thickness;
        lineRenderer.endWidth = thickness;     
    }

    void Update()
    {

    }


    private IEnumerator FadeIn()
    {
        float alpha = 0f;
        color = new Color(color.r, color.g, color.b, alpha);
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        while (!alpha.Equals(maxAlpha))
        {
            alpha += Increment.One;
            alpha = Mathf.Clamp(alpha, 0, maxAlpha);
            color = new Color(color.r, color.g, color.b, alpha);
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;

            yield return Wait.Tick();
        }
    }





}
