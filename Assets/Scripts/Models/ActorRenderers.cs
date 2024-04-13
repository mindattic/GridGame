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
    public TextMeshPro healthText;
    public SpriteRenderer actionBarBack;
    public SpriteRenderer actionBar;
    public TextMeshPro actionText;

 
    public void SetColor(Color color)
    {
        if (thumbnail.enabled)
            thumbnail.color = color;

        if (frame.enabled)
            frame.color = color;

        if (statusIcon.enabled)
            statusIcon.color = new Color(1, 1, 1, color.a);

        if (healthBarBack.enabled)
            healthBarBack.color = new Color(1, 1, 1, color.a);

        if (healthBar.enabled)
            healthBar.color = new Color(1, 1, 1, color.a);

        if (healthText.enabled)
            healthText.color = new Color(1, 1, 1, color.a);

        if (actionBarBack.enabled)
            actionBarBack.color = new Color(1, 1, 1, color.a);

        if (actionBar.enabled)
            actionBar.color = new Color(1, 1, 1, color.a);

        if (actionText.enabled)
            actionText.color = new Color(1, 1, 1, color.a);
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

