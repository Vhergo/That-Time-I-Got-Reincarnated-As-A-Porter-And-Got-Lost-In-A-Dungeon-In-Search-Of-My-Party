using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPositionAsCheckpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            FearManager.Instance.SetCheckPointPos(transform);
        }
    }
}
