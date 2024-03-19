using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBehavior : ExtendedMonoBehavior
{

    //Constants
    const int Thumbnail = 0;
    const int Frame = 1;

    #region Components

    public Transform parent
    {
        get => gameObject.transform.parent;
        set => gameObject.transform.SetParent(value, true);
    }

    public Vector3 position
    {
        get => gameObject.transform.position;
        set => gameObject.transform.position = value;
    }


    public GhostRenderers render = new GhostRenderers();


    public Sprite thumbnail
    {
        get => render.thumbnail.sprite;
        set => render.thumbnail.sprite = value;
    }

    public Sprite frame
    {
        get => render.frame.sprite;
        set => render.frame.sprite = value;
    }


    public int sortingOrder
    {
        set
        {
            render.thumbnail.sortingOrder = value;
            render.frame.sortingOrder = value + 1;
        }
    }


    #endregion

    public void Spawn(ActorBehavior actor)
    {
        //TODO: Fix later...
        this.render.frame.enabled = false;

        this.render.thumbnail.size = new Vector2(tileSize, tileSize);
        //this.render.frame.size = new Vector2(tileSize, tileSize);
        this.render.thumbnail.color = Common.ColorRGBA(255, 255, 255, 100);
        //this.render.frame.color = Common.ColorRGBA(255, 255, 255, 100);
        this.position = actor.position;
        StartCoroutine(FadeOut());
    }



    private void Awake()
    {
        render.thumbnail = gameObject.transform.GetChild(Thumbnail).GetComponent<SpriteRenderer>();
        render.frame = gameObject.transform.GetChild(Frame).GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
    }

    private IEnumerator FadeOut()
    {
        float alpha = render.thumbnail.color.a;
        Color color = render.thumbnail.color;

        while (alpha > 0)
        {
            alpha -= Increment.FivePercent;
            alpha = Mathf.Max(alpha, 0f);
            color.a = alpha;
            render.thumbnail.color = color;
            render.frame.color = color;

            yield return Wait.For(Interval.Five);
        }

        Destroy(this.gameObject);
    }

}
