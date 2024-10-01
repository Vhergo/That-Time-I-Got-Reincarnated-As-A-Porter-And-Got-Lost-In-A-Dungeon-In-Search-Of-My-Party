using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archetype1 : Monster
{
    [SerializeField] private float aggroRange;
    [SerializeField] private float disengageRange;
    private bool isAggroed = false;
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

    private void Update()
    {
        // CheckIfTargetIsInRange();
    }

    private void FixedUpdate()
    {
        if (canMove) Move();
    }

    private void Move()
    {
        if (tier == Tier.King && isAggroed) FacePlayer();

        if (!GroundAhead() || WallAhead() || LightAhead()) {
            TurnAround();
        }

        float moveDir = movingRight ? 1 : -1;
        rb.velocity = new Vector2(moveDir * moveSpeed, rb.velocity.y);
    }

    private void FacePlayer()
    {
        if (player.position.x < transform.position.x && movingRight)  TurnAround();
        else if (player.position.x > transform.position.x && !movingRight) TurnAround();
    }

    //private bool CheckIfTargetIsInRange()
    //{
    //    if (isAggroed) {
    //        isAggroed = false;
    //        return Vector2.Distance(transform.position, player.transform.position) < disengageRange;
    //    } else {
    //        isAggroed = true;
    //        return Vector2.Distance(transform.position, player.transform.position) < aggroRange;
    //    }
    //}

    protected override void MonsterAttack()
    {
        canMove = false;
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        anim.Play(attackAnimation.name);

        yield return null;
        yield return new WaitForSeconds(anim.GetCurrentAnimatorClipInfo(0).Length);
        anim.Play(idleAnimation.name);

        yield return new WaitForSeconds(attackRate);
        anim.Play(walkAnimation.name);
        canMove = true;
    }

    private void ContactWithPlayer()
    {
        switch (tier) {
            case Tier.Normal:
                TurnAround();
                break;
            case Tier.King:
                if (canMove) MonsterAttack();
                break;
        }
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
            ContactWithPlayer();
        }
    }
}
