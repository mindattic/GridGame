using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour
{


    private float cellSize => GameManager.instance.tileSize;

    // Play is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        //if (other.collider.prefab.CompareTag(Tag.Actor))
        //{
        //    var player = other.collider.prefab;

        //    player.transform.position = new Vector3(transform.position.x - tileSize,
        //                                            player.transform.position.y,
        //                                            player.transform.position.z);


        //}



    }

}
