using UnityEngine;

public class InputManager : MonoBehaviour
{
    #region Properties

    // Actor related objects
    protected ActorInstance focusedActor => GameManager.instance.focusedActor;
    protected ActorInstance previousSelectedPlayer => GameManager.instance.previousSelectedPlayer;
    protected ActorInstance selectedPlayer => GameManager.instance.selectedPlayer;
    protected bool hasFocusedActor => focusedActor != null;
    protected bool hasSelectedPlayer => selectedPlayer != null;

    // Managers
    protected SelectedPlayerManager selectedPlayerManager => GameManager.instance.selectedPlayerManager;
    protected StageManager stageManager => GameManager.instance.stageManager;

    // Float
    protected float dragThreshold;
    protected float tileSize => GameManager.instance.tileSize;

    // Boolean
    private bool isDragging;

    #endregion



    private void Awake()
    {

    }

    void Start()
    {
        dragThreshold = tileSize / 24;
    }

    //public bool IsDragging => dragStart != null;

    //public Vector3? dragStart = null;
    //[SerializeField] public float dragThreshold = 5f;

   


    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            selectedPlayerManager.Focus();
            isDragging = hasFocusedActor;

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
        //Check abort conditions
        if (!isDragging || !hasFocusedActor)
            return;

        var dragDistance = Vector3.Distance(focusedActor.position, focusedActor.currentTile.position);
        if (dragDistance > dragThreshold)
        {
            selectedPlayerManager.Select();
        }

    }


}
