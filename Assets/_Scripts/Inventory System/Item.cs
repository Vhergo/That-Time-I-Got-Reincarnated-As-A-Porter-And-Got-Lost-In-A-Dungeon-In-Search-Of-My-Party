using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Item : MonoBehaviour, ICollectable
{
    [SerializeField] private ItemData itemData;
    public ItemData ItemData => itemData;

    public static event Action<ItemData> OnItemCollected;

    public void Collect()
    {
        if (Inventory.Instance.InventoryHasSpace(itemData)) {
            OnItemCollected?.Invoke(itemData);
            Destroy(gameObject);
        }
    }

    private void OnValidate()
    {
        gameObject.name = itemData.itemName;
        GetComponent<SpriteRenderer>().sprite = itemData.itemSprite;
    }
}
