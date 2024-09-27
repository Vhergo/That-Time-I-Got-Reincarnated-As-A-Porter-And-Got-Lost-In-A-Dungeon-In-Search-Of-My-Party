using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDrop : MonoBehaviour
{
    [SerializeField] private List<ItemData> lootItems;

    public GameObject GetLoot()
    {
        return lootItems[Random.Range(0, lootItems.Count)].itemPrefab;
    }
}
