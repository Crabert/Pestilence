using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    public float stackingDistance;
    
    public List<GameObject> allItems = new List<GameObject>();  //list of every already instantiated loot item on the ground
    public GameObject lootPrefab;

    public Sprite pickaxeSprite;
    public Sprite axeSprite;
    public Sprite woodSprite;

    public Item pickaxe;
    public Item axe;
    public Item wood;

    private void Start()    //change the pass to 0 after they are refined 
    {
        pickaxe = new Item("pickaxe", Item.ItemType.Tool, Item.ToolType.Pickaxe, false, 1, 1, pickaxeSprite, 5);
        axe = new Item("axe", Item.ItemType.Tool, Item.ToolType.Axe, false, 1, 1, axeSprite, 5);
        wood = new Item("wood", Item.ItemType.Resource, Item.ToolType.Not_Tool, true, 10, 1, woodSprite, 0);


        foreach (GameObject item in allItems)
        {
            LootItem s = item.GetComponent<LootItem>();
            s.CallForLoot((Item)this.GetType().GetField(s.heldLootName).GetValue(this));
        }
    }

    public void GenerateNewLoot(Item item, int amount, Vector3 position)
    {
        GameObject s = Instantiate(lootPrefab, position, Quaternion.identity);
        allItems.Add(s);
        s.GetComponent<LootItem>().loot = item;
        s.GetComponent<LootItem>().heldAmount = amount;
        UpdateGroundStacks();
    }
    public void RemoveLoot(GameObject loot)
    {
        allItems.Remove(loot);
        UpdateGroundStacks();
    }
    public void UpdateGroundStacks()
    {
        GameObject previous = null;
        int i = 0;
        foreach(GameObject loot in allItems)
        {
            if (previous == null)
            {
                previous = loot;
                i++;
                continue;
            }
                
            if(Vector2.Distance(previous.transform.position, loot.transform.position) < stackingDistance)
            {
                LootItem lootItem = loot.GetComponent<LootItem>();
                LootItem previousItem = previous.GetComponent<LootItem>();

                if (lootItem.loot.canStack && previousItem.loot.canStack && lootItem.loot.itemName == previousItem.loot.itemName)
                {
                    LootItem smaller;
                    LootItem bigger;

                    if (lootItem.heldAmount == previousItem.heldAmount)
                    {
                        smaller = lootItem;
                        bigger = previousItem;
                    }

                    bigger = lootItem.heldAmount > previousItem.heldAmount ? lootItem : previousItem;
                    smaller = lootItem.heldAmount > previousItem.heldAmount ? previousItem : lootItem;

                    bigger.heldAmount += smaller.heldAmount;
                    smaller.heldAmount = 0;
                    if (bigger.heldAmount > bigger.loot.maxStack)
                    {
                        smaller.heldAmount = bigger.heldAmount - bigger.loot.maxStack;
                        bigger.heldAmount = bigger.loot.maxStack;
                    }
                    if(smaller.heldAmount == 0)
                    {
                        RemoveLoot(smaller.gameObject);
                        Destroy(smaller.gameObject);
                        break;
                    }
                }
            }
            i++;
            previous = loot;
        }
    }
    
}
