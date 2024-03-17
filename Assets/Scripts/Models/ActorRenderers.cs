using TMPro;
using UnityEngine;

public class ActorRenderers
{
    public ActorRenderers() { }

    public SpriteRenderer glow;
    public SpriteRenderer thumbnail;
    public SpriteRenderer frame;
    public SpriteRenderer healthBarBack;
    public SpriteRenderer healthBar;
    public SpriteRenderer statusIcon;
    public TextMeshPro turnDelay;
    public TextMeshPro healthText;

    public void SetColor(Color color) {
        //glow.color = color;
        thumbnail.color = color;
        frame.color = color;
        statusIcon.color = color;
        turnDelay.color = color;
        healthBarBack.color = color;
        healthBar.color = color;
        healthText.color = color;
    }

}

