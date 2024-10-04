using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public static PlayerAnimation Instance { get; private set; }

    private Animator anim;
    private AnimationState currentState;

    public enum AnimationState
    {
        PorterIdle,
        PorterWalk
    }

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void ChangeAnimationState(AnimationState newState)
    {
        // Stop the same animation from interrupting itself
        if (currentState == newState) return;

        anim.Play(newState.ToString());
        currentState = newState;
    }

    public void PlayIdleAnim()
    {
        ChangeAnimationState(AnimationState.PorterIdle);
    }

    public void PlayWalkAnim()
    {
        ChangeAnimationState(AnimationState.PorterWalk);
    }
}
