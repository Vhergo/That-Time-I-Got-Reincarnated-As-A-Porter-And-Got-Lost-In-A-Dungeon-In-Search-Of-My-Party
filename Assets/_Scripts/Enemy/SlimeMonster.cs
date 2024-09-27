using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMonster : Monster
{
    public float detectionRadius = 5f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;

    private Rigidbody2D rb;
    private Transform player;
    private bool movingRight;

    private void Start()
    {
        monsterName = "Slime";
        rb = GetComponent<Rigidbody2D>();
        player = Player.Instance.transform;
    }

    private void Update()
    {
        MoveSlime();
        // AttackIfNearPlayer();
    }

    private void MoveSlime()
    {
        if (!GroundAhead() || WallAhead()) {
            TurnAround();
        }

        float moveDir = movingRight ? 1 : -1;
        rb.velocity = new Vector2(moveDir * moveSpeed, rb.velocity.y);
    }

    private bool GroundAhead() => Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    private bool WallAhead() => Physics2D.OverlapCircle(wallCheck.position, 0.2f, groundLayer);

    private void TurnAround()
    {
        Debug.Log("Turn");
        movingRight = !movingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    //private void AttackIfNearPlayer()
    //{
    //    if (player != null) {
    //        float distanceToPlayer = Vector2.Distance(player.position, transform.position);
    //        if (distanceToPlayer <= detectionRadius) {
    //            FearManager.Instance.AddFear((int)(fearFactor * Time.deltaTime));
    //            Debug.Log(monsterName + " is attacking the player!");
    //        }
    //    }
    //}

    [ContextMenu("Die")]
    public override void MonsterDie()
    {
        base.MonsterDie();
        Debug.Log(monsterName + " has been destroyed!");

        GameObject droppedLoot = GetComponent<LootDrop>().GetLoot();
        Instantiate(droppedLoot, transform.position, Quaternion.identity);
    }

    // Draw Gizmos to visualize groundCheck and wallCheck
    private void OnDrawGizmos()
    {
        if (groundCheck != null) {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, 0.2f);  // Ground detection radius
        }

        if (wallCheck != null) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(wallCheck.position, 0.2f);    // Wall detection radius
        }
    }
}
