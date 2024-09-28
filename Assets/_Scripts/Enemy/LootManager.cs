using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LootManager : MonoBehaviour
{
    public static LootManager Instance { get; private set; }

    [SerializeField] private List<ItemData> allLoot;
    [SerializeField] private List<LootPool> lootPools;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public GameObject GetLoot(Archetype requestedArchetype)
    {
        LootPool requestedPool = lootPools.FirstOrDefault(pool => pool.monsterArchetype == requestedArchetype);
        if (requestedPool != null && requestedPool.lootItems.Count > 0) {
            List<ItemData> lootPool = requestedPool.lootItems;
            return lootPool[UnityEngine.Random.Range(0, lootPool.Count)].itemPrefab;
        }
        return allLoot[UnityEngine.Random.Range(0, allLoot.Count)].itemPrefab;
    }
}

[Serializable]
public class LootPool
{
    public Archetype monsterArchetype;
    public List<ItemData> lootItems;
}
