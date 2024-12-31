using UnityEngine;

public class InputManager : MonoBehaviour
{
    protected SelectedPlayerManager selectedPlayerManager => GameManager.instance.selectedPlayerManager;
    protected ActorInstance focusedActor
    {
        get { return GameManager.instance.focusedActor; }
        set { GameManager.instance.focusedActor = value; }
    }

    protected ActorInstance previousSelectedPlayer
    {
        get { return GameManager.instance.previousSelectedPlayer; }
        set { GameManager.instance.previousSelectedPlayer = value; }
    }

    protected ActorInstance selectedPlayer
    {
        get { return GameManager.instance.selectedPlayer; }
        set { GameManager.instance.selectedPlayer = value; }
    }
    protected bool HasFocusedActor => focusedActor != null;
    protected bool HasSelectedPlayer => selectedPlayer != null;
    protected StageManager stageManager => GameManager.instance.stageManager;
    protected float tileSize => GameManager.instance.tileSize;

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

        var dragDistance = Vector3.Distance(focusedActor.position, focusedActor.CurrentTile.position);
        if (dragDistance > tileSize / 24)
        {
            selectedPlayerManager.Select();
        }

    }


}
