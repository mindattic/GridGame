using System.Collections;
using UnityEngine;

public class DottedLineInstance : MonoBehaviour
{
    #region Properties
    protected Vector3 tileScale => GameManager.instance.tileScale;
    protected ResourceManager resourceManager => GameManager.instance.resourceManager;
    #endregion


    //Method which is used for initialization tasks that need to occur before the game starts 
    private void Awake()
    {


    }


    public Transform parent
    {
        get => gameObject.transform.parent;
        set => gameObject.transform.SetParent(value, true);
    }

    public Vector3 position
    {
        get => gameObject.transform.position;
        set => gameObject.transform.position = value;
    }

    public Quaternion Rotation
    {
        get => gameObject.transform.rotation;
        set => gameObject.transform.rotation = value;
    }

    SpriteRenderer spriteRenderer;


    public Sprite sprite
    {
        get => spriteRenderer.sprite;
        set => spriteRenderer.sprite = value;
    }


    public void Spawn(DottedLineSegment segment, Vector2Int location)
    {
        this.position = Geometry.GetPositionByLocation(location);
        this.transform.localScale = tileScale;

        spriteRenderer = GetComponent<SpriteRenderer>(); // Ensure SpriteRenderer is assigned
                                                         // Set sprite and rotation based on segment
        switch (segment)
        {
            case DottedLineSegment.Vertical:
                sprite = resourceManager.DottedLine("Line").sprite;
                Rotation = Quaternion.identity; // Default orientation
                break;

            case DottedLineSegment.Horizontal:
                sprite = resourceManager.DottedLine("Line").sprite;
                Rotation = Quaternion.Euler(0, 0, 90); // Rotate 90 degrees for horizontal
                break;

            case DottedLineSegment.TurnTopLeft:
                sprite = resourceManager.DottedLine("Turn").sprite;
                Rotation = Quaternion.Euler(0, 0, -180); // Corrected rotation
                break;

            case DottedLineSegment.TurnTopRight:
                sprite = resourceManager.DottedLine("Turn").sprite;
                Rotation = Quaternion.Euler(0, 0, 90); // Corrected rotation
                break;

            case DottedLineSegment.TurnBottomLeft:
                sprite = resourceManager.DottedLine("Turn").sprite;
                Rotation = Quaternion.Euler(0, 0, -90); // Corrected rotation
                break;

            case DottedLineSegment.TurnBottomRight:
                sprite = resourceManager.DottedLine("Turn").sprite;
                Rotation = Quaternion.identity; // Z = 0
                break;

            case DottedLineSegment.ArrowUp:
                sprite = resourceManager.DottedLine("Arrow").sprite;
                Rotation = Quaternion.identity; // Default orientation
                break;

            case DottedLineSegment.ArrowDown:
                sprite = resourceManager.DottedLine("Arrow").sprite;
                Rotation = Quaternion.Euler(0, 0, 180); // Corrected rotation
                break;

            case DottedLineSegment.ArrowLeft:
                sprite = resourceManager.DottedLine("Arrow").sprite;
                Rotation = Quaternion.Euler(0, 0, 90); // Corrected rotation
                break;

            case DottedLineSegment.ArrowRight:
                sprite = resourceManager.DottedLine("Arrow").sprite;
                Rotation = Quaternion.Euler(0, 0, -90); // Corrected rotation
                break;

            default:
                Debug.LogError($"Unhandled segment type: {segment}");
                break;
        }

    }
}


