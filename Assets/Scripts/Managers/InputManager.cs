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

    public bool IsDragging => dragStart != null;

    public Vector3? dragStart = null;
    [SerializeField] public float dragThreshold = 5f;

    void Update()
    {



        if (Input.GetMouseButtonDown(0))
        {
            actorManager.TargetPlayer();

            if (HasTargettedPlayer)
                dragStart = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            dragStart = null;
            actorManager.DeselectPlayer();
        }

        if (IsDragging)
        {
            var dragDistance = Vector3.Distance(Input.mousePosition, dragStart.Value);
            if (dragDistance > dragThreshold)
            {
                actorManager.SelectPlayer();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Pressed R");
            stageManager.Load();
        }

    }
}
