using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerBehavior : MonoBehaviour
{
    private bool isRunning = false;
    private const float maxTime = 6f;
    private float timeRemaining = 6f;
    private RectTransform rect;
    private Image progressBar;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        progressBar = GetComponent<Image>();


        rect.offsetMin = new Vector2(0, 1550); //Set left, bottom
        rect.offsetMax = new Vector2(0, -100); //Set right, top


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

    //private void SetRect(RectTransform trs, float left, float top, float right, float bottom)
    //{
    //    trs.offsetMin = new Vector2(left, bottom);
    //    trs.offsetMax = new Vector2(-right, -top);
    //}
}
