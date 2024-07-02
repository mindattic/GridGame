using Game.Behaviors.Actor;
using System;
using System.Linq;
using UnityEngine;

public class GhostManager : ExtendedMonoBehavior
{
    //Variables
    [SerializeField] public GameObject ghostPrefab;
    ActorBehavior actor;
    float threshold;
    Vector3 previousPosition;

    void Awake()
    {
       

    }

    void Start() {
        threshold = tileSize / 12;
    }

    void FixedUpdate() { }


    public void Start(ActorBehavior actor)
    {
        if (actor == null || actor.IsDead || actor.IsInactive)
            return;

        this.actor = actor;
        previousPosition = this.actor.position;
    }

    public void Stop()
    {
        actor = null;
    }


    void Update()
    {
        if (actor == null || actor.IsDead || actor.IsInactive)
            return;

        var distance = Vector3.Distance(actor.position, previousPosition);
        if (distance < threshold)
            return;

        previousPosition = actor.position;

        Spawn();
    }

    private void Spawn()
    {
        GameObject prefab = Instantiate(ghostPrefab, Vector2.zero, Quaternion.identity);
        GhostBehavior ghost = prefab.GetComponent<GhostBehavior>();
        ghost.thumbnail = actor.thumbnail;
        ghost.name = $"Ghost_{Guid.NewGuid()}";
        ghost.Parent = board.transform;
        ghost.Spawn(actor);
    }

    public void Clear()
    {
        GameObject.FindGameObjectsWithTag(Tag.Ghost).ToList().ForEach(x => Destroy(x));
    }

}
