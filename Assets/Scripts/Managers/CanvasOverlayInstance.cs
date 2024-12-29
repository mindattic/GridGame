using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CanvasOverlayInstance : ExtendedMonoBehavior
{

    //Variables
    private RectTransform rectTransform;
    private Image image;
    float alpha = 0;
    float minAlpha = Opacity.Transparent;
    float maxAlpha = Opacity.Opaque;
    Color color = new Color(0f, 0f, 0f, Opacity.Opaque);

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

    public Color Color
    {
        get => image.color;
        set => image.color = value;
    }

    #endregion

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        var canvas = this.GetComponentInParent<Canvas>();
        var sizeDeltaY = Screen.height / canvas.scaleFactor;
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, sizeDeltaY);
 
        image = this.gameObject.GetComponent<Image>();
        image.color = new Color(0f, 0f, 0f, Opacity.Opaque);     
    }

    public void Show()
    {
        FadeOut();
        titleManager.FadeOut();
    }


    public void FadeIn()
    {
        StartCoroutine(_FadeIn());
    }

    private IEnumerator _FadeIn()
    {
        //Before:
        alpha = minAlpha;
        color.a = alpha;
        image.color = color;

        //During:
        while (alpha < maxAlpha)
        {
            alpha += Increment.OnePercent;
            alpha = Mathf.Clamp(alpha, minAlpha, maxAlpha);
            color.a = alpha;
            image.color = color;
            yield return Wait.OneTick();
        }

        //After:
        alpha = maxAlpha;
        color.a = alpha;
        image.color = color;
    }


    public void FadeOut()
    {
        StartCoroutine(_FadeOut());
    }

    private IEnumerator _FadeOut()
    {
        // Before:
        alpha = maxAlpha;
        color.a = alpha;
        image.color = color;

        // During:
        while (alpha > 0)
        {
            alpha -= Increment.OnePercent;
            alpha = Mathf.Clamp(alpha, minAlpha, maxAlpha);
            color.a = alpha;
            image.color = color;
            yield return Wait.OneTick();
        }

        // After:
        alpha = minAlpha;
        color.a = alpha;
        image.color = color;
    }

}
