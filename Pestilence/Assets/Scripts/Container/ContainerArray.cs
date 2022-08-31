using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContainerArray
{
    public Item[] container;  //temporary player will be the 0 index of global containers
    List<GameObject> slots = new List<GameObject>();
    LootManager lootManager;
    public string currentFunction;  //the item handling currently running

    [HideInInspector]public int indexSpecialPass = -1;    //this is a variable i dont want to put in the main add item func because it is specifically for not putting a specific item take back into itself

    public ContainerArray(int size, GameObject originObject)
    {
        lootManager = GameObject.Find("EventSystem").GetComponent<LootManager>();
        container = new Item[size];
        foreach(Transform child in originObject.transform)
        {
            if(child.name == "Equipment Slots")
            {
                foreach(Transform _child in child)
                {
                    slots.Add(_child.gameObject);
                }
            }
            else
            {
                slots.Add(child.gameObject);
            }
        }
    }

    public void Test()
    {
        AddItem(lootManager.pickaxe, 0, "auto", 0, null, null,1, this);
        AddItem(lootManager.axe, 0, "auto", 0, null, null, 1, this);
        AddItem(lootManager.wood, 0, "auto", 0, null, null, 1, this);
    }
                                                            //index request originated from             amount for if we have no slots
    public bool AddItem(Item item, int index, string means, int index_o, Slot slot1, Slot slot2, int amount, ContainerArray originArray) //means is if we are adding it from like the left most slot automatically or if we are manually inserting it
    {
        currentFunction = "Adding Item";
        if(means == "auto")
        {
            //checking for stacking
            if(item.canStack)
            {
                int g = 0;
                foreach (Item _item in container)
                {
                    if(_item != null && g != indexSpecialPass)
                    {
                        if (_item.itemName == item.itemName && _item.canStack)
                        {
                            if(index_o != 0)
                            {
                                if (StackItem(index, index_o, originArray))  //shift clicking etc
                                {
                                    indexSpecialPass = -1;
                                    return true;
                                }
                            }
                            else
                            {
                                if (indexSpecialPass != -1 && container[indexSpecialPass] == item && g > indexSpecialPass)    //pass bypass for auto item pickup from ground
                                    g = indexSpecialPass;

                                Slot s = slots[g].GetComponent<Slot>();
                                if(s.heldItem != null && s.heldAmount != s.heldItem.maxStack && slots[g].tag != "Equip Slot")    //slot 25 thing here is an issue
                                {
                                    s.heldAmount += amount;
                                    if (s.heldAmount > s.heldItem.maxStack)
                                    {
                                        int b = s.heldAmount - s.heldItem.maxStack;
                                        s.heldAmount = s.heldItem.maxStack;
                                        return AddItem(item, 0, "auto", 0, null, null, amount, this);
                                    }
                                    else
                                    {
                                        indexSpecialPass = -1;
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                    g++;
                }
            }
            //add to new slot
            int i = 0;
            foreach (Item _item in container)
            {
                if (_item == null && slots[i].tag != "Equip Slot")
                {
                    container[i] = item;
                    if (slot2 != null && slot1 != null && index == 0 && index_o == 0 && originArray == this)    //virtually impossible to occur such way
                    {
                        slots[i].GetComponent<Slot>().updateSlot = true;
                    }
                    else if(slot2 == null)
                    {
                        slots[i].GetComponent<Slot>().heldAmount = amount;
                    }
                    else
                    {
                        slot1.heldAmount = slot2.heldAmount;
                    }
                    return true;
                }
                i++;
            }
            return false;
        }   //manual insertion if auto isnt used
        else if(means == "manual" && slot1 != slot2)  //if the two indexes are the same nothing is required
        {
            //basic adding to slot
            if (container[index] == null)
            {
                container[index] = item;
                originArray.container[index_o] = null;
                slot2.heldAmount = slot1.heldAmount;
                return true;
            }
            else
            {
                if (slot1.tag == "Equip Slot" || slot2.tag == "Equip Slot")
                    return false;
                //checking if the filled slot can be stacked into
                if (container[index].canStack && originArray.container[index_o].canStack && container[index].itemName == item.itemName)
                {
                    if (StackItem(index, index_o, originArray))
                    {
                        return true;
                    }
                    else
                    {
                        return SwapItems(index, index_o, slot1, slot2, originArray);
                    }
                }
                else
                {
                    //checking if the slot we are originating from can be swapped with
                    if (originArray.container[index_o] == null)
                    {
                        return false;
                    }
                    else
                    {
                        return SwapItems(index, index_o, slot1, slot2, originArray);
                    }
                }
            }
        }
        else if(means == "equip")
        {
            //basic adding to slot
            if (index == index_o)
            {
                container[index] = null;
                return AddItem(item, 0, "auto", 0, null, null, amount, originArray);
            }
                
            if (container[index] == null)
            {
                if(originArray.container[index_o].heldItemType == Item.ItemType.Melee_Weapon || originArray.container[index_o].heldItemType == Item.ItemType.Ranged_Weapon || 
                    originArray.container[index_o].heldItemType == Item.ItemType.Tool)
                {
                    container[index] = item;
                    originArray.container[index_o] = null;

                    if(slot1.index == 25)
                        slot1.heldAmount = slot2.heldAmount;
                    else
                        slot2.heldAmount = slot1.heldAmount;

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (originArray.container[index_o].heldItemType == Item.ItemType.Melee_Weapon || originArray.container[index_o].heldItemType == Item.ItemType.Ranged_Weapon || 
                    originArray.container[index_o].heldItemType == Item.ItemType.Tool)
                {
                    return SwapItems(index, index_o, slot1, slot2, originArray);
                }
                else
                {
                    return false;
                }
            }
        }
        else if(means == "shift")
        {
            //check for stackability
            if (item.canStack)
            {
                int g = 0;
                foreach (Item _item in container)
                {
                    if (_item != null)
                    {
                        if (_item.itemName == item.itemName && _item.canStack)
                        {
                            if (slot2 != null)
                            {
                                return StackItem(g, index_o, originArray);
                            }
                        }
                    }
                    g++;
                }
            }
            //add to new slot
            int i = 0;
            foreach (Item _item in container)
            {
                if (_item == null && slots[i].tag != "Equip Slot")
                {
                    container[i] = item;
                    slots[i].GetComponent<Slot>().heldAmount = slot2.heldAmount;
                    return true;
                }
                i++;
            }
            return false;
        }
        return false;   //essentially an error at this point
    }

    public bool DropItem(int index)
    {
        currentFunction = "Dropping Item";
        lootManager.GenerateNewLoot(container[index], slots[index].GetComponent<Slot>().heldAmount, GameObject.Find("Player").transform.position);
        container[index] = null;
        slots[index].GetComponent<Slot>().heldAmount = 0;
        return true;
    }
    public bool RemoveItem(int index) //deleting item
    {
        currentFunction = "Removing Item";
        container[index] = null;
        return true;
    }

    public bool SwapItems(int index, int index_o, Slot slot1, Slot slot2, ContainerArray originArray)
    {
        currentFunction = "Swapping Items";

        Item item1 = container[index];
        Item item2 = originArray.container[index_o];

        int passAmount1 = slot1.heldAmount;
        int passAmount2 = slot2.heldAmount; 

        container[index] = item2;
        originArray.container[index_o] = item1;
        slot1.heldAmount = passAmount2;
        slot2.heldAmount = passAmount1;

        return true;
    }

    public bool StackItem(int index, int index_o, ContainerArray origin)
    {
        Slot slot1 = new Slot();
        Slot slot2 = new Slot();
        if(origin == this)
        {
            slot1 = slots[index].GetComponent<Slot>();
            slot2 = slots[index_o].GetComponent<Slot>();
        }
        else
        {
            slot1 = slots[index].GetComponent<Slot>();
            slot2 = origin.slots[index_o].GetComponent<Slot>();
        }

        currentFunction = "Stacking Items";
        if (slot1.heldAmount < container[index].maxStack)    //checking if stack is full
        {
            slot1.heldAmount += slot2.heldAmount;
            slot2.heldAmount = 0;
            if (slot1.heldAmount > container[index].maxStack)//checking if stack is overfilled
            {
                slot2.heldAmount = slot1.heldAmount - container[index].maxStack;
                slot1.heldAmount = container[index].maxStack;
                return true;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }
    } 

    public void ToggleInventory()
    {
        foreach(GameObject slot in slots)
        {
            slot.GetComponent<Image>().enabled = !slot.GetComponent<Image>().enabled;
            slot.GetComponent<Slot>().childItem.SetActive(!slot.GetComponent<Slot>().childItem.activeInHierarchy);
        }
    }
}
