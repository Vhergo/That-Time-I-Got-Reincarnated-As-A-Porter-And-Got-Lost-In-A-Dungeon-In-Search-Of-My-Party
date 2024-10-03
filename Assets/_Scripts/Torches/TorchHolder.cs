using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TorchHolder : MonoBehaviour
{
    private Animator torchAnim;
    private TorchLight torchLight;
    private Light2D areaLight;
    private bool interacted;

    private void Start()
    {
        torchAnim = GetComponent<Animator>();
        torchLight = GetComponentInChildren<TorchLight>();

        areaLight = transform.GetChild(0).GetComponent<Light2D>();
        areaLight.enabled = false;
    }

    public void PlaceTorch()
    {
        torchAnim.SetTrigger("On");
        torchLight.EnableLight();
        areaLight.enabled = true;
    }

    public void RemoveTorch()
    {
        torchAnim.SetTrigger("Off");
        torchLight.DisableLight();
        areaLight.enabled = false;
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
