using System;
using System.Linq;
using UnityEngine;

public class DottedLineManager : MonoBehaviour
{
    #region Properties
    protected float tileSize => GameManager.instance.tileSize;
    protected ResourceManager resourceManager => GameManager.instance.resourceManager;
    protected BoardInstance board => GameManager.instance.board;
    #endregion

    //Variables
    [SerializeField] public GameObject DottedLinePrefab;


    //Method which is automatically called before the first frame update  
    void Start()
    {
        
    }


    public void Spawn(DottedLineSegment segment, Vector2Int location)
    {
        GameObject prefab = Instantiate(DottedLinePrefab, Vector2.zero, Quaternion.identity);
        var instance = prefab.GetComponent<DottedLineInstance>();
        instance.name = $"DottedLine_{Guid.NewGuid()}";
        instance.parent = board.transform;
        instance.Spawn(segment, location);
    }


    public void Clear()
    {
        GameObject.FindGameObjectsWithTag(Tag.DottedLine).ToList().ForEach(x => Destroy(x));
    }


}

