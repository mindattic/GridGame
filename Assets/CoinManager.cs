using Game.Behaviors;
using System;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CoinManager : ExtendedMonoBehavior
{
    [SerializeField] public GameObject CoinPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn(Vector3 position)
    {
        var prefab = Instantiate(CoinPrefab, Vector2.zero, Quaternion.identity);
        CoinBehavior coinBehavior = prefab.GetComponent<CoinBehavior>();
        coinBehavior.name = $"Coin_{Guid.NewGuid()}";
        coinBehavior.Spawn(position);
    }

    public IEnumerator SpawnAsync(Vector3 position)
    {
        Spawn(position);
        yield return null;
    }
}
