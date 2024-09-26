using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMonster : Monster
{
    public float moveSpeed = 1f;
    public float detectionRadius = 5f;
    private bool movingRight = true;
    private Rigidbody2D rb;
    private Transform player;

    void Start()
    {
        monsterName = "Slime";
        monsterStrength = 1;  
        rb = GetComponent<Rigidbody2D>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        MoveSlime();
        AttackIfNearPlayer();
    }

    void MoveSlime()
    {
        rb.velocity = new Vector2((movingRight ? 1 : -1) * moveSpeed, rb.velocity.y);

        RaycastHit2D groundInfo = Physics2D.Raycast(transform.position, Vector2.down, 1f);
        if (groundInfo.collider == null)
        {
            TurnAround();
        }
    }

    void AttackIfNearPlayer()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(player.position, transform.position);
            if (distanceToPlayer <= detectionRadius)
            {
               
                FearManager.Instance.AddFear((int)(monsterStrength * Time.deltaTime));
                Debug.Log(monsterName + " is attacking the player!");
            }
        }
    }

    void TurnAround()
    {
        movingRight = !movingRight;
        transform.eulerAngles = new Vector3(0, movingRight ? 0 : 180, 0);
    }

    public override void MonsterDie()
    {
        base.MonsterDie();
        Debug.Log(monsterName + " has been destroyed!");
    }
}
