using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerManager : MonoBehaviour
{

    public Coordinates coodinates = new Coordinates();

    public Rigidbody2D selectedRigidBody;


    private Vector3 mouseOffset;

    //Declare and initialize a new List of GameObjects called currentCollisions.
    private List<Collision2D> currentCollisions = new List<Collision2D>();

    private Vector3 mousePosition;
    private Collider2D target;

    private float cellSize => Global.instance.cellSize;
    private Vector3 position => transform.position;


    Vector3 newPosition;

    Vector3 offset;

    private void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
            selectedRigidBody.position += Vector2.up;


        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0) && !selectedRigidBody)
        {
            PickupPlayer();
        }
        if (Input.GetMouseButtonUp(0) && selectedRigidBody)
        {
            DropPlayer();
        }
    }

    void FixedUpdate()
    {
        if (!selectedRigidBody) return;
        //{
        //    selectedRigidBody.MovePosition(mousePosition + offset);
        //}
        selectedRigidBody.MovePosition(mousePosition + offset);
        //selectedRigidBody.position = Vector2.MoveTowards(selectedRigidBody.position, mousePosition, 0.5f);

    }

    void OnMouseDown()
    {
        Debug.Log("OnMouseDown");
        //PickupPlayer();
    }

    void OnMouseUp()
    {
        Debug.Log("OnMouseUp");
        DropPlayer();
    }

    private void PickupPlayer()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D targetObject = Physics2D.OverlapPoint(mousePosition);
        if (targetObject)
        {
            selectedRigidBody = targetObject.transform.gameObject.GetComponent<Rigidbody2D>();
            offset = selectedRigidBody.transform.position - mousePosition;
        }
    }

    private void DropPlayer()
    {
        var closestCell = Common.FindClosestByTag(selectedRigidBody.gameObject.transform.position, Tag.Cell);

        selectedRigidBody.transform.position = closestCell.transform.position;
        selectedRigidBody.velocity = Vector2.zero;

        selectedRigidBody.gameObject.GetComponent<PlayerManager>().coodinates = closestCell.GetComponent<CellManager>().coodinates;

        selectedRigidBody = null;
    }





    private bool isWallCollision(Collision2D other)
    {
        return other.gameObject.CompareTag(Tag.Wall);
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (isWallCollision(other))
        {
            //selectedRigidBody.velocity = Vector2.zero;

            //var point = other.contacts[0].point;

            //bool isPlayerAbove = position.y > point.y;
            //bool isPlayerBelow = position.y < point.y;
            //bool isPlayerRight = position.x > point.x;
            //bool isPlayerLeft = position.x < point.x;



            //if (isPlayerAbove)
            //    selectedRigidBody.MovePosition(new Vector2(selectedRigidBody.position.x, point.y + 0.001f));



            //        //if (other.contacts[0].point == Vector2.up)
            //        //{
            //        //    Debug.Log("Top");
            //        //}
            //        //else if (other.contacts[0].point == Vector2.right)
            //        //{
            //        //    Debug.Log("Right");
            //        //}
            //        //else if (other.contacts[0].point == Vector2.down)
            //        //{
            //        //    Debug.Log("Bottom");
            //        //}
            //        //else if (other.contacts[0].point == Vector2.left)
            //        //{
            //        //    Debug.Log("Left");
            //        //}



            //        //var x = transform.position.normalized
            //        //if()
            Debug.Log("Enter Wall");

            //        // Add the collision with to the list.
            currentCollisions.Add(other);
        }
    }


    private void OnCollisionExit2D(Collision2D other)
    {
        if (isWallCollision(other))
        {
            //Debug.Log("Exit Wall");

            //        //isMoveableUp = other.contacts[0].point.y <= transform.position.y;
            //        //isMoveableDown = other.contacts[0].point.y > transform.position.y;
            //        //isMoveableRight = other.contacts[0].point.x <= transform.position.x;
            //        //isMoveableLeft = other.contacts[0].point.x > transform.position.x;

            //        //Remove the GameObject collided with from the list.
            currentCollisions.Remove(other);
        }
    }

}
