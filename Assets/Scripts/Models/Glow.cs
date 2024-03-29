
using UnityEngine;

public class Glow
{
    public float r; 
    public float g; 
    public float b; 
    public float a;

    public Color Color => new Color(r, g, b, a);
}

