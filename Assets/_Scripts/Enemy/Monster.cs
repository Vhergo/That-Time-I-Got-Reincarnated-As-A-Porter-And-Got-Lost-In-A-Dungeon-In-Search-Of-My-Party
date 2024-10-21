using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public Archetype archetype;

    public float moveSpeed;
    public float fearFactor;
    [SerializeField] protected float attackCooldown;
    protected bool movingRight;
    protected Animator anim;

    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected LayerMask lightLayer;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected Transform wallCheck;
    protected Rigidbody2D rb;

    public Monster()
    {
        fearFactor = 1;
    }

    public Monster(Archetype archetype, float fear)
    {
        this.archetype = archetype;
        this.fearFactor = fear;
    }

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    public virtual void TurnAround()
    {
        movingRight = !movingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    protected virtual void MonsterAttack()
    {
        FearManager.Instance.AddFear(fearFactor);
    }

    [ContextMenu("Die")]
    public virtual void MonsterDie()
    {
        Destroy(gameObject);
    }

    protected virtual void OnDrawGizmos()
    {
        if (groundCheck != null) {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, 0.2f);
        }

        if (wallCheck != null) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(wallCheck.position, 0.2f);
        }
    }
}
