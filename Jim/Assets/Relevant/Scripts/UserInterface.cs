using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    public ProfileManager profileManager;

    public List<GameObject> macroUI = new List<GameObject>();
    public GameObject macroUIPrefab;
    public GameObject macroGrid;

    public GameObject macroCreationButton;
    public GameObject macroConfirmationButton;
    public GameObject macroNameField;
    public GameObject macroGoalField;
    public GameObject macroChangeField;
    public TMP_InputField macroChangeFieldText;
    public TMP_InputField macroNameFieldText;
    public TMP_InputField macroGoalFieldText;
    string nameInField;
    string goalInField;
    GameObject selectedMacro;   //for changing macros;

    private void Start()
    {
        foreach (SavedMacro macro in profileManager.loadedProfile.SavedMacros)
        {
            profileManager.loadedProfile.SavedMacros.Remove(macro);
        }
    }

    public void OpenMacroCreationWindow()
    {
        macroCreationButton.SetActive(false);
        foreach (GameObject macro in macroUI)
        {
            macro.SetActive(false);
        }
        macroNameField.SetActive(true);
        macroGoalField.SetActive(true);
        macroConfirmationButton.SetActive(true);
    }
    public void GetStrings()
    {
        GameObject newMacro = Instantiate(macroUIPrefab, macroGrid.transform);
        macroUI.Add(newMacro);

        nameInField = macroNameFieldText.text;
        goalInField = macroGoalFieldText.text;

        macroNameFieldText.text = "Type Macro Name";
        macroGoalFieldText.text = "Type Macro Goal";

        string newMacroDisplay = "";
        TextMeshProUGUI text = newMacro.GetComponentInChildren<TextMeshProUGUI>();

        newMacroDisplay += nameInField + ": ";
        newMacroDisplay += "0/" + goalInField;
        text.text = newMacroDisplay;

        profileManager.CreateNewMacro(nameInField, int.Parse(goalInField));
        newMacro.GetComponentInChildren<MacroHolder>().heldMacro = profileManager.loadedProfile.SavedMacros[^1];

        macroCreationButton.SetActive(true);
        foreach (GameObject macro in macroUI)
        {
            macro.SetActive(true);
        }
        
        macroNameField.SetActive(false);
        macroGoalField.SetActive(false);
        macroConfirmationButton.SetActive(false);
    }

    public void OpenMacroChange(GameObject selectedMacro)
    {
        foreach (GameObject ui in macroUI)
        {
            if (ui != selectedMacro)
                ui.SetActive(false);
        }
        macroCreationButton.SetActive(false);
        foreach (GameObject macro in macroUI)
        {
            macro.SetActive(false);
        }
        macroNameField.SetActive(false);
        macroGoalField.SetActive(false);
        macroConfirmationButton.SetActive(false);
        this.selectedMacro = selectedMacro;
    }
    public void ChangeMacro(string gain)
    {
        selectedMacro.GetComponent<MacroHolder>().heldMacro._macroCurrent += int.Parse(macroChangeFieldText.text);
        macroChangeFieldText.text = "Change Macro by...";

        foreach (GameObject ui in macroUI)
        {
            if (ui != selectedMacro)
                ui.SetActive(true);
        }
        macroCreationButton.SetActive(true);
        foreach (GameObject macro in macroUI)
        {
            macro.SetActive(true);
        }
        macroNameField.SetActive(true);
        macroGoalField.SetActive(true);
        macroConfirmationButton.SetActive(true);

    }

}
