using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OverlayManager : ExtendedMonoBehavior
{

    //Variables
    private float maxAlpha = 0.5f;
    private Image image;

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
        image = GameObject.Find("Overlay").GetComponent<Image>();
        image.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
    }

    // Start is called before the first frame update
    void Start()
    {
        image.color = new Color(0f, 0f, 0f, maxAlpha);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator FadeInOut()
    {
        var increment = 0.1f;
        var interval = 0.01f;
        var cooldown = 3f;
        var alpha = 0f;
        image.color = new Color(0f, 0f, 0f, alpha);

        while (alpha < maxAlpha)
        {
            alpha += increment;
            alpha = Mathf.Clamp(alpha, 0f, maxAlpha);
            image.color = new Color(0f, 0f, 0f, alpha);
            yield return new WaitForSeconds(interval);
        }

        yield return new WaitForSeconds(cooldown);

        increment = 0.01f;
        alpha = maxAlpha;

        while (alpha > 0f)
        {
            alpha -= increment;
            alpha = Mathf.Clamp(alpha, 0f, maxAlpha);
            image.color = new Color(0f, 0f, 0f, alpha);

            yield return new WaitForSeconds(interval);
        }
    }

}
