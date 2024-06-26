using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DamageTextBehavior : ExtendedMonoBehavior
{


    public TextMeshPro TextMesh;

    #region Components

    public string Name
    {
        get => name;
        set => Name = value;
    }

    public Transform Parent
    {
        get => gameObject.transform.parent;
        set => gameObject.transform.SetParent(value, true);
    }

    public Vector3 Position
    {
        get => gameObject.transform.position;
        set => gameObject.transform.position = value;
    }

    #endregion


    void Awake()
    {
        TextMesh = GetComponent<TextMeshPro>();
    }

    // Start is called before the first Frame update
    void Start()
    {

    }

    // Update is called once per Frame
    void Update()
    {

    }

    public void Spawn(string text, Vector3 position)
    {
        TextMesh.text = text;
        var x = position.x + Random.Range(TileSize / 4);
        var y = position.y + (TileSize / 2 * Random.Percent());
        transform.position = new Vector3(x, y, 1);
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float alpha = TextMesh.color.a;
        Color color = TextMesh.color;

        while (TextMesh.color.a > 0)
        {
            alpha -= 0.1f;
            alpha = Mathf.Max(alpha, 0);
            color.a = alpha;
            TextMesh.color = color;


            transform.position += new Vector3(0, TileSize / 16, 0);
            yield return Wait.For(Interval.FiveTicks); // update interval
        }
        StopCoroutine(FadeOut());
        Destroy(gameObject);
    }

}
