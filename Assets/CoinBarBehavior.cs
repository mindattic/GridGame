using TMPro;
using UnityEngine;

public class CoinBarBehavior : ExtendedMonoBehavior
{
    public GameObject icon;
    public TextMeshPro textMesh;

    void Awake()
    {
        icon = gameObject.transform.GetChild(0).gameObject;
        textMesh = gameObject.transform.GetChild(1).GetComponent<TextMeshPro>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
