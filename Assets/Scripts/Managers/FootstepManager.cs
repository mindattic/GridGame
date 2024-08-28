using Game.Behaviors.Actor;
using System;
using System.Linq;
using UnityEngine;

public class FootstepManager : ExtendedMonoBehavior
{

    //Variables
    [SerializeField] public GameObject FootstepPrefab;
    ActorBehavior Actor;
    Vector3 PreviousPosition;
    bool IsRightFoot = false;
    float Threshold;

    void Awake()
    {



    }

    void Start()
    {
        Threshold = tileSize / 4;
    }


    void FixedUpdate() { }


    public void Play(ActorBehavior actor)
    {
        if (actor == null || actor.IsDying || actor.IsInactive)
            return;

        this.Actor = actor;
        PreviousPosition = this.Actor.position;
    }

    public void Stop()
    {
        Actor = null;
        IsRightFoot = false;
    }


    void Update()
    {
        if (Actor == null || Actor.IsDying || Actor.IsInactive)
            return;

        var distance = Vector3.Distance(Actor.position, PreviousPosition);
        if (distance < Threshold)
            return;

        Spawn();
    }


    private void Spawn()
    {
        GameObject prefab = Instantiate(FootstepPrefab, Vector2.zero, Quaternion.identity);
        FootstepBehavior footstep = prefab.GetComponent<FootstepBehavior>();
        footstep.sprite = resourceManager.Prop("Footstep");
        footstep.name = $"Footstep_{Guid.NewGuid()}";
        footstep.Parent = board.transform;
        footstep.Spawn(Actor.position, Shared.RotationByDirection(Actor.position, PreviousPosition), IsRightFoot);
        PreviousPosition = Actor.position;
        IsRightFoot = !IsRightFoot;
    }


    public void Clear()
    {
        GameObject.FindGameObjectsWithTag(Tag.Footstep).ToList().ForEach(x => Destroy(x));
    }


}
