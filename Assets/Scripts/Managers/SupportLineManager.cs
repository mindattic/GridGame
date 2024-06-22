using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SupportLineManager : ExtendedMonoBehavior
{
    [SerializeField] public GameObject supportLinePrefab;

    public Dictionary<string, SupportLineBehavior> supportLines = new Dictionary<string, SupportLineBehavior>();

    private const string NameFormat = "SupportLine_{0)+{1}";


    private void Start()
    {

    }

    private void Update()
    {

    }

    public bool Spawn(ActorPair pair)
    {
        //Determine if there is a duplicate
        var key = NameFormat.Replace("{0}", pair.actor1.name).Replace("{1}", pair.actor2.name);
        var altKey = NameFormat.Replace("{0}", pair.actor2.name).Replace("{1}", pair.actor1.name);
        if (supportLines.ContainsKey(key) || supportLines.ContainsKey(altKey))
            return false;

        CheckAlignment(pair, out bool hasEnemiesBetween, out bool hasPlayersBetween, out bool hasGapsBetween);
        if (hasEnemiesBetween || hasEnemiesBetween)
            return false;

        var prefab = Instantiate(supportLinePrefab, Vector2.zero, Quaternion.identity);
        var supportLine = prefab.GetComponent<SupportLineBehavior>();
        supportLine.name = key;
        supportLine.parent = board.transform;
        supportLine.Spawn(pair.actor1.currentTile.position, pair.actor2.currentTile.position);

        supportLines.Add(key, supportLine);

        return true;
    }


    public void Destroy(string key)
    {
        supportLines[key].Destroy();
        supportLines.Remove(key);
    }

    public void Clear()
    {
        supportLines.ToList().ForEach(x => x.Value.Destroy());
        supportLines.Clear();
    }

}
