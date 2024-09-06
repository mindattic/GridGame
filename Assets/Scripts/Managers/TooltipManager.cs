using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Manager
{
    public class TooltipManager : ExtendedMonoBehavior
    {
        //Variables
        [SerializeField] public GameObject tooltipPrefab;


        public void Spawn(string text, Vector3 position)
        {
            GameObject prefab = Instantiate(tooltipPrefab, Vector2.zero, Quaternion.identity);
            var tooltip = prefab.GetComponent<TooltipBehavior>();
            tooltip.name = $"Tooltip_{Guid.NewGuid()}";
            tooltip.Spawn(text, position);
        }

        public void Clear()
        {
            GameObject.FindGameObjectsWithTag(Tag.Tooltip).ToList().ForEach(x => Destroy(x));
        }


    }

}