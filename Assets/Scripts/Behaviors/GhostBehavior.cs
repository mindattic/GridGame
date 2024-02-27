using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBehavior : MonoBehaviour
{

    //Constants
    const int Thumbnail = 0;
    const int Frame = 1;

    //Variables
    [SerializeField] public string id;
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


    public GhostRenderers sprite = new GhostRenderers();


    public Sprite thumbnail
    {
        get => sprite.thumbnail.sprite;
        set => sprite.thumbnail.sprite = value;
    }

 
    public int sortingOrder
    {
        set
        {
            sprite.thumbnail.sortingOrder = value;
            sprite.frame.sortingOrder = value + 1;
        }
    }


    #endregion

    public void Set(ActorBehavior actor)
    {
        this.sprite.thumbnail.color = Common.ColorRGBA(255, 255, 255, 100);
        this.sprite.frame.color = Common.ColorRGBA(255, 255, 255, 100);
        this.position = actor.position;
        StartCoroutine(FadeOut());
    }



    private void Awake()
    {
        sprite.thumbnail = gameObject.transform.GetChild(Thumbnail).GetComponent<SpriteRenderer>();
        sprite.frame = gameObject.transform.GetChild(Frame).GetComponent<SpriteRenderer>();
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
        float alpha = sprite.thumbnail.color.a;
        Color color = sprite.thumbnail.color;

        while (alpha > 0)
        {
            alpha -= Increment.Five;
            alpha = Mathf.Max(alpha, 0f);
            color.a = alpha;
            sprite.thumbnail.color = color;
            sprite.frame.color = color;

            yield return new WaitForSeconds(Interval.Five);
        }

        Destroy(this.gameObject);
    }

}
