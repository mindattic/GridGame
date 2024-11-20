using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SupportLineManager : ExtendedMonoBehavior
{
    //Variables
    [SerializeField] public GameObject supportLinePrefab;
    public List<SupportLineInstance> supportLines = new List<SupportLineInstance>();

    public bool Exists(ActorPair pair)
    {
        var name = NameFormat.SupportLine(pair);
        return supportLines.Any(x => x.name == name);
    }

    public void Spawn(ActorPair pair)
    {
        if (Exists(pair))
            return;

        var prefab = Instantiate(supportLinePrefab, Vector2.zero, Quaternion.identity);
        var instance = prefab.GetComponent<SupportLineInstance>();
        supportLines.Add(instance);
        instance.Spawn(pair);
    }


    public IEnumerator Despawn(ActorPair pair)
    {
        var list = supportLines.Where(x => x.name.Contains(pair.actor1.name) || x.name.Contains(pair.actor2.name)).ToList();
        foreach (var x in list)
        {
            while (x.alpha > 0)
            {
                yield return x.Despawn();
            }
        }
    }

    public void DespawnAsync(ActorPair pair)
    {
        var list = supportLines.Where(x => x.name.Contains(pair.actor1.name) || x.name.Contains(pair.actor2.name));
        foreach (var x in list)
        {
            x.DespawnAsync();
        }
    }

    public void DespawnAll()
    {
        supportLines.ForEach(x => x.DespawnAsync());

    }

    public void Clear()
    {
        supportLines.ForEach(x => Destroy(x.gameObject));
        supportLines.Clear();
    }
}
