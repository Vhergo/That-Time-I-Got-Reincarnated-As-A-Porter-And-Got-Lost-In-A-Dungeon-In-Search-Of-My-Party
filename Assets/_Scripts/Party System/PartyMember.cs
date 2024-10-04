using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMember : MonoBehaviour
{
    private bool canBeSaved;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canBeSaved) {
            SavePartyMember();
        }
    }

    private void SavePartyMember()
    {
        canBeSaved = false;
        transform.position = PartyManager.Instance.AddPartyMember(this).position;
        GetComponent<Collider2D>().enabled = false;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            canBeSaved = true;
            Player.Instance.PlayGuide(GuideType.Interact, KeyCode.E);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            canBeSaved = false;
            Player.Instance.StopActiveGuide();
        }
    }
}
