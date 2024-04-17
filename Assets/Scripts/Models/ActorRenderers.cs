using TMPro;
using UnityEngine;

public class ActorRenderers
{
    public ActorRenderers() { }

    public Color glowColor;
    public Color backColor;


    public SpriteRenderer glow;
    public SpriteRenderer back;
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

    public void SetAlpha(float alpha)
    {
        thumbnail.color = new Color(1, 1, 1, alpha);
        frame.color = new Color(1, 1, 1, alpha);
        statusIcon.color = new Color(1, 1, 1, alpha);
        healthBarBack.color = new Color(1, 1, 1, Mathf.Clamp(alpha, 0, 0.5f));
        healthBar.color = new Color(1, 1, 1, alpha);
        healthBarFront.color = new Color(1, 1, 1, alpha);
        healthText.color = new Color(1, 1, 1, alpha);
        actionBarBack.color = new Color(1, 1, 1, alpha);
        actionBar.color = new Color(1, 1, 1, alpha);
        actionText.color = new Color(1, 1, 1, alpha);
        radialBack.color = new Color(0.2f, 0.2f, 0.2f, Mathf.Clamp(alpha, 0, 0.7f));
        radialFill.color = new Color(1, 1, 1, alpha);
    }

    public void SetGlowColor(Color color)
    {
        glowColor = color;
        this.glow.color = color;
    }

    public void SetGlowAlpha(float alpha)
    {
        glowColor = new Color(glowColor.r, glowColor.g, glowColor.b, alpha);
        this.glow.color = glowColor;
    }

    public void SetBackColor(Color color)
    {
        const float maxAlpha = 0.5f;
        backColor = new Color(color.r, color.g, color.b, Mathf.Min(color.a, maxAlpha));
        this.back.color = color;
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
}

