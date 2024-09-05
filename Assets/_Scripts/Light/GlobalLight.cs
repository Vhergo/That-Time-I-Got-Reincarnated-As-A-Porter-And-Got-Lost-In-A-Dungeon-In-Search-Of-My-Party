using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GlobalLight : MonoBehaviour
{
    [SerializeField] private float dimLightFloat;

    public static GlobalLight instance;
    private void Awake()
    {
        instance = this;
    }

    public void changeGlobalLight(bool torch)
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
