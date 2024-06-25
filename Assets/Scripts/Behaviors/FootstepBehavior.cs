using System.Collections;
using UnityEngine;

public class FootstepBehavior : ExtendedMonoBehavior
{
    float duration;

    private void Awake()
    {
        transform.localScale = tileScale / 2;
        spriteRenderer = GetComponent<SpriteRenderer>();
        duration = Interval.OneSecond * 10;

    }
    void Start() { }
    void Update() { }
    void FixedUpdate() { }



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

    public Quaternion rotation
    {
        get => gameObject.transform.rotation;
        set => gameObject.transform.rotation = value;
    }

    SpriteRenderer spriteRenderer;


    public Sprite sprite
    {
        get => spriteRenderer.sprite;
        set => spriteRenderer.sprite = value;
    }
    #endregion



    public void Spawn(Vector3 position, Quaternion rotation, bool isRightFoot)
    {
        this.position = position;
        this.rotation = rotation;
        spriteRenderer.sprite = resourceManager.Prop($"Footstep{(isRightFoot ? "Right" : "Left")}");
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        yield return Wait.For(duration);

        float alpha = spriteRenderer.color.a;
        spriteRenderer.color = new Color(1, 1, 1, alpha);

        while (alpha > 0)
        {
            alpha -= Increment.OnePercent;
            alpha = Mathf.Max(alpha, 0f);
            spriteRenderer.color = new Color(1, 1, 1, alpha);

            yield return Wait.For(Interval.TenTicks);
        }

        Destroy(this.gameObject);
    }

}
