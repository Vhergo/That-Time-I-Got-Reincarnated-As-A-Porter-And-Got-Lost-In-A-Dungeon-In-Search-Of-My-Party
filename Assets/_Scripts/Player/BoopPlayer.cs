using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoopPlayer : MonoBehaviour
{
    [SerializeField] private Vector2 boolForce;

    private void BoopPlayerOffTheEdge(Rigidbody2D playerRB)
    {
        playerRB.AddForce(Vector2.up * boolForce.y, ForceMode2D.Impulse);
        playerRB.AddForce(Vector2.right * boolForce.x, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            BoopPlayerOffTheEdge(collision.GetComponent<Rigidbody2D>());
            Destroy(gameObject);
        }
    }
}
