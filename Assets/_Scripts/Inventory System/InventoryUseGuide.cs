using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUseGuide : MonoBehaviour
{
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        image.enabled = false;
    }

    public void ShowInventoryUseGuide()
    {
        image.enabled = true;
        Invoke("HideInventoryUseGuide", 3f);
    }

    public void HideInventoryUseGuide()
    {
        image.enabled = false;
    }
}
