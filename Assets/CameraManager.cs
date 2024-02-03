using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    private void Awake()
    {
        Global.instance.screenSize = new Vector2(Common.GetScreenToWorldWidth, Common.GetScreenToWorldHeight);
        Global.instance.cellSize = Global.instance.screenSize.x / 6;
        Global.instance.cellScale = new Vector2(Global.instance.cellSize, Global.instance.cellSize);



        Global.instance.spriteScale *= 0.75f;



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
