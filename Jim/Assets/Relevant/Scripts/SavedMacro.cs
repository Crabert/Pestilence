using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavedMacro
{
    [HideInInspector] public string _macroName;
    [HideInInspector] public int _macroGoal;
    [HideInInspector] public int _macroCurrent;
    [HideInInspector] public Image _macroPicture;

    public SavedMacro(string macroName, int macroGoal)
    {
        _macroName = macroName;
        _macroGoal = macroGoal;
    }

    public SavedMacro(string macroName, int macroGoal, Image macroPicture)
    {
        _macroName = macroName;
        _macroGoal = macroGoal;
        _macroPicture = macroPicture;
    }
    
}
