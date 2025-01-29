using Game.Behaviors;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DottedLineInstance : MonoBehaviour
{
    #region Properties
    protected Vector3 tileScale => GameManager.instance.tileScale;
    protected ResourceManager resourceManager => GameManager.instance.resourceManager;
    protected DottedLineManager dottedLineManager => GameManager.instance.dottedLineManager;
    protected LogManager logManager => GameManager.instance.logManager;
    protected ActorInstance selectedPlayer => GameManager.instance.selectedPlayer;
    protected bool hasSelectedPlayer => selectedPlayer != null;
    #endregion

    SpriteRenderer spriteRenderer;
    Sprite line;
    Sprite turn;
    Sprite arrow;
    public Vector2Int location;
    public DottedLineSegment segment;
    //public bool isOccupied => hasSelectedPlayer && selectedPlayer.location == location;

    public Vector2Int top => location + new Vector2Int(0, -1);
    public Vector2Int right => location + new Vector2Int(1, 0);
    public Vector2Int bottom => location + new Vector2Int(0, 1);
    public Vector2Int left => location + new Vector2Int(-1, 0);

    public List<Vector2Int> connections = new List<Vector2Int>();

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

    public void SetColor()
    {
        spriteRenderer.color = ColorHelper.Translucent.Yellow;
    }

    public void ResetColor()
    {
        spriteRenderer.color = ColorHelper.Translucent.White;
    }

    public void Spawn(DottedLineSegment segment, Vector2Int location)
    {
        this.segment = segment;
        this.location = location;
        this.position = Geometry.GetPositionByLocation(this.location);
        this.transform.localScale = tileScale;

        // Load resources
        var line = resourceManager.Sprite("DottedLine").Value;
        var turn = resourceManager.Sprite("DottedLineTurn").Value;
        var arrow = resourceManager.Sprite("DottedLineArrow").Value;

        connections.Clear(); // Ensure connections are reset
        connections.Add(location); // Add self-location to connections

        switch (this.segment)
        {
            case DottedLineSegment.Vertical:
                sprite = line;
                rotation = Quaternion.identity;
                connections.AddRange(new[] { top, bottom });
                break;

            case DottedLineSegment.Horizontal:
                sprite = line;
                rotation = Quaternion.Euler(0, 0, 90);
                connections.AddRange(new[] { left, right });
                break;

            case DottedLineSegment.TurnTopLeft:
                sprite = turn;
                rotation = Quaternion.Euler(0, 0, -180);
                connections.AddRange(new[] { top, left });
                break;

            case DottedLineSegment.TurnTopRight:
                sprite = turn;
                rotation = Quaternion.Euler(0, 0, 90);
                connections.AddRange(new[] { top, right });
                break;

            case DottedLineSegment.TurnBottomLeft:
                sprite = turn;
                rotation = Quaternion.Euler(0, 0, -90);
                connections.AddRange(new[] { bottom, left });
                break;

            case DottedLineSegment.TurnBottomRight:
                sprite = turn;
                rotation = Quaternion.identity;
                connections.AddRange(new[] { bottom, right });
                break;

            case DottedLineSegment.ArrowUp:
                sprite = arrow;
                rotation = Quaternion.identity;
                connections.Add(bottom);
                break;

            case DottedLineSegment.ArrowDown:
                sprite = arrow;
                rotation = Quaternion.Euler(0, 0, 180);
                connections.Add(top);
                break;

            case DottedLineSegment.ArrowLeft:
                sprite = arrow;
                rotation = Quaternion.Euler(0, 0, 90);
                connections.Add(right);
                break;

            case DottedLineSegment.ArrowRight:
                sprite = arrow;
                rotation = Quaternion.Euler(0, 0, -90);
                connections.Add(left);
                break;

            default:
                logManager.Warning($"Unhandled segment type: {segment}");
                break;
        }
    }

    //public List<Vector2Int> GetConnections()
    //{
    //    var connections = new List<Vector2Int> { location };

    //    Vector2Int up = location + new Vector2Int(0, 1);
    //    Vector2Int down = location + new Vector2Int(0, -1);
    //    Vector2Int left = location + new Vector2Int(-1, 0);
    //    Vector2Int right = location + new Vector2Int(1, 0);

    //    switch (segment)
    //    {
    //        case DottedLineSegment.Vertical:
    //            connections.Add(up);
    //            connections.Add(down);
    //            break;

    //        case DottedLineSegment.Horizontal:
    //            connections.Add(left);
    //            connections.Add(right);
    //            break;

    //        case DottedLineSegment.TurnTopLeft:
    //            connections.Add(up);
    //            connections.Add(left);
    //            break;

    //        case DottedLineSegment.TurnTopRight:
    //            connections.Add(up);
    //            connections.Add(right);
    //            break;

    //        case DottedLineSegment.TurnBottomLeft:
    //            connections.Add(down);
    //            connections.Add(left);
    //            break;

    //        case DottedLineSegment.TurnBottomRight:
    //            connections.Add(down);
    //            connections.Add(right);
    //            break;

    //        case DottedLineSegment.ArrowUp:
    //            connections.Add(up);
    //            break;

    //        case DottedLineSegment.ArrowDown:
    //            connections.Add(down);
    //            break;

    //        case DottedLineSegment.ArrowLeft:
    //            connections.Add(left);
    //            break;

    //        case DottedLineSegment.ArrowRight:
    //            connections.Add(right);
    //            break;

    //        default:
    //            logManager.Warning($"Unhandled segment type: {segment}");
    //            break;
    //    }

    //    return connections;
    //}



    public void Despawn()
    {
        dottedLineManager.Despawn(this);
    }
}


