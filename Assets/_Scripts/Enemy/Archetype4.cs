using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Archetype4 : Monster
{
    [Header("Pathfinding")]
    [SerializeField] private Transform target;
    [SerializeField] private float activeDistance = 50f;
    [SerializeField] private float pathUpdateSec = 0.5f;
    [SerializeField] private int moveRange;


    [Header("Physics")]
    [SerializeField] private float speed = 200f;
    [SerializeField] private float nextWaypoint = 3f;

    [Header("Attack")]
    [SerializeField] private float attackDelay = 2.0f;
    [SerializeField] private float passedTime = 2.0f;
    [SerializeField] private float attackRange = 2.0f;


    Path path;
    int currentWaypoint = 0;
    bool reachedEOP = false;
    bool flyCooldown = false;
    Seeker seeker;

    public Archetype4()
    {
        this.archetype = Archetype.Archetype4;
        this.fearFactor = 3;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, pathUpdateSec);
    }

    void UpdatePath()
    {
        if (seeker.IsDone() && TargetInDistance())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    private void FixedUpdate()
    {
        if (TargetInDistance())
        {
            float distance = Vector2.Distance(target.position, rb.position);
            if (distance < attackRange)
            {
                Attack();
            }
            else
            {
                MoveAlongPath();
            }
        }
        else if(!flyCooldown)
        {
            IdleFly();
        }
        if (passedTime < attackDelay)
            passedTime += Time.deltaTime;
    }

    private void MoveAlongPath()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEOP = true;
            return;
        }
        else
        {
            reachedEOP = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypoint)
            currentWaypoint++;
    }

    private void IdleFly()
    {
        int movement = Random.Range(0, moveRange + 1);
        int direction = Random.Range(0, 2);
        if (direction == 0)
        {
            Vector3 targetPos = (transform.position + new Vector3(movement, 0, 0)) * -1;
            rb.AddForce(targetPos);
            //rb.velocity = targetPos;

        }
        else
        {
            Vector3 targetPos = (transform.position + new Vector3(movement, 0, 0));
            rb.AddForce(targetPos);
            //rb.velocity = targetPos;
        }
        StartCoroutine(FlyCooldown());
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
    private bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < activeDistance;
    }

    private void Attack()
    {
        rb.velocity = Vector2.zero;
        if (passedTime >= attackDelay)
        {
            passedTime = 0;
            MonsterAttack();
        }
    }

    IEnumerator FlyCooldown()
    {
        flyCooldown = true;
        yield return new WaitForSeconds(1f);
        flyCooldown = false;
    }
}
