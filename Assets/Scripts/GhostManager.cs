using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GhostManager : ExtendedMonoBehavior
{
    //Variables
    [SerializeField] public string id;
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
        ghost.thumbnail = actor.thumbnail;
        ghost.name = $"Ghost_{Guid.NewGuid()}";
        ghost.id = actor.id;
        ghost.parent = board.transform;
        ghost.Set(actor);
    }

    public void Clear()
    {
        var gameObjects = GameObject.FindGameObjectsWithTag(Tag.Ghost).ToList();
        gameObjects.ForEach(x => Destroy(x));
    }

}
