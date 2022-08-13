using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public enum ItemType
    {
        Melee_Weapon,
        Ranged_Weapon,
        Consumable,
        Tool,
        Resource,
    }
    public ItemType heldItemType;

    public enum ToolType
    {
        Not_Tool,
        Axe,
        Pickaxe,
    }
    public ToolType heldToolType;
    
    public string itemName;
    public int itemAmount;
    public bool canStack;
    public int maxStack;
    public float damage;
    public GameObject slot; //this is the ui object relative to the item indexed in our array
    public Sprite inventoryIcon;

    public Item(string newName, ItemType itemType, ToolType toolType, bool _canStack, int _maxStack, int passNum, Sprite _inventoryIcon, float _damage)
    {
        heldItemType = itemType;
        heldToolType = toolType;
        itemName = newName;
        canStack = _canStack;
        maxStack = _maxStack;
        itemAmount = passNum;
        inventoryIcon = _inventoryIcon;
        damage = _damage;
    }
}
