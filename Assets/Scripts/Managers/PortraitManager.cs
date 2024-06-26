using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PortraitManager : ExtendedMonoBehavior
{

    //Variables
    [SerializeField] public GameObject portraitPrefab;
    public int sortingOrder = 1;

    private void Start()
    {

    }

    private void Update()
    {

    }


    public void SlideIn(ActorBehavior actor, Direction direction)
    {
        var prefab = Instantiate(portraitPrefab, Vector2.zero, Quaternion.identity);
        var portrait = prefab.GetComponent<PortraitBehavior>();
        portrait.name = $"Portrait_{Guid.NewGuid()}";
        portrait.parent = Board.transform;
        portrait.sortingOrder = sortingOrder++;
        portrait.sprite = ResourceManager.ActorPortrait(actor.Archetype.ToString());
        portrait.transform.localScale = new Vector3(0.5f, 0.5f, 1);
        portrait.spriteRenderer.color = new Color(1, 1, 1, 0.9f);
        portrait.actor = actor;
        portrait.direction = direction;
        portrait.startTime = Time.time;

        StartCoroutine(portrait.SlideIn());
    }


    public void Dissolve()
    {
        var actor = Players[Random.Int(0, Players.Count - 1)];
        Dissolve(actor);
    }

    public void Dissolve(ActorBehavior actor)
    {
        var prefab = Instantiate(portraitPrefab, Vector2.zero, Quaternion.identity);
        var portrait = prefab.GetComponent<PortraitBehavior>();
        portrait.name = $"Portrait_{Guid.NewGuid()}";
        portrait.parent = Board.transform;
        portrait.sortingOrder = 100;
        portrait.sprite = ResourceManager.ActorPortrait(actor.Archetype.ToString());
        portrait.transform.localScale = new Vector3(0.5f, 0.5f, 1);
        portrait.spriteRenderer.color = new Color(1, 1, 1, 0.9f);
        portrait.actor = actor;
        portrait.position = actor.position;
        portrait.startPosition = actor.position;
        portrait.transform.localScale = new Vector3(0.25f, 0.25f, 1);

        StartCoroutine(portrait.Dissolve());
    }

}
