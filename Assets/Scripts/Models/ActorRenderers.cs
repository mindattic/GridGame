using TMPro;
using UnityEngine;

public class ActorRenderers
{
    public ActorRenderers() { }

    public SpriteRenderer glow;
    public SpriteRenderer back;
    public SpriteRenderer thumbnail;
    public SpriteRenderer frame;
    public SpriteRenderer healthBarBack;
    public SpriteRenderer healthBar;
    public SpriteRenderer statusIcon;
    public TextMeshPro turnDelay;
    public TextMeshPro healthText;

    public Color glowColor;
    public Color backColor;

    public void SetColor(Color color)
    {
        thumbnail.color = color;
        frame.color = color;
        statusIcon.color = new Color(1, 1, 1, color.a);
        turnDelay.color = new Color(1, 1, 1, color.a);
        healthBarBack.color = new Color(1, 1, 1, color.a);
        healthBar.color = new Color(1, 1, 1, color.a);
        healthText.color = new Color(1, 1, 1, color.a);
    }

    public void SetGlowColor(Color color)
    {
        glowColor = color;
        this.glow.color = color;
    }

    public void SetGlowAlpha(float alpha = 1f)
    {
        glowColor = new Color(glowColor.r, glowColor.g, glowColor.b, alpha);
        this.glow.color = glowColor;
    }

    public void SetBackColor(Color color, float alpha = 0.5f)
    {
        backColor = new Color(color.r, color.g, color.b, Mathf.Min(color.a, alpha));
        this.back.color = color;
    }

    public void SetBackAlpha(float alpha = 0.5f)
    {
        backColor = new Color(backColor.r, backColor.g, backColor.b, Mathf.Min(alpha, 0.5f));
        this.back.color = backColor;
    }
}

