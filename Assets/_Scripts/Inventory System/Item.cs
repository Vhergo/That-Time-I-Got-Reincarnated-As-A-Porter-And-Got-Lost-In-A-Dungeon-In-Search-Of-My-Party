using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Item : MonoBehaviour, ICollectable
{
    [SerializeField] private ItemData itemData;
    [SerializeField] private bool showCollectionGuide;
    private SpriteRenderer indicator;

    public ItemData ItemData => itemData;

    public static event Action<ItemData> OnItemCollected;

    private void Start()
    {
        if (indicator == null) indicator = transform.GetChild(0).GetComponent<SpriteRenderer>();

        if (!showCollectionGuide) Destroy(indicator.gameObject);
        else indicator.enabled = false;
    }

    public void Collect()
    {
        if (Inventory.Instance.InventoryHasSpace(itemData)) {
            OnItemCollected?.Invoke(itemData);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && indicator) {
            indicator.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && indicator) {
            indicator.enabled = false;
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
