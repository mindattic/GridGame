using UnityEngine;

public class Common
{
    public static Collider2D GetHighestObject(Collider2D[] results)
    {
        int highestValue = 0;
        Collider2D highestObject = results[0];
        foreach (Collider2D col in results)
        {
            Renderer ren = col.gameObject.GetComponent<Renderer>();
            if (ren && ren.sortingOrder > highestValue)
            {
                highestValue = ren.sortingOrder;
                highestObject = col;
            }
        }
        return highestObject;
    }

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



    //public static bool InRange(float a, float b, float range)
    //{

    //    return a <= b + range && a >= b - range;
    //}
}
