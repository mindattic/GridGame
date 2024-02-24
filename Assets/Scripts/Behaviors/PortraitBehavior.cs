using System.Collections;
using UnityEngine;
using State = PortraitTransitionState;
using Settings = PortraitTransitionSettings;

public class PortraitBehavior : ExtendedMonoBehavior
{
    //Variables
    [SerializeField] public string id;

    public ActorBehavior actor;
    public State state;
    public Settings settings;


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

    public Vector3 scale
    {
        get => gameObject.transform.localScale;
        set => gameObject.transform.localScale = value;
    }

    public SpriteRenderer spriteRenderer;

    public Sprite sprite
    {
        get => spriteRenderer.sprite;
        set => spriteRenderer.sprite = value;
    }

    public Color color
    {
        get => spriteRenderer.color;
        set => spriteRenderer.color = value;
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
        //var x = actor.position.x;
        //position = new Vector3(x, position.y, position.z);
    }

    void FixedUpdate()
    {
    }



    public void Play(ActorBehavior actor, State state, Settings settings = null)
    {
        this.actor = actor;
        this.state = state;
        this.settings = settings;

        switch (state)
        {
            case State.FadeIn: StartCoroutine(FadeIn()); break;
            case State.FadeOut: StartCoroutine(FadeOut()); break;
            case State.FadeInOut: StartCoroutine(FadeInOut()); break;
            case State.SlideIn: StartCoroutine(SlideIn()); break;
            case State.None: default: break;
        }
    }

    IEnumerator FadeIn()
    {
        var position = settings?.position ?? new Vector3(actor.position.x, -4, 1);
        var scale = settings?.scale ?? new Vector3(0.5f, 0.5f, 1);
        var warmup = settings?.warmup ?? 0f;
        var increment = settings?.warmup ?? 0.01f;
        var interval = settings?.interval ?? 0.01f;
        var cooldown = settings?.cooldown ?? 0f;

        this.transform.position = position;
        this.transform.localScale = scale;
        this.color = new Color(1f, 1f, 1f, 0f);

        yield return new WaitForSeconds(warmup);

        float alpha = this.color.a;
        while (alpha < 1f)
        {
            alpha += increment; //0.1f
            alpha = Mathf.Min(alpha, 1f);
            this.color = new Color(1f, 1f, 1f, alpha);

            yield return new WaitForSeconds(interval); //0.01f
        }

        yield return new WaitForSeconds(cooldown);
    }

    IEnumerator FadeOut()
    {
        var position = settings?.position ?? new Vector3(actor.position.x, -4, 1);
        var scale = settings?.scale ?? new Vector3(0.5f, 0.5f, 1);
        var warmup = settings?.warmup ?? 0f;
        var increment = settings?.warmup ?? 0.01f;
        var interval = settings?.interval ?? 0.01f;
        var cooldown = settings?.cooldown ?? 0f;

        this.transform.position = position;
        this.transform.localScale = scale;
        this.color = new Color(1f, 1f, 1f, 1f);

        yield return new WaitForSeconds(warmup);

        float alpha = this.color.a;
        while (alpha > 0f)
        {
            alpha -= increment; //0.1f
            alpha = Mathf.Max(alpha, 0f);
            this.color = new Color(1f, 1f, 1f, alpha);

            yield return new WaitForSeconds(interval); //0.01f
        }

        yield return new WaitForSeconds(cooldown);

        portraitManager.portraits.Remove(this);
        Destroy(this.gameObject);
    }

    IEnumerator FadeInOut()
    {
        yield return StartCoroutine(FadeIn());
        yield return StartCoroutine(FadeOut());
    }


    IEnumerator SlideIn()
    {
        var position = settings?.position ?? new Vector3(actor.position.x, -4, 1);
        var destination = settings?.destination ?? new Vector3(-2, -4, 1);
        var scale = settings?.scale ?? new Vector3(0.5f, 0.5f, 1);
        var warmup = settings?.warmup ?? 0f;
        var increment = settings?.warmup ?? 0.01f;
        var interval = settings?.interval ?? 0.01f;
        var cooldown = settings?.cooldown ?? 0f;

        this.transform.position = position;
        this.transform.localScale = scale;

        yield return new WaitForSeconds(warmup);

        while (!position.Equals(destination))
        {
            position = Vector3.MoveTowards(position, destination, increment);
            bool isCloseToDestination = Vector3.Distance(position, destination) < increment * 2;
            if (isCloseToDestination)
            {
                position = destination;
            }

            yield return new WaitForSeconds(interval); //0.01f
        }

        yield return new WaitForSeconds(cooldown);
    }

}
