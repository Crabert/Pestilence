using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldInteractable")]
public class WorldInteractable : ScriptableObject
{
    public float health;

    public string _name;
    public string lootReqName;
    public Item.ToolType toolRequired;

    public Item drop;
    public int dropRange1, dropRange2;
}
