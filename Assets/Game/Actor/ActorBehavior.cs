using UnityEngine;

public class ActorBehavior : MonoBehaviour
{
    //Variables
    [field: SerializeField] public Vector2Int location { get; set; }



    private Vector3 offset;
    private BoxCollider2D boxCollider2D;
    //private Vector3 mousePosition;
    private GameObject targetPlayer;
    private GameObject targetCell;
    private Vector2? targetPosition;
    bool inMotion = false;

    //Global properties
    private float tileSize => GameManager.instance.tileSize;
    //private Rigidbody2D selectedRigidBody
    //{

    //    get { return GameManager.instance.selectedRigidBody; }
    //    set { GameManager.instance.selectedRigidBody = value; }
    //}

    private GameObject selectedPlayer
    {
        get { return GameManager.instance.selectedPlayer; }
        set { GameManager.instance.selectedPlayer = value; }
    }


    public Sprite sprite
    {
        get => gameObject.GetComponent<SpriteRenderer>().sprite;
        set => gameObject.GetComponent<SpriteRenderer>().sprite = value;
    }

    public Transform parent
    {
        get => gameObject.transform.parent;
        set => gameObject.transform.SetParent(value, true);
    }

    //Local properties
    private Vector2 GetPosition(GameObject go) => go.transform.position;
    private ActorBehavior GetPlayerBehavior(GameObject player) => player.GetComponent<ActorBehavior>();
    private TileBehavior GetCellBehavior(GameObject cell) => cell.GetComponent<TileBehavior>();

    private SpriteRenderer GetSpriteRenderer(GameObject go) => go.GetComponent<SpriteRenderer>();
    private BoxCollider2D GetBoxCollider(GameObject go) => go.GetComponent<BoxCollider2D>();
    private Rigidbody2D GetRigidBody(GameObject go) => go.GetComponent<Rigidbody2D>();


    //private void SetCoordinates(GameObject cell)
    //{
    //    var behavior = GetCellBehavior(cell);
    //    X = behavior.x;
    //    Y = behavior.y;
    //}

    private void Awake()
    {

    }

    private void Start()
    {
        transform.position = Geometry.PointFromGrid(location);
        transform.localScale = GameManager.instance.tileScale;


        //var closestCell = Common.FindClosestByTag(transform.position, Tag.Tile);
        //transform.position = closestCell.transform.position;
        //SetCoordinates(closestCell);
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

        Collider2D targetCollider2D = Physics2D.OverlapPoint(GameManager.instance.mousePosition3D);
        if (targetCollider2D == null || !targetCollider2D.CompareTag(Tag.Actor))
            return;

        selectedPlayer = targetCollider2D.transform.gameObject;
        GameManager.instance.selectedPlayerName = selectedPlayer.name;
        //selectedRigidBody = GetRigidBody(selectedPlayer);
        offset = selectedPlayer.transform.position - GameManager.instance.mousePosition3D;


        //Reduce all collider sizes by 50%
        GameManager.instance.actors.ForEach(x => GetComponent<BoxCollider2D>().size = GameManager.instance.size50);
    }

    private void DropPlayer()
    {
        if (selectedPlayer == null)
            return;

        var closestCell = Common.FindClosestByTag(selectedPlayer.transform.position, Tag.Tile);
        selectedPlayer.transform.position = closestCell.transform.position;
        selectedPlayer = null;
        GameManager.instance.selectedPlayerName = "";

        //Increase all collider sizes by 50%
        GameManager.instance.actors.ForEach(x => GetComponent<BoxCollider2D>().size = GameManager.instance.size100);
    }

    void FixedUpdate()
    {
        float speed = GameManager.instance.tileSize / 5; //20 * Time.deltaTime;


        //If selected go...
        if (selectedPlayer != null)
        {
            //selectedRigidBody.MovePosition(GameManager.instance.mousePosition3D + offset);
            selectedPlayer.transform.position
                = Vector2.MoveTowards(selectedPlayer.transform.position,
                                      GameManager.instance.mousePosition3D + offset,
                                      speed);
        }


        //if (selectedRigidBody)
        //{
        //    selectedRigidBody.MovePosition(GameManager.instance.mousePosition3D + offset);
        //}

        //Otherwise...
        if (inMotion && targetPosition != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition.Value, speed);

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

        GameManager.instance.targetPlayerName = targetPlayer.name;
    }

    private bool CollisionFromAbove(GameObject sender, GameObject receiver)
    {
        return sender.transform.position.y > receiver.transform.position.y
            && sender.transform.position.x <= receiver.transform.position.x + tileSize / 2
            && sender.transform.position.x >= receiver.transform.position.x - tileSize / 2;
    }

    private bool CollisionFromBelow(GameObject sender, GameObject receiver)
    {
        return sender.transform.position.y < receiver.transform.position.y
            && sender.transform.position.x <= receiver.transform.position.x + tileSize / 2
            && sender.transform.position.x >= receiver.transform.position.x - tileSize / 2;
    }

    private bool CollisionFromRight(GameObject sender, GameObject receiver)
    {
        return sender.transform.position.x > receiver.transform.position.x
            && sender.transform.position.y <= receiver.transform.position.y + tileSize / 2
            && sender.transform.position.y >= receiver.transform.position.y - tileSize / 2;
    }

    private bool CollisionFromLeft(GameObject sender, GameObject receiver)
    {
        return sender.transform.position.x < receiver.transform.position.x
            && sender.transform.position.y <= receiver.transform.position.y + tileSize / 2
            && sender.transform.position.y >= receiver.transform.position.y - tileSize / 2;
    }

    private GameObject GetClosestCell(Vector2 position)
    {
        return Common.FindClosestByTag(position, Tag.Tile);
    }

    private GameObject GetCellAbove(Vector2 position, int spaces = 1)
    {
        position.y += tileSize * spaces;
        return GetClosestCell(position);
    }

    private GameObject GetCellBelow(Vector2 position, int spaces = 1)
    {
        position.y -= tileSize * spaces;
        return GetClosestCell(position);
    }

    private GameObject GetCellRight(Vector2 position, int spaces = 1)
    {
        position.x += tileSize * spaces;
        return GetClosestCell(position);
    }

    private GameObject GetCellLeft(Vector2 position, int spaces = 1)
    {
        position.x -= tileSize * spaces;
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

        bool wasAbove = sender.transform.position.y >= targetCell.transform.position.y + tileSize / 2;
        bool wasBelow = sender.transform.position.y < targetCell.transform.position.y - tileSize / 2;
        bool wasLeft = sender.transform.position.x >= targetCell.transform.position.x + tileSize / 2;
        bool wasRight = sender.transform.position.x < targetCell.transform.position.x - tileSize / 2;

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
        var playerBehavior = player.GetComponent<ActorBehavior>();
        if (playerBehavior.inMotion)
            return;

        playerBehavior.targetPosition = targetCell.transform.position;
        playerBehavior.inMotion = true;
        targetPlayer = null;
        targetCell = null;
        GameManager.instance.targetPlayerName = "";
    }







    private bool IsWallCollision(Collision2D other)
    {
        return other.gameObject.CompareTag(Tag.Wall);
    }

    private bool IsPlayerCollision(Collider2D other)
    {
        return other.gameObject.CompareTag(Tag.Actor);
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
