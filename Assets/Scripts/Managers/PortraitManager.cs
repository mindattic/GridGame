using Game.Behaviors.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PortraitManager : MonoBehaviour
{
    #region Properties
    protected ResourceManager resourceManager => GameManager.instance.resourceManager;
    protected BoardInstance board => GameManager.instance.board;
    protected IQueryable<ActorInstance> players => GameManager.instance.players;
    #endregion


    //Variables
    [SerializeField] public GameObject portraitPrefab;
    public int sortingOrder = 1;

    public void SlideIn(ActorInstance actor, Direction direction)
    {
        var prefab = Instantiate(portraitPrefab, Vector2.zero, Quaternion.identity);
        var instance = prefab.GetComponent<PortraitInstance>();
        instance.name = $"Portrait_{Guid.NewGuid()}";
        instance.parent = board.transform;
        instance.sortingOrder = sortingOrder++;
        instance.sprite = resourceManager.ActorSprite(actor.character.ToString()).portrait;
        instance.transform.localScale = new Vector3(0.5f, 0.5f, 1);
        instance.spriteRenderer.color = new Color(1, 1, 1, Opacity.Percent90);
        //instance.actor = actor;
        instance.direction = direction;
        instance.startTime = Time.time;

        StartCoroutine(instance.SlideIn());
    }

    public void Dissolve()
    {
        var actor = players.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
        Dissolve(actor);
    }

    public void Dissolve(ActorInstance actor)
    {
        var prefab = Instantiate(portraitPrefab, Vector2.zero, Quaternion.identity);
        var instance = prefab.GetComponent<PortraitInstance>();
        instance.name = $"Portrait_{Guid.NewGuid()}";
        instance.parent = board.transform;
        instance.sortingOrder = SortingOrder.Max;
        instance.sprite = resourceManager.ActorSprite(actor.character.ToString()).portrait;
        instance.transform.localScale = new Vector3(0.5f, 0.5f, 1);
        instance.spriteRenderer.color = new Color(1, 1, 1, Opacity.Percent90);
        //instance.actor = actor;
        instance.position = actor.position;
        instance.startPosition = actor.position;
        instance.transform.localScale = new Vector3(0.25f, 0.25f, 1);

        StartCoroutine(instance.Dissolve());
    }

}
