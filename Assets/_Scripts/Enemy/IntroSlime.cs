using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroSlime : MonoBehaviour
{
    [SerializeField] private Rigidbody2D slimeRB;

    private void Start() => DisableSlimeMovement();

    public void AllowSlimeMovement() => slimeRB.constraints = RigidbodyConstraints2D.FreezeRotation;
    public void DisableSlimeMovement() => slimeRB.constraints = RigidbodyConstraints2D.FreezePositionX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            AllowSlimeMovement();
        }
    }
}
