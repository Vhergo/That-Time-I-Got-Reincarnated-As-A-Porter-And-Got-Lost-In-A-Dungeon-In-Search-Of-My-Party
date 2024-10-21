using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootstep : MonoBehaviour
{
    private Player2DPlatformerMovement player;

    private void Start() => player = GetComponentInParent<Player2DPlatformerMovement>();

    public void PlayFootstepSound() => player.PlayFootstepSound();
}
