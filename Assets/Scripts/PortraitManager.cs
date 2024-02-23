using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PortraitManager : ExtendedMonoBehavior
{

    //Variables
    [SerializeField] public string id;
    [SerializeField] public GameObject portraitPrefab;


    public List<PortraitBehavior> portraits = new List<PortraitBehavior>();


    private int sortingOrder = 1;

    private void Start()
    {

    }

    private void Update()
    {

    }

    public void SlideIn(ActorBehavior actor)
    {
        if (portraits.Any(x => x.id.Equals(actor.id)))
            return;

        GameObject prefab = Instantiate(portraitPrefab, Vector2.zero, Quaternion.identity);
        PortraitBehavior portrait = prefab.GetComponent<PortraitBehavior>();
        portrait.name = $"Portrait_{Guid.NewGuid()}";
        portrait.id = actor.id;
        portrait.parent = canvas3D.transform;
        var sprite = resourceManager.actorSprites.First(x => x.id.Equals(actor.id)).portrait;
        portrait.SlideIn(sprite, sortingOrder++);
        portraits.Add(portrait);
    }

    public void FadeIn(ActorBehavior actor)
    {
        GameObject prefab = Instantiate(portraitPrefab, Vector2.zero, Quaternion.identity);
        PortraitBehavior portrait = prefab.GetComponent<PortraitBehavior>();
        portrait.name = $"Portrait_{Guid.NewGuid()}";
        portrait.id = actor.id;
        portrait.parent = canvas3D.transform;
        var sprite = resourceManager.actorSprites.First(x => x.id.Equals(actor.id)).portrait;
        portrait.FadeIn(sprite, sortingOrder++);
        portraits.Add(portrait);
    }

    public void FadeOut(ActorBehavior actor)
    {
        var portrait = portraitManager.portraits.FirstOrDefault(x => x.id.Equals(actor.id));
        if (portrait == null)
            return;

        portrait.FadeOut();
        portraits.Add(portrait);
    }


    public void FadeInOut(ActorBehavior actor)
    {
        GameObject prefab = Instantiate(portraitPrefab, Vector2.zero, Quaternion.identity);
        PortraitBehavior portrait = prefab.GetComponent<PortraitBehavior>();
        portrait.name = $"Portrait_{Guid.NewGuid()}";
        portrait.id = actor.id;
        portrait.parent = canvas3D.transform;
        var sprite = resourceManager.actorSprites.First(x => x.id.Equals(actor.id)).portrait;
        portrait.FadeInOut(sprite, sortingOrder++);
        portraits.Add(portrait);
    }


    public void Clear()
    {
        portraits.ForEach(x => Destroy(x));
        portraits.Clear();
    }
}
