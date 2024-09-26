using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public bool isOccupied;

    public InventoryItem inventoryItem;
    public Image itemImage;
    public int stackSize = 0;
    public TMP_Text stackSizeDisplay;

    public RectTransform fuelSlotUI;

    private Vector2 originalPosition;
    private FuelSlot fuelSlot;

    private bool isDragging;

    private void Start()
    {
        stackSizeDisplay.text = stackSize.ToString();
        originalPosition = transform.position;
        fuelSlotUI = GameObject.FindGameObjectWithTag("FuelSlot").GetComponent<RectTransform>();
        fuelSlot = FindObjectOfType<FuelSlot>();
    }

    public void AddItemToUI(InventoryItem item)
    {
        if (isOccupied) {
            stackSize = item.stackSize;
            stackSizeDisplay.text = stackSize.ToString();
        } else {
            inventoryItem = item;
            itemImage.GetComponent<Image>().sprite = inventoryItem.itemData.itemSprite;
            stackSize = inventoryItem.stackSize;
            stackSizeDisplay.text = stackSize.ToString();

            isOccupied = true;
        }
        
    }

    public void RemoveItemFromUI()
    {
        if (stackSize > 1) {
            stackSize--;
            stackSizeDisplay.text = stackSize.ToString();
        }else {
            InventoryUI.Instance.DeleteThisSlot(this);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isOccupied) {
            originalPosition = transform.position;
            transform.position = eventData.position;
            Invoke("DelayedFuelSlotUIOpen", 0.25f);
        }
    }

    private void DelayedFuelSlotUIOpen()
    {
        if (isDragging) fuelSlot.OpenFuelSlot();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (IsOverFuelSlot()) {
            fuelSlot.SetItemData(inventoryItem.itemData);
        }else {
            fuelSlot.CloseFuelSlot();
        }

        if (isOccupied) transform.position = originalPosition;
        isDragging = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Pointer Drag");
        isDragging = true;
        if (isOccupied) transform.position = eventData.position;
    }

    private FuelSlot GetFuelSlot()
    {
        FuelSlot fuelSlot = fuelSlotUI.GetComponent<FuelSlot>();
        if (fuelSlot != null) return fuelSlot;
        else return null;
    }

    private bool IsOverFuelSlot()
    {
        if (fuelSlotUI == null) Debug.Log("No fuel slot found");
        return RectTransformUtility.RectangleContainsScreenPoint(fuelSlotUI, Input.mousePosition);
    }
}