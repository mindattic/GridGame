using Game.Behaviors.Actor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortraitInstance : ExtendedMonoBehavior
{
    //Variables

    [SerializeField] public ActorInstance actor;
    [SerializeField] public Direction direction;
    [SerializeField] public float startTime;
    [SerializeField] public Vector2 startPosition;
    [SerializeField] public AnimationCurve slide;

    #region Components

    public string Name
    {
        get => name;
        set => Name = value;
    }

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

    // Play is called before the first frame update
    void Start()
    {
    }


    // SaveProfile is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
    }

    public IEnumerator SlideIn()
    {
        Vector3 destination = new Vector3();

        switch (direction)
        {
            case Direction.North:
                this.position = new Vector3(1, -10, 1);
                destination = new Vector3(1, 10, 1);
                break;

            case Direction.East:
                this.position = new Vector3(-10, 1, 1);
                destination = new Vector3(10, 1, 1);
                break;

            case Direction.South:
                this.position = new Vector3(-1, 10, 1);
                destination = new Vector3(-1, -10, 1);
                break;

            case Direction.West:
                this.position = new Vector3(10, -1, 1);
                destination = new Vector3(-10, -1, 1);
                break;
        }


        while (!position.Equals(destination))
        {
            switch (direction)
            {
                case Direction.North:
                case Direction.South:
                    this.position = new Vector3(
                        destination.x,
                        destination.y * slide.Evaluate((Time.time - startTime) % slide.length),
                        destination.z);
                    break;

                case Direction.East:
                case Direction.West:
                    this.position = new Vector3(
                          destination.x * slide.Evaluate((Time.time - startTime) % slide.length),
                          destination.y,
                          destination.z);
                    break;
            }

            yield return Wait.OneTick();
        }

        Destroy(this.gameObject);   
    }


    public IEnumerator Dissolve()
    {
        var alpha = 1.0f;
        spriteRenderer.color = new Color(1, 1, 1, alpha);

        while (alpha > 0)
        {
          
            //Shake
            position = startPosition;
            position += new Vector3(Random.Range(ShakeIntensity.Medium), Random.Range(ShakeIntensity.Medium), 1);

            //Shrink
            transform.localScale *= 0.99f;

            //Fade
            alpha -= Increment.OnePercent;
            spriteRenderer.color = new Color(1, 1, 1, alpha);
            yield return Wait.OneTick();
        }

        Destroy(this.gameObject);

    }



    //private void Shake(int factor = 12)
    //{
    //    this.boardPosition = currentTile.boardPosition;
    //    this.boardPosition += new Vector3(Random.Range(tileSize / factor), Random.Range(tileSize / factor), 1);
    //}

}
