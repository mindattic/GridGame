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

    //public bool IsDragging => dragStart != null;

    //public Vector3? dragStart = null;
    //[SerializeField] public float dragThreshold = 5f;

    bool isDragging = false;

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            selectedPlayerManager.Target();
            isDragging = HasTargettedPlayer;

        }
        else if (Input.GetMouseButtonUp(0))
        {
            selectedPlayerManager.Untarget();
            selectedPlayerManager.Deselect();
        }


        if (isDragging)
        {
            //Determine if targetted player has left current tile
            //var closestTile = Geometry.ClosestTileByPosition(targettedPlayer.position);
            //if (closestTile.location != targettedPlayer.location)
            //{
            //    selectedPlayerManager.Select();
            //}

            //var dragDistance = Vector3.Distance(Input.mousePosition, dragStart.Value);
            var dragDistance = Vector3.Distance(targettedPlayer.position, targettedPlayer.currentTile.position);
            if (dragDistance > tileSize / 2)
            {
                selectedPlayerManager.Select();
                isDragging = !HasSelectedPlayer;
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Pressed R");
            stageManager.Load();
        }

    }
}


/*
 * {

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
            selectedPlayerManager.TargetPlayer();

            if (HasTargettedPlayer)
                dragStart = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            dragStart = null;
            selectedPlayerManager.Deselect();
        }

        if (IsDragging)
        {

            var dragDistance = Vector3.Distance(Input.mousePosition, dragStart.Value);
            if (dragDistance > dragThreshold)
            {
                selectedPlayerManager.Select();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Pressed R");
            stageManager.Load();
        }

    }
}

 */
