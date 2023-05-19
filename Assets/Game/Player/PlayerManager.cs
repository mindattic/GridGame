using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public Coordinates coodinates = new Coordinates();
    public GameObject selectedPlayer;

    private Vector3 mouseOffset;
    //private float cellSize;

    private Vector3 mousePosition;
    private Collider2D target;


    private float cellSize => Global.instance.cellSize;


    private bool isWall(Collision2D other)
    {
        return other.collider.gameObject.CompareTag(Tag.Wall);
    }


    private void Start()
    {
        //var screenSize = new Vector2(Common.GetScreenToWorldWidth, Common.GetScreenToWorldHeight);
        //cellSize = screenSize.x / 6;
        transform.localScale = Vector3.one * cellSize;

        var collider = GetComponent<BoxCollider2D>();
        collider.size = new Vector2(cellSize, cellSize);
    }

    void Update()
    {
        if (selectedPlayer == null) return;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        selectedPlayer.transform.position = mousePosition + mouseOffset;


        //var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        ////Determine if mouse down
        //if (!Input.GetMouseButtonDown(0)) return;

        ////Select player object      
        //target = Physics2D.OverlapPoint(mousePosition);
        //if (target == null || !target.CompareTag(Tag.Player)) return;
        //selectedPlayer = target.transform.gameObject;
        //if (selectedPlayer == null) return;

        ////Assign player object position
        //offset = selectedPlayer.transform.position - mousePosition;
        //selectedPlayer.transform.position = mousePosition + offset;
    }

    void OnMouseDown()
    {
        //Debug.Log("OnMouseDown");
        PickupPlayer();
    }

    void OnMouseUp()
    {
        //Debug.Log("OnMouseUp");
        DropPlayer();
    }

    private void PickupPlayer()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D targetObject = Physics2D.OverlapPoint(mousePosition);
        if (targetObject == null || !targetObject.CompareTag(Tag.Player)) return;

        selectedPlayer = targetObject.transform.gameObject;
        mouseOffset = selectedPlayer.transform.position - mousePosition;
    }

    private void DropPlayer()
    {
        if (!selectedPlayer)
            return;

        var closestCell = Common.FindClosestByTag(selectedPlayer.transform.position, Tag.Cell);

        selectedPlayer.transform.position = closestCell.transform.position;
        selectedPlayer.GetComponent<PlayerManager>().coodinates = closestCell.GetComponent<CellManager>().coodinates;

        selectedPlayer = null;
    }




    private void OnCollisionEnter2D(Collision2D other)
    {
        if (isWall(other))
        {
            //var x = transform.position.normalized
            //if()
            Debug.Log("Enter Wall");


        }



    }


    private void OnCollisionExit2D(Collision2D other)
    {
        if (isWall(other))
        {
            //var x = transform.position.normalized
            //if()
            Debug.Log("Exit Wall");


        }



    }

}
