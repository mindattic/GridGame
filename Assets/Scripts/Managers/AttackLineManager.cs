using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackLineManager : ExtendedMonoBehavior
{
    [SerializeField] public GameObject attackLinePrefab;

    private void Start()
    {

    }

    private void Update()
    {

    }

    public void Add(ActorPair pair)
    {
        Vector3 a = pair.highest.position;
        Vector3 b = pair.lowest.position;

        if (pair.axis == Axis.Vertical)
        {
            a += new Vector3(0, -tileSize / 2 + -tileSize * 0.1f, 0);
            b += new Vector3(0, tileSize / 2 + tileSize * 0.1f, 0);

        }
        else if (pair.axis == Axis.Horizontal)
        {
            a += new Vector3(tileSize / 2 + tileSize * 0.1f, 0, 0);
            b += new Vector3(-tileSize / 2 + -tileSize * 0.1f, 0, 0);
        }

        //if (pair.axis == Axis.Vertical)
        //{
        //    a += new Vector3(0, -tileSize / 2, 0);
        //    b += new Vector3(0, tileSize / 2, 0);

        //}
        //else if (pair.axis == Axis.Horizontal)
        //{
        //    a += new Vector3(tileSize / 2, 0, 0);
        //    b += new Vector3(-tileSize / 2, 0, 0);
        //}

        var prefab = Instantiate(attackLinePrefab, Vector2.zero, Quaternion.identity);
        var attackLine = prefab.GetComponent<AttackLineBehavior>();
        attackLine.name = $"AttackLine_{Guid.NewGuid()}";
        attackLine.parent = board.transform;
        attackLine.Set(a, b);
    }

    public void Clear()
    {
        var gameObjects = GameObject.FindGameObjectsWithTag(Tag.AttackLine).ToList();
        gameObjects.ForEach(x => Destroy(x));
    }

}
