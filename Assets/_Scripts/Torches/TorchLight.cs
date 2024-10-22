using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TorchLight : MonoBehaviour
{
    [Header("Light")]
    [SerializeField] private float minRadius;
    [SerializeField] private float innerRadius;
    [SerializeField] private float outerRadius;

    [Header("Flicker")]
    [SerializeField] private float minIntensity;
    [SerializeField] private float maxIntensity;
    [SerializeField] private float frequency;
    [SerializeField] private float magnitude;

    [Header("Collider")]
    [SerializeField] private float colliderRadius;

    private TorchManager torchManager;
    private Light2D torchLight;
    private CircleCollider2D torchCollider;
    private float torchReach;
    private bool isLit;

    private float torchBurnRate;
    private float interval = 1;
    private float timer;

    private Coroutine flickerCoroutine;

    void Start()
    {
        torchManager = TorchManager.Instance;

        torchBurnRate = torchManager.GetTorchBurnRate();
        torchLight = gameObject.GetComponent<Light2D>();
        torchCollider = gameObject.GetComponent<CircleCollider2D>();

        innerRadius = torchLight.pointLightInnerRadius;
        outerRadius = torchLight.pointLightOuterRadius;

        torchCollider.enabled = false;
        colliderRadius = torchCollider.radius;
    }

    public void EnableLight()
    {
        FearManager.Instance.isLit = isLit = true;
        torchLight.enabled = true;
        torchCollider.enabled = true;
        flickerCoroutine = StartCoroutine(Flicker());
    }

    public void DisableLight()
    {
        FearManager.Instance.isLit = isLit = false;
        torchLight.enabled = false;
        torchCollider.enabled = false;
    }

    public void UpdateTorchLightRadius(float torchFuelPercentage)
    {
        torchCollider.radius = Mathf.Max(minRadius, Mathf.Lerp(0, colliderRadius, torchFuelPercentage));
        torchLight.pointLightInnerRadius = Mathf.Max(minRadius, Mathf.Lerp(0, innerRadius, torchFuelPercentage));
        torchLight.pointLightOuterRadius = Mathf.Max(minRadius, Mathf.Lerp(0, outerRadius, torchFuelPercentage));
    }

    private IEnumerator Flicker()
    {
        while (isLit) {
            float noise = Mathf.PerlinNoise(Time.time * magnitude, 0.0f);
            float intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);

            torchLight.intensity = intensity;

            yield return new WaitForSeconds(frequency);
        }
    }

    public float GetTorchLightRadius() => torchLight.pointLightOuterRadius;
    public bool IsLit() => isLit;


    private void OnTriggerEnter2D(Collider2D col)
    {
        if ((col.CompareTag("Monster") || col.CompareTag("Abomination")) && isLit) {
            if (col.TryGetComponent(out Monster monster)) {
                monster.MonsterDie();
            }
        }
    }
}
