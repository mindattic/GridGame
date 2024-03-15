using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GhostManager : ExtendedMonoBehavior
{
    //Variables
    [SerializeField] public GameObject ghostPrefab;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void Spawn()
    {
        StartCoroutine(SpawnGhost());
    }

    private IEnumerator SpawnGhost()
    {
        while (HasSelectedPlayer)
        {
            this.Spawn(selectedPlayer);
            yield return Wait.Tick();
        }
    }

    public void Spawn(ActorBehavior actor)
    {
        GameObject prefab = Instantiate(ghostPrefab, Vector2.zero, Quaternion.identity);
        GhostBehavior ghost = prefab.GetComponent<GhostBehavior>();
        ghost.thumbnail = actor.thumbnail;
        ghost.name = $"Ghost_{Guid.NewGuid()}";
        ghost.parent = board.transform;
        ghost.Spawn(actor);
    }

    public void Clear()
    {
        GameObject.FindGameObjectsWithTag(Tag.Ghost).ToList().ForEach(x => Destroy(x));
    }

}
