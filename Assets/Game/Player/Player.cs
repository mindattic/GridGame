using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Coordinates coodinates = new Coordinates();
    public GameObject selectedPlayer;
   
    private GameObject currentGridCell;
    private Vector3 offset;


    private void Start()
    {
        var screenSize = new Vector2(Common.GetScreenToWorldWidth, Common.GetScreenToWorldHeight);
        float size = screenSize.x / 6;
        transform.localScale = Vector3.one * size;

        var collider = GetComponent<BoxCollider2D>();
        collider.size = new Vector2(size, size);
    }

    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D targetObject = Physics2D.OverlapPoint(mousePosition);

            if (targetObject && targetObject.tag == "Player")
            {
                selectedPlayer = targetObject.transform.gameObject;
                offset = selectedPlayer.transform.position - mousePosition;
            }
        }

        if (selectedPlayer)
        {
            selectedPlayer.transform.position = mousePosition + offset;
        }
    }

    void OnMouseDown()
    {
        //Debug.Log("OnMouseDown");
    }

    void OnMouseUp()
    {
        //Debug.Log("OnMouseUp");
        DropPlayer();


    }


    private void DropPlayer()
    {
        if (!selectedPlayer)
            return;

        //List<RaycastHit2D> hits = Physics2D.RaycastAll(selectedPlayer.transform.position, selectedPlayer.transform.forward).ToList();
        //if (hits == null || hits.Count < 1)
        //    return;
        //GameObject gridCell = hits.Where(x => x.collider.gameObject.CompareTag("GridCell")).FirstOrDefault().collider?.gameObject;
        //if (gridCell == null)
        //{
        //    gridCell = Common.FindClosestByTag(selectedObject.transform.position, "GridCell");
        //}

        var closestCell = Common.FindClosestByTag(selectedPlayer.transform.position, "GridCell");

        selectedPlayer.transform.position = closestCell.transform.position;
        selectedPlayer.GetComponent<Player>().coodinates = closestCell.GetComponent<GridCell>().coodinates;

        selectedPlayer = null;
    }


}
