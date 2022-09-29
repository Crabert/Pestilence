using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
{
    public Profile[] savedProfiles;
    public Profile loadedProfile;
    public UserInterface userInterface;


    public SavedMacro CreateNewMacro(string macroName, int goal)
    {
        SavedMacro newMacro = new SavedMacro(macroName, goal);
        loadedProfile.SavedMacros.Add(newMacro);
        return newMacro;
    }
    public SavedMacro CreateNewMacro(string macroName, int goal, Image picture)
    {
        SavedMacro newMacro = new SavedMacro(macroName, goal, picture);
        loadedProfile.SavedMacros.Add(newMacro);
        return newMacro;
    }

    public void ClearMacroProgress()
    {
        foreach(SavedMacro macro in loadedProfile.SavedMacros)
            macro._macroCurrent = 0;
    }
    public void ClearMacroProgress(string macroName)
    {
        foreach(SavedMacro macro in loadedProfile.SavedMacros)
            if(macro._macroName == macroName)
                macro._macroCurrent = 0;
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
