using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archetype1 : Monster
{
    [SerializeField] private float aggroRange;
    [SerializeField] private float disengageRange;
    private Transform player;
    private bool canMove = true;

    [SerializeField] private AnimationClip idleAnimation;
    [SerializeField] private AnimationClip walkAnimation;
    [SerializeField] private AnimationClip attackAnimation;

    protected override void Start()
    {
        base.Start();
        archetype = Archetype.Archetype1;
        player = Player.Instance.transform;
    }

    private void FixedUpdate()
    {
        if (canMove) Move();
    }

    private void Move()
    { 
        if (!GroundAhead() || WallAhead() || LightAhead()) {
            TurnAround();
        }

        float moveDir = movingRight ? 1 : -1;
        rb.velocity = new Vector2(moveDir * moveSpeed, rb.velocity.y);
    }

    private bool GroundAhead() => Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    private bool WallAhead() => Physics2D.OverlapCircle(wallCheck.position, 0.2f, groundLayer);
    private bool LightAhead() => Physics2D.OverlapCircle(wallCheck.position, 0.2f, lightLayer);

    public override void MonsterDie()
    {
        base.MonsterDie();
        Debug.Log(archetype + " has been destroyed!");

        GameObject droppedLoot = LootManager.Instance.GetLoot(archetype);
        if (droppedLoot != null)
            Instantiate(droppedLoot, transform.position, Quaternion.identity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            TurnAround();
        }
    }
}
