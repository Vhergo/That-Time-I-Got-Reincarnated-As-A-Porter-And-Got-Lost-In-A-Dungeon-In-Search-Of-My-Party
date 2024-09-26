using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public int itemID;
    public string itemName;
    public string itemDescription;
    public float itemFuelValue;
    public Sprite itemSprite;
    public GameObject itemPrefab;
}
