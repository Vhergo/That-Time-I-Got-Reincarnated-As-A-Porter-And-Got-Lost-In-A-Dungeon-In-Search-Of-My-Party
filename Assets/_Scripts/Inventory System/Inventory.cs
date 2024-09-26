using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    [SerializeField] private int inventorySize = 6;
    [SerializeField] private List<InventoryItem> inventory = new List<InventoryItem>();
    private Dictionary<ItemData, InventoryItem> itemDictionary = new Dictionary<ItemData, InventoryItem>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void OnEnable()
    {
        Item.OnItemCollected += AddItemToInventory;
    }
    
    private void OnDisable()
    {
        Item.OnItemCollected -= AddItemToInventory;
    }

    public void AddItemToInventory(ItemData itemData)
    {
        if (itemDictionary.TryGetValue(itemData, out InventoryItem item)) {
            item.AddToStack();
            InventoryUI.Instance.AddItemToUI(item, inventory.IndexOf(item));
        } else {
            InventoryItem newItem = new InventoryItem(itemData);
            inventory.Add(newItem);
            InventoryUI.Instance.AddItemToUI(newItem, inventory.IndexOf(newItem));
            itemDictionary.Add(itemData, newItem);
            Debug.Log("ItemData: " + itemData.itemName + " added to inventory");
        }
    }

    public void RemoveFromInventory(ItemData itemData)
    {
        int itemIndex;
        if (itemDictionary.TryGetValue(itemData, out InventoryItem item)) {
            itemIndex = inventory.IndexOf(item);
            if (item.stackSize > 1) {
                item.RemoveFromStack();
            } else {
                Debug.Log("Removing");
                inventory.Remove(item);
                itemDictionary.Remove(itemData);
            }

            InventoryUI.Instance.RemoveItemFromUI(itemIndex);
        }
    }

    public bool InventoryHasSpace(ItemData itemData)
    {
        if (itemDictionary.TryGetValue(itemData, out InventoryItem item)) {
            return inventory.Count < inventorySize;
        }
        return inventory.Count < inventorySize;
    }

    public int GetInventorySize() => inventorySize;
}
