using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance { get; private set; }

    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject inventorySlotPrefab;
    public List<InventorySlot> inventorySlots = new List<InventorySlot>();
    private int inventorySize;

    [SerializeField] private KeyCode inventoryKey = KeyCode.Tab;
    [SerializeField] private Animator inventoryAnim;
    [SerializeField] private InventoryUseGuide inventoryUseGuide;
    private bool inventoryOpen = false;
    private bool firstOpen = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        InitializeInventoryUI();
        CursorManager.Instance.ToggleCursor(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(inventoryKey)) {
            if (!inventoryOpen) OpenInventory();
            else CloseInventory();

            inventoryOpen = !inventoryOpen;

            if (!firstOpen) {
                inventoryUseGuide.ShowInventoryUseGuide();
                firstOpen = true;
            }
        }
    }

    public void OpenInventory()
    {
        CursorManager.Instance.ToggleCursor(true);
        inventoryAnim.SetTrigger("Open");
    }
    public void CloseInventory()
    {
        CursorManager.Instance.ToggleCursor(false);
        inventoryAnim.SetTrigger("Close");
    }

    private void InitializeInventoryUI()
    {
        foreach (Transform child in inventoryPanel.transform) {
            Destroy(child.gameObject);
        }

        inventoryPanel.SetActive(true);
        inventorySize = Inventory.Instance.GetInventorySize();

        inventorySlots.Clear();
        for (int i = 0; i < inventorySize; i++) {
            GameObject newSlot = Instantiate(inventorySlotPrefab, inventoryPanel.transform);
            InventorySlot slot = newSlot.GetComponent<InventorySlot>();

            inventorySlots.Add(slot);
        }
    }

    public void AddItemToUI(InventoryItem item, int index)
    {
        Debug.Log("Index: " + index);
        InventorySlot slot = inventorySlots[index];
        slot.AddItemToUI(item);
    }

    public void RemoveItemFromUI(int index)
    {
        Debug.Log("Removed");
        InventorySlot slot = inventorySlots[index];
        slot.RemoveItemFromUI();
    }

    public void DeleteThisSlot(InventorySlot slotToRemove)
    {
        inventorySlots.Remove(slotToRemove);
        Destroy(slotToRemove.gameObject);

        GameObject newSlot = Instantiate(inventorySlotPrefab, inventoryPanel.transform);
        InventorySlot slot = newSlot.GetComponent<InventorySlot>();

        inventorySlots.Add(slot);
    }
}
