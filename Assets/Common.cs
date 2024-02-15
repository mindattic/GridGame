using System.Collections.Generic;
using UnityEngine;

public class Common
{
    public static float GetScreenToWorldWidth
    {
        get
        {
            Vector2 topRightCorner = new Vector2(1f, 1f);
            Vector2 edgeVector = Camera.main.ViewportToWorldPoint(topRightCorner);
            var width = edgeVector.x * 2f;
            return width;
        }
    }

    public static float GetScreenToWorldHeight
    {
        get
        {
            Vector2 topRightCorner = new Vector2(1f, 1f);
            Vector2 edgeVector = Camera.main.ViewportToWorldPoint(topRightCorner);
            var height = edgeVector.y * 2f;
            return height;
        }
    }



    public static List<Vector2Int> GenerateUniqueLocations(int amount = 10)
    {
        var rnd = GameManager.instance.rnd;
        var locations = new List<Vector2Int>();
        do
        {
            var l = new Vector2Int(rnd.Next(1, 5), rnd.Next(1, 8));
            if (!locations.Contains(l))
                locations.Add(l);
        } while (locations.Count < amount);

        return locations;
    }

    //public static bool InRange(float sw, float b, float range)
    //{

    //    return sw <= b + range && sw >= b - range;
    //}
}
