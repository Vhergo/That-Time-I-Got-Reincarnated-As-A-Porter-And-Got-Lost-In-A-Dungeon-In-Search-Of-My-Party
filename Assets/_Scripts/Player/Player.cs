using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float damageCooldown = 4f;
    private bool canTakeDamage = true;

    public static Player Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private IEnumerator TakeDamageCooldown()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canTakeDamage = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Monster") && canTakeDamage) {
            Monster monster = collision.gameObject.GetComponent<Monster>();
            monster.MonsterAttack();
            StartCoroutine(TakeDamageCooldown());
        }
    }
}
