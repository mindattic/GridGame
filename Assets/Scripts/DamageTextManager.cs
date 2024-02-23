using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DamageTextManager : ExtendedMonoBehavior
{
    [SerializeField] public GameObject damageTextPrefab;

    private void Start()
    {

    }

    private void Update()
    {

    }

    public void Add(string text, Vector3 position)
    {
        GameObject prefab;
        DamageTextBehavior damageText;

        prefab = Instantiate(damageTextPrefab, Vector2.zero, Quaternion.identity);
        damageText = prefab.GetComponent<DamageTextBehavior>();
        damageText.name = $"TextMesh_{Guid.NewGuid()}";
        damageText.parent = canvas3D.transform;
        damageText.Set(text, position);
    }

    public void Clear()
    {
        var gameObjects = GameObject.FindGameObjectsWithTag(Tag.DamageText).ToList();
        gameObjects.ForEach(x => Destroy(x));
    }

}
