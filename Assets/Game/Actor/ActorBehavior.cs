using System.Collections.Generic;
using UnityEngine;

public class ActorBehavior : MonoBehaviour
{
    //Variables
    [field: SerializeField] public Vector2Int location { get; set; }
    [field: SerializeField] public Team team = Team.Neutral;

    private Vector3 mouseOffset;
    private GameObject targetPlayer;
    private GameObject targetCell;
    private Vector2? targetPosition;
    bool inMotion;
    float cursorSpeed;


    


    //GameManager properties
    private float tileSize => GameManager.instance.tileSize;
    private Vector2 size50 => GameManager.instance.size50;
    private Vector2 size100 => GameManager.instance.size100;
    private Vector3 mousePosition3D => GameManager.instance.mousePosition3D;
    private List<ActorBehavior> actors => GameManager.instance.actors;


    private GameObject selectedPlayer
    {
        get { return GameManager.instance.selectedPlayer; }
        set { GameManager.instance.selectedPlayer = value; }
    }

    public BoxCollider2D boxCollider2D
    {
        get => gameObject.GetComponent<BoxCollider2D>();
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

    private void Awake()
    {

    }

    private void Start()
    {
        cursorSpeed = GameManager.instance.tileSize / 5;
        transform.position = Geometry.PointFromGrid(location);
        transform.localScale = GameManager.instance.tileScale;
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

        Collider2D targetCollider2D = Physics2D.OverlapPoint(mousePosition3D);
        if (targetCollider2D == null || !targetCollider2D.CompareTag(Tag.Actor))
            return;

        selectedPlayer = targetCollider2D.transform.gameObject;
        GameManager.instance.selectedPlayerName = selectedPlayer.name;
        mouseOffset = selectedPlayer.transform.position - mousePosition3D;


        //Reduce all box collider sizes by 50%
        GameManager.instance.actors.ForEach(x => x.boxCollider2D.size = size50);
    }

    private void DropPlayer()
    {
        if (selectedPlayer == null)
            return;

        var closestCell = Common.FindClosestByTag(selectedPlayer.transform.position, Tag.Tile);
        selectedPlayer.transform.position = closestCell.transform.position;
        selectedPlayer = null;
        GameManager.instance.selectedPlayerName = "";

        //Increase all box collider sizes by 50%
        GameManager.instance.actors.ForEach(x => x.boxCollider2D.size = size100);
    }

    void FixedUpdate()
    {
       

        //If selected go...
        if (selectedPlayer != null)
        {
            //selectedRigidBody.MovePosition(GameManager.instance.mousePosition3D + mouseOffset);
            selectedPlayer.transform.position
                = Vector2.MoveTowards(selectedPlayer.transform.position,
                                      GameManager.instance.mousePosition3D + mouseOffset,
                                      cursorSpeed);
        }


        //if (selectedRigidBody)
        //{
        //    selectedRigidBody.MovePosition(GameManager.instance.mousePosition3D + mouseOffset);
        //}

        //Otherwise...
        if (inMotion && targetPosition != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition.Value, cursorSpeed);

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
