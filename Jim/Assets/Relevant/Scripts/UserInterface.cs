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
    public GameObject selectedMacro;   //for changing macros;

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
        nameInField = macroNameFieldText.text;
        goalInField = macroGoalFieldText.text;
        if (macroUI.Count < 8)
        {
            GameObject newMacro = Instantiate(macroUIPrefab, macroGrid.transform);
            macroUI.Add(newMacro);

            macroNameFieldText.text = "Type Macro Name";
            macroGoalFieldText.text = "Type Macro Goal";

            string newMacroDisplay = "";
            TextMeshProUGUI text = newMacro.GetComponentInChildren<TextMeshProUGUI>();

            newMacroDisplay += nameInField + ": ";
            newMacroDisplay += "0/" + goalInField;
            text.text = newMacroDisplay;

            newMacro.GetComponentInChildren<MacroHolder>().heldMacro = profileManager.CreateNewMacro(nameInField, int.Parse(goalInField));
        }
        else
        {
            profileManager.CreateNewMacro(nameInField, int.Parse(goalInField));
        }

        macroCreationButton.SetActive(true);
        foreach (GameObject macro in macroUI)
        {
            macro.SetActive(true);
        }
        
        macroNameField.SetActive(false);
        macroGoalField.SetActive(false);
        macroConfirmationButton.SetActive(false);
    }

    public void OpenMacroChange()
    {
        foreach (GameObject ui in macroUI)
        {
            if (ui != selectedMacro.transform.parent.parent.gameObject)
                ui.SetActive(false);
        }
        macroCreationButton.SetActive(false);
        macroNameField.SetActive(false);
        macroGoalField.SetActive(false);
        macroConfirmationButton.SetActive(false);

        macroChangeField.SetActive(true);
    }
    public void ChangeMacro()
    {
        profileManager.AddToMacro(selectedMacro.GetComponent<MacroHolder>().heldMacro._macroName, int.Parse(macroChangeFieldText.text));
        UpdateMacroTexts();
        macroChangeFieldText.text = "Change Macro by...";

        macroCreationButton.SetActive(true);
        foreach (GameObject macro in macroUI)
        {
            macro.SetActive(true);
            macro.GetComponentInChildren<InteractionDetection>().stop = false;
        }
        macroNameField.SetActive(true);
        macroGoalField.SetActive(true);
        macroConfirmationButton.SetActive(true);

        macroChangeField.SetActive(false);
    }

    void UpdateMacroTexts()
    {
        foreach (GameObject macro in macroUI)
        {
            TextMeshProUGUI text = macro.GetComponentInChildren<TextMeshProUGUI>();
            MacroHolder currentMacro = macro.GetComponentInChildren<MacroHolder>();
            string newText = currentMacro.heldMacro._macroName.ToString() + ": " + currentMacro.heldMacro._macroCurrent.ToString() + "/" + currentMacro.heldMacro._macroGoal;
            text.text = newText;
        }
    }
    public void StartSelectionWait()
    {
        StartCoroutine(WaitForSelection());
    }
    IEnumerator WaitForSelection()
    {
        if(macroUI.Count == 0)
        {
            StopCoroutine(WaitForSelection());
        }
        yield return new WaitUntil(Selection);
        yield return new WaitForSeconds(.5f);
        yield return new WaitUntil(Selection);
        ChangeMacro();
    }
    bool Selection()
    {
        if (selectedMacro != null)
            return true;
        else
            return false;
    }
}
