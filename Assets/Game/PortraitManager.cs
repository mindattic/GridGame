using System;
using System.Linq;
using UnityEngine;

public class PortraitManager : ExtendedMonoBehavior
{
    [SerializeField] public GameObject portraitPrefab;

    private int sortingOrder = 1;

    private void Start()
    {

    }

    private void Update()
    {

    }

    public void Hide()
    {

    }

    public void Add(ActorBehavior actor)
    {
        GameObject prefab;
        PortraitBehavior portrait;

        prefab = Instantiate(portraitPrefab, Vector2.zero, Quaternion.identity);
        portrait = prefab.GetComponent<PortraitBehavior>();
        portrait.name = $"Portrait_{Guid.NewGuid()}";
        portrait.parent = canvas3D.transform;
        portrait.Set(resourceManager.actorSprites.First(x => x.id.Equals(actor.name)).portrait, sortingOrder++);
    }


    public void Clear()
    {
        var gameObjects = GameObject.FindGameObjectsWithTag(Tag.DamageText).ToList();
        gameObjects.ForEach(x => Destroy(x));
    }
}
