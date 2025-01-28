using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class DottedLineManager : MonoBehaviour
{
    protected float tileSize => GameManager.instance.tileSize;
    protected ResourceManager resourceManager => GameManager.instance.resourceManager;
    protected BoardInstance board => GameManager.instance.board;
    protected ActorInstance selectedPlayer => GameManager.instance.selectedPlayer;
    protected bool hasSelectedPlayer => GameManager.instance.hasSelectedPlayer;
    protected UnityEvent<Vector2Int> onSelectedPlayerLocationChanged => GameManager.instance.onSelectedPlayerLocationChanged;


    //Variables
    [SerializeField] public GameObject DottedLinePrefab;

    public List<DottedLineInstance> dottedLines = new List<DottedLineInstance>();

    //Method which is automatically called before the first frame update  
    void Start()
    {
        onSelectedPlayerLocationChanged?.AddListener(OnSelectedPlayerLocationChanged);
    }


    private void OnSelectedPlayerLocationChanged(Vector2Int newLocation)
    {
        var occupiedLine = dottedLines.FirstOrDefault(line => line.location == newLocation);
        if (occupiedLine == null)
            return;

        // Reset all dotted lines to white
        dottedLines.ForEach(line => line.ResetColor());

        // Highlight all connected lines
        foreach (var connection in occupiedLine.connections)
        {
            var connectedLine = dottedLines.FirstOrDefault(line => line.location == connection);
            connectedLine?.SetColor();
        }
    }



    public void Spawn(DottedLineSegment segment, Vector2Int location)
    {
        GameObject prefab = Instantiate(DottedLinePrefab, Vector2.zero, Quaternion.identity);
        var instance = prefab.GetComponent<DottedLineInstance>();
        instance.name = $"DottedLine_{Guid.NewGuid()}";
        instance.parent = board.transform;
        instance.Spawn(segment, location);
        dottedLines.Add(instance);
    }

    public void Despawn(DottedLineInstance instance)
    {
        dottedLines.Remove(instance);
        Destroy(instance.gameObject);
    }

    public void Clear()
    {
        GameObject.FindGameObjectsWithTag(Tag.DottedLine).ToList().ForEach(x => Destroy(x));
    }

    private void OnDestroy()
    {
        onSelectedPlayerLocationChanged?.RemoveListener(OnSelectedPlayerLocationChanged);
    }
}

