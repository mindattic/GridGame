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
            selectedPlayerManager.Focus();
            isDragging = HasFocusedActor;

        }
        else if (Input.GetMouseButtonUp(0))
        {
            selectedPlayerManager.Unfocus();
            selectedPlayerManager.Unselect();
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

        if (!HasFocusedActor)
            return;

        var dragDistance = Vector3.Distance(focusedActor.position, focusedActor.currentTile.position);
        if (dragDistance > tileSize / 24)
        {
            selectedPlayerManager.Select();
        }

    }


}
