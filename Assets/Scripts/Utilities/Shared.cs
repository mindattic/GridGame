using Game.Models;
using System.Linq;
using UnityEngine;

public class Shared
{
    public static RectFloat ScreenInWorldUnits
    {
        get
        {
            Vector2 topRightCorner = new Vector2(1f, 1f);
            Vector2 edgeVector = Camera.main.ViewportToWorldPoint(topRightCorner);
            var width = edgeVector.x * 2f;
            var height = edgeVector.y * 2f;
            return new RectFloat(0, width, height, 0);
        }
    }

    public static RectFloat ScreenInPixels
    {
        get
        {
            return new RectFloat(0, Screen.width, Screen.height, 0);
        }
    }

    public static Vector3 ConvertWorldToScreenPosition(Vector3 position)
    {
        return Camera.main.WorldToScreenPoint(position);
    }

    public static Vector3 ConvertScreenToWorldPosition(Vector3 position)
    {
        return Camera.main.ScreenToWorldPoint(position);
    }

    public static bool IsInRange(float a, float b, float range)
    {
        return a <= b + range && a >= b - range;
    }

    public static bool IsBetween(float a, float b, float c)
    {
        return a > b && a < c;
    }


    /// <summary>
    /// Assumes sprite is facing right, if facing up subtract 90 from angle (or fix sprite)
    /// </summary>
    /// <param name="target"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    public static Quaternion RotationByDirection(Vector3 target, Vector3 source)
    {
        var direction = target - source;
        var angle = Vector2.SignedAngle(Vector2.right, direction);
        var targetRotation = new Vector3(0, 0, angle);
        var rotation = Quaternion.Euler(targetRotation);
        return rotation;
    }



    public static Alignment AssignAlignment(ActorInstance actor1, ActorInstance actor2, Axis axis)
    {

        ActorInstance highestActor = axis == Axis.Vertical ? actor1.location.y > actor2.location.y ? actor1 : actor2 : actor1.location.x > actor2.location.x ? actor1 : actor2;
        ActorInstance lowestActor = (axis == Axis.Vertical) ? actor1.location.y < actor2.location.y ? actor1 : actor2 : actor1.location.x < actor2.location.x ? actor1 : actor2;
        float ceiling = axis == Axis.Vertical ? highestActor.location.y : highestActor.location.x;
        float floor = axis == Axis.Vertical ? lowestActor.location.y : lowestActor.location.x;

        var alignment = new Alignment();

        if (axis == Axis.Vertical)
        {
            alignment.enemies = GameManager.instance.actors.Where(x => x.IsPlaying && x.IsEnemy && x.IsSameColumn(actor1.location) && IsBetween(x.location.y, floor, ceiling)).OrderBy(x => x.location.y).ToList();
            alignment.players = GameManager.instance.actors.Where(x => x.IsPlaying && x.IsPlayer && x.IsSameColumn(actor1.location) && IsBetween(x.location.y, floor, ceiling)).OrderBy(x => x.location.y).ToList();
            alignment.gaps = GameManager.instance.tiles.Where(x => !x.IsOccupied && actor1.IsSameColumn(x.location) && IsBetween(x.location.y, floor, ceiling)).OrderBy(x => x.location.y).ToList();
        }
        else if (axis == Axis.Horizontal)
        {
            alignment.enemies = GameManager.instance.actors.Where(x => x.IsPlaying && x.IsEnemy && x.IsSameRow(actor1.location) && IsBetween(x.location.x, floor, ceiling)).OrderBy(x => x.location.x).ToList();
            alignment.players = GameManager.instance.actors.Where(x => x.IsPlaying && x.IsPlayer && x.IsSameRow(actor1.location) && IsBetween(x.location.x, floor, ceiling)).OrderBy(x => x.location.x).ToList();
            alignment.gaps = GameManager.instance.tiles.Where(x => !x.IsOccupied && actor1.IsSameRow(x.location) && IsBetween(x.location.x, floor, ceiling)).OrderBy(x => x.location.x).ToList();
        }

        return alignment;
    }

    public static Color RGB(float r, float g, float b)
    {
        return new Color(
            Mathf.Clamp(r, 0, 255) / 255,
            Mathf.Clamp(g, 0, 255) / 255,
            Mathf.Clamp(b, 0, 255) / 255,
            255 / 255);
    }

    public static Color RGBA(float r, float g, float b, float a)
    {
        return new Color(
            Mathf.Clamp(r, 0, 255) / 255,
            Mathf.Clamp(g, 0, 255) / 255,
            Mathf.Clamp(b, 0, 255) / 255,
            Mathf.Clamp(a, 0, 255) / 255);
    }



}

