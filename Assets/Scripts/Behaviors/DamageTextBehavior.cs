using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DamageTextBehavior : ExtendedMonoBehavior
{


    public TextMeshPro textMesh;

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
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Spawn(string text, Vector3 position)
    {
        textMesh.text = text;
        var x = position.x + -(tileSize / 4) + (tileSize / 4 * Random.Percent());
        var y = position.y + (tileSize / 2 * Random.Percent());
        transform.position = new Vector3(x, y, 1);
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float alpha = textMesh.color.a;
        Color color = textMesh.color;

        while (textMesh.color.a > 0)
        {
            alpha -= 0.1f;
            alpha = Mathf.Max(alpha, 0);
            color.a = alpha;
            textMesh.color = color;


            transform.position += new Vector3(0, tileSize / 16, 0);
            yield return new WaitForSeconds(0.05f); // update interval
        }
        StopCoroutine(FadeOut());
        Destroy(gameObject);
    }

}
