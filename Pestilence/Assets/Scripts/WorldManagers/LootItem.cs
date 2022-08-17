using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LootItem : MonoBehaviour, IPointerDownHandler
{
    public string heldLootName;
    public Item loot;
    public LootManager lootManager;
    public int heldAmount;
    public float autoLootDistance;
    public ContainerArray playerInven;
    SpriteRenderer sr;
    [HideInInspector] public bool equal;
    //add more transfer stats of this item

    private void Start()
    {
        playerInven = GameObject.Find("Player").GetComponent<Player>().inventory;
        lootManager = GameObject.Find("EventSystem").GetComponent<LootManager>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void CallForLoot(Item _loot)   //this is on scene start we are going to tell the loot manager to give us what we have saved
    {
        loot = _loot;
        sr.sprite = loot.inventoryIcon;
        if(heldAmount == 0)
            heldAmount = loot.itemAmount;
        
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        if (!Full() && playerInven.AddItem(loot, 0, "auto", 0, null, null, heldAmount, playerInven))
        {
            lootManager.RemoveLoot(gameObject);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        sr.sprite = loot.inventoryIcon;
        if (Vector2.Distance(GameObject.Find("Player").transform.position, gameObject.transform.position) < autoLootDistance && Input.GetKey(KeyCode.E) && !Full())
        {
            if (playerInven.AddItem(loot, 0, "auto", 0, null, null, heldAmount, playerInven))
            {
                lootManager.RemoveLoot(gameObject);
                Destroy(gameObject);
            }
        }
    }

    bool Full()
    {
        int i = 0;
        foreach (Item item in playerInven.container)
        {
            if (item == null && i != 25)
                return false;
            
            if (i == 25)
                return true;

            if (item.canStack && item.itemName == loot.itemName)
                return false;
            i++;
        }
        return true;
    }
    bool Equal(LootItem b)
    {
        if (b.equal)
            equal = false;
        else
            equal = true;
        return equal;
    }
}
