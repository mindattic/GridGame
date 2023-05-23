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

    private List<Collision2D> currentCollisions = new List<Collision2D>();

    private Vector3 mousePosition;
    private Collider2D target;

    private float cellSize => Global.instance.cellSize;
    private Vector3 position => transform.position;


    Vector3 offset;

    private void Start()
    {

    }

    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && !selectedRigidBody)
            PickupPlayer();
   
        if (Input.GetMouseButtonUp(0) && selectedRigidBody)
            DropPlayer();
    }

    void FixedUpdate()
    {
        if (!selectedRigidBody) return;

        selectedRigidBody.MovePosition(mousePosition + offset);
        //selectedRigidBody.position = Vector2.MoveTowards(selectedRigidBody.position, mousePosition, 0.5f);
    }

    void OnMouseDown()
    {
        //Debug.Log("OnMouseDown");
        //PickupPlayer();
    }

    void OnMouseUp()
    {
        //Debug.Log("OnMouseUp");
        //DropPlayer();
    }

    private void PickupPlayer()
    {
        Collider2D targetObject = Physics2D.OverlapPoint(mousePosition);
        if (targetObject && targetObject.CompareTag(Tag.Player))
        {
            selectedRigidBody = targetObject.transform.gameObject.GetComponent<Rigidbody2D>();
            offset = selectedRigidBody.transform.position - mousePosition;
        }
    }

    public void DropPlayer()
    {
        if (!selectedRigidBody) return;

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
            //Debug.Log("Enter Wall");
            currentCollisions.Add(other);
        }
    }


    private void OnCollisionExit2D(Collision2D other)
    {
        if (isWallCollision(other))
        {
            //Debug.Log("Exit Wall");
            currentCollisions.Remove(other);
        }
    }

}
