using UnityEngine;

public class TimerBehavior : ExtendedMonoBehavior
{
    private bool IsRunning = false;
    private const float MaxDuration = 6f;
    private float TimeRemaining = 6f;


    private SpriteRenderer SpriteRenderer;
    Vector3 Scale = new Vector3(1f, 1f, 1f);

    private void Awake()
    {
        SpriteRenderer = GameObject.Find("TimerBar").transform.GetChild(1).GetComponent<SpriteRenderer>();
        SpriteRenderer.transform.localScale = new Vector3(Scale.x, Scale.y, Scale.z);
    }


    void Start()
    {



    }


    void Update()
    {


    }


    void FixedUpdate()
    {
        if (IsRunning && TimeRemaining > 0)
        {
            TimeRemaining -= Time.deltaTime;
            SpriteRenderer.transform.localScale = new Vector3(Scale.x * (TimeRemaining / MaxDuration), Scale.y, Scale.z);
        }
        else
        {
            IsRunning = false;
            SelectedPlayerManager.Drop();
        }
    }

    public void Set(float scaleX = 1f, bool start = false)
    {
        SpriteRenderer.transform.localScale = new Vector3(scaleX, Scale.y, Scale.z);
        TimeRemaining = MaxDuration;
        IsRunning = start;
    }


    public void Pause()
    {
        IsRunning = false;
    }

    public void Play()
    {
        IsRunning = false;
    }

}
