using UnityEngine;

public class PortraitTransitionSettings
{
    public Vector3 position;
    public Vector3 destination;
    public Vector3 rotation;
    public Vector3 scale;
    public Direction direction = Direction.None;
    public float increment = 0f;
    public float interval = 0f;
    public float warmup = 0f;
    public float cooldown = 0f;

    public PortraitTransitionSettings() { }
}
