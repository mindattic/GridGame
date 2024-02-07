using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    private void Awake()
    {
        GameManager.instance.screenSize = new Vector2(Common.GetScreenToWorldWidth, Common.GetScreenToWorldHeight);
        GameManager.instance.tileSize = GameManager.instance.screenSize.x / 6;
        GameManager.instance.tileScale = new Vector2(GameManager.instance.tileSize, GameManager.instance.tileSize);



        GameManager.instance.spriteScale *= 0.75f;



    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
