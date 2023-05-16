using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{

    public GameObject selectedObject;
    Vector3 offset;

    private GameObject currentGridCell;

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
                selectedObject = targetObject.transform.gameObject;
                offset = selectedObject.transform.position - mousePosition;
            }
        }
        if (selectedObject)
        {
            selectedObject.transform.position = mousePosition + offset;
        }
    }

    void OnMouseDown()
    {
        Debug.Log("OnMouseDown");
    }

    void OnMouseUp()
    {
        Debug.Log("OnMouseUp");
        DropPlayer();


    }


    private void DropPlayer()
    {
        if (!selectedObject)
            return;

        List<RaycastHit2D> hits = Physics2D.RaycastAll(selectedObject.transform.position, selectedObject.transform.forward).ToList();
        if (hits == null || hits.Count < 1)
            return;


        //GameObject gridCell = hits.Where(x => x.collider.gameObject.CompareTag("GridCell")).FirstOrDefault().collider?.gameObject;
        //if (gridCell == null)
        //{
        //    gridCell = Common.FindClosestByTag(selectedObject.transform.position, "GridCell");
        //}
        var gridCell = Common.FindClosestByTag(selectedObject.transform.position, "GridCell");

        selectedObject.transform.position = gridCell.transform.position;

        selectedObject = null;
    }




}
