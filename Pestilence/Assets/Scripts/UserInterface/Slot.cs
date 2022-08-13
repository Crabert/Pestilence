using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler 
{
    public Item heldItem;
    public int heldAmount;
    [HideInInspector]public bool updateSlot = false;

    public ContainerArray originArray;  //the container this slot is in
    public Image Highlight;
    public GameObject childItem;    //the gameobject holding the item sprite
    public GameObject childItemPrefab;
    TextMeshProUGUI amountText;

    public int index;
    bool isAttatched;
    bool shift;

    void Start()    
    {
        
        //stupid dumb retarded stupid code dumb bad doesnt work why 
        childItem = Instantiate(childItemPrefab);
        childItem.transform.parent = GameObject.Find("Canvas").transform;
        childItem.transform.parent = gameObject.transform;
        childItem.transform.position = gameObject.GetComponent<RectTransform>().position;
        amountText = childItem.GetComponent<TextHolder>().hold.GetComponent<TextMeshProUGUI>();
        string[] h = gameObject.name.Split("(");    //monkaS
        index = int.Parse(h[1].Substring(0, h[1].Length - 1));
    }

    void Update()
    {
        if (transform.parent.name == "Player Grid" || transform.parent.name == "Equipment Slots")
            originArray = GameObject.Find("Player").GetComponent<Player>().inventory;
        else
            originArray = transform.parent.GetComponent<ContainerArrayParent>().containerArray;

        if (Input.GetKeyUp(KeyCode.LeftShift))
            shift = false;
        if (Input.GetKeyDown(KeyCode.LeftShift))
            shift = true;

        if (Highlight.enabled == true && heldItem != null)
        {
            if(Input.GetMouseButtonDown(1) && heldAmount > 1)
            {
                originArray.indexSpecialPass = index;
                if (originArray.AddItem(heldItem, 0, "auto", 0, null, null, 1, originArray))
                    heldAmount--;
            }
            if (Input.GetMouseButton(0) && shift)
            {
                if(transform.parent.name == "Container Grid")
                {
                    if (GameObject.Find("Player").GetComponent<Player>().inventory.AddItem(heldItem, 0, "shift", index, null, this, heldAmount, originArray))
                        originArray.container[index] = null;
                }
                else if(transform.parent.name == "Player Grid" || transform.parent.name == "Equipment Slots")
                {
                    if (GameObject.Find("Container Grid").GetComponent<ContainerArrayParent>().containerArray != null)
                    {
                        if (!GameObject.Find("Container Grid").GetComponent<ContainerArrayParent>().containerArray.AddItem(heldItem, 0, "shift", index, null, this, heldAmount, originArray))
                        {
                            if (heldItem.heldItemType == Item.ItemType.Melee_Weapon || heldItem.heldItemType == Item.ItemType.Ranged_Weapon || heldItem.heldItemType == Item.ItemType.Tool)
                                originArray.AddItem(heldItem, GameObject.Find("Equipment Slots").GetComponentInChildren<Slot>().index, "equip", index, GameObject.Find("Equipment Slots").GetComponentInChildren<Slot>(),
                                    this, heldAmount, originArray);
                        }
                        else
                        {
                            originArray.container[index] = null;
                        }
                    }
                    else
                    {
                        if (heldItem.heldItemType == Item.ItemType.Melee_Weapon || heldItem.heldItemType == Item.ItemType.Ranged_Weapon || heldItem.heldItemType == Item.ItemType.Tool)
                            originArray.AddItem(heldItem, GameObject.Find("Equipment Slots").GetComponentInChildren<Slot>().index, "equip", index, GameObject.Find("Equipment Slots").GetComponentInChildren<Slot>(),
                                this, heldAmount, originArray);
                    }
                }
                shift = false;
            }
        }

        if (originArray.container[index] == null)
            heldAmount = 0;

        if(heldItem != null && heldAmount != 1) //updating item amount text
        {
            amountText.text = heldAmount.ToString();
            amountText.enabled = true;
        }
        else
            amountText.enabled = false;

        if (heldItem != null)   //updating image for slot
        {
            childItem.GetComponent<Image>().enabled = true;
            childItem.GetComponent<Image>().sprite = heldItem.inventoryIcon;
        }
        else
        {
            childItem.GetComponent<Image>().enabled = false;
        }

        heldItem = originArray.container[index];    //updating slots held item
        
        if (updateSlot && heldItem != null)
        {
            updateSlot = false;
            UpdateSlotStats();
        }

        if (isAttatched) //moving item to mouse
        {
            childItem.transform.position = Input.mousePosition;
        }

        if(Highlight.enabled && Input.GetKeyDown(KeyCode.Delete) && heldItem != null)
        {
            originArray.DropItem(index);
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        Highlight.enabled = true;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        Highlight.enabled = false;
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        if (heldItem != null && Input.GetMouseButton(0))
        {
            childItem.transform.position = Input.mousePosition;
            isAttatched = true;
        }
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        GameObject slot = pointerEventData.pointerCurrentRaycast.gameObject;
        if (slot != null && isAttatched && shift == false)
        {
            if (slot.GetComponent<Slot>().originArray.AddItem(heldItem, slot.GetComponent<Slot>().index,
                slot.tag == "Equip Slot" && transform.parent.name == "Player Grid"  ? "equip" : "manual", index, this,
                slot.GetComponent<Slot>(), heldAmount, originArray) && isAttatched) //cries in C#
            {
                if (slot.GetComponent<Slot>().originArray.currentFunction == "Stacking Items")
                    if (heldAmount <= 0)
                        slot.GetComponent<Slot>().originArray.container[index] = null;
                isAttatched = false;
                childItem.transform.position = gameObject.GetComponent<RectTransform>().position;
            }
            else
            {
                isAttatched = false;
                childItem.transform.position = gameObject.GetComponent<RectTransform>().position;
            }
        }
        else if(isAttatched)
        {
            if (heldItem != null)
            {
                originArray.DropItem(index);
                isAttatched = false;
                childItem.transform.position = gameObject.GetComponent<RectTransform>().position;
            }
            else
            {
                isAttatched = false;
                childItem.transform.position = gameObject.GetComponent<RectTransform>().position;
            }
        }
    }

    void UpdateSlotStats()
    {
        if (heldItem == null)
        {
            heldAmount = 0;
        }
        else
        {
            heldAmount = originArray.container[index].itemAmount;
        }
    }
}
