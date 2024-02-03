using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    //Variables
    [field: SerializeField] public int X { get; set; }
    [field: SerializeField] public int Y { get; set; }
    private Vector3 offset;
    private BoxCollider2D boxCollider2D;
    //private Vector3 mousePosition;
    private GameObject targetPlayer;
    private GameObject targetCell;
    private Vector2? targetPosition;
    bool inMotion = false;

    //Global properties
    private float cellSize => Global.instance.cellSize;
    //private Rigidbody2D selectedRigidBody
    //{

    //    get { return Global.instance.selectedRigidBody; }
    //    set { Global.instance.selectedRigidBody = value; }
    //}

    private GameObject selectedPlayer
    {

        get { return Global.instance.selectedPlayer; }
        set { Global.instance.selectedPlayer = value; }
    }

    //Local properties
    private Vector2 GetPosition(GameObject go) => go.transform.position;
    private PlayerBehavior GetPlayerBehavior(GameObject player) => player.GetComponent<PlayerBehavior>();
    private CellBehavior GetCellBehavior(GameObject cell) => cell.GetComponent<CellBehavior>();

    private SpriteRenderer GetSpriteRenderer(GameObject go) => go.GetComponent<SpriteRenderer>();
    private BoxCollider2D GetBoxCollider(GameObject go) => go.GetComponent<BoxCollider2D>();
    private Rigidbody2D GetRigidBody(GameObject go) => go.GetComponent<Rigidbody2D>();


    private void SetCoordinates(GameObject cell)
    {
        var behavior = GetCellBehavior(cell);
        X = behavior.X;
        Y = behavior.Y;
    }

    private void Awake()
    {

    }

    private void Start()
    {
        var closestCell = Common.FindClosestByTag(transform.position, Tag.Cell);
        transform.position = closestCell.transform.position;
        SetCoordinates(closestCell);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            PickupPlayer();
        else if (Input.GetMouseButtonUp(0))
            DropPlayer();
    }


    private void PickupPlayer()
    {
        if (selectedPlayer != null)
            return;

        Collider2D targetCollider2D = Physics2D.OverlapPoint(Global.instance.mousePosition3D);
        if (targetCollider2D == null || !targetCollider2D.CompareTag(Tag.Player))
            return;

        selectedPlayer = targetCollider2D.transform.gameObject;
        Global.instance.selectedPlayerName = selectedPlayer.name;
        //selectedRigidBody = GetRigidBody(selectedPlayer);
        offset = selectedPlayer.transform.position - Global.instance.mousePosition3D;


        //Reduce all collider sizes by 50%
        Global.instance.players.ForEach(x => GetComponent<BoxCollider2D>().size = Global.instance.size50);
    }

    private void DropPlayer()
    {
        if (selectedPlayer == null)
            return;

        var closestCell = Common.FindClosestByTag(selectedPlayer.transform.position, Tag.Cell);
        selectedPlayer.transform.position = closestCell.transform.position;
        selectedPlayer = null;
        Global.instance.selectedPlayerName = "";

        //Increase all collider sizes by 50%
        Global.instance.players.ForEach(x => GetComponent<BoxCollider2D>().size = Global.instance.size100);
    }

    void FixedUpdate()
    {
        //If selected go...
        if (selectedPlayer != null)
        {
            //selectedRigidBody.MovePosition(Global.instance.mousePosition3D + offset);
            selectedPlayer.transform.position
                = Vector2.MoveTowards(selectedPlayer.transform.position,
                                      Global.instance.mousePosition3D + offset,
                                      10 * Time.deltaTime);
        }


        //if (selectedRigidBody)
        //{
        //    selectedRigidBody.MovePosition(Global.instance.mousePosition3D + offset);
        //}

        //Otherwise...
        if (inMotion && targetPosition != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition.Value, 10 * Time.deltaTime);

            float distance = Vector2.Distance(transform.position, targetPosition.Value);
            if (distance < 0.1f)
            {
                transform.position = targetPosition.Value;
                targetPosition = null;
                inMotion = false;
            }

        }

    }


    void OnDrawGizmos()
    {

        //Gizmos.color = Color.green;
        //Gizmos.matrix = this.transform.localToWorldMatrix;
        //Gizmos.DrawCube(Vector3.zero, Vector3.one);

    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (selectedPlayer == null)
            return;

        var sender = collider.gameObject;
        var receiver = gameObject;

        //Ignore collisions by receiver
        if (sender != selectedPlayer)
            return;

        if (!IsPlayerCollision(collider))
            return;

        targetPlayer = receiver;

        if (CollisionFromAbove(sender, receiver))
            targetCell = GetCellAbove(targetPlayer.transform.position);
        else if (CollisionFromRight(sender, receiver))
            targetCell = GetCellRight(targetPlayer.transform.position);
        else if (CollisionFromBelow(sender, receiver))
            targetCell = GetCellBelow(targetPlayer.transform.position);
        else if (CollisionFromLeft(sender, receiver))
            targetCell = GetCellLeft(targetPlayer.transform.position);

        Global.instance.targetPlayerName = targetPlayer.name;
    }

    private bool CollisionFromAbove(GameObject sender, GameObject receiver)
    {
        return sender.transform.position.y > receiver.transform.position.y
            && sender.transform.position.x <= receiver.transform.position.x + cellSize / 2
            && sender.transform.position.x >= receiver.transform.position.x - cellSize / 2;
    }

    private bool CollisionFromBelow(GameObject sender, GameObject receiver)
    {
        return sender.transform.position.y < receiver.transform.position.y
            && sender.transform.position.x <= receiver.transform.position.x + cellSize / 2
            && sender.transform.position.x >= receiver.transform.position.x - cellSize / 2;
    }

    private bool CollisionFromRight(GameObject sender, GameObject receiver)
    {
        return sender.transform.position.x > receiver.transform.position.x
            && sender.transform.position.y <= receiver.transform.position.y + cellSize / 2
            && sender.transform.position.y >= receiver.transform.position.y - cellSize / 2;
    }

    private bool CollisionFromLeft(GameObject sender, GameObject receiver)
    {
        return sender.transform.position.x < receiver.transform.position.x
            && sender.transform.position.y <= receiver.transform.position.y + cellSize / 2
            && sender.transform.position.y >= receiver.transform.position.y - cellSize / 2;
    }

    private GameObject GetClosestCell(Vector2 position)
    {
        return Common.FindClosestByTag(position, Tag.Cell);
    }

    private GameObject GetCellAbove(Vector2 position, int spaces = 1)
    {
        position.y += cellSize * spaces;
        return GetClosestCell(position);
    }

    private GameObject GetCellBelow(Vector2 position, int spaces = 1)
    {
        position.y -= cellSize * spaces;
        return GetClosestCell(position);
    }

    private GameObject GetCellRight(Vector2 position, int spaces = 1)
    {
        position.x += cellSize * spaces;
        return GetClosestCell(position);
    }

    private GameObject GetCellLeft(Vector2 position, int spaces = 1)
    {
        position.x -= cellSize * spaces;
        return GetClosestCell(position);
    }


    private void OnTriggerStay2D(Collider2D collider)
    {
        if (selectedPlayer == null || targetPlayer == null || targetCell == null)
            return;

        var sender = collider.gameObject;
        var receiver = gameObject;

        if (selectedPlayer != sender || targetPlayer != receiver)
            return;

        if (!IsPlayerCollision(collider))
            return;

        bool wasAbove = sender.transform.position.y >= targetCell.transform.position.y + cellSize / 2;
        bool wasBelow = sender.transform.position.y < targetCell.transform.position.y - cellSize / 2;
        bool wasLeft = sender.transform.position.x >= targetCell.transform.position.x + cellSize / 2;
        bool wasRight = sender.transform.position.x < targetCell.transform.position.x - cellSize / 2;

        if (!wasAbove && !wasBelow && !wasLeft && !wasRight)
            return;

        SlidePlayer(receiver);
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        //if (selectedPlayer == null)
        //    return;

        //var sender = collider.gameObject;
        //var receiver = gameObject;

        //if (selectedPlayer != sender || targetPlayer != receiver)
        //    return;

        //if (!IsPlayerCollision(collider))
        //    return;

        //SlidePlayer(receiver);
    }

    private void SlidePlayer(GameObject player)
    {
        var playerBehavior = player.GetComponent<PlayerBehavior>();
        if (playerBehavior.inMotion)
            return;

        playerBehavior.targetPosition = targetCell.transform.position;
        playerBehavior.inMotion = true;
        targetPlayer = null;
        targetCell = null;
        Global.instance.targetPlayerName = "";
    }







    private bool IsWallCollision(Collision2D other)
    {
        return other.gameObject.CompareTag(Tag.Wall);
    }

    private bool IsPlayerCollision(Collider2D other)
    {
        return other.gameObject.CompareTag(Tag.Player);
    }

    public static bool IsInside(Collider2D c, Vector3 point)
    {
        Vector3 closest = c.ClosestPoint(point);
        // Because closest=point if point is inside - not clear from docs I feel
        return closest == point;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (IsWallCollision(other))
        {
            //Debug.Log("Exit Wall");
            //currentCollisions.Remove(other);
        }
    }

}
