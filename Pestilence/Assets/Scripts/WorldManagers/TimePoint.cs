using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePoint
{
    public List<float> heldTime = new List<float>();    //the held values of seconds minutes hours days etc 
    public TimePoint(List<float> timeToHold)
    {
        heldTime = timeToHold;
    }
}
