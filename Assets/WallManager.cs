using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour
{


    private float cellSize => Global.instance.cellSize;

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
        //if (other.collider.gameObject.CompareTag(Tag.Player))
        //{
        //    var player = other.collider.gameObject;

        //    player.transform.position = new Vector3(transform.position.x - cellSize,
        //                                            player.transform.position.y,
        //                                            player.transform.position.z);


        //}



    }

}
