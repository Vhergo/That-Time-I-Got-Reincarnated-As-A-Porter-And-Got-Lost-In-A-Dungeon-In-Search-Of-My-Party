using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMember : MonoBehaviour
{
    private GameObject cage;
    private bool canBeSaved;

    private void Start() => cage = transform.GetChild(0).gameObject;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canBeSaved) {
            StartCoroutine(SavePartyMember());
        }
    }

    private IEnumerator SavePartyMember()
    {
        canBeSaved = false;
        cage.SetActive(false);
        yield return new WaitForSeconds(1f);

        transform.position = PartyManager.Instance.AddPartyMember(this).position;
        GetComponent<Collider2D>().enabled = false;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            canBeSaved = true;
            Player.Instance.PlayGuide(GuideType.Interact, KeyCode.E, true);
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
