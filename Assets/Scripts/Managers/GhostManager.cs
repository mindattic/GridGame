using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GhostManager : ExtendedMonoBehavior
{
    //Variables
    [SerializeField] public GameObject ghostPrefab;

    public List<GhostBehavior> ghosts = new List<GhostBehavior>();

    //public float start;
    //public float duration = 0.001f;



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
            yield return new WaitForSeconds(Interval.One);
        }
    }

    public void Spawn(ActorBehavior actor)
    {
        //if (Time.time - start < duration)
        //    return;

        //start = Time.time;
        GameObject prefab = Instantiate(ghostPrefab, Vector2.zero, Quaternion.identity);
        GhostBehavior ghost = prefab.GetComponent<GhostBehavior>();
        ghost.thumbnail = actor.thumbnail;
        ghost.name = $"Ghost_{Guid.NewGuid()}";
        ghost.id = actor.id;
        ghost.parent = board.transform;
        ghost.Spawn(actor);
        ghosts.Add(ghost);
    }

    public void Clear()
    {
        GameObject.FindGameObjectsWithTag(Tag.Ghost).ToList().ForEach(x => Destroy(x));
        ghosts.Clear();
    }

}
