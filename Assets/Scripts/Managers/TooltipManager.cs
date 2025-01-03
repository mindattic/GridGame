using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Manager
{
    public class TooltipManager : MonoBehaviour
    {
        //Variables
        [SerializeField] public GameObject tooltipPrefab;


        public void Spawn(string text, Vector3 position)
        {
            var prefab = Instantiate(tooltipPrefab, Vector2.zero, Quaternion.identity);
            var instance = prefab.GetComponent<TooltipInstance>();
            instance.name = $"Tooltip_{Guid.NewGuid()}";
            instance.Spawn(text, position);
        }

        public void Clear()
        {
            GameObject.FindGameObjectsWithTag(Tag.Tooltip).ToList().ForEach(x => Destroy(x));
        }


    }

}