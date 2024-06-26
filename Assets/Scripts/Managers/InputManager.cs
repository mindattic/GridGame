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
            SelectedPlayerManager.Select();
            isDragging = HasFocusedPlayer;

        }
        else if (Input.GetMouseButtonUp(0))
        {
            SelectedPlayerManager.Unselect();
            SelectedPlayerManager.Drop();
            isDragging = false;
        }

        CheckDragging();

        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Pressed R");
            StageManager.Load();
        }

    }

    private void CheckDragging()
    {
        if (!isDragging)
            return;

        if (!HasFocusedPlayer)
            return;

        var dragDistance = Vector3.Distance(FocusedPlayer.position, FocusedPlayer.currentTile.position);
        if (dragDistance > TileSize / 10)
        {
            SelectedPlayerManager.Pickup();
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
            SelectedPlayerManager.TargetPlayer();

            if (HasFocusedPlayer)
                dragStart = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            dragStart = null;
            SelectedPlayerManager.Drop();
        }

        if (IsDragging)
        {

            var dragDistance = Vector3.Distance(Input.mousePosition, dragStart.Value);
            if (dragDistance > dragThreshold)
            {
                SelectedPlayerManager.Pickup();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Pressed R");
            StageManager.Load();
        }

    }
}

 */
