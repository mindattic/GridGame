using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Global : Singleton<Global> 
{
    public Vector3 mousePosition2D;
    public Vector3 mousePosition3D;

    public Vector2 screenSize;
    public float cellSize;
    public Vector2 cellScale;

    public Global()
    {
       //Calculate values of global variables
    }
}