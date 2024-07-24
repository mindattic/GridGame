using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Print(string text, bool showOverlay = false)
    {
        label.text = text;
        label.color = new Color(1, 1, 1, 0);

        if (showOverlay)
        {
            StartCoroutine(overlayManager.FadeInOut());
        }
           
        StartCoroutine(FadeInOut());
    }

    public IEnumerator FadeIn()
    {
        float alpha = 0f;
        label.color = new Color(1f, 1f, 1f, alpha);

        while (alpha < 1)
        {
            alpha += Increment.TenPercent;
            alpha = Mathf.Clamp(alpha, 0, 1);
            label.color = new Color(1, 1, 1f, alpha);
            yield return Wait.OneTick();
        }
    }

    public IEnumerator FadeOut()
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
