using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

public class Critter : Monster
{
    [SerializeField] private List<CritterInfo> critterAnimations;
    [SerializeField] private float runAwayTimer;
    [SerializeField] private float checkRadius;
    private bool sawPlayer;
    private bool isRunningAway;

    protected override void Start()
    {
        base.Start();
        ChooseCritter();

        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), Player.Instance.GetComponent<Collider2D>());
    }

    private void FixedUpdate() => Move();

    private void ChooseCritter()
    {
        var critter = critterAnimations[UnityEngine.Random.Range(0, critterAnimations.Count)];
        anim.runtimeAnimatorController = critter.controller;
        transform.localScale = new Vector3(critter.scale, critter.scale, critter.scale);
    }

    private void Move()
    {
        if (!GroundAhead() || WallAhead()) {
            if (!sawPlayer) TurnAround();
        }

        float moveDir = movingRight ? 1 : -1;
        rb.velocity = new Vector2(moveDir * moveSpeed, rb.velocity.y);
    }

    private IEnumerator RunAway()
    {
        isRunningAway = true;
        moveSpeed *= 2;
        anim.speed *= 2;

        yield return new WaitForSeconds(runAwayTimer);
        Destroy(gameObject);
    }

    private bool GroundAhead() => Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
    private bool WallAhead() => Physics2D.OverlapCircle(wallCheck.position, checkRadius, groundLayer);

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            TurnAround();
            sawPlayer = true;
            if (!isRunningAway) StartCoroutine(RunAway());
        }
    }

    protected override void OnDrawGizmos()
    {
        if (groundCheck != null) {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }

        if (wallCheck != null) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(wallCheck.position, checkRadius);
        }
    }
}

[Serializable]
public class  CritterInfo
{
    public AnimatorController controller;
    public float scale;
}
