using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DamageTextManager : ExtendedMonoBehavior
{
    [SerializeField] public GameObject DamageTextPrefab;

    public void Spawn(string text, Vector3 position)
    {
        var prefab = Instantiate(DamageTextPrefab, Vector2.zero, Quaternion.identity);
        var instance = prefab.GetComponent<DamageTextInstance>();
        instance.name = $"DamageText_{Guid.NewGuid()}";
        instance.parent = canvas3D.transform;
        instance.Spawn(text, position);
    }

    public void Clear()
    {
        var gameObjects = GameObject.FindGameObjectsWithTag(Tag.DamageText).ToList();
        gameObjects.ForEach(x => Destroy(x));
    }

}
