using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortraitBehavior : ExtendedMonoBehavior
{
    //Variables
    [SerializeField] public string id;

    public ActorBehavior actor;
    public Direction direction;

    #region Components

    public Transform parent
    {
        get => gameObject.transform.parent;
        set => gameObject.transform.SetParent(value, true);
    }

    public Vector3 position
    {
        get => gameObject.transform.position;
        set => gameObject.transform.position = value;
    }

    public Vector3 scale
    {
        get => gameObject.transform.localScale;
        set => gameObject.transform.localScale = value;
    }

    public SpriteRenderer spriteRenderer;

    public Sprite sprite
    {
        get => spriteRenderer.sprite;
        set => spriteRenderer.sprite = value;
    }

    public Color color
    {
        get => spriteRenderer.color;
        set => spriteRenderer.color = value;
    }

    public int sortingOrder
    {
        set
        {
            spriteRenderer.sortingOrder = value;
        }
    }

    #endregion

    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
    }

    public void Play(ActorBehavior actor, Direction direction)
    {
        this.actor = actor;
        this.direction = direction;

        StartCoroutine(Slide());
    }

    IEnumerator Slide()
    {
        List<Vector3> destination = new List<Vector3>();

        switch (direction)
        {
            case Direction.North:
                this.position = new Vector3(0, -10, 1);
                destination.Add(new Vector3(0, 0, 1));
                destination.Add(new Vector3(0, 10, 1));
                break;

            case Direction.East:
                this.position = new Vector3(-10, 0, 1);
                destination.Add(new Vector3(0, 0, 1));
                destination.Add(new Vector3(10, 0, 1));
                break;

            case Direction.South:
                this.position = new Vector3(0, 10, 1);
                destination.Add(new Vector3(0, 0, 1));
                destination.Add(new Vector3(0, -10, 1));
                break;

            case Direction.West:
                this.position = new Vector3(10, 0, 1);
                destination.Add(new Vector3(0, 0, 1));
                destination.Add(new Vector3(-10, 0, 1));
                break;
        }

        int index = 0;
        while (index < destination.Count)
        {
            var distance = Vector3.Distance(position, destination[index]);
            this.position = Vector3.MoveTowards(position, destination[index], 5 * distance * Time.deltaTime);
            bool isCloseToDestination = distance < 0.05f;
            if (isCloseToDestination)
            {
                this.position = destination[index];
                index++;
            }

            yield return new WaitForSeconds(Interval.One);
        }

        portraitManager.Remove(this);
        Destroy(this.gameObject);
    }

}
