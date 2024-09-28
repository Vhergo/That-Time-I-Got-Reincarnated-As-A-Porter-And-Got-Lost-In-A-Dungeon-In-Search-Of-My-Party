using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Unity.VisualScripting;

public class Archetype2 : Monster
{
    [Header("Pathfinding")]
    [SerializeField] private Transform target;
    [SerializeField] private float aggroRange = 10f;
    [SerializeField] private float disengageRange = 20f;
    [SerializeField] private float pathUpdateSec = 0.5f;
    private bool isAggroed;

    [Header("Physics")]
    [SerializeField] private float speed = 200f;
    [SerializeField] private float nextWaypoint = 3f;
    [SerializeField] private float jumpNodeHeightReq = 0.8f;
    [SerializeField] private float jumpHeight = 0.3f;
    [SerializeField] private float jumpCheckOffset = 0.1f;

    [Header("Behavior")]
    [SerializeField] private float attackCooldown = 4;
    //[SerializeField] private float attackRange = 3;
    [SerializeField] private bool followEnabled = true;
    [SerializeField] private bool jumpEnabled = true;
    [SerializeField] private int moveRange;
    private bool canAttack = true;

    [Header("Idle Movement")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;

    [Header("Animation")]
    [SerializeField] private Animator anim;

    [Space(10)]
    private Path path;
    private int currentWaypoint = 0;
    [SerializeField] private bool isJumping, isInAir, onCooldown;
    [SerializeField] private RaycastHit2D isGrounded;
    Seeker seeker;
    Rigidbody2D rb;

    public void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        isJumping = false;
        isInAir = false;
        onCooldown = false;

        InvokeRepeating("UpdatePath", 0f, pathUpdateSec);
    }

    private void FixedUpdate()
    {
        if (TargetInAttackDistance()) {
            if (canAttack) StartCoroutine(Attack());
            else PathFollow(); // probably check for followEnabled here unless we swap this to idle
        }else if(TargetInDistance() && followEnabled) {
            PathFollow();
        }else {
            IdleMove();
        }
    }

    private IEnumerator Attack()
    {
        Debug.Log("Attacking");
        canAttack = false;
        anim.SetTrigger("Attack");

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private void UpdatePath()
    {
        if (followEnabled && TargetInDistance() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    private void IdleMove()
    {
        if (!GroundAhead() || WallAhead()) {
            TurnAround();
        }

        float moveDir = movingRight ? 1 : -1;
        rb.velocity = new Vector2(moveDir * moveSpeed, rb.velocity.y);
    }

    private bool GroundAhead() => Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    private bool WallAhead() => Physics2D.OverlapCircle(wallCheck.position, 0.2f, groundLayer);

    private void PathFollow()
    {
        if (path == null)
            return;
        if (currentWaypoint >= path.vectorPath.Count)
            return;

        Jump();

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if(distance < nextWaypoint) currentWaypoint++;

        if (rb.velocity.x > 0f && !movingRight) TurnAround();
        else if (rb.velocity.x < 0f && movingRight) TurnAround();
    }

    private void Jump()
    {
        Vector3 startOffest = transform.position - new Vector3(0f, GetComponent<Collider2D>().bounds.extents.y + jumpCheckOffset, transform.position.z);
        isGrounded = Physics2D.Raycast(startOffest, -Vector3.up, 0.05f);

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed;
        Vector2 currentVelocity = rb.velocity;

        if (jumpEnabled && isGrounded && !isInAir && !onCooldown) {
            if (direction.y > jumpNodeHeightReq) {
                if (isInAir)
                    return;

                isJumping = true;
                // rb.AddForce(Vector2.up * jumpHeight);
                rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
                Debug.Log(direction.y.ToString());
                StartCoroutine(JumpCoolDown());
            }
        }

        if (isGrounded) {
            isJumping = false;
            isInAir = false;
        } else {
            isInAir = true;
        }

        rb.velocity = Vector2.SmoothDamp(rb.velocity, force, ref currentVelocity, 0.5f);
    }

    public override void MonsterDie()
    {
        base.MonsterDie();

        GameObject droppedLoot = LootManager.Instance.GetLoot(archetype);
        if (droppedLoot != null)
            Instantiate(droppedLoot, transform.position, Quaternion.identity);
    }

    private bool TargetInDistance()
    {
        if (isAggroed) {
            isAggroed = false;
            return Vector2.Distance(transform.position, target.transform.position) < disengageRange;
        } else {
            isAggroed = true;
            return Vector2.Distance(transform.position, target.transform.position) < aggroRange;
        }  
    }

    private bool TargetInAttackDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < nextWaypoint;
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    IEnumerator JumpCoolDown()
    {
        onCooldown = true;
        yield return new WaitForSeconds(1f);
        onCooldown = false;
    }

    private void OnDrawGizmos()
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
