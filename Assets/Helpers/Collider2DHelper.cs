using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Helpers
{
    public class Collider2DHelper
    {

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



    }
}
