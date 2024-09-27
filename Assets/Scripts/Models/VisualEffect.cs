using UnityEngine;

[CreateAssetMenu()]
public class VisualEffect : ScriptableObject
{
    [SerializeField] public string id;
    [SerializeField] public GameObject prefab;
    [SerializeField] public Vector3 relativeOffset;
    [SerializeField] public Vector3 angularRotation;
    [SerializeField] public Vector3 relativeScale = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField] public float duration = 2;
    [SerializeField] public bool isLoop;
    [SerializeField] public float triggerEventAt = -1; //-1 = disabled, 0 = immediately
}
