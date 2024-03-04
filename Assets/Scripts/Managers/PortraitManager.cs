using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    public void Play(ActorBehavior actor, Direction direction)
    {
        GameObject prefab = Instantiate(portraitPrefab, Vector2.zero, Quaternion.identity);
        var portrait = prefab.GetComponent<PortraitBehavior>();
        portrait.name = $"Portrait_{Guid.NewGuid()}";
        portrait.id = portrait.name;
        portrait.parent = canvas3D.transform;
        portrait.sortingOrder = sortingOrder++;
        portrait.sprite = resourceManager.ActorPortrait(actor.id);
        portrait.transform.localScale = new Vector3(0.5f, 0.5f, 1);
        portrait.spriteRenderer.color = new Color(1, 1, 1, 0.9f);
        portraits.Add(portrait);

        portrait.Play(actor, direction);
    }



    public void Spawn()
    {
        Play(Random.Player(), Random.Direction());
    }


    public void Clear()
    {
        portraits.ForEach(x => Destroy(x.gameObject));
        portraits.Clear();
        sortingOrder = 1;
    }

    public void Remove(PortraitBehavior portrait)
    {
       portraits.Remove(portrait);
    }
}
