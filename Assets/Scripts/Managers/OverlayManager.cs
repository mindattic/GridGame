using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OverlayManager : ExtendedMonoBehavior
{

    //Variables
    private RectTransform rectTransform;
    private Image image;

    #region Components

    public string Name
    {
        get => name;
        set => Name = value;
    }

    public Transform Parent
    {
        get => gameObject.transform.parent;
        set => gameObject.transform.SetParent(value, true);
    }

    public Color Color
    {
        get => image.color;
        set => image.color = value;
    }

    #endregion

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        var canvas = this.GetComponentInParent<Canvas>();
        var sizeDeltaY = Screen.height / canvas.scaleFactor;
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, sizeDeltaY);


        image = GameObject.Find(Constants.Overlay).GetComponent<Image>();
        image.color = new Color(0f, 0f, 0f, 1f);

        
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public void Show()
    {
        StartCoroutine(FadeIn());
    }

    public void Hide()
    {
        StartCoroutine(FadeOut());
    }

    public IEnumerator FadeIn()
    {
        //float alpha = image.Color.a;
        //image.Color = new Color(0f, 0f, 0f, alpha);

        //while (alpha < 1)
        //{
        //    alpha += Increment.FivePercent;
        //    alpha = Mathf.Clamp(alpha, 0, 1);
        //    image.Color = new Color(0, 0, 0, alpha);
        //    yield return wait.OneTick();
        //}


        image.color = new Color(0, 0, 0, 1);
        yield return Wait.Ticks(20);
    }

    public IEnumerator FadeOut()
    {
        float alpha = 1f;
        image.color = new Color(0, 0, 0, alpha);

        yield return Wait.For(Interval.TwoSeconds);

        while (alpha > 0f)
        {
            alpha -= Increment.OnePercent;
            alpha = Mathf.Clamp(alpha, 0, 1);
            image.color = new Color(0, 0, 0, alpha);
            yield return Wait.OneTick();
        }

        image.color = new Color(0, 0, 0, 0);
    }


    public IEnumerator FadeInOut()
    {
        StopCoroutine(FadeInOut());
        StopCoroutine(FadeIn());
        StopCoroutine(FadeOut());

        yield return FadeIn();
        yield return Wait.For(Interval.OneSecond);
        yield return FadeOut();
    }
}
