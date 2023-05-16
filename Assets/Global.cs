using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Global : Singleton<Global> 
{
    public Vector3 mousePosition2D;
    public Vector3 mousePosition3D;
    
    public float cellSize;

    public Global()
    {
       //Calculate values of global variables


    }
}