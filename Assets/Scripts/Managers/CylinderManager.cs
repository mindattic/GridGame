using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderManager : MonoBehaviour
{
    [SerializeField] public float ceiling = 1f;
    [SerializeField] public float floor = -1f;
    [SerializeField] public float speed = 0.05f;

    private bool isRising = true;

    void Start()
    {

    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        if (isRising && transform.position.y < 1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, ceiling, transform.position.z), speed * Time.deltaTime);
        }
        else
        {
            speed = Random.Int(2, 5) * 0.01f;
            floor = -1f + (-1f * Random.Percent());
            isRising = false;
        }

        if (!isRising && transform.position.y > -1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, floor, transform.position.z), speed * Time.deltaTime);
        }
        else
        {
            speed = Random.Int(2, 5) * 0.01f;
            ceiling = 1f + (1f * Random.Percent());
            isRising = true;
        }

        transform.Rotate(Vector3.up * (3f * Time.deltaTime));
    }
}
