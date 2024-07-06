using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SupportLineManager : ExtendedMonoBehavior
{
    [SerializeField] public GameObject supportLinePrefab;

    public List<SupportLineBehavior> supportLines = new List<SupportLineBehavior>();
    private const string NameFormat = "SupportLine_{0)+{1}";

    public bool HasPair(ActorPair pair)
    {
        return supportLines.Any(x => 
        (x.pair.actor1 == pair.actor1 && x.pair.actor2 == pair.actor2) 
        || (x.pair.actor1 == pair.actor2 && x.pair.actor2 == pair.actor1));
    }

    public void Spawn(ActorPair pair)
    {
        if (HasPair(pair))
            return;

        var prefab = Instantiate(supportLinePrefab, Vector2.zero, Quaternion.identity);
        var supportLine = prefab.GetComponent<SupportLineBehavior>();
        supportLines.Add(supportLine);
        supportLine.name = NameFormat.Replace("{0}", pair.actor1.Name).Replace("{1}", pair.actor2.Name);
        supportLine.parent = board.transform;
        supportLine.pair = pair;
        supportLine.Spawn();
    }


    public IEnumerator Despawn(ActorPair pair)
    {
        var supportLine = supportLines.FirstOrDefault(x => x.pair == pair);
        while (supportLine != null && supportLine.alpha > 0)
        {
            yield return supportLine.Despawn();
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
            foreach (var supportLine in supportLines)
            {
                while (supportLine != null && supportLine.alpha > 0)
                {
                    yield return supportLine.Despawn();
                }

                supportLine.Destroy();
            }
            supportLines.Clear();
        }

        StartCoroutine(_());
    }

}
