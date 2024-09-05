using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Unity.VisualScripting;

public class GoblinAI : MonoBehaviour
{
    [Header("Pathfinding")]
    [SerializeField] private Transform target;
    [SerializeField] private float activeDistance = 50f;
    [SerializeField] private float pathUpdateSec = 0.5f;

    [Header("Physics")]
    [SerializeField] private float speed = 200f;
    [SerializeField] private float nextWaypoint = 3f;
    [SerializeField] private float jumpNodeHeightReq = 0.8f;
    [SerializeField] private float jumpHeight = 0.3f;
    [SerializeField] private float jumpCheckOffset = 0.1f;

    [Header("Behavior")]
    [SerializeField] private bool followEnabled = true;
    [SerializeField] private bool jumpEnabled = true;
    [SerializeField] private bool directionLookEnabled = true;
    [SerializeField] private int moveRange;

    private Path path;
    private int currentWaypoint = 0;
    [SerializeField] private bool isJumping, isInAir, onCooldown,walkCooldown;
    [SerializeField] private RaycastHit2D isGrounded;
    Seeker seeker;
    Rigidbody2D rb;

    public void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        isJumping = false;
        isInAir = false;
        onCooldown = false;
        walkCooldown = false;

        InvokeRepeating("UpdatePath", 0f, pathUpdateSec);
    }

    private void FixedUpdate()
    {
        if(TargetInDistance() && followEnabled)
        {
            PathFollow();
        }
        else if(!walkCooldown)
        {
            IdleWalk();
        }
    }

    private void UpdatePath()
    {
        if(followEnabled && TargetInDistance() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    private void IdleWalk()
    {
        int movement = Random.Range(0, moveRange + 1);
        int direction = Random.Range(0, 2);
        if (direction == 0)
        {
            Vector3 targetPos = (transform.position + new Vector3(movement, 0, 0)) * -1;
            rb.velocity = targetPos;
        }
        else
        {
            Vector3 targetPos = (transform.position + new Vector3(movement, 0, 0));
            rb.velocity = targetPos;
        }
        StartCoroutine(WalkCooldown());
    }

    private void PathFollow()
    {
        if (path == null)
            return;
        if (currentWaypoint >= path.vectorPath.Count)
            return;

        Vector3 startOffest = transform.position - new Vector3(0f, GetComponent<Collider2D>().bounds.extents.y + jumpCheckOffset,transform.position.z);
        isGrounded = Physics2D.Raycast(startOffest, -Vector3.up, 0.05f);

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed;
        Vector2 currentVelocity = rb.velocity;

        if (jumpEnabled && isGrounded && !isInAir && !onCooldown)
        {
            if(direction.y > jumpNodeHeightReq)
            {
                if (isInAir)
                    return;

                isJumping = true;
                // rb.AddForce(Vector2.up * jumpHeight);
                rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
                Debug.Log(direction.y.ToString());
                StartCoroutine(JumpCoolDown());
            }
        }

        if (isGrounded)
        {
            isJumping = false;
            isInAir = false;
        }
        else
        {
            isInAir = true;
        }

        
        rb.velocity = Vector2.SmoothDamp(rb.velocity, force, ref currentVelocity, 0.5f);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if(distance < nextWaypoint)
        {
            currentWaypoint++;
        }

        if (directionLookEnabled)
        {
            if(rb.velocity.x > 0.05f)
            {
                transform.localScale = new Vector3(-1 * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }else if(rb.velocity.x < -0.05f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }

    private bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < activeDistance;
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

    IEnumerator WalkCooldown()
    {
        walkCooldown = true;
        yield return new WaitForSeconds(1f);
        walkCooldown = false;
    }
}
