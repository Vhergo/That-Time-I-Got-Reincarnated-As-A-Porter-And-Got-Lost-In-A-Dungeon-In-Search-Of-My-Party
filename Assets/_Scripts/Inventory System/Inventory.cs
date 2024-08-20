using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    [SerializeField] private int inventorySize = 10;
    [SerializeField] private List<InventoryItem> inventory = new List<InventoryItem>();
    private Dictionary<ItemData, InventoryItem> itemDictionary = new Dictionary<ItemData, InventoryItem>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void OnEnable() => Item.OnItemCollected += AddItemToInventory;
    private void OnDisable() => Item.OnItemCollected -= AddItemToInventory;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            if (inventory.Count > 0) {
                RemoveFromInventory(inventory[0].itemData);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            if (inventory.Count > 1) {
                RemoveFromInventory(inventory[1].itemData);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            if (inventory.Count > 2) {
                RemoveFromInventory(inventory[2].itemData);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            if (inventory.Count > 3) {
                RemoveFromInventory(inventory[3].itemData);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha5)) {
            if (inventory.Count > 4) {
                RemoveFromInventory(inventory[4].itemData);
            }
        }
    }

    public void AddItemToInventory(ItemData itemData)
    {
        if (itemDictionary.TryGetValue(itemData, out InventoryItem item)) {
            item.AddToStack();
        } else {
            InventoryItem newItem = new InventoryItem(itemData);
            inventory.Add(newItem);
            itemDictionary.Add(itemData, newItem);
        }
    }

    public void RemoveFromInventory(ItemData itemData)
    {
        if (itemDictionary.TryGetValue(itemData, out InventoryItem item)) {
            if (item.stackSize > 1) {
                item.RemoveFromStack();
            }else {
                inventory.Remove(item);
            }
        }
    }

    public bool InventoryHasSpace(ItemData itemData)
    {
        if (itemDictionary.TryGetValue(itemData, out InventoryItem item)) {
            return inventory.Count < inventorySize;
        }
        return inventory.Count < inventorySize;
    }
}
