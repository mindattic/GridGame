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
            selectedPlayerManager.Select();
            isDragging = HasSelectedPlayer;

        }
        else if (Input.GetMouseButtonUp(0))
        {
            selectedPlayerManager.Unselect();
            selectedPlayerManager.Drop();
            isDragging = false;
        }

        CheckDragging();

        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Pressed R");
            stageManager.Load();
        }

    }

    private void CheckDragging()
    {
        if (!isDragging)
            return;

        if (!HasSelectedPlayer)
            return;

        var dragDistance = Vector3.Distance(selectedPlayer.position, selectedPlayer.currentTile.position);
        if (dragDistance > tileSize / 10)
        {
            selectedPlayerManager.Pickup();
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

            if (HasSelectedPlayer)
                dragStart = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            dragStart = null;
            selectedPlayerManager.Drop();
        }

        if (IsDragging)
        {

            var dragDistance = Vector3.Distance(Input.mousePosition, dragStart.Value);
            if (dragDistance > dragThreshold)
            {
                selectedPlayerManager.Pickup();
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
