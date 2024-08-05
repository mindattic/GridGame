using UnityEngine;

public class TimerBehavior : ExtendedMonoBehavior
{
    private bool isRunning = false;
    private const float maxDuration = 6f;
    private float timeRemaining = 6f;
    private SpriteRenderer spriteRenderer;
    Vector3 scale = new Vector3(1f, 1f, 1f);

    private void Awake()
    {
        spriteRenderer = GameObject.Find(Constants.TimerBar).transform.GetChild(1).GetComponent<SpriteRenderer>();
        spriteRenderer.transform.localScale = new Vector3(scale.x, scale.y, scale.z);
    }

    void FixedUpdate()
    {
        if (isRunning && timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            spriteRenderer.transform.localScale = new Vector3(scale.x * (timeRemaining / maxDuration), scale.y, scale.z);
        }
        else
        {
            isRunning = false;
            selectedPlayerManager.Unselect();
        }
    }

    public void Set(float scaleX, bool start)
    {
        spriteRenderer.transform.localScale = new Vector3(scaleX, scale.y, scale.z);
        timeRemaining = maxDuration;
        isRunning = start;
    }

    public void Reset()
    {
        Set(scaleX: 1f, start: false);
    }

    public void Restart()
    {
        Set(scaleX: 1f, start: true);
    }

    public void Empty()
    {
        Set(scaleX: 0f, start: false);
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
