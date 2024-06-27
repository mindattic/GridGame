using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour
{


    private float cellSize => GameManager.instance.TileSize;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        //if (other.collider.GameObject.CompareTag(Tag.Actor))
        //{
        //    var player = other.collider.GameObject;

        //    player.transform.position = new Vector3(transform.position.x - TileSize,
        //                                            player.transform.position.y,
        //                                            player.transform.position.z);


        //}



    }

}
