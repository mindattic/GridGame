using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using State = PortraitTransitionState;
using Settings = PortraitTransitionSettings;

public class PortraitManager : ExtendedMonoBehavior
{

    //Variables
    [SerializeField] public GameObject portraitPrefab;
    public List<PortraitBehavior> portraits = new List<PortraitBehavior>();
    public int sortingOrder = 1;

    private void Start()
    {

    }

    private void Update()
    {

    }

    public void Play(ActorBehavior actor, State state, Settings settings = null)
    {
        //Retrieve existing or create a new portrait
        var portrait = portraits.FirstOrDefault(x => x.id.Equals(actor.id));
        if (portrait == null)
            portrait = Add(actor.id);

        //Play transition
        portrait.Play(actor, state, settings);
    }


    public PortraitBehavior Add(string id)
    {
        GameObject prefab = Instantiate(portraitPrefab, Vector2.zero, Quaternion.identity);
        var portrait = prefab.GetComponent<PortraitBehavior>();
        portrait.name = $"Portrait_{Guid.NewGuid()}";
        portrait.id = id;
        portrait.parent = canvas3D.transform;
        portrait.sortingOrder = sortingOrder++;
        portrait.sprite = resourceManager.ActorPortrait(id);
        portraits.Add(portrait);

        return portrait;
    }


    public void Clear()
    {
        portraits.ForEach(x => Destroy(x.gameObject));
        portraits.Clear();
        sortingOrder = 1;
    }
}
