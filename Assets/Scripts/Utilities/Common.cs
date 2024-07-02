using UnityEngine;

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
}

