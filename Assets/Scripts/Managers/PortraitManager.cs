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


    public void SlideIn()
    {
        var actor = players[Random.Int(0, players.Count - 1)];
        var direction = Random.Direction();
        SlideIn(actor, direction);
    }

    public void SlideIn(ActorBehavior actor, Direction direction)
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
        portrait.actor = actor;
        portrait.direction = direction;
        portrait.startTime = Time.time;
        portraits.Add(portrait);

        StartCoroutine(portrait.SlideIn());
    }


    public void Dissolve()
    {
        var actor = players[Random.Int(0, players.Count - 1)];
        Dissolve(actor);
    }

    public void Dissolve(ActorBehavior actor)
    {
        GameObject prefab = Instantiate(portraitPrefab, Vector2.zero, Quaternion.identity);
        var portrait = prefab.GetComponent<PortraitBehavior>();
        portrait.name = $"Portrait_{Guid.NewGuid()}";
        portrait.id = portrait.name;
        portrait.parent = board.transform;
        portrait.sortingOrder = 100;
        portrait.sprite = resourceManager.ActorPortrait(actor.id);
        portrait.transform.localScale = new Vector3(0.5f, 0.5f, 1);
        portrait.spriteRenderer.color = new Color(1, 1, 1, 0.9f);
        portrait.actor = actor;
        portrait.position = actor.position;
        portrait.startPosition = actor.position;
        portrait.transform.localScale = new Vector3(0.25f, 0.25f, 1);
        portraits.Add(portrait);

        StartCoroutine(portrait.Dissolve());
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
