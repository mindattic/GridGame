using Game.Behaviors.Actor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostInstance : ExtendedMonoBehavior
{

    //Constants
    const int Thumbnail = 0;
    const int Frame = 1;

    #region Components

    public string Name
    {
        get => name;
        set => Name = value;
    }

    public Transform parent
    {
        get => gameObject.transform.parent;
        set => gameObject.transform.SetParent(value, true);
    }

    public Vector3 Position
    {
        get => gameObject.transform.position;
        set => gameObject.transform.position = value;
    }


    public GhostRenderers renderers = new GhostRenderers();


    public Sprite thumbnail
    {
        get => renderers.thumbnail.sprite;
        set => renderers.thumbnail.sprite = value;
    }

    public Sprite frame
    {
        get => renderers.frame.sprite;
        set => renderers.frame.sprite = value;
    }


    public int sortingOrder
    {
        set
        {
            renderers.thumbnail.sortingOrder = value;
            renderers.frame.sortingOrder = value + 1;
        }
    }


    #endregion

    public void Spawn(ActorInstance actor)
    {
        //TODO: Fix later...
        this.renderers.frame.enabled = false;

        this.renderers.thumbnail.size = new Vector2(tileSize, tileSize);
        //this.renderers.frame.size = new Vector2(tileSize, tileSize);
        this.renderers.thumbnail.color = ColorHelper.RGBA(255, 255, 255, 64);
        //this.renderers.frame.color = ScreenHelper.ColorRGBA(255, 255, 255, 100);
        this.Position = actor.position;
        StartCoroutine(FadeOut());
    }



    private void Awake()
    {
        renderers.thumbnail = gameObject.transform.GetChild(Thumbnail).GetComponent<SpriteRenderer>();
        renderers.frame = gameObject.transform.GetChild(Frame).GetComponent<SpriteRenderer>();
    }


    private IEnumerator FadeOut()
    {
        float alpha = renderers.thumbnail.color.a;
        Color color = renderers.thumbnail.color;

        while (alpha > 0)
        {
            alpha -= Increment.FivePercent;
            alpha = Mathf.Max(alpha, 0f);
            color.a = alpha;
            renderers.thumbnail.color = color;
            renderers.frame.color = color;

            yield return Wait.For(Interval.FiveTicks);
        }

        Destroy(this.gameObject);
    }

}
