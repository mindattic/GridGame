using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private BoardBehavior board => GameManager.instance.board;
    private TimerBehavior timer => GameManager.instance.timer;

    private List<ActorBehavior> actors => GameManager.instance.actors;



    private void Awake()
    {
        
    }

    void Start()
    {
        
    }
   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Pressed R");
            board.ResetBoard();
        }

        //if (Input.GetMouseButtonDown(0))
        //    PickupPlayer();
        //else if (Input.GetMouseButtonUp(0))
        //    DropPlayer();



    }
}
