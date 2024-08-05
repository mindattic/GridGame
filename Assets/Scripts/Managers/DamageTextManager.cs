using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DamageTextManager : ExtendedMonoBehavior
{
    [SerializeField] public GameObject DamageTextPrefab;

    private void Start()
    {

    }

    private void Update()
    {

    }

    public void Spawn(string text, Vector3 position)
    {
        var prefab = Instantiate(DamageTextPrefab, Vector2.zero, Quaternion.identity);
        var damageText = prefab.GetComponent<DamageTextBehavior>();
        damageText.name = $"DamageText_{Guid.NewGuid()}";
        damageText.parent = canvas3D.transform;
        damageText.Spawn(text, position);
    }

    public void Clear()
    {
        var gameObjects = GameObject.FindGameObjectsWithTag(Tag.DamageText).ToList();
        gameObjects.ForEach(x => Destroy(x));
    }

}
