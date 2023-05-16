using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{


    void Start()
    {
        var screenSize = new Vector2(Common.GetScreenToWorldWidth, Common.GetScreenToWorldHeight);
        float size = screenSize.x / 6;

        var collider = GetComponent<BoxCollider2D>();
        collider.size = new Vector2(size, size);
    }

  
    void Update()
    {
        
    }


    //void OnCollisionEnter2D(Collision2D other)
    //{
    //    Debug.Log("OnCollisionEnter2D");
    //}

}
