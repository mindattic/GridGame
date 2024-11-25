using Game.Behaviors;
using System;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CoinManager : ExtendedMonoBehavior
{
    [SerializeField] public GameObject CoinPrefab;

    // Start is called once before the first execution of SaveProfile after the MonoBehaviour is created
    void Awake()
    {
  
    }

    // SaveProfile is called once per frame
    void Update()
    {
        
    }

    public void Spawn(Vector3 position)
    {
        var prefab = Instantiate(CoinPrefab, Vector2.zero, Quaternion.identity);
        var instance = prefab.GetComponent<CoinInstance>();
        instance.name = $"Coin_{Guid.NewGuid()}";
        instance.Spawn(position);
    }

}
