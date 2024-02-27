using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : ExtendedMonoBehavior
{
    //Variables
    public Text title;
    const float MaxAlpha = 1f;

    #region Components

    public string text
    {
        get => title.text;
        set => title.text = value;
    }


    public Color color
    {
        get => title.color;
        set => title.color = value;
    }

    #endregion

    void Awake()
    {
        title = GameObject.Find("Title").GetComponent<Text>();
        title.transform.localPosition = new Vector3(0, 0, 0);
        title.color = new Color(0f, 0f, 0f, 0f);
    }



    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator FadeIn()
    {  
        float alpha = 0f;
        title.color = new Color(0f, 0f, 0f, alpha);

        while (alpha < MaxAlpha)
        {
            alpha += Increment.One;
            alpha = Mathf.Clamp(alpha, 0f, MaxAlpha);
            title.color = new Color(1f, 1f, 1f, alpha);
            yield return new WaitForSeconds(Interval.One);
        }
    }

    public IEnumerator FadeOut()
    {
        float alpha = 1f;
        title.color = new Color(0f, 0f, 0f, alpha);

        while (alpha > 0f)
        {
            alpha -= Increment.One;
            alpha = Mathf.Clamp(alpha, 0f, MaxAlpha);
            title.color = new Color(1f, 1f, 1f, alpha);
            yield return new WaitForSeconds(Interval.One);
        }
    }

}
