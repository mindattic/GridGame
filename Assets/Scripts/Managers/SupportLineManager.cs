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
        GameObject prefab;
        SupportLineBehavior line;

        prefab = Instantiate(supportLinePrefab, Vector2.zero, Quaternion.identity);
        line = prefab.GetComponent<SupportLineBehavior>();
        line.name = $"SupportLine_{Guid.NewGuid()}";
        line.id = line.name;
        line.parent = board.transform;
        line.Set(a, b);
    }

    public void Clear()
    {
        var gameObjects = GameObject.FindGameObjectsWithTag(Tag.SupportLine).ToList();
        gameObjects.ForEach(x => Destroy(x));
    }

}
