using Game.Behaviors.Actor;
using System;
using System.Collections;
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
        this.actor = actor;
        previousPosition = this.actor.position;
        StartCoroutine(CheckSpawn());
    }

    public void Stop()
    {
        actor = null;
    }


    private IEnumerator CheckSpawn()
    {
        while (actor.IsPlaying)
        {
            var distance = Vector3.Distance(actor.position, previousPosition);
            if (distance >= threshold)
            {
                previousPosition = actor.position;
                Spawn();
            }

            yield return new WaitForFixedUpdate();
        }
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
