using UnityEngine;

public class TimerBehavior : ExtendedMonoBehavior
{
    private bool isRunning = false;
    private const float maxTime = 6f;
    private float timeRemaining = 6f;


    private SpriteRenderer spriteRenderer;
    Vector3 scale = new Vector3(1f, 1f, 1f);

    private void Awake()
    {
        spriteRenderer = GameObject.Find("TimerBar").transform.GetChild(1).GetComponent<SpriteRenderer>();
        spriteRenderer.transform.localScale = new Vector3(scale.x, scale.y, scale.z);
    }


    void Start()
    {



    }


    void Update()
    {


    }


    void FixedUpdate()
    {
        if (isRunning && timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            spriteRenderer.transform.localScale = new Vector3(scale.x * (timeRemaining / maxTime), scale.y, scale.z);
        }
        else
        {
            isRunning = false;
            selectedPlayerManager.Drop();
        }
    }

    public void Set(float scaleX = 1f, bool start = false)
    {
        spriteRenderer.transform.localScale = new Vector3(scaleX, scale.y, scale.z);
        timeRemaining = maxTime;
        isRunning = start;
    }


    public void Pause()
    {
        isRunning = false;
    }

    public void Play()
    {
        isRunning = false;
    }

}
