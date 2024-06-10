using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SupportLineManager : ExtendedMonoBehavior
{
    [SerializeField] public GameObject supportLinePrefab;

    public Dictionary<string, SupportLineBehavior> supportLines = new Dictionary<string, SupportLineBehavior>();


    private void Start()
    {

    }

    private void Update()
    {

    }

    public void Add(ActorPair pair)
    {
        var key = $"SupportLine_{pair.actor1.name}-{pair.actor2.name}";
        var altKey = $"SupportLine_{pair.actor2.name}-{pair.actor1.name}";

        if (supportLines.ContainsKey(key) || supportLines.ContainsKey(altKey))
            return;

        var prefab = Instantiate(supportLinePrefab, Vector2.zero, Quaternion.identity);
        var supportLine = prefab.GetComponent<SupportLineBehavior>();
        supportLine.name = key;
        supportLine.parent = board.transform;
        supportLine.Add(pair.actor1.currentTile.position, pair.actor2.currentTile.position);

        supportLines.Add(key, supportLine);
    }

    public void Remove(string key)
    {
        supportLines[key].Remove();
        supportLines.Remove(key);
    }

    public void Clear()
    {
        supportLines.ToList().ForEach(x => x.Value.Remove());
        supportLines.Clear();
    }

}
