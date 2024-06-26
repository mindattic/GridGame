using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour
{


    private float cellSize => GameManager.instance.TileSize;

    // Start is called before the first Frame update
    void Start()
    {
    }

    // Update is called once per Frame
    void Update()
    {

    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        //if (other.collider.GameObject.CompareTag(Tag.Actor))
        //{
        //    var player = other.collider.GameObject;

        //    player.transform.Position = new Vector3(transform.Position.x - TileSize,
        //                                            player.transform.Position.y,
        //                                            player.transform.Position.z);


        //}



    }

}
