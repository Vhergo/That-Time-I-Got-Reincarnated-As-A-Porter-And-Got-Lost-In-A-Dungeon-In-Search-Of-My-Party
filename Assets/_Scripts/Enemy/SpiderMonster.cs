using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderMonster : Monster
{
    public float detectionRadius = 7f;  
    private bool movingRight = true;
    private Rigidbody2D rb;
    private Transform player;  
    void Start()
    {

        archetype = Archetype.Archetype2;
        fearFactor = 3; 
        rb = GetComponent<Rigidbody2D>();

       
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        MoveSpider();
        AttackIfNearPlayer();  
    }

    
    void MoveSpider()
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
                MonsterAttack();  
            }
        }
    }

    protected override void MonsterAttack()
    {
       
        FearManager.Instance.AddFear(fearFactor);
       
        Debug.Log(archetype.ToString() + " is attacking the player!");
    }

    
    public override void MonsterDie()
    {
        base.MonsterDie();
        Debug.Log(archetype.ToString() + " has been destroyed!");
    }
}
