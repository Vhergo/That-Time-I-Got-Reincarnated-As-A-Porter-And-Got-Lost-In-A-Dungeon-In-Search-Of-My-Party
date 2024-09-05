using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeholder : MonoBehaviour
{
    [SerializeField] private float torchTime;

    public float strength = 0.0f;
    public static Placeholder Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Update()
    {
        if (strength > 0.0f)
        {
            float duration = (strength / 100) * torchTime;
            StartCoroutine(startReduce(strength, 0, duration));
        }
    }

    public IEnumerator startReduce(float startStrength, float endStrength, float duration)
    {
        float currentTime = 0;
        while (strength != 0)
        {
            currentTime += Time.deltaTime;
            strength = Mathf.Lerp(startStrength, endStrength, currentTime / duration);
            yield break;
        }
        yield break;
    }

}
