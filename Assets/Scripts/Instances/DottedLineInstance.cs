using Game.Behaviors;
using System.Collections;
using UnityEngine;

public class DottedLineInstance : MonoBehaviour
{
    #region Properties
    protected Vector3 tileScale => GameManager.instance.tileScale;
    protected ResourceManager resourceManager => GameManager.instance.resourceManager;
    protected LogManager logManager => GameManager.instance.logManager;

    #endregion

    SpriteRenderer spriteRenderer;
    Sprite line;
    Sprite turn;
    Sprite arrow;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        line = resourceManager.Sprite("DottedLine").Value;
        turn = resourceManager.Sprite("DottedLineTurn").Value;
        arrow = resourceManager.Sprite("DottedLineArrow").Value;
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

    public Quaternion rotation
    {
        get => gameObject.transform.rotation;
        set => gameObject.transform.rotation = value;
    }



    public Sprite sprite
    {
        get => spriteRenderer.sprite;
        set => spriteRenderer.sprite = value;
    }


    public void Spawn(DottedLineSegment segment, Vector2Int location)
    {
        this.position = Geometry.GetPositionByLocation(location);
        this.transform.localScale = tileScale;

        //TODO: Put these in the "DottedLineManager" so they aren't reloaded each instance...
        var line = resourceManager.Sprite("DottedLine").Value;
        var turn = resourceManager.Sprite("DottedLineTurn").Value;
        var arrow = resourceManager.Sprite("DottedLineArrow").Value;

        switch (segment)
        {
            case DottedLineSegment.Vertical:
                sprite = line;
                rotation = Quaternion.identity; // Default orientation
                break;

            case DottedLineSegment.Horizontal:
                sprite = line;
                rotation = Quaternion.Euler(0, 0, 90);
                break;

            case DottedLineSegment.TurnTopLeft:
                sprite = turn;
                rotation = Quaternion.Euler(0, 0, -180);
                break;

            case DottedLineSegment.TurnTopRight:
                sprite = turn;
                rotation = Quaternion.Euler(0, 0, 90);
                break;

            case DottedLineSegment.TurnBottomLeft:
                sprite = turn;
                rotation = Quaternion.Euler(0, 0, -90);
                break;

            case DottedLineSegment.TurnBottomRight:
                sprite = turn;
                rotation = Quaternion.identity;
                break;

            case DottedLineSegment.ArrowUp:
                sprite = arrow;
                rotation = Quaternion.identity; // Default orientation
                break;

            case DottedLineSegment.ArrowDown:
                sprite = arrow;
                rotation = Quaternion.Euler(0, 0, 180);
                break;

            case DottedLineSegment.ArrowLeft:
                sprite = arrow;
                rotation = Quaternion.Euler(0, 0, 90);
                break;

            case DottedLineSegment.ArrowRight:
                sprite = arrow;
                rotation = Quaternion.Euler(0, 0, -90);
                break;

            default:
                logManager.Warning($"Unhandled segment type: {segment}");
                break;
        }

    }
}


