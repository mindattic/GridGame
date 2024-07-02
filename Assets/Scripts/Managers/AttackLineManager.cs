using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Behaviors
{
    public class AttackLineManager : ExtendedMonoBehavior
    {
        [SerializeField] public GameObject AttackLinePrefab;
        public Dictionary<string, AttackLineBehavior> attackLines = new Dictionary<string, AttackLineBehavior>();


        private const string NameFormat = "AttackLine_{0)+{1}";


        private void Start()
        {

        }

        private void Update()
        {

        }

        public void Spawn(ActorPair pair)
        {
            //Determine if there is a duplicate
            var key = NameFormat.Replace("{0}", pair.actor1.name).Replace("{1}", pair.actor2.name);
            var altKey = NameFormat.Replace("{0}", pair.actor2.name).Replace("{1}", pair.actor1.name);
            if (attackLines.ContainsKey(key) || attackLines.ContainsKey(altKey))
                return;

            var prefab = Instantiate(AttackLinePrefab, Vector2.zero, Quaternion.identity);
            var attackLine = prefab.GetComponent<AttackLineBehavior>();
            attackLine.name = key;
            attackLine.parent = board.transform;
            attackLine.Spawn(pair);

            attackLines.Add(key, attackLine);
        }


        public void Despawn(ActorPair pair)
        {
            var key = NameFormat.Replace("{0}", pair.actor1.name).Replace("{1}", pair.actor2.name);
            if (!attackLines.ContainsKey(key))
                return;

            attackLines[key].Despawn();
            attackLines.Remove(key);
        }

        public void Clear()
        {
            attackLines.ToList().ForEach(x => x.Value.Despawn());
            attackLines.Clear();
        }

    }

}
