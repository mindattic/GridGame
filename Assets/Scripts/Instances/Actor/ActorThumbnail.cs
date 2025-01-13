using Assets.Scripts.Models;
using UnityEngine;

public class ActorThumbnail
{
    #region Properties

    protected ResourceManager resourceManager => GameManager.instance.resourceManager;
    protected ActorInstance selectedPlayer => GameManager.instance.selectedPlayer;
    protected ActorRenderers render => instance.render;
    protected ActorStats stats => instance.stats;
    #endregion

    private ActorInstance instance;
    public Texture2D texture; // The full 1024x1024 texture
    public Sprite sprite;


    public void Initialize(ActorInstance parentInstance)
    {
        this.instance = parentInstance;
    }

    public void CreateThumbnail()
    {
        // Get the full texture from the resource manager
        texture = resourceManager.ActorPortrait(instance.character.ToString()).texture;

        Size cutout = new Size(256, 256);

        // Adjustments for specific characters
        switch (instance.character)
        {
            case Character.Paladin:
                cutout = new Size(196, 196);
                break;
            case Character.Yeti:
                cutout = new Size(256, 256);
                break;
            case Character.Cleric:
                cutout = new Size(316, 316);
                break;
            case Character.Barbarian:
                cutout = new Size(256, 256);
                break;
            case Character.Ninja:
                cutout = new Size(196, 196);
                break;
            default:
                cutout = new Size(256, 256);
                break;
        }

        Vector2Int offset = new Vector2Int();
        offset.x = (texture.width - cutout.Width) / 2;
        offset.y = texture.height - cutout.Height;

        Vector2 pivot = new Vector2(0.5f, 0.5f); // Default pivot

        // Adjustments for specific characters
        switch (instance.character)
        {
            case Character.Paladin:
                offset.Shift(20, -25);
                break;
            case Character.Yeti:
                offset.Shift(-150, -100);
                break;
            case Character.Cleric:
                offset.Shift(0, -50);
                break;
            case Character.Barbarian:
                offset.Shift(-60, -80);
                break;
            case Character.Ninja:
                offset.Shift(00, 0);
                break;
            default:
                offset.Shift(0, 0);
                break;
        }

        // Clamp values to ensure the Rect doesn't go out of bounds
        offset.x = Mathf.Clamp(offset.x, 0, texture.width - cutout.Width);
        offset.y = Mathf.Clamp(offset.y, 0, texture.height - cutout.Height);

        // Define the portion to cut out
        Rect rect = new Rect(offset.x, offset.y, cutout.Width, cutout.Height);

        // Create a sprite from the selected portion of the texture
        sprite = Sprite.Create(texture, rect, pivot, 100f);

        // Assign the sprite to the SpriteRenderer
        render.thumbnail.sprite = sprite;
    }



}