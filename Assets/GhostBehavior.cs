using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBehavior : MonoBehaviour
{


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

    public SpriteRenderer spriteRenderer;

    public Sprite sprite
    {
        get => spriteRenderer.sprite;
        set => spriteRenderer.sprite = value;
    }

    public int sortingOrder
    {
        set
        {
            spriteRenderer.sortingOrder = value;
        }
    }


    #endregion

    public void Set(ActorBehavior actor)
    {
        this.spriteRenderer.color = Common.ColorRGBA(255, 255, 255, 100);
        this.position = actor.position;
        StartCoroutine(FadeOut());
    }



    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
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
        float alpha = spriteRenderer.color.a;
        Color color = spriteRenderer.color;

        while (spriteRenderer.color.a > 0)
        {
            alpha -= 0.05f;
            alpha = Mathf.Max(alpha, 0f);
            color.a = alpha;
            spriteRenderer.color = color;
            yield return new WaitForSeconds(0.05f); // update interval
        }

        Destroy(this.gameObject);
    }

}
