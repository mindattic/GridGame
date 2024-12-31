using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SupportLineManager : MonoBehaviour
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

    public void Despawn(ActorPair pair)
    {
        var list = supportLines.Where(x => x.name.Contains(pair.actor1.name) || x.name.Contains(pair.actor2.name));
        foreach (var x in list)
        {
            x.Despawn();
        }
    }

    public void Clear()
    {
        supportLines.ForEach(x => Destroy(x.gameObject));
        supportLines.Clear();
    }
}
