using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SupportLineManager : ExtendedMonoBehavior
{
    //Variables
    [SerializeField] public GameObject supportLinePrefab;
    public List<SupportLineBehavior> supportLines = new List<SupportLineBehavior>();

    public bool Exists(ActorPair pair)
    {
        var name = NameFormat.SupportLine(pair);
        return supportLines.Count(x => x.name == name) > 0;
    }

    public void Spawn(ActorPair pair)
    {
        if (Exists(pair))
            return;

        var prefab = Instantiate(supportLinePrefab, Vector2.zero, Quaternion.identity);
        SupportLineBehavior supportLine = prefab.GetComponent<SupportLineBehavior>();
        supportLines.Add(supportLine);
        supportLine.Spawn(pair);
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

    public void Clear()
    {
        supportLines.ForEach(x => x.Destroy());
        supportLines.Clear();

        //IEnumerator _()
        //{
        //    foreach (var supportLine in supportLines)
        //    {
        //        while (supportLine != null && supportLine.alpha > 0)
        //        {
        //            yield return supportLine.Despawn();
        //        }

        //        supportLine.Destroy();
        //    }
        //    supportLines.Clear();
        //}

        //StartCoroutine(_());
    }

}
