using System.Collections;
using UnityEngine;

public class AttackLineBehavior : ExtendedMonoBehavior
{
    //Variables
    ActorPair Pair;
    float Thickness = 1.2f;
    float MaxAlpha = 0.5f;
    Color BaseColor = Colors.RGBA(100, 195, 200, 0);
    public LineRenderer LineRenderer;

    #region Components

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

   

    #endregion


    private void Awake()
    {
        LineRenderer = gameObject.GetComponent<LineRenderer>();
        LineRenderer.positionCount = 2;

    }

    void Start()
    {
        LineRenderer.startWidth = TileSize * this.Thickness;
        LineRenderer.endWidth = TileSize * this.Thickness;
    }

    public void Spawn(ActorPair pair)
    {
        this.Pair = pair;
        Vector3 start = pair.HighestActor.position;
        Vector3 end = pair.LowestActor.position;

        if (pair.Axis == Axis.Vertical)
        {
            start += new Vector3(0, -(TileSize / 2) + -(TileSize * 0.1f) , 0);
            end += new Vector3(0, TileSize / 2 + (TileSize * 0.1f), 0);
        }
        else if (pair.Axis == Axis.Horizontal)
        {
            start += new Vector3(TileSize / 2 + (TileSize * 0.1f), 0, 0);
            end += new Vector3(-(TileSize / 2) + -(TileSize * 0.1f), 0, 0);
        }

        LineRenderer.sortingOrder = ZAxis.Min;
        LineRenderer.SetPosition(0, start);
        LineRenderer.SetPosition(1, end);
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
        Color color = BaseColor;

        LineRenderer.startColor = color;
        LineRenderer.endColor = color;

        while (alpha < MaxAlpha)
        {
            alpha += Increment.OnePercent;
            alpha = Mathf.Clamp(alpha, 0, MaxAlpha);

            color = new Color(BaseColor.r, BaseColor.g, BaseColor.b, alpha);
            LineRenderer.startColor = color;
            LineRenderer.endColor = color;

            yield return Wait.OneTick();
        }

        color = BaseColor;
        LineRenderer.startColor = color;
        LineRenderer.endColor = color;
    }

    public void Destroy()
    {
        StartCoroutine(StartFadeOut());
    }

    private IEnumerator StartFadeOut()
    {
        float alpha = MaxAlpha;
        var color = BaseColor;
        LineRenderer.startColor = color;
        LineRenderer.endColor = color;

        while (alpha > 0)
        {
            alpha -= Increment.OnePercent;
            alpha = Mathf.Clamp(alpha, 0, MaxAlpha);
            color = new Color(BaseColor.r, BaseColor.g, BaseColor.b, alpha);
            LineRenderer.startColor = color;
            LineRenderer.endColor = color;

            yield return Wait.OneTick();
        }

        Destroy(this.gameObject);
    }


}
