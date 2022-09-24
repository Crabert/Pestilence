using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "ScriptableObject/Profile")]
public class Profile : ScriptableObject
{
    public List<SavedMacro> SavedMacros = new List<SavedMacro>();
    public int Weight;
    public int HeightFeet;
    public int HeightInch;
    public int Age;

    public string ProfileName;
    public Image ProfilePicture;

    //something about the theme of the user interface
}
