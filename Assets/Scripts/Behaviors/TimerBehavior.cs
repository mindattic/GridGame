using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerBehavior : ExtendedMonoBehavior
{
    private bool isRunning = false;
    private const float maxTime = 6f;
    private float timeRemaining = 6f;
    private RectTransform rect;
    private Image progressBar;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        progressBar = GetComponent<Image>();

      
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
            rect.localScale = new Vector3(timeRemaining / maxTime * 1f, 1f, 1f);
        }
        else
        {
            isRunning = false;
            selectedPlayerManager.Drop();
        }
    }

    public void Set(float scale = 1f, bool start = false)
    {
        rect.localScale = new Vector3(scale, 1f, 1f);
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
