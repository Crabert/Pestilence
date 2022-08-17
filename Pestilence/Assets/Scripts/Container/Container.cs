using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Container : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ContainerArray containerArray;
    bool hover;
    public int size;
    public int slotAmount;
    public float closeDistance;
    [HideInInspector]public ContainerArrayParent grid;
    [HideInInspector]public bool open;

    private void Start()
    {
        grid = GameObject.Find("Container Grid").GetComponent<ContainerArrayParent>();
        containerArray = new ContainerArray(slotAmount, GameObject.Find("Container Grid"));
        foreach(Transform child in transform)
        {
            //if (child.GetComponent<Slot>().index > slotAmount)
                //Destroy(child);
        }
    }

    private void Update()
    {
        if(hover && Input.GetMouseButtonDown(1) || Vector2.Distance(gameObject.transform.position, GameObject.Find("Player").transform.position) > closeDistance)
        {
            if(open)
            {
                open = false;
                grid.containerArray = null;
                grid.containerSize = 0;
                grid.currentContainer = null;
                grid.UpdateSlots();
            }
            else if(Vector2.Distance(gameObject.transform.position, GameObject.Find("Player").transform.position) < closeDistance)
            {
                open = true;
                grid.containerArray = containerArray;
                grid.containerSize = slotAmount;
                grid.currentContainer = this;
                grid.UpdateSlots();
            }
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        hover = true;
    }
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        hover = false;
    }
}
