using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class ArtManager : ExtendedMonoBehavior
{
    [SerializeField] public GameObject artPrefab;

    [SerializeField] public Sprite paladin;
    [SerializeField] public Sprite barbarian;
    [SerializeField] public Sprite ninja;
    [SerializeField] public Sprite cleric;
    [SerializeField] public Sprite sentinel;
    [SerializeField] public Sprite pandagirl;

    [SerializeField] public Sprite slime;
    [SerializeField] public Sprite bat;



    int sortingOrder = 0;


    private void Start()
    {

    }

    private void Update()
    {

    }

    public void Add(ActorBehavior actor)
    {

        var y = -5f + 2 * RNG.RandomPercent();

        var scaler = 0.5f + 0.5f * RNG.RandomPercent();
        var scale = new Vector3(scaler, scaler, 1);

        sortingOrder++;

        switch (actor.name)
        {
            case "Paladin": Add(paladin, new Vector3(actor.position.x, y, 1), scale, sortingOrder); break;
            case "Barbarian": Add(barbarian, new Vector3(actor.position.x, y, 1), scale, sortingOrder); break;
            case "Ninja": Add(ninja, new Vector3(actor.position.x, y, 1), scale, sortingOrder); break;
            case "Cleric": Add(cleric, new Vector3(actor.position.x, y, 1), scale, sortingOrder); break;
            case "Sentinel": Add(sentinel, new Vector3(actor.position.x, y, 1), scale, sortingOrder); break;
            case "Panda Girl": Add(pandagirl, new Vector3(actor.position.x, y, 1), scale, sortingOrder); break;
        }
    }

    public void Hide()
    {

    }

    public void Add(Sprite sprite, ActorBehavior actor)
    {
        GameObject prefab;
        ArtBehavior art;

        prefab = Instantiate(artPrefab, Vector2.zero, Quaternion.identity);
        art = prefab.GetComponent<ArtBehavior>();
        art.name = $"Art_{Guid.NewGuid()}";
        art.parent = canvas3D.transform;
        art.Set(sprite);
    }

    public void Add(Sprite sprite, Vector3 position, Vector3 scale, int sortingOrder)
    {
        GameObject prefab;
        ArtBehavior art;

        prefab = Instantiate(artPrefab, Vector2.zero, Quaternion.identity);
        art = prefab.GetComponent<ArtBehavior>();
        art.name = $"Art_{Guid.NewGuid()}";
        art.parent = canvas3D.transform;
        art.Set(sprite, position, scale, sortingOrder);
    }

    public void Clear()
    {
        var gameObjects = GameObject.FindGameObjectsWithTag(Tag.DamageText).ToList();
        gameObjects.ForEach(x => Destroy(x));
    }
}
