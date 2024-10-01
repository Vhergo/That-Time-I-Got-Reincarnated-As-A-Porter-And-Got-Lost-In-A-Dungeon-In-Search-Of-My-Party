using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GlobalLight : MonoBehaviour
{
    [SerializeField] private float dimLightFloat;

    public static GlobalLight Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ChangeGlobalLight(bool torch)
    {
        if (torch)
        {
            gameObject.GetComponent<Light2D>().intensity = dimLightFloat;
        }
        else
        {
            gameObject.GetComponent<Light2D>().intensity = 0f;
        }
    }
}
