using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OverlayManager : ExtendedMonoBehavior
{

    //Variables
    private RectTransform rectTransform;
    private Image image;
    const float MaxAlpha = 0.5f;

    
    #region Components

    public Transform parent
    {
        get => gameObject.transform.parent;
        set => gameObject.transform.SetParent(value, true);
    }

    public Color color
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


        image = GameObject.Find("Overlay").GetComponent<Image>();
        image.color = new Color(0f, 0f, 0f, 0f);

        
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public void Show()
    {
        image.color = new Color(0f, 0f, 0f, 1);
        StartCoroutine(FadeOut());
    }

    public IEnumerator FadeIn()
    {
        float alpha = 0f;
        image.color = new Color(0f, 0f, 0f, alpha);

        while (alpha < MaxAlpha)
        {
            alpha += Increment.Five;
            alpha = Mathf.Clamp(alpha, 0f, MaxAlpha);
            image.color = new Color(0f, 0f, 0f, alpha);
            yield return Wait.Tick();
        }
    }

    public IEnumerator FadeOut()
    {
        float alpha = 1f;
        image.color = new Color(0f, 0f, 0f, alpha);

        yield return new WaitForSeconds(2f);

        while (alpha > 0f)
        {
            alpha -= Increment.One;
            alpha = Mathf.Clamp(alpha, 0f, MaxAlpha);
            image.color = new Color(0f, 0f, 0f, alpha);
            yield return Wait.Tick();
        }
    }
}
