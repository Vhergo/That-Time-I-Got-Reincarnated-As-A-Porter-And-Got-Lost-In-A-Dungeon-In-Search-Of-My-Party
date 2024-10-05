using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Unity.VisualScripting;
using System.Linq;

public class Archetype2 : Monster
{
    #region VARIABLES
    [Header("Pathfinding")]
    [SerializeField] private Transform target;
    [SerializeField] private float aggroRange = 10f;
    [SerializeField] private float disengageRange = 20f;
    [SerializeField] private float pathUpdateSec = 0.5f;
    private bool isAggroed;

    [Header("Physics")]
    [SerializeField] private float speed = 200f;
    [SerializeField] private float acceleration = 0.5f;
    [SerializeField] private float nextWaypoint = 3f;
    [SerializeField] private float jumpNodeHeightReq = 0.8f;
    [SerializeField] private float jumpHeight = 0.3f;
    [SerializeField] private float jumpCheckOffset = 0.1f;

    [Header("Behavior")]
    [SerializeField] private Transform attackArea;
    [SerializeField] private float attackRadius;
    [SerializeField] private float attackDelay = .5f;
    [SerializeField] private bool avoidLight;
    [SerializeField] private float avoidLightDuration = 5f;
    [SerializeField] private bool followEnabled = true;
    [SerializeField] private bool jumpEnabled = true;
    private bool canAttack = true;
    private bool isAttacking;
    private bool ignorePlayer;

    [Header("Animation")]
    [SerializeField] private AnimationClip idleAnimation;
    [SerializeField] private AnimationClip walkAnimation;
    [SerializeField] private AnimationClip attackAnimation;

    [Space(10)]
    private Path path;
    private int currentWaypoint = 0;
    private bool isJumping, isInAir, onCooldown;
    Seeker seeker;
    #endregion

    protected override void Start()
    {
        base.Start();
        seeker = GetComponent<Seeker>();
        anim = GetComponent<Animator>();

        isJumping = false;
        isInAir = false;
        onCooldown = false;

        InvokeRepeating("UpdatePath", 0f, pathUpdateSec);
    }

    private void FixedUpdate()
    {
        if (!canAttack) return;

        if (TargetInAttackDistance()) {
            StartCoroutine(AttackSequence());
        }else if(TargetInDistance() && followEnabled && canAttack && !ignorePlayer) {
            if (LightAhead() && avoidLight) {
                StartCoroutine(AvoidPlayerSequence());
                return;
            }
            PathFollow();
        }else {
            Move();
        }
    }

    private IEnumerator AttackSequence()
    {
        canAttack = false;

        anim.Play(idleAnimation.name);

        // Slight delay before attack (give player a chance to react)
        yield return new WaitForSeconds(attackDelay);
        anim.Play(attackAnimation.name);
        FacePlayer();
        isAttacking = true;

        // Swap to idle animation after delay
        yield return null;
        yield return new WaitForSeconds(anim.GetCurrentAnimatorClipInfo(0).Length);
        anim.Play(idleAnimation.name);

        // Start moving after your attack cooldown
        yield return new WaitForSeconds(attackCooldown);

        if (TargetInAttackDistance()) anim.Play(idleAnimation.name);
        else anim.Play(walkAnimation.name);

        canAttack = true;
    }

    public void Attack() => StartCoroutine(RegisterAttack());
    public void StopAttack() => isAttacking = false;
    private IEnumerator RegisterAttack()
    {
        while (isAttacking) {
            Collider2D[] hitObjects = Physics2D.OverlapCircleAll(attackArea.position, attackRadius);
            if (hitObjects.Any(obj => obj.CompareTag("Player"))) {
                Player.Instance.TakeDamage(fearFactor);
                Inventory.Instance.DestroyRandomItem();
                yield break;
            }
            yield return null;
        }
    }

    private IEnumerator AvoidPlayerSequence()
    {
        Debug.Log("Avoiding Player??");
        ignorePlayer = true;
        TurnAround();

        yield return new WaitForSeconds(avoidLightDuration);
        ignorePlayer = false;
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
    private bool IsGrounded() => Physics2D.OverlapCircle(transform.position, 0.2f, groundLayer);

    private void UpdatePath()
    {
        if (followEnabled && TargetInDistance() && seeker.IsDone() && !ignorePlayer) {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    private void PathFollow()
    {
        if (path == null) return;
        if (currentWaypoint >= path.vectorPath.Count) return;

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed;
        Vector2 currentVelocity = rb.velocity;

        Jump(direction);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if(distance < nextWaypoint) currentWaypoint++;

        if (rb.velocity.x > 0f && !movingRight) TurnAround();
        else if (rb.velocity.x < 0f && movingRight) TurnAround();

        rb.velocity = Vector2.SmoothDamp(rb.velocity, force, ref currentVelocity, acceleration);
    }

    private void Jump(Vector2 direction)
    {
        if (jumpEnabled && IsGrounded() && !isInAir && !onCooldown) {
            if (direction.y > jumpNodeHeightReq) {
                isJumping = true;
                rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
                StartCoroutine(JumpCoolDown());
            }
        }

        if (IsGrounded()) {
            isJumping = false;
            isInAir = false;
        } else {
            isInAir = true;
        }
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
            return isAggroed = Vector2.Distance(transform.position, target.transform.position) < disengageRange;
        } else {
            return isAggroed = Vector2.Distance(transform.position, target.transform.position) < aggroRange;
        }  
    }

    private bool TargetInAttackDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < nextWaypoint;
    }

    private void FacePlayer()
    {
        if (target.position.x < transform.position.x && movingRight) TurnAround();
        else if (target.position.x > transform.position.x && !movingRight) TurnAround();
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

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        if (isAttacking) {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(attackArea.position, attackRadius);
        }

        Color newColor = Color.yellow;
        newColor.a = 0.1f;

        Gizmos.color = newColor;
        Gizmos.DrawWireSphere(transform.position, aggroRange);

        Color newColor2 = Color.red;
        newColor2.a = 0.1f;

        Gizmos.color = newColor2;
        Gizmos.DrawWireSphere(transform.position, disengageRange);
    }
}
