using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemRandomizer : MonoBehaviour
{
    [SerializeField] private List<ItemData> allCollectableItems = new List<ItemData>();
    [SerializeField] private List<Item> placedCollectables = new List<Item>();

    private void Start()
    {
        placedCollectables = FindObjectsOfType<Item>().ToList();
        foreach(Item item in placedCollectables) {
            item.itemData = allCollectableItems[Random.Range(0, allCollectableItems.Count)];
        }
    }
}
