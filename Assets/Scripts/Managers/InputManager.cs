using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : ExtendedMonoBehavior
{
  
    private void Awake()
    {
        
    }

    void Start()
    {
        
    }
   
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            actorManager.PickupPlayer();
        else if (Input.GetMouseButtonUp(0))
            actorManager.DropPlayer();


        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Pressed R");
            stageManager.Load();
        }
     
    }
}
