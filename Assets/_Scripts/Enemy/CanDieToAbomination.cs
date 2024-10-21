using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanDieToAbomination : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Abomination")) {
            GetComponent<Monster>().MonsterDie();
        }
    }
}
