using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerArtBehavior : ExtendedMonoBehavior
{

    public RawImage image;

    // Start is called before the first frame update
    void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        //if (isFadingIn)
        //{
        //    alpha++;
        //    if (alpha < 255)
        //        image.color = new Color(255, 255, 255, alpha);
        //    else
        //        isFadingIn = false;
        //}
        //else if (isFadingOut)
        //{
        //    alpha--;
        //    if (alpha > 0)
        //        image.color = new Color(255, 255, 255, alpha);
        //    else
        //        isFadingOut = false;
        //}
    }


    public void Show()
    {
        StopCoroutine(FadeOut());
        StartCoroutine(FadeIn());

    }

    public void Hide()
    {
        StopCoroutine(FadeIn());
        StartCoroutine(FadeOut());
    }



    private IEnumerator FadeIn()
    {
        float alpha = image.color.a;
        Color color = image.color;

        while (image.color.a < 1)
        {
            alpha += 0.05f;
            alpha = Mathf.Min(alpha, 1);
            color.a = alpha;
            image.color = color;

            yield return new WaitForSeconds(0.01f); // update interval
        }
    }

    private IEnumerator FadeOut()
    {
        float alpha = image.color.a;
        Color color = image.color;

        while (image.color.a > 0)
        {
            alpha -= 0.05f;
            alpha = Mathf.Max(alpha, 0);
            color.a = alpha;
            image.color = color;

            yield return new WaitForSeconds(0.01f); // update interval
        }
    }

}
