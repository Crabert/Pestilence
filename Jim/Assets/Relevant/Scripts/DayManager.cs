using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    Profile loadedProfile;

    private void Start()
    {
        loadedProfile = GetComponent<ProfileManager>().loadedProfile;
    }

    public void CheckGoals()
    {
        foreach (SavedMacro macro in loadedProfile.SavedMacros)
        {
            if(macro._macroCurrent >= macro._macroGoal)
            {
                print("Good Job! :)))))))))");
            }
        }
    }
}
