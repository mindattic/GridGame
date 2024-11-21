using Game.Behaviors.Actor;
using System;
using System.Linq;
using UnityEngine;

public class GhostManager : ExtendedMonoBehavior
{
    //Variables
    [SerializeField] public GameObject ghostPrefab;
    ActorInstance actor;
    float threshold;
    Vector3 previousPosition;

    void Awake()
    {
       

    }

    void Start() {
        threshold = tileSize / 12;
    }

    void FixedUpdate() { }


    public void Play(ActorInstance actor)
    {
        if (actor == null || actor.IsDying || actor.IsInactive)
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
        if (actor == null || actor.IsDying || actor.IsInactive)
            return;

        var distance = Vector3.Distance(actor.position, previousPosition);
        if (distance < threshold)
            return;

        previousPosition = actor.position;

        Spawn();
    }

    private void Spawn()
    {
        var prefab = Instantiate(ghostPrefab, Vector2.zero, Quaternion.identity);
        var instance = prefab.GetComponent<GhostInstance>();
        instance.thumbnail = actor.thumbnail;
        instance.name = $"Ghost_{Guid.NewGuid()}";
        instance.parent = board.transform;
        instance.Spawn(actor);
    }

    public void Clear()
    {
        GameObject.FindGameObjectsWithTag(Tag.Ghost).ToList().ForEach(x => Destroy(x));
    }

}
