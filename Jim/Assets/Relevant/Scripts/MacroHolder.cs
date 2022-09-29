using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacroHolder : MonoBehaviour
{
    public SavedMacro heldMacro;

    private void Update()
    {
        print(heldMacro);
    }
    public void TriggerMacroChange()
    {
        GameObject.Find("EventSystem").GetComponent<UserInterface>().OpenMacroChange();
    }
}
