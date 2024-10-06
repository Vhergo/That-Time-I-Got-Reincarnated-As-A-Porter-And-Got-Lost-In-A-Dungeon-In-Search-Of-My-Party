using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayerOnCollision : MonoBehaviour
{
    private float fearFactor;
    private void Start()
    {
        fearFactor = GetComponentInParent<Monster>().fearFactor;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            Player.Instance.TakeDamage(fearFactor);
        }
    }
}
