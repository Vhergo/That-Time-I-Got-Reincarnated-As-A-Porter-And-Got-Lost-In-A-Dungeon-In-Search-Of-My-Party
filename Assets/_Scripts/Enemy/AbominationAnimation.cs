using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbominationAnimation : MonoBehaviour
{
    [SerializeField] private AnimationClip eyelidAnimation;
    [SerializeField] private float openDelay;
    [SerializeField] private bool openOnStart;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        if (openOnStart) TriggerEyelidOpen();
    }

    public void TriggerEyelidOpen() => StartCoroutine(OpenEyelid());

    private IEnumerator OpenEyelid()
    {
        yield return new WaitForSeconds(openDelay);
        anim.Play(eyelidAnimation.name);
    }
}