using System;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static Unity.VisualScripting.Member;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.GridBrushBase;

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
        Threshold = TileSize / 4;
    }


    void FixedUpdate() { }


    public void Start(ActorBehavior actor)
    {
        if (actor == null || actor.IsDead || actor.IsInactive)
            return;

        this.Actor = actor;
        PreviousPosition = this.Actor.Position;
    }

    public void Stop()
    {
        Actor = null;
        IsRightFoot = false;
    }


    void Update()
    {
        if (Actor == null || Actor.IsDead || Actor.IsInactive) 
            return;

        var distance = Vector3.Distance(Actor.Position, PreviousPosition);
        if (distance < Threshold) 
            return;
  
        Spawn();
    }


    private void Spawn()
    {
        GameObject prefab = Instantiate(FootstepPrefab, Vector2.zero, Quaternion.identity);
        FootstepBehavior footstep = prefab.GetComponent<FootstepBehavior>();
        footstep.sprite = ResourceManager.Prop("Footstep");
        footstep.name = $"Footstep_{Guid.NewGuid()}";
        footstep.Parent = Board.transform;
        footstep.Spawn(Actor.Position, Common.RotationByDirection(Actor.Position, PreviousPosition), IsRightFoot);
        PreviousPosition = Actor.Position;
        IsRightFoot = !IsRightFoot;
    }


    public void Clear()
    {
        GameObject.FindGameObjectsWithTag(Tag.Footstep).ToList().ForEach(x => Destroy(x));
    }


}
