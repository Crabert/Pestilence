using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterface : MonoBehaviour
{
    public ProfileManager profileManager;
    
    private void Start()
    {
        foreach (SavedMacro macro in profileManager.loadedProfile.SavedMacros)
        {
            profileManager.loadedProfile.SavedMacros.Remove(macro);
        }
        profileManager.CreateNewMacro("Protein", 1600);
        profileManager.AddToMacro("Protein", 400);
        print(profileManager.loadedProfile.SavedMacros[0]._macroName);
        profileManager.AddToMacro("Protein", 400);
        profileManager.AddToMacro("Protein", 400);
        profileManager.AddToMacro("Protein", 400);
        profileManager.AddToMacro("Protein", 400);
        GetComponent<DayManager>().CheckGoals();

    }
}
