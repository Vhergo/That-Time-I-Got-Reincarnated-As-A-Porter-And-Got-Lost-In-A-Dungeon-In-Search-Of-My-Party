using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRoar : MonoBehaviour
{
    private AudioSource roarSource;

    private void Start()
    {
        roarSource = GetComponent<AudioSource>();
    }

    public void TriggerMonsterRoar()
    {
        roarSource.Play();
        CameraShake.Instance.TriggerCameraShake(1.5f, 1f, roarSource.clip.length + 0.5f);
    }
}
