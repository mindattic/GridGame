using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackLineManager : ExtendedMonoBehavior
{
    [SerializeField] public GameObject AttackLinePrefab;
    public Dictionary<string, AttackLineBehavior> AttackLines = new Dictionary<string, AttackLineBehavior>();


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
        var key = NameFormat.Replace("{0}", pair.Actor1.name).Replace("{1}", pair.Actor2.name);
        var altKey = NameFormat.Replace("{0}", pair.Actor2.name).Replace("{1}", pair.Actor1.name);
        if (AttackLines.ContainsKey(key) || AttackLines.ContainsKey(altKey))
            return;

        var prefab = Instantiate(AttackLinePrefab, Vector2.zero, Quaternion.identity);
        var attackLine = prefab.GetComponent<AttackLineBehavior>();
        attackLine.name = key;
        attackLine.Parent = Board.transform;
        attackLine.Spawn(pair);

        AttackLines.Add(key, attackLine);
    }


    public void Destroy(ActorPair pair)
    {
        var key = NameFormat.Replace("{0}", pair.Actor1.name).Replace("{1}", pair.Actor2.name);
        if (!AttackLines.ContainsKey(key))
            return;

        AttackLines[key].Destroy();
        AttackLines.Remove(key);
    }

    public void Clear()
    {
        AttackLines.ToList().ForEach(x => x.Value.Destroy());
        AttackLines.Clear();
    }

}
