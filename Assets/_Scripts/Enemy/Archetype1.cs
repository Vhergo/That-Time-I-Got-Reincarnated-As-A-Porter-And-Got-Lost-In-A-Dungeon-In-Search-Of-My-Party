using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archetype1 : Monster
{
    public float detectionRadius = 5f;
    
    private Transform player;

    protected override void Start()
    {
        base.Start();
        archetype = Archetype.Archetype1;
        player = Player.Instance.transform;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (!GroundAhead() || WallAhead()) {
            TurnAround();
        }

        float moveDir = movingRight ? 1 : -1;
        rb.velocity = new Vector2(moveDir * moveSpeed, rb.velocity.y);
    }

    private bool GroundAhead() => Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    private bool WallAhead() => Physics2D.OverlapCircle(wallCheck.position, 0.2f, groundLayer);

    public override void MonsterDie()
    {
        base.MonsterDie();
        Debug.Log(archetype + " has been destroyed!");

        GameObject droppedLoot = LootManager.Instance.GetLoot(archetype);
        if (droppedLoot != null)
            Instantiate(droppedLoot, transform.position, Quaternion.identity);
    }
}
