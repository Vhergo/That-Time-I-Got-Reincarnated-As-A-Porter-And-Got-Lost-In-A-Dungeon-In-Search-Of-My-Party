using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obelisk : MonoBehaviour
{
    [SerializeField] private ObeliskData obeliskData;
    private SpriteRenderer spriteRenderer;
    private bool inRange;
    private bool canInteract = true;

    public static Action<ObeliskData, Obelisk> OnObeliskInteracted;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && inRange && canInteract) OnObeliskInteracted?.Invoke(obeliskData, this);
    }

    public void CanInteract(bool isInteracting) => canInteract = isInteracting;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            Player.Instance.PlayGuide(GuideType.Interact, KeyCode.E);
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            Player.Instance.StopActiveGuide();
            inRange = false;
        }
    }
}
