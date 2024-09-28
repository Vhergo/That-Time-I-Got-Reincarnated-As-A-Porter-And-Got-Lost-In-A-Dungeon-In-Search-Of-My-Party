using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ItemGrabber : MonoBehaviour
{
    [SerializeField] private KeyCode grabKey;
    private Item lootWithinRange;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(grabKey)) {
            GrabItem();
        }
    }

    private void GrabItem()
    {
        if (lootWithinRange != null) {
            lootWithinRange.Collect();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item")) {
            if (collision.TryGetComponent(out Item item)) {
                lootWithinRange = item;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Item")) {
            lootWithinRange = null;
        }
    }

    //private void GrabItemWithMouse()
    //{
    //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

    //    // Casts the ray into the scene based on mouse position
    //    RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

    //    // Raycast can only detect object with colliders
    //    if (hit.collider != null) {
    //        //Debug.Log("Found an item!");
    //        //Debug.Log("Item name: " + hit.collider.name);

    //        Item item = hit.collider.GetComponent<Item>();
    //        if (item != null) {
    //            if (Inventory.Instance.InventoryHasSpace(item.ItemData)) {
    //                item.Collect();
    //            } else {
    //                Debug.Log("Inventory is full!");
    //            }
    //        } else {
    //            //Debug.Log("Hit object is not an Item");
    //        }
    //    } else {
    //        //Debug.Log("No item found!");
    //    }
    //}
}
