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
        GameObject prefab;
        AttackLineBehavior line;

        Vector3 a = pair.highest.position;
        Vector3 b = pair.lowest.position;

        //if (pair.axis == Axis.Vertical)
        //{
        //    a += new Vector3(0, -tileSize / 2 + -tileSize * 0.1f, 0);
        //    b += new Vector3(0, tileSize / 2 + tileSize * 0.1f, 0);

        //}
        //else if (pair.axis == Axis.Horizontal)
        //{
        //    a += new Vector3(tileSize / 2 + tileSize * 0.1f, 0, 0);
        //    b += new Vector3(-tileSize / 2 + -tileSize * 0.1f, 0, 0);
        //}

        if (pair.axis == Axis.Vertical)
        {
            a += new Vector3(0, -tileSize / 2, 0);
            b += new Vector3(0, tileSize / 2, 0);

        }
        else if (pair.axis == Axis.Horizontal)
        {
            a += new Vector3(tileSize / 2, 0, 0);
            b += new Vector3(-tileSize / 2, 0, 0);
        }

        prefab = Instantiate(attackLinePrefab, Vector2.zero, Quaternion.identity);
        line = prefab.GetComponent<AttackLineBehavior>();
        line.name = $"AttackLine_{Guid.NewGuid()}";
        line.id = line.name;
        line.parent = board.transform;
        line.Set(a, b);
    }

    public void Clear()
    {
        var gameObjects = GameObject.FindGameObjectsWithTag(Tag.AttackLine).ToList();
        gameObjects.ForEach(x => Destroy(x));
    }

}
