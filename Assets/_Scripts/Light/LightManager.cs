using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightManager : MonoBehaviour
{
    [SerializeField] private float innerRadius;
    [SerializeField] private float outerRadius;

    private float torchStrength;

    // Start is called before the first frame update
    void Start()
    {
        torchStrength = gameObject.transform.parent.gameObject.GetComponent<Placeholder>().strength;
    }

    // Update is called once per frame
    void Update()
    {
        torchStrength = gameObject.transform.parent.gameObject.GetComponent<Placeholder>().strength;
        gameObject.GetComponent<Light2D>().pointLightInnerRadius = innerRadius * (torchStrength / 100);
        gameObject.GetComponent<Light2D>().pointLightOuterRadius = outerRadius * (torchStrength / 100);
    }
}
