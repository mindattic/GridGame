using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostManager : ExtendedMonoBehavior
{
    [SerializeField] public GameObject ghostPrefab;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Add(ActorBehavior actor)
    {
        GameObject prefab;
        GhostBehavior ghost;

        prefab = Instantiate(ghostPrefab, Vector2.zero, Quaternion.identity);
        ghost = prefab.GetComponent<GhostBehavior>();
        ghost.sprite = actor.thumbnail;
        ghost.name = $"Ghost_{Guid.NewGuid()}";
        ghost.parent = board.transform;
        ghost.Set(actor);
    }
}
