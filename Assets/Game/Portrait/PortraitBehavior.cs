using System.Collections;
using UnityEngine;

public class PortraitBehavior : ExtendedMonoBehavior
{
    //Variables
    [SerializeField] public string id;


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

  
    public void SlideIn(Sprite sprite, int sortingOrder)
    {
        this.sprite = sprite;
        this.sortingOrder = sortingOrder;
        this.transform.position = new Vector3(0, -4.5f, 1);
        this.transform.localScale = new Vector3(0.5f, 0.5f, 1);
        StartCoroutine(SlideIn());

    }

    public void FadeIn(Sprite sprite, int sortingOrder)
    {
        this.sprite = sprite;
        this.sortingOrder = sortingOrder;
        this.transform.position = new Vector3(0, -4.5f, 1);
        this.transform.localScale = new Vector3(0.5f, 0.5f, 1);
        StartCoroutine(FadeIn());
    }

    public void FadeOut()
    {
        StartCoroutine(_FadeOut());
    }


    public void FadeInOut(Sprite sprite, int sortingOrder)
    {
        this.sprite = sprite;
        this.sortingOrder = sortingOrder;
        this.transform.position = new Vector3(0, -4.5f, 1);
        this.transform.localScale = new Vector3(0.5f, 0.5f, 1);
        StartCoroutine(FadeInOut());

    }

    #region Coroutines

    private IEnumerator SlideIn()
    {
        var x = board.offset.x + tileSize * portraitManager.portraits.Count;
        Vector3 destination = new Vector3(x, position.y, position.z);

        while (position.x > destination.x)
        {
            position = Vector3.MoveTowards(position, destination, 0.1f);
            yield return new WaitForSeconds(0.01f); //Wait 1/100th of a second
        }

        yield return new WaitForSeconds(1f); //Wait 1 second
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

            yield return new WaitForSeconds(0.01f); //Wait 1/100th of a second
        }
    }


    private IEnumerator _FadeOut()
    {
        float alpha = spriteRenderer.color.a;
        Color color = spriteRenderer.color;

        var direction = RNG.RandomBoolean() ? new Vector3(0.1f, 0, 0) : new Vector3(-0.1f, 0, 0);

        while (spriteRenderer.color.a > 0)
        {
            alpha -= 0.1f;
            alpha = Mathf.Max(alpha, 0f);
            color.a = alpha;
            spriteRenderer.color = color;

            position += direction;
            yield return new WaitForSeconds(0.01f); // update interval
        }

        portraitManager.portraits.Remove(this);
        Destroy(this.gameObject);
    }


    private IEnumerator FadeInOut()
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

        yield return new WaitForSeconds(1f); // update interval

        alpha = spriteRenderer.color.a;
        color = spriteRenderer.color;

        var direction = RNG.RandomBoolean() ? new Vector3(0.1f, 0, 0) : new Vector3(-0.1f, 0, 0);

        while (spriteRenderer.color.a > 0)
        {
            alpha -= 0.1f;
            alpha = Mathf.Max(alpha, 0f);
            color.a = alpha;
            spriteRenderer.color = color;

            position += direction;
            yield return new WaitForSeconds(0.01f); // update interval
        }

        portraitManager.portraits.Remove(this);
        Destroy(this.gameObject);
    }

    #endregion




}
