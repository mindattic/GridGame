using TMPro;
using UnityEngine;

public class ActorRenderers
{
    public ActorRenderers() { }

    public Color glowColor;
    public Color backColor;
    public Color frameColor;

    public SpriteRenderer back;
    public SpriteRenderer parallax;
    public SpriteRenderer glow; 
    public SpriteRenderer thumbnail;
    public SpriteRenderer frame;

    public SpriteRenderer statusIcon;

    public SpriteRenderer healthBarBack;
    public SpriteRenderer healthBar;
    public SpriteRenderer healthBarFront;
    public TextMeshPro healthText;

    public SpriteRenderer actionBarBack;
    public SpriteRenderer actionBar;
    public TextMeshPro actionText;

    public SpriteRenderer radialBack;
    public SpriteRenderer radialFill;
    public TextMeshPro radialText;

    public SpriteRenderer selection;

    public SpriteMask mask;


    public void SetAlpha(float alpha)
    {
        back.color = new Color(backColor.r, backColor.g, backColor.b, Mathf.Clamp(backColor.a, 0, 0.7f));
        parallax.color = new Color(backColor.r, backColor.g, backColor.b, alpha);
        glow.color = new Color(glowColor.r, glowColor.g, glowColor.b, Mathf.Clamp(glowColor.a, 0, 0.25f)); 
        thumbnail.color = new Color(1, 1, 1, alpha);
        frame.color = new Color(frameColor.r, frameColor.g, frameColor.b, alpha);

        statusIcon.color = new Color(1, 1, 1, alpha);

        healthBarBack.color = new Color(1, 1, 1, Mathf.Clamp(alpha, 0, 0.7f));
        healthBar.color = new Color(1, 1, 1, alpha);
        healthBarFront.color = new Color(1, 1, 1, alpha);
        healthText.color = new Color(1, 1, 1, alpha);

        actionBarBack.color = new Color(1, 1, 1, Mathf.Clamp(alpha, 0, 0.7f));
        actionBar.color = new Color(1, 1, 1, alpha);
        actionText.color = new Color(1, 1, 1, alpha);

        radialBack.color = new Color(0, 0, 0, Mathf.Clamp(alpha, 0, 0.5f));
        radialFill.color = new Color(1, 1, 1, alpha);
        radialText.color = new Color(1, 1, 1, alpha);

        selection.color = new Color(1, 1, 1, alpha);
    }

    public void SetBackColor(Color color)
    {
        backColor = new Color(color.r, color.g, color.b, color.a);
        this.back.color = backColor;
    }

    public void SetBackScale(Vector3 scale)
    {
        this.back.transform.localScale = scale;
    }

    public void SetBackAlpha(float alpha)
    {
        backColor = new Color(backColor.r, backColor.g, backColor.b, Mathf.Min(alpha, 0.5f));
        this.back.color = backColor;
    }

    public void SetGlowColor(Color color)
    {
        glowColor = color;
        this.glow.color = color;

        var intensity = (glowColor.r + glowColor.g + glowColor.b) / 3f;
        var factor = 1f / intensity;
        var emissionColor = new Color(glowColor.r * factor, glowColor.g * factor, glowColor.b * factor, glowColor.a);
        this.glow.material.SetColor("_EmissionColor", emissionColor);
    }

    public void SetGlowAlpha(float alpha)
    {
        glowColor = new Color(glowColor.r, glowColor.g, glowColor.b, alpha);
        this.glow.color = glowColor;

        var intensity = (glowColor.r + glowColor.g + glowColor.b) / 3f;
        var factor = 1f / intensity;
        var emissionColor = new Color(glowColor.r * factor, glowColor.g * factor, glowColor.b * factor, alpha);
        this.glow.material.SetColor("_EmissionColor", emissionColor);
    }

    public void SetFrameColor(Color color)
    {
        frameColor = new Color(color.r, color.g, color.b, color.a);
        this.frame.color = frameColor;
    }


    public void SetFocus(bool isActive = true)
    {
        selection.gameObject.SetActive(isActive);
    }


    public void SetParallaxSprite(Sprite sprite)
    {
        parallax.sprite = sprite;
    }

    public void SetParallaxMaterial(Material material)
    {
        parallax.material = material;
    }

}

