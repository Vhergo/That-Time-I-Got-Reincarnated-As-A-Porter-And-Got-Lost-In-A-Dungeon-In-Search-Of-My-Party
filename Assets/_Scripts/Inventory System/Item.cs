using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(CircleCollider2D))]
public class Item : MonoBehaviour, ICollectable
{
    [SerializeField] public ItemData itemData;
    private CircleCollider2D lootCollider;

    public ItemData ItemData => itemData;

    public static event Action<ItemData> OnItemCollected;

    private void Start() => lootCollider = GetComponent<CircleCollider2D>();

    public void Collect()
    {
        if (Inventory.Instance.InventoryHasSpace(itemData)) {
            OnItemCollected?.Invoke(itemData);
            Destroy(gameObject);
        }
    }

    private bool IsInCollectRange(Transform player)
    {
        if (player == null) return false;

        float offset = lootCollider.radius + (player.GetComponent<Collider2D>().bounds.extents.x);
        Debug.Log("Offset: " + offset);
        Debug.Log("Distance: " + Vector2.Distance(transform.position, player.position));
        if (Vector2.Distance(transform.position, player.position) <= offset) return true;
        else return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            if (IsInCollectRange(collision.transform))
                Player.Instance.PlayGuide(GuideType.Interact, KeyCode.E);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            Player.Instance.StopActiveGuide();
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
