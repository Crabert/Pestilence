using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableManager : MonoBehaviour
{
    public WorldInteractable interactable;
    LootManager lootManager;

    float heldHealth;

    private void Start()
    {
        heldHealth = interactable.health;
        lootManager = GameObject.Find("EventSystem").GetComponent<LootManager>();
        interactable.drop = (Item)lootManager.GetType().GetField(interactable.lootReqName).GetValue(lootManager);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Damage" && collision.transform.parent.parent.parent.GetComponent<Player>().currentWeapon.heldToolType == interactable.toolRequired)
        {
            heldHealth -= collision.transform.parent.parent.parent.GetComponent<Player>().currentWeapon.damage;
        }
    }
    private void Update()
    {
        if(heldHealth <= 0)
        {
            lootManager.GenerateNewLoot(interactable.drop, Random.Range(interactable.dropRange1, interactable.dropRange2), gameObject.transform.position);
            Destroy(gameObject);
        }    
    }
}
