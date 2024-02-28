using System;
using System.Collections.Generic;
using System.Linq;
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



    public static List<Vector2Int> RandomLocations()
    {
        return GameManager.instance.boardLocations.OrderBy(x => Guid.NewGuid()).ToList();
    }

    public static bool IsInRange(float a, float b, float range)
    {
        return a <= b + range && a >= b - range;
    }


    public static Color ColorRGBA(float r, float g, float b, float a = 255)
    {
        return new Color(
            Mathf.Clamp(r, 0, 255) / 255f, 
            Mathf.Clamp(g, 0, 255) / 255f, 
            Mathf.Clamp(b, 0, 255) / 255f, 
            Mathf.Clamp(a, 0, 255) / 255f);
    }




    public static int CalculateTurnDelay()
    {
        //TODO: Use enemy statistics to determine turn delay...
        return Random.Int(2, 4);
    }

}
