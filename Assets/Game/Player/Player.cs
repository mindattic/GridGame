using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{

    public GameObject selectedObject;
    Vector3 offset;

    private GameObject currentGridCell;


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
        //if (!Input.GetMouseButtonUp(0) && selectedObject)
        //{
        //    selectedObject = null;

        //    RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.forward, 10f);

        //    if (hit.collider != null && hit.collider.gameObject.tag == "GridCell")
        //    {
        //        transform.position = hit.collider.gameObject.transform.position;
        //    }


        //    ////if (currentGridCell != null)
        //    //    transform.position = currentGridCell.transform.position;
        //}
    }

    void OnMouseDown()
    {
        // If your mouse hovers over the GameObject with the script attached, output this message
        Debug.Log("OnMouseDown");
    }

    void OnMouseUp()
    {
        // If your mouse hovers over the GameObject with the script attached, output this message
        Debug.Log("OnMouseUp");

        if (selectedObject)
        {
            RaycastHit2D[] hit = Physics2D.RaycastAll(selectedObject.transform.position, selectedObject.transform.forward);

            GameObject gridCell = hit.Where(x => x.collider.gameObject.CompareTag("GridCell")).FirstOrDefault().collider.gameObject;
            if (gridCell)
            {
                selectedObject.transform.position = gridCell.transform.position;
            }

            selectedObject = null;
        }


        //if (!Input.GetMouseButtonUp(0) && selectedObject)
        //{
        //    selectedObject = null;

        //    RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.forward, 10f);

        //    if (hit.collider != null && hit.collider.gameObject.tag == "GridCell")
        //    {
        //        transform.position = hit.collider.gameObject.transform.position;
        //    }


        //    ////if (currentGridCell != null)
        //    //    transform.position = currentGridCell.transform.position;
        //}



    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter2D");

        //if (other.gameObject.tag == "GridCell")
        //{
        //    currentGridCell = other.gameObject;
        //}
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("OnTriggerExit2D");

        //if (other.gameObject.tag == "GridCell")
        //{
        //    currentGridCell = null;
        //}
    }
}
