using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelSlot : MonoBehaviour
{
    [SerializeField] private Button fuelButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Animator fuelSlotAnimator;
    private bool fuelSlotOpen;

    public ItemData itemData;
    public Image itemSprite;
    private TorchManager torch;

    private void Start()
    {
        torch = TorchManager.Instance;

        if (fuelSlotAnimator == null) fuelSlotAnimator = GetComponent<Animator>();

        fuelButton.onClick.AddListener(UseItemInSlot);
        cancelButton.onClick.AddListener(CancelItemUse);

        cancelButton.gameObject.SetActive(false);
    }

    public void SetItemData(ItemData itemData)
    {
        this.itemData = itemData;
        itemSprite.sprite = itemData.itemSprite;

        cancelButton.gameObject.SetActive(true);
    }

    public void RemoveItemData()
    {
        Inventory.Instance.RemoveFromInventory(itemData);
        itemData = null;
        itemSprite.sprite = null;
    }

    public void UseItemInSlot()
    {
        if (itemData != null) {
            torch.AddTorchFuel(itemData);
            RemoveItemData();
            cancelButton.gameObject.SetActive(false);
            Invoke("CloseFuelSlot", 0.25f); // Delay in case there are any visual to see when used
        } else {
            Debug.Log("No item in slot");
        }
    }

    public void CancelItemUse()
    {
        if (itemData != null) {
            itemData = null;
            itemSprite.sprite = null;
            cancelButton.gameObject.SetActive(false);
            CloseFuelSlot();
        }
    }

    public void OpenFuelSlot()
    {
        if (!fuelSlotOpen) {
            fuelSlotAnimator.SetTrigger("Open");
            fuelSlotOpen = true;
        } 
    }

    public void CloseFuelSlot()
    {
        if (fuelSlotOpen) {
            fuelSlotAnimator.SetTrigger("Close");
            fuelSlotOpen = false;
        }
    }
}
