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
    [SerializeField] public GameObject footstepPrefab;
    ActorBehavior actor;
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
        isRightFoot = false;
    }


    void Update()
    {
        if (actor == null || actor.IsDead || actor.IsInactive) 
            return;

        var distance = Vector3.Distance(actor.position, previousPosition);
        if (distance < threshold) 
            return;
  
        Spawn();
    }


    private void Spawn()
    {
        GameObject prefab = Instantiate(footstepPrefab, Vector2.zero, Quaternion.identity);
        FootstepBehavior footstep = prefab.GetComponent<FootstepBehavior>();
        footstep.sprite = resourceManager.Prop("Footstep");
        footstep.name = $"Footstep_{Guid.NewGuid()}";
        footstep.parent = board.transform;
        footstep.Spawn(actor.position, Common.RotationByDirection(actor.position, previousPosition), isRightFoot);
        previousPosition = actor.position;
        isRightFoot = !isRightFoot;
    }


    public void Clear()
    {
        GameObject.FindGameObjectsWithTag(Tag.Footstep).ToList().ForEach(x => Destroy(x));
    }


}
