using UnityEngine;

[CreateAssetMenu()]
public class VisualEffect : ScriptableObject
{
    [SerializeField] public string id;
    [SerializeField] public GameObject gameObject;
    [SerializeField] public Vector3 position;
    [SerializeField] public Vector3 offset;
    [SerializeField] public Quaternion rotation;
    [SerializeField] public Vector3 scale;
    [SerializeField] public float duration;
}
