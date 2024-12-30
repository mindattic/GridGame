using Game.Behaviors.Actor;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class FootstepManager : ExtendedMonoBehavior
{

    //Variables
    [SerializeField] public GameObject FootstepPrefab;
    ActorInstance actor;
    Vector3 previousPosition;
    bool isRightFoot = false;
    float threshold;

    void Awake()
    {



    }

    void Start()
    {
        threshold = tileSize / 4;
    }



    public void Play(ActorInstance actor)
    {
        if (!actor.IsActive || !actor.IsAlive)
            return;

        this.actor = actor;
        previousPosition = this.actor.position;
        StartCoroutine(CheckSpawn());
    }

    public void Stop()
    {
        actor = null;
        isRightFoot = false;
    }

    private IEnumerator CheckSpawn()
    {
        while (actor != null && actor.IsActive && actor.IsAlive)
        {
            var distance = Vector3.Distance(actor.position, previousPosition);
            if (distance >= threshold)
            {
                Spawn();
            }

            yield return new WaitForFixedUpdate();
        }
    }

    private void Spawn()
    {
        GameObject prefab = Instantiate(FootstepPrefab, Vector2.zero, Quaternion.identity);
        var instance = prefab.GetComponent<FootstepInstance>();
        instance.sprite = resourceManager.Prop("Footstep");
        instance.name = $"Footstep_{Guid.NewGuid()}";
        instance.Parent = board.transform;
        instance.Spawn(actor.position, RotationHelper.ByDirection(actor.position, previousPosition), isRightFoot);
        previousPosition = actor.position;
        isRightFoot = !isRightFoot;
    }


    public void Clear()
    {
        GameObject.FindGameObjectsWithTag(Tag.Footstep).ToList().ForEach(x => Destroy(x));
    }


}
