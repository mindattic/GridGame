using TMPro;
using UnityEngine;

public class ActorRenderers
{
    public ActorRenderers() { }

    public Color glowColor;
    public Color backColor;
    public Color frameColor;

    public SpriteRenderer Back;
    public SpriteRenderer Parallax;
    public SpriteRenderer Glow; 
    public SpriteRenderer Thumbnail;
    public SpriteRenderer Frame;

    public SpriteRenderer StatusIcon;

    public SpriteRenderer HealthBarBack;
    public SpriteRenderer HealthBar;
    public SpriteRenderer HealthBarFront;
    public TextMeshPro HealthText;

    public SpriteRenderer ActionBarBack;
    public SpriteRenderer ActionBar;
    public TextMeshPro ActionText;

    public SpriteRenderer RadialBack;
    public SpriteRenderer RadialFill;
    public TextMeshPro RadialText;

    public SpriteRenderer Selection;

    public void SetAlpha(float alpha)
    {
        Back.color = new Color(backColor.r, backColor.g, backColor.b, Mathf.Clamp(backColor.a, 0, 0.7f));
        Parallax.color = new Color(backColor.r, backColor.g, backColor.b, Mathf.Clamp(backColor.a, 0, 0.7f));
        Glow.color = new Color(glowColor.r, backColor.g, glowColor.b, Mathf.Clamp(backColor.a, 0, 0.25f)); 
        Thumbnail.color = new Color(1, 1, 1, alpha);
        Frame.color = new Color(frameColor.r, frameColor.g, frameColor.b, alpha);

        StatusIcon.color = new Color(1, 1, 1, alpha);

        HealthBarBack.color = new Color(1, 1, 1, Mathf.Clamp(alpha, 0, 0.7f));
        HealthBar.color = new Color(1, 1, 1, alpha);
        HealthBarFront.color = new Color(1, 1, 1, alpha);
        HealthText.color = new Color(1, 1, 1, alpha);

        ActionBarBack.color = new Color(1, 1, 1, Mathf.Clamp(alpha, 0, 0.7f));
        ActionBar.color = new Color(1, 1, 1, alpha);
        ActionText.color = new Color(1, 1, 1, alpha);

        RadialBack.color = new Color(0, 0, 0, Mathf.Clamp(alpha, 0, 0.5f));
        RadialFill.color = new Color(1, 1, 1, alpha);
        RadialText.color = new Color(1, 1, 1, alpha);

        Selection.color = new Color(1, 1, 1, alpha);
    }

    public void SetBackColor(Color color)
    {
        backColor = new Color(color.r, color.g, color.b, color.a);
        this.Back.color = backColor;
    }

    public void SetBackScale(Vector3 scale)
    {
        this.Back.transform.localScale = scale;
    }

    public void SetBackAlpha(float alpha)
    {
        backColor = new Color(backColor.r, backColor.g, backColor.b, Mathf.Min(alpha, 0.5f));
        this.Back.color = backColor;
    }

    public void SetGlowColor(Color color)
    {
        glowColor = color;
        this.Glow.color = color;

        var intensity = (glowColor.r + glowColor.g + glowColor.b) / 3f;
        var factor = 1f / intensity;
        var emissionColor = new Color(glowColor.r * factor, glowColor.g * factor, glowColor.b * factor, glowColor.a);
        this.Glow.material.SetColor("_EmissionColor", emissionColor);
    }

    public void SetGlowAlpha(float alpha)
    {
        glowColor = new Color(glowColor.r, glowColor.g, glowColor.b, alpha);
        this.Glow.color = glowColor;

        var intensity = (glowColor.r + glowColor.g + glowColor.b) / 3f;
        var factor = 1f / intensity;
        var emissionColor = new Color(glowColor.r * factor, glowColor.g * factor, glowColor.b * factor, alpha);
        this.Glow.material.SetColor("_EmissionColor", emissionColor);
    }

    public void SetFrameColor(Color color)
    {
        frameColor = new Color(color.r, color.g, color.b, color.a);
        this.Frame.color = frameColor;
    }


    public void SetFocus(bool isActive = true)
    {
        Selection.gameObject.SetActive(isActive);
    }


    public void SetParallaxSprite(Sprite sprite)
    {
        Parallax.sprite = sprite;
    }
}

