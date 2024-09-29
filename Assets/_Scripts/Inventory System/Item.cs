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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            Player.Instance.PlayGuide(GuideType.Interact, KeyCode.E);
        }
    }

    private void OnValidate()
    {
        if (itemData != null) {
            gameObject.name = itemData.itemName;
            GetComponent<SpriteRenderer>().sprite = itemData.itemSprite;
        }
    }
}
