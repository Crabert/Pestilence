using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
{
    public Profile[] savedProfiles;
    public Profile loadedProfile;
    public UserInterface userInterface;

    public void CreateNewMacro(string macroName, int goal)
    {
        loadedProfile.SavedMacros.Add(new SavedMacro(macroName, goal));
    }
    public void CreateNewMacro(string macroName, int goal, Image picture)
    {
        loadedProfile.SavedMacros.Add(new SavedMacro(macroName, goal, picture));
    }

    public void AddToMacro(string macroName, int addAmount)
    {
        foreach (SavedMacro macro in loadedProfile.SavedMacros)
        {
            if(macro != null)
            if(macro._macroName == macroName)
                macro._macroCurrent += addAmount;
        }
    }
    public void RemoveFromMacro(string macroName, int removeAmount)
    {
        foreach (SavedMacro macro in loadedProfile.SavedMacros)
        {
            if (macro._macroName == "macroName")
                macro._macroCurrent -= removeAmount;
        }
    }
}
