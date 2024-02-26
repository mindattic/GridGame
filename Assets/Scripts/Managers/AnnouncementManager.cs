using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AnnouncementManager : ExtendedMonoBehavior {

    //Variables
    private Text announcement;

    #region Components

    public Vector3 position
    {
        get => transform.transform.localPosition;
        set => transform.transform.localPosition = value;
    }


    public Color color
    {
        get => announcement.color;
        set => announcement.color = value;
    }


    #endregion

    public void Awake()
    {
        position = new Vector3(0, 0, 0);
        announcement = gameObject.GetComponent<Text>();
        announcement.color = new Color(1f, 1f, 1f, 0f);
    }



    void Start()
    {
        
    }

    void Update()
    {
        
    }


    public void Show(string text)
    {
        announcement.text = text;
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {

        var warmup = 1f;
        var increment = 0.01f;
        var interval = 0.01f;
        var cooldown =  0f;

        yield return new WaitForSeconds(warmup);

        var alpha = 0f;
        while (alpha < 1f)
        {
            alpha += increment;
            alpha = Mathf.Clamp(alpha, 0f, 1f);
            this.color = new Color(1f, 1f, 1f, alpha);
            yield return new WaitForSeconds(interval);
        }

        yield return new WaitForSeconds(cooldown);
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {

        var warmup = 3f;
        var increment = 0.01f;
        var interval = 0.01f;
        var cooldown = 0f;

        yield return new WaitForSeconds(warmup);

        var alpha = 1f;
        while (alpha > 0f)
        {
            alpha -= increment;
            alpha = Mathf.Clamp(alpha, 0f, 1f);
            this.color = new Color(1f, 1f, 1f, alpha);
            yield return new WaitForSeconds(interval);
        }

        yield return new WaitForSeconds(cooldown);
        //turnManager.NextPhase();
    }


}
