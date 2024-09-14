using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderMonster : Monster
{
    public float moveSpeed = 2f;
    public float detectionRadius = 7f;  
    private bool movingRight = true;
    private Rigidbody2D rb;
    private Transform player;  
    void Start()
    {
      
        monster_name = "Spider";
        monster_strength = 3; 
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
                monster_attack();  
            }
        }
    }

   
    void TurnAround()
    {
        movingRight = !movingRight;
        transform.eulerAngles = new Vector3(0, movingRight ? 0 : 180, 0);
    }


    public override void monster_attack()
    {
       
        FearScript.instance.addFear(monster_strength);
       
        Debug.Log(monster_name + " is attacking the player!");
    }

    
    public override void monster_die()
    {
        base.monster_die();
        Debug.Log(monster_name + " has been destroyed!");
    }
}
