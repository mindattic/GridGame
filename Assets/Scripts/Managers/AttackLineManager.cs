using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Behaviors
{
    public class AttackLineManager : ExtendedMonoBehavior
    {
        [SerializeField] public GameObject AttackLinePrefab;
        public List<AttackLineBehavior> attackLines = new List<AttackLineBehavior>();
        private const string NameFormat = "AttackLine_{0)+{1}";

        public bool HasPair(ActorPair pair)
        {
            return attackLines.Any(x =>
            (x.pair.actor1 == pair.actor1 && x.pair.actor2 == pair.actor2)
            || (x.pair.actor1 == pair.actor2 && x.pair.actor2 == pair.actor1));
        }

        public void Spawn(ActorPair pair)
        {
            if (HasPair(pair))
                return;

            var prefab = Instantiate(AttackLinePrefab, Vector2.zero, Quaternion.identity);
            var attackLine = prefab.GetComponent<AttackLineBehavior>();
            attackLines.Add(attackLine);
            attackLine.name = NameFormat.Replace("{0}", pair.actor1.Name).Replace("{1}", pair.actor2.Name);
            attackLine.parent = board.transform;
            attackLine.pair = pair;
            attackLine.Spawn();
        }


        public IEnumerator Despawn(ActorPair pair)
        {
            var attackLine = attackLines.FirstOrDefault(x => x.pair == pair);
            while (attackLine != null && attackLine.alpha > 0)
            {
                yield return attackLine.Despawn();
            }
        }

        public void DespawnAsync(ActorPair pair)
        {
            StartCoroutine(Despawn(pair));
        }

        public void Clear()
        {
            IEnumerator _()
            {
                foreach (var attackLine in attackLines)
                {
                    while (attackLine != null && attackLine.alpha > 0)
                    {
                        yield return attackLine.Despawn();
                    }

                    attackLine.Destroy();
                }
                attackLines.Clear();
            }

            StartCoroutine(_());
        }

    }

}
