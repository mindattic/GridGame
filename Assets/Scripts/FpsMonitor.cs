//via: https://discussions.unity.com/t/accurate-frames-per-second-count/21088/6

using UnityEngine;

public class FpsMonitor
{
    const float measurePeriod = 0.5f;
    private int i = 0;
    private float nextPeriod = 0;
    public int current;

    public void Start()
    {
        nextPeriod = Time.realtimeSinceStartup + measurePeriod;
    }

    public void Update()
    {
        i++;

        if (Time.realtimeSinceStartup < nextPeriod)
            return;

        current = (int)(i / measurePeriod);
        nextPeriod += measurePeriod;
        i = 0;
    }
}

