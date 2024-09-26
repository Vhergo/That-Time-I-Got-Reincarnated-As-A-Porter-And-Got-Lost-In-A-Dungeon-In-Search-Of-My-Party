using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchHolder : MonoBehaviour
{
    private Animator torchAnim;
    private TorchLight torchLight;

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
}
