using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.Rendering.Universal;
using System;

public class Archetype4 : Monster
{
    [Header("Pathfinding")]
    [SerializeField] private Transform target;
    [SerializeField] private float aggroRange = 10f;
    [SerializeField] private float disengageRange = 20f;
    [SerializeField] private float pathUpdateSec = 0.5f;
    private bool isAggroed;

    [Header("Physics")]
    [SerializeField] private float speed = 75f;
    [SerializeField] private float acceleration = 0.5f;
    [SerializeField] private float nextWaypoint = 3f;

    [Header("Attack")]
    [SerializeField] private float attackDelay = 2.0f;
    [SerializeField] private float rotationSpeed;
    private bool canAttack = true;

    [Header("Bait Light")]
    [SerializeField] private Light2D baitLight;
    [SerializeField] private BaitLightInfo baitInfo;
    [SerializeField] private BaitLightInfo chaseInfo;
    private bool isChasing;

    private Path path;
    private int currentWaypoint = 0;
    Seeker seeker;

    public Archetype4()
    {
        this.archetype = Archetype.Archetype4;
        this.fearFactor = 3;
    }

    protected override void Start()
    {
        base.Start();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        SetBaitLightInfo(baitInfo);

        InvokeRepeating("UpdatePath", 0f, pathUpdateSec);
    }

    void UpdatePath()
    {
        if (seeker.IsDone() && TargetInDistance())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    private void Update()
    {
        if (isAggroed) RotateToFacePlayer();
    }

    private void FixedUpdate()
    {
        if (!canAttack) return;

        if (TargetInAttackDistance()) {
            StartCoroutine(AttackSequence());
        } else if (TargetInDistance()) {
            PathFollow();
        }
    }

    private void RotateToFacePlayer()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }

    private void PathFollow()
    {
        if (path == null)
            return;
        if (currentWaypoint >= path.vectorPath.Count)
            return;

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed;
        Vector2 currentVelocity = rb.velocity;

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypoint) currentWaypoint++;

        rb.velocity = Vector2.SmoothDamp(rb.velocity, force, ref currentVelocity, acceleration);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void SetBaitLightInfo(BaitLightInfo info)
    {
        baitLight.intensity = info.lightIntensity;
        baitLight.pointLightOuterRadius = info.lightRange;
        baitLight.pointLightInnerAngle = info.lightAngle;
        baitLight.color = info.lightColor;
    }

    private bool TargetInDistance()
    {
        if (isAggroed) {
            isAggroed = Vector2.Distance(transform.position, target.transform.position) < disengageRange;
        } else {
            isAggroed = Vector2.Distance(transform.position, target.transform.position) < aggroRange;
        }

        if (isAggroed && !isChasing) {
            isChasing = true;
            SetBaitLightInfo(chaseInfo);
        }else if (!isAggroed && isChasing) {
            isChasing = false;
            SetBaitLightInfo(baitInfo);
        }

        return isAggroed;
    }

    private bool TargetInAttackDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < nextWaypoint;
    }

    private IEnumerator AttackSequence()
    {
        yield return null;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, aggroRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, disengageRange);
    }
}

[Serializable]
public class BaitLightInfo
{
    public float lightIntensity;
    public float lightRange;
    public float lightAngle;
    public Color lightColor;
}
