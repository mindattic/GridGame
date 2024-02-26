using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : ExtendedMonoBehavior
{
    //Variables
    public Text title;

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
    }



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
  

    public IEnumerator FadeInOut()
    {
        var increment = 0.01f;
        var interval = 0.01f;
        var cooldown = 1f;
        var alpha = 0f;
        title.color = new Color(1f, 1f, 1f, alpha);

        while (alpha < 1f)
        {
            alpha += increment;
            alpha = Mathf.Clamp(alpha, 0f, 1f);
            title.color = new Color(1f, 1f, 1f, alpha);
            yield return new WaitForSeconds(interval);
        }
        yield return new WaitForSeconds(cooldown);

        alpha = 1f;
        title.color = new Color(1f, 1f, 1f, alpha);

        while (alpha > 0f)
        {
            alpha -= increment;
            alpha = Mathf.Clamp(alpha, 0f, 1f);
            title.color = new Color(1f, 1f, 1f, alpha);
            yield return new WaitForSeconds(interval);
        }
    }


}
