using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehavior : MonoBehaviour
{
    [field: SerializeField] public Vector2Int location { get; set; }

    public void Awake()
    {
      
    }

    public void Start()
    {
        transform.position = Geometry.PointFromGrid(location);
        transform.localScale = GameManager.instance.tileScale;
    }
}
