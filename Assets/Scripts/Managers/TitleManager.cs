using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : ExtendedMonoBehavior
{
    //Variables
    public Text label;
    const float MaxAlpha = 1f;

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
        label = GameObject.Find("Title").GetComponent<Text>();
        label.transform.localPosition = new Vector3(0, 0, 0);
        label.color = new Color(1f, 1f, 1f, 0f);
    }



    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Show(string text)
    {
        label.text = text;
        label.color = new Color(1f, 1f, 1f, 1f);

        //StopCoroutine(FadeIn());
        //StopCoroutine(FadeOut());
        StartCoroutine(FadeOut());
    }

    public IEnumerator FadeIn()
    {  
        float alpha = 0f;
        label.color = new Color(1f, 1f, 1f, alpha);

        while (alpha < MaxAlpha)
        {
            alpha += Increment.One;
            alpha = Mathf.Clamp(alpha, 0f, MaxAlpha);
            label.color = new Color(1f, 1f, 1f, alpha);
            yield return new WaitForSeconds(Interval.One);
        }
    }

    public IEnumerator FadeOut()
    {
        float alpha = 1f;
        label.color = new Color(1f, 1f, 1f, alpha);

        yield return new WaitForSeconds(2f);

        while (alpha > 0f)
        {
            alpha -= Increment.One;
            alpha = Mathf.Clamp(alpha, 0f, MaxAlpha);
            label.color = new Color(1f, 1f, 1f, alpha);
            yield return new WaitForSeconds(Interval.One);
        }
    }



    public IEnumerator FadeInOut()
    {
        yield return StartCoroutine(FadeIn());
        yield return StartCoroutine(FadeOut());

        //float alpha = 0f;
        //label.color = new Color(1f, 1f, 1f, alpha);

        //while (alpha < MaxAlpha)
        //{
        //    alpha += Increment.One;
        //    alpha = Mathf.Clamp(alpha, 0f, MaxAlpha);
        //    label.color = new Color(1f, 1f, 1f, alpha);
        //    yield return new WaitForSeconds(Interval.One);
        //}

        //alpha = 1f;
        //label.color = new Color(1f, 1f, 1f, alpha);

        //while (alpha > 0f)
        //{
        //    alpha -= Increment.One;
        //    alpha = Mathf.Clamp(alpha, 0f, MaxAlpha);
        //    label.color = new Color(1f, 1f, 1f, alpha);
        //    yield return new WaitForSeconds(Interval.One);
        //}
    }




}
