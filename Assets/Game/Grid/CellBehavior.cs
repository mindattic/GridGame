using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBehavior : MonoBehaviour
{

    private float cellSize => Global.instance.cellSize;

    [field: SerializeField] public int X { get; set; }
    [field: SerializeField] public int Y { get; set; }

}
