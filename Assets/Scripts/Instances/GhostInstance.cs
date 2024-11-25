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


    public GhostRenderers Renderers = new GhostRenderers();


    public Sprite thumbnail
    {
        get => Renderers.thumbnail.sprite;
        set => Renderers.thumbnail.sprite = value;
    }

    public Sprite frame
    {
        get => Renderers.frame.sprite;
        set => Renderers.frame.sprite = value;
    }


    public int sortingOrder
    {
        set
        {
            Renderers.thumbnail.sortingOrder = value;
            Renderers.frame.sortingOrder = value + 1;
        }
    }


    #endregion

    public void Spawn(ActorInstance actor)
    {
        //TODO: Fix later...
        this.Renderers.frame.enabled = false;

        this.Renderers.thumbnail.size = new Vector2(tileSize, tileSize);
        //this.renderers.frame.size = new Vector2(tileSize, tileSize);
        this.Renderers.thumbnail.color = Colors.RGBA(255, 255, 255, 64);
        //this.renderers.frame.color = Shared.ColorRGBA(255, 255, 255, 100);
        this.Position = actor.position;
        StartCoroutine(FadeOut());
    }



    private void Awake()
    {
        Renderers.thumbnail = gameObject.transform.GetChild(Thumbnail).GetComponent<SpriteRenderer>();
        Renderers.frame = gameObject.transform.GetChild(Frame).GetComponent<SpriteRenderer>();
    }

    // Play is called before the first frame update
    void Start()
    {
        
    }


    // SaveProfile is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
    }

    private IEnumerator FadeOut()
    {
        float alpha = Renderers.thumbnail.color.a;
        Color color = Renderers.thumbnail.color;

        while (alpha > 0)
        {
            alpha -= Increment.FivePercent;
            alpha = Mathf.Max(alpha, 0f);
            color.a = alpha;
            Renderers.thumbnail.color = color;
            Renderers.frame.color = color;

            yield return Wait.For(Interval.FiveTicks);
        }

        Destroy(this.gameObject);
    }

}
