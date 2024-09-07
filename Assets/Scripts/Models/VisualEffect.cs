using UnityEngine;

[CreateAssetMenu()]
public class VisualEffect : ScriptableObject
{
    [SerializeField] public string id;
    [SerializeField] public GameObject gameObject;
    [SerializeField] public Vector3 relativeOffset;  
    [SerializeField] public Vector3 relativeScale;
    [SerializeField] public Vector3 rotation;
    [SerializeField] public float duration;
    [SerializeField] public bool isLoop;

}
