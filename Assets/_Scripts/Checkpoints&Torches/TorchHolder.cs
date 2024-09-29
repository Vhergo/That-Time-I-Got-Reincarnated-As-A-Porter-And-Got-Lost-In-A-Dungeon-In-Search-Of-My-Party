using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchHolder : MonoBehaviour
{
    private Animator torchAnim;
    private TorchLight torchLight;
    private bool interacted;

    private void Start()
    {
        torchAnim = GetComponent<Animator>();
        torchLight = GetComponentInChildren<TorchLight>();
    }

    public void PlaceTorch()
    {
        torchAnim.SetTrigger("On");
        torchLight.EnableLight();
    }

    public void RemoveTorch()
    {
        torchAnim.SetTrigger("Off");
        torchLight.DisableLight();
    }

    public void UpdateTorchLightRadius(float torchFuelPercentage)
    {
        torchLight.UpdateTorchLightRadius(torchFuelPercentage);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            if (!interacted) {
                Player.Instance.PlayGuide(GuideType.Interact, KeyCode.E);
                interacted = true;
            }
        }
    }
}
