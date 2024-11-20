using Game.Instances;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Behaviors
{
    public class AttackLineManager : ExtendedMonoBehavior
    {
        //Variables
        [SerializeField] public GameObject AttackLinePrefab;
        public List<AttackLineInstance> attackLines = new List<AttackLineInstance>();

        public bool Exists(ActorPair pair)
        {
            var name = NameFormat.AttackLine(pair);
            return attackLines.Any(x => x.name == name);
        }

        public void Spawn(ActorPair pair)
        {
            if (Exists(pair))
                return;

            var prefab = Instantiate(AttackLinePrefab, Vector2.zero, Quaternion.identity);
            var instance = prefab.GetComponent<AttackLineInstance>();
            attackLines.Add(instance);
            instance.Spawn(pair);

        }

        public void DespawnAsync(ActorPair pair)
        {
            var list = attackLines.Where(x => x.name.Contains(pair.actor1.name) || x.name.Contains(pair.actor2.name));
            foreach (var x in list)
            {
                x.DespawnAsync();
            }
        }

        public void DespawnAll()
        {
            attackLines.ForEach(x => x.DespawnAsync());
        }

        public void Clear()
        {
            attackLines.ForEach(x => Destroy(x.gameObject));
            attackLines.Clear();
        }

    }

}
