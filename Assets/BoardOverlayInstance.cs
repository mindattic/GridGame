using System.Collections;
using UnityEngine;

public class BoardOverlayInstance : ExtendedMonoBehavior
{
    SpriteRenderer spriteRenderer;

    float alpha = 0;
    float minAlpha = Opacity.Transparent;
    float maxAlpha = Opacity.Percent70;
    Color color = Colors.Translucent.DarkBlack;
    float increment = Increment.TwoPercent;

    private void Awake()
    {
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = SortingOrder.BoardOverlay;
        spriteRenderer.enabled = false;
    }

    public void Show()
    {
        FadeInAsync();
    }

    public void Hide()
    {
        FadeOutAsync();
    }


    public void FadeInAsync()
    {
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeIn()
    {
        //Before:
        alpha = minAlpha;
        color.a = alpha;
        spriteRenderer.color = color;
        spriteRenderer.enabled = true;

        //During:
        while (alpha < maxAlpha)
        {
            alpha += increment;
            alpha = Mathf.Clamp(alpha, minAlpha, maxAlpha);
            color.a = alpha;
            spriteRenderer.color = color;
            yield return Wait.OneTick();
        }

        //After:
        alpha = maxAlpha;
        color.a = alpha;
        spriteRenderer.color = color;
    }


    public void FadeOutAsync()
    {
        StartCoroutine(FadeOut());
    }

    public IEnumerator FadeOut()
    {
        // Before:
        alpha = maxAlpha;
        color.a = alpha;
        spriteRenderer.color = color;

        // During:
        while (alpha > minAlpha)
        {
            alpha -= increment;
            alpha = Mathf.Clamp(alpha, minAlpha, maxAlpha);
            color.a = alpha;
            spriteRenderer.color = color;
            yield return Wait.OneTick();
        }

        // After:
        alpha = minAlpha;
        color.a = alpha;
        spriteRenderer.color = color;
        spriteRenderer.enabled = false;
    }

}
