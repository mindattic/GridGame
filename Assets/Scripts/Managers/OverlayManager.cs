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
        FadeOutAsync();
        titleManager.FadeOutAsync();
    }

   

    //public IEnumerator FadeIn()
    //{
    //    //float alpha = image.color.a;
    //    //image.color = new color(0f, 0f, 0f, alpha);

    //    //while (alpha < 1)
    //    //{
    //    //    alpha += Increment.FivePercent;
    //    //    alpha = Mathf.Clamp(alpha, 0, 1);
    //    //    image.color = new color(0, 0, 0, alpha);
    //    //    yield return ap.OneTick();
    //    //}


    //    image.color = new color(0, 0, 0, 1);
    //    yield return Destroy.Ticks(20);
    //}

    public void FadeOutAsync()
    {
        IEnumerator _()
        {
            float alpha = 1f;
            image.color = new Color(0, 0, 0, alpha);

            yield return Wait.For(Interval.TwoSeconds);

            while (alpha > 0f)
            {
                alpha -= Increment.TenPercent;
                alpha = Mathf.Clamp(alpha, 0, 1);
                image.color = new Color(0, 0, 0, alpha);
                yield return Wait.OneTick();
            }

            image.color = new Color(0, 0, 0, 0);
        }

        StartCoroutine(_());
    }




    //public IEnumerator FadeInOut()
    //{
    //    StopCoroutine(FadeInOut());
    //    StopCoroutine(FadeIn());
    //    StopCoroutine(FadeOutAsync());

    //    yield return FadeIn();
    //    yield return Destroy.For(Interval.OneSecond);
    //    yield return FadeOutAsync();
    //}
}
