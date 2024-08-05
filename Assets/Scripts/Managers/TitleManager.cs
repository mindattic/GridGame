using System.Collections;
using TMPro;
using UnityEngine;

public class TitleManager : ExtendedMonoBehavior
{
    //Variables
    private RectTransform rectTransform;
    public TextMeshProUGUI label;

    #region Components

    public string text
    {
        get => label.text;
        set => label.text = value;
    }


    public Color color
    {
        get => label.color;
        set => label.color = value;
    }

    #endregion

    void Awake()
    {
        label = GameObject.Find(Constants.Title).GetComponent<TextMeshProUGUI>();
        label.transform.localPosition = new Vector3(0, 680, 0);
        label.color = new Color(1, 1, 1, 0);
    }



    // Play is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void Print(string text, Color color)
    {
        IEnumerator _()
        {
            const float start = -90f;
            const float end = 0f;
            const float increment = 4f;

            //Before:
            float x = start;
            transform.eulerAngles = new Vector3(x, 0, 0);
            label.color = color;
            label.text = text;

            //During
            while (x < 0)
            {
                x += increment;
                x = Mathf.Clamp(x, -90f, 0f);
                transform.eulerAngles = new Vector3(x, 0, 0);
                yield return Wait.For(Interval.OneTick);
            }

            //After:
            x = end;
            transform.eulerAngles = new Vector3(x, 0, 0);
        }

        StartCoroutine(_());
    }

    public void Print(string text)
    {
        Print(text, Colors.Solid.White);
    }

    //public IEnumerator FadeIn()
    //{
    //    float alpha = 0f;
    //    label.color = new Color(1f, 1f, 1f, alpha);

    //    while (alpha < 1)
    //    {
    //        alpha += Increment.TenPercent;
    //        alpha = Mathf.Clamp(alpha, 0, 1);
    //        label.color = new Color(1, 1, 1f, alpha);
    //        yield return Wait.OneTick();
    //    }
    //}



    public void FadeOutAsync()
    {
        IEnumerator _()
        {
            float alpha = 1f;
            label.color = new Color(1f, 1f, 1f, alpha);

            yield return Wait.For(Interval.TwoSeconds);

            while (alpha > 0f)
            {
                alpha -= Increment.TenPercent;
                alpha = Mathf.Clamp(alpha, 0f, 1);
                label.color = new Color(1, 1, 1, alpha);
                yield return Wait.OneTick();
            }
        }

        StartCoroutine(_());
    }


    //public IEnumerator FadeInOut()
    //{
    //    StopCoroutine(FadeInOut());
    //    StopCoroutine(FadeIn());
    //    StopCoroutine(FadeOutAsync());

    //    yield return FadeIn();
    //    yield return Wait.For(Interval.OneSecond);
    //    yield return FadeOutAsync();
    //}




}
