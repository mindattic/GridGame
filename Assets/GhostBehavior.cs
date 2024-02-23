using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBehavior : MonoBehaviour
{

    //Constants
    const int Thumbnail = 0;
    const int Frame = 1;

    //Variables
    public float duration = 3f;

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


    public GhostRenderers renderers = new GhostRenderers();


    public Sprite thumbnail
    {
        get => renderers.thumbnail.sprite;
        set => renderers.thumbnail.sprite = value;
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

    public void Set(ActorBehavior actor)
    {
        this.renderers.thumbnail.color = Common.ColorRGBA(255, 255, 255, 100);
        this.renderers.frame.color = Common.ColorRGBA(255, 255, 255, 100);
        this.position = actor.position;
        StartCoroutine(FadeOut());
    }



    private void Awake()
    {
        renderers.thumbnail = gameObject.transform.GetChild(Thumbnail).GetComponent<SpriteRenderer>();
        renderers.frame = gameObject.transform.GetChild(Frame).GetComponent<SpriteRenderer>();
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
        float alpha = renderers.thumbnail.color.a;
        Color color = renderers.thumbnail.color;

        while (alpha > 0)
        {
            alpha -= 0.05f;
            alpha = Mathf.Max(alpha, 0f);
            color.a = alpha;
            renderers.thumbnail.color = color;
            renderers.frame.color = color;

            yield return new WaitForSeconds(0.05f); // update interval
        }

        Destroy(this.gameObject);
    }

}
