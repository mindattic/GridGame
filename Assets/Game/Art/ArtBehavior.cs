using System.Collections;
using UnityEngine;

public class ArtBehavior : ExtendedMonoBehavior
{





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

    public void Set(Sprite sprite)
    {
        this.sprite = sprite;
        this.spriteRenderer.flipX = RNG.RandomBoolean();
        this.Show();
    }


    public void Set(Sprite sprite, Vector3 position, Vector3 scale, int sortingOrder)
    {
        this.sprite = sprite;
        this.sortingOrder = sortingOrder;
        this.transform.position = position;
        this.transform.localScale = scale;
        this.Show();
    }

    public void Show()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float alpha = spriteRenderer.color.a;
        Color color = spriteRenderer.color;

        while (spriteRenderer.color.a < 1f)
        {
            alpha += 0.1f;
            alpha = Mathf.Min(alpha, 1f);
            color.a = alpha;
            spriteRenderer.color = color;

            yield return new WaitForSeconds(0.01f); // update interval
        }

        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float alpha = spriteRenderer.color.a;
        Color color = spriteRenderer.color;

        var direction = RNG.RandomBoolean() ? new Vector3(0.1f, 0, 0) : new Vector3(-0.1f, 0, 0);

        yield return new WaitForSeconds(2f);

        while (spriteRenderer.color.a > 0)
        {
            alpha -= 0.1f;
            alpha = Mathf.Max(alpha, 0f);
            color.a = alpha;
            spriteRenderer.color = color;

            position += direction;
            yield return new WaitForSeconds(0.01f); // update interval
        }

        Destroy(this.gameObject);
    }

}
