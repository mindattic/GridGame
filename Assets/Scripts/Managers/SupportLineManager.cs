using System;
using System.Linq;
using UnityEngine;

public class SupportLineManager : ExtendedMonoBehavior
{
    [SerializeField] public GameObject supportLinePrefab;

    private void Start()
    {

    }

    private void Update()
    {

    }

    public void Add(Vector3 a, Vector3 b)
    {
        var prefab = Instantiate(supportLinePrefab, Vector2.zero, Quaternion.identity);
        var supportLine = prefab.GetComponent<SupportLineBehavior>();
        supportLine.name = $"SupportLine_{Guid.NewGuid()}";
        supportLine.parent = board.transform;
        supportLine.Set(a, b);
    }

    public void Clear()
    {
        var gameObjects = GameObject.FindGameObjectsWithTag(Tag.SupportLine).ToList();
        gameObjects.ForEach(x => Destroy(x));
    }

}
