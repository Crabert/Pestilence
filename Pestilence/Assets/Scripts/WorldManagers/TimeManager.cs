using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float inGameSecondLength;
    
    public float seconds;
    public float minutes;
    public float hours;
    public float days;

    public TimePoint savedTime;
    
    private void Start()
    {
        Invoke("UpdateSeconds", inGameSecondLength);
    }
    private void Update()
    {
        if(seconds == 60)
        {
            seconds = 0;
            minutes++;
        }
        if(minutes == 60)
        {
            minutes = 0;
            hours++;
        }
        if(hours == 24)
        {
            hours = 0;
            days++;
        }

    }

    void UpdateSeconds()
    {
        seconds += 1;
        Invoke("UpdateSeconds", inGameSecondLength);
    }
}
