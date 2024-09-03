using System.Collections;
using TMPro;
using UnityEngine;

public class DamageTextBehavior : ExtendedMonoBehavior
{
    public TextMeshPro textMesh;
    public float floatSpeed;

    #region Components

    public Transform parent
    {
        get => gameObject.transform.parent;
        set => gameObject.transform.SetParent(value, true);
    }

    public Vector3 position
    {
        get => gameObject.transform.position;
        set => gameObject.transform.position = value;
    }

    #endregion


    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        floatSpeed = tileSize / 32;
    }

    public void Spawn(string text, Vector3 position)
    {
        textMesh.text = text;
        var x = position.x + -(tileSize / 12 * Random.Percent) + (tileSize / 12 * Random.Percent);
        var y = position.y + -(tileSize / 12 * Random.Percent) + (tileSize / 12 * Random.Percent);
        transform.position = new Vector3(x, y, 0);
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {

        //Before:
        float alpha = 1;
        Color color = Colors.Solid.White;

        //During:
        while (textMesh.color.a > 0)
        {
            alpha -= Increment.OnePercent * 3;
            alpha = Mathf.Max(alpha, 0);

            if (alpha < 0.5)
            {
                color.a = alpha;
                textMesh.color = color;
            }

            transform.position += new Vector3(0, floatSpeed, 0);
            yield return Wait.For(Interval.OneTick);
        }

        //After:
        Destroy(gameObject);

    }

}
