using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TorchHolder : MonoBehaviour
{
    [SerializeField] private bool respawnPoint;

    private Animator torchAnim;
    private TorchLight torchLight;
    private Light2D areaLight;
    private bool interacted;

    private FearManager fearManager;

    private void Start()
    {
        torchAnim = GetComponent<Animator>();
        torchLight = GetComponentInChildren<TorchLight>();

        areaLight = transform.GetChild(0).GetComponent<Light2D>();
        areaLight.enabled = false;

        fearManager = FearManager.Instance;
    }

    public void PlaceTorch()
    {
        torchAnim.SetTrigger("On");
        torchLight.EnableLight();
        areaLight.enabled = true;

        if (respawnPoint) fearManager.SetCheckPointPos(transform);
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

    private bool PlayerIsInRange()
    {
        return Vector2.Distance(transform.position, Player.Instance.transform.position) < torchLight.GetTorchLightRadius();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            if (!interacted & PlayerIsInRange()) {
                Player.Instance.PlayGuide(GuideType.Interact, KeyCode.E);
                interacted = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            Player.Instance.StopActiveGuide();
            interacted = false;
        }
    }
}
