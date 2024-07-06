using System.Linq;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Common
{
    public static Vector2 ScreenToWorldSize
    {
        get
        {
            Vector2 topRightCorner = new Vector2(1f, 1f);
            Vector2 edgeVector = Camera.main.ViewportToWorldPoint(topRightCorner);
            var width = edgeVector.x * 2f;
            var height = edgeVector.y * 2f;
            return new Vector2(width, height);
        }
    }

    public static Vector3 WorldToScreenPosition(Vector3 position)
    {
        return Camera.main.WorldToScreenPoint(position);
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



    public static Alignment AssignAlignment(ActorPair pair)
    {
        var alignment = new Alignment();

        if (pair.axis == Axis.Vertical)
        {
            alignment.enemies = GameManager.instance.actors.Where(x => x.IsPlaying && x.IsEnemy && x.IsSameColumn(pair.actor1.location) && IsBetween(x.location.y, pair.floor, pair.ceiling)).OrderBy(x => x.location.y).ToList();
            alignment.players = GameManager.instance.actors.Where(x => x.IsPlaying && x.IsPlayer && x.IsSameColumn(pair.actor1.location) && IsBetween(x.location.y, pair.floor, pair.ceiling)).OrderBy(x => x.location.y).ToList();
            alignment.gaps = GameManager.instance.tiles.Where(x => !x.IsOccupied && pair.actor1.IsSameColumn(x.location) && IsBetween(x.location.y, pair.floor, pair.ceiling)).OrderBy(x => x.location.y).ToList();
        }
        else if (pair.axis == Axis.Horizontal)
        {
            alignment.enemies = GameManager.instance.actors.Where(x => x.IsPlaying && x.IsEnemy && x.IsSameRow(pair.actor1.location) && IsBetween(x.location.x, pair.floor, pair.ceiling)).OrderBy(x => x.location.x).ToList();
            alignment.players = GameManager.instance.actors.Where(x => x.IsPlaying && x.IsPlayer && x.IsSameRow(pair.actor1.location) && IsBetween(x.location.x, pair.floor, pair.ceiling)).OrderBy(x => x.location.x).ToList();
            alignment.gaps = GameManager.instance.tiles.Where(x => !x.IsOccupied && pair.actor1.IsSameRow(x.location) && IsBetween(x.location.x, pair.floor, pair.ceiling)).OrderBy(x => x.location.x).ToList();
        }

        return alignment;
    }



}

