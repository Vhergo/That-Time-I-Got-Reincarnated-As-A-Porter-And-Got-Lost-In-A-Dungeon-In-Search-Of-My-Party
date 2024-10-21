using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbominationWall : MonoBehaviour
{
    [SerializeField] private AnimationClip forcePlayerToTheCorridor;
    public void MoveWall()
    {
        GetComponent<Animator>().Play(forcePlayerToTheCorridor.name);
    }

    public void EnableWall() => gameObject.SetActive(true);
    public void DisableWall() => gameObject.SetActive(false);
}
