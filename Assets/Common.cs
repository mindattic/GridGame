using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Common
{
    public static Vector2 ViewportToWorldSize
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

    public static List<Vector2Int> RandomLocations()
    {
        return GameManager.instance.boardLocations.OrderBy(x => Guid.NewGuid()).ToList();
    }

    //public static bool InRange(float sw, float b, float range)
    //{

    //    return sw <= b + range && sw >= b - range;
    //}
}
