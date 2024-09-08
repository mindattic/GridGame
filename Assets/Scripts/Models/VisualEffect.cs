using UnityEngine;

[CreateAssetMenu()]
public class VisualEffect : ScriptableObject
{
    [SerializeField] public string id;
    [SerializeField] public GameObject prefab;
    [SerializeField] public Vector3 relativeOffset;
    [SerializeField] public Vector3 angularRotation;
    [SerializeField] public Vector3 relativeScale;
    [SerializeField] public float duration;
    [SerializeField] public bool isLoop;
    [SerializeField] public float triggerTimestamp;
}
