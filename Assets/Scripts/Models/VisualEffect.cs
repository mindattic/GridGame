using UnityEngine;

[CreateAssetMenu()]
public class VisualEffect : ScriptableObject
{
    [SerializeField] public string id;
    [SerializeField] public GameObject prefab;
    [SerializeField] public Vector3 relativeOffset;
    [SerializeField] public Vector3 angularRotation;
    [SerializeField] public Vector3 relativeScale = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField] public float delay = 0f;
    [SerializeField] public float duration = 2f;
    [SerializeField] public bool isLoop;
}
