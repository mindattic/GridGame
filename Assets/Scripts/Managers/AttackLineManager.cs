using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackLineManager : ExtendedMonoBehavior
{
    [SerializeField] public GameObject attackLinePrefab;
    private List<AttackLineBehavior> attackLines = new List<AttackLineBehavior>();


    private const string NameFormat = "AttackLine_{0)+{1}";


    private void Start()
    {

    }

    private void Update()
    {

    }

    public void Spawn(ActorPair pair)
    {
        var prefab = Instantiate(attackLinePrefab, Vector2.zero, Quaternion.identity);
        var attackLine = prefab.GetComponent<AttackLineBehavior>();
        var name = NameFormat.Replace("{0}", pair.actor1.name).Replace("{1}", pair.actor2.name);
        attackLine.name = name;
        attackLine.parent = board.transform;
        attackLine.Spawn(pair);

        attackLines.Add(attackLine);
    }


    public void Destroy(ActorPair pair)
    {
        var name = NameFormat.Replace("{0}", pair.actor1.name).Replace("{1}", pair.actor2.name);
        var attackLine = attackLines.FirstOrDefault(x => x.name == name);
        if (attackLine == null) return;
        attackLines.Remove(attackLine);
        attackLine.FadeOut();
    }

    public void Clear()
    {
        attackLines.ForEach(x => Destroy(x.gameObject));
        attackLines.Clear();
    }

}
