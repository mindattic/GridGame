using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;


public class Common
{
    private float cellSize => Global.instance.cellSize;
    private Vector2 cellScale => Global.instance.cellScale;




    public Collider2D GetHighestObject(Collider2D[] results)
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
            Vector2 topRightCorner = new Vector2(1, 1);
            Vector2 edgeVector = Camera.main.ViewportToWorldPoint(topRightCorner);
            var width = edgeVector.x * 2;
            return width;
        }
    }

    public static float GetScreenToWorldHeight
    {
        get
        {
            Vector2 topRightCorner = new Vector2(1, 1);
            Vector2 edgeVector = Camera.main.ViewportToWorldPoint(topRightCorner);
            var height = edgeVector.y * 2;
            return height;
        }
    }



    public static GameObject FindClosestByTag(Vector3 position, string tag)
    {
        return GameObject.FindGameObjectsWithTag(tag).OrderBy(x => Vector3.Distance(x.transform.position, position)).FirstOrDefault();
    }



    public static GameObject GetCellByCoordinates(int x, int y)
    {
        return GameObject.Find($"Cell_{x}x{y}");
    }

  

    //public static Vector2 GetPositionByCoordinates(Coordinates coordinates)
    //{
    //    float x = coordinates.x * Global.instance.cellSize;
    //    float y = coordinates.y * Global.instance.cellSize;
    //    return new Vector2(x, y);
    //}

    //public static Vector2 GetPositionByCoordinates(int x, int y)
    //{

    //    return GetPositionByCoordinates(new Coordinates(x, y));
    //}


}
