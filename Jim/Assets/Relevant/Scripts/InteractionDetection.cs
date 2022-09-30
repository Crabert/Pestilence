using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionDetection : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    UserInterface userInterface;
    public bool stop;
    bool start = false;
    public void Start()
    {
        userInterface = GameObject.Find("EventSystem").GetComponent<UserInterface>();
    }
    public void Update()
    {
        if (start && !stop)
            if (Input.GetMouseButton(0))
                stop = true;
        if (Input.GetMouseButton(0) && stop)
            stop = false;
    }
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (stop) return;

        userInterface.selectedMacro = GetComponentInChildren<MacroHolder>().gameObject;
        start = true;
    }
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (stop) return;

        userInterface.selectedMacro = null;
        start = false;
    }
}
