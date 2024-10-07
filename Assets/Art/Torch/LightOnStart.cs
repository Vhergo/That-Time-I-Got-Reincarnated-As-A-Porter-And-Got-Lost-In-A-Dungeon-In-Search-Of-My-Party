using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightOnStart : MonoBehaviour
{
    [SerializeField] private AnimationClip lightTorch;
    [SerializeField] private float outterRadiusStart;
    [SerializeField] private float outterRadiusEnd;
    [SerializeField] private float startDelay;
    private Animator anim;
    private Light2D light2D;

    private void Start()
    {
        anim = transform.parent.GetComponent<Animator>();
        light2D = GetComponent<Light2D>();

        light2D.enabled = true;
        light2D.pointLightOuterRadius = outterRadiusStart;

        StartCoroutine(LightTorchOnStart());
    }

    private IEnumerator LightTorchOnStart()
    {
        yield return new WaitForSeconds(startDelay);

        anim.Play(lightTorch.name);
        yield return new WaitForSeconds(lightTorch.length / 4);

        float animationLength = lightTorch.length;
        float elapsedTime = 0f;

        float radiusDifference = outterRadiusEnd - outterRadiusStart;

        while (elapsedTime < animationLength) {
            light2D.pointLightOuterRadius = Mathf.Lerp(outterRadiusStart, outterRadiusEnd, elapsedTime / animationLength);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        light2D.pointLightOuterRadius = outterRadiusEnd;
    }
}
