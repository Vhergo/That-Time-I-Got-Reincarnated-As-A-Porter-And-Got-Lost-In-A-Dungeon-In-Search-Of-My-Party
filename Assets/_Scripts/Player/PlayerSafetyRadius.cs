using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class PlayerSafetyRadius : MonoBehaviour
{
    private FearManager fearManager;

    private void Start() => fearManager = FearManager.Instance;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster")) {
            if (collision.TryGetComponent(out Monster monster)) {
                fearManager.IncreaseFear(monster.fearFactor);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster")) {
            if (collision.TryGetComponent(out Monster monster)) {
                fearManager.DecreaseFear();
            }
        }
    }
}
