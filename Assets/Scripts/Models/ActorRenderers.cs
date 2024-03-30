
using TMPro;
using UnityEngine;

public class ActorRenderers
{
    public ActorRenderers() { }

    public SpriteRenderer glow;
    public SpriteRenderer shadow;
    public SpriteRenderer thumbnail;
    public SpriteRenderer frame;
    public SpriteRenderer healthBarBack;
    public SpriteRenderer healthBar;
    public SpriteRenderer statusIcon;
    public TextMeshPro turnDelay;
    public TextMeshPro healthText;

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

    public void SetGlow(Color color)
    {

        this.glow.color = color;
    }

    public void SetShadow(Color color)
    {
        this.shadow.color = color;
    }


}

