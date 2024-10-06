using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D), typeof(LineRenderer))]
public class Archetype3 : Monster
{
    #region VARIABLES
    [Header("Tentacle Stats")]
    [SerializeField] private int length;
    [SerializeField] private float deathDuration;
    [SerializeField] private TentacleStats reachStats;
    [SerializeField] private TentacleStats deathStats;
    [SerializeField] private TentacleStats deathStats2;

    [Header("Tentacle Control")]
    [SerializeField] private Transform targetDirection;
    [SerializeField] private Transform wiggleDirection;
    [SerializeField] private GameObject player;
    [Space(5)]
    [SerializeField] private float targetDistance;
    [SerializeField] private float smoothSpeed;
    [SerializeField] private float trailSpeed;
    [Space(5)]
    [SerializeField] private float wiggleSpeed;
    [SerializeField] private float wiggleMagnitude;
    [Space(5)]
    [SerializeField] private float reachRange;
    [SerializeField] private float grabRange;
    [SerializeField] private float reachStrength;
    [SerializeField] private float rotationSpeed;

    [Space(20)]
    [SerializeField] private bool isForShow;

    private LineRenderer lineRenderer;
    private EdgeCollider2D edgeCollider;
    private Vector3[] segmentPoses;
    private Vector3[] segmentV;
    private Vector2[] colliderPoints;
    private Vector3 tentacleBase;
    private bool canReach;
    private bool isReaching;
    private bool startReach;
    private bool isDying;
    private bool canAttack = true;


    [Space(15)]
    [SerializeField] private float finalTargetDistance;
    [SerializeField] private float finalGrabRange;
    [SerializeField] private float finalWiggleSpeed;
    [SerializeField] private float finalWiggleMagnitude;

    private Rigidbody2D playerRB;
    #endregion

    protected override void Start()
    {
        base.Start();
        archetype = Archetype.Archetype3;

        InitializeTentacleData();
        SetDefaultPosition();
    }

    private void Update()
    {
        canReach = PlayerInRange();
        if (!isReaching) isReaching = PlayerInGrabRange();

        IdleWiggle();

        if (canReach) {
            if (!isDying) ReachForPlayer();
            RotateToPlayer();
            
        } else {
            ResetTentacleStats();
            startReach = false;
        }

        UpdateEdgeCollider();
    }

    private void InitializeTentacleData()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerRB = player.GetComponent<Rigidbody2D>();

        lineRenderer = GetComponent<LineRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();
        lineRenderer.positionCount = length;
        segmentPoses = new Vector3[length];
        segmentV = new Vector3[length];
        colliderPoints = new Vector2[length];

        finalTargetDistance = targetDistance;
        finalWiggleSpeed = wiggleSpeed;
        finalWiggleMagnitude = wiggleMagnitude;
    }

    private void SetDefaultPosition()
    {
        segmentPoses[0] = targetDirection.position;
        for (int i = 1; i < length; i++) {
            segmentPoses[i] = segmentPoses[i - 1] + targetDirection.right * finalTargetDistance;
        }
        lineRenderer.SetPositions(segmentPoses);
    }

    private void IdleWiggle()
    {
        wiggleDirection.localRotation = Quaternion.Euler(0, 0,
        Mathf.Sin(Time.time * finalWiggleSpeed) * finalWiggleMagnitude);

        segmentPoses[0] = targetDirection.position;
        for (int i = 1; i < length; i++) {
            segmentPoses[i] = Vector3.SmoothDamp(
                segmentPoses[i],
                segmentPoses[i - 1] + targetDirection.right * finalTargetDistance,
                ref segmentV[i],
                smoothSpeed + (i / trailSpeed));
        }
        lineRenderer.SetPositions(segmentPoses);
    }

    private void ReachForPlayer()
    {
        if (!startReach) startReach = true;

        for (int i = length - 1; i > 0; i--) {
            float influenceFactor = Mathf.Pow(0.5f, i / (float)(length - 1));

            segmentPoses[i] = Vector3.MoveTowards(
                segmentPoses[i],
                player.transform.position,
                reachStrength * Time.deltaTime * influenceFactor
            );
        }
        lineRenderer.SetPositions(segmentPoses);
    }

    private void RotateToPlayer()
    {
        Vector2 playerDirection = player.transform.position - transform.position;
        float angle = Mathf.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle + 90);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }

    private void UpdateEdgeCollider()
    {
        for (int i = 0; i < length; i++) {
            colliderPoints[i] = transform.InverseTransformPoint(segmentPoses[i]);
        }
        edgeCollider.points = colliderPoints;
    }

    private void UpdateTentacleStats(TentacleStats stats)
    {
        finalTargetDistance = stats.targetDistance;
        finalGrabRange = stats.grabRange;
        finalWiggleSpeed = stats.wiggleSpeed;
        finalWiggleMagnitude = stats.wiggleMagnitude;
    }

    [ContextMenu("Update Tentacle Stats")]
    private void ResetTentacleStats()
    {
        if (isDying) return;

        finalTargetDistance = targetDistance;
        finalGrabRange = grabRange;
        finalWiggleSpeed = wiggleSpeed;
        finalWiggleMagnitude = wiggleMagnitude;
    }

    public override void MonsterDie()
    {
        StartCoroutine(MonsterDeathSequence());
    }

    private IEnumerator MonsterDeathSequence()
    {
        isDying = true;
        rotationSpeed *= 1.5f;
        UpdateTentacleStats(deathStats);
        yield return new WaitForSeconds(deathDuration);

        UpdateTentacleStats(deathStats2);
        yield return new WaitForSeconds(deathDuration / 4);

        Destroy(gameObject);
    }

    private void Attack() => StartCoroutine(AttackSequence());
    private IEnumerator AttackSequence()
    {
        canAttack = false;
        Player.Instance.TakeDamage(fearFactor);
        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Color newColor = Color.magenta;
        newColor.a = 0.1f;

        Gizmos.color = newColor;
        Gizmos.DrawWireSphere(transform.position, reachRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, finalTargetDistance);

        Gizmos.color = Color.cyan;

        // Get the direction based on the object's rotation (using down as the default direction)
        Vector3 rotatedDirection = transform.rotation * Vector3.down;

        // Draw the line in the rotated direction
        Gizmos.DrawLine(transform.position, transform.position + rotatedDirection * 10f);


    }

    #region HELPER METHODS
    private bool PlayerInGrabRange()
    {
        if (player == null) return false;

        tentacleBase = segmentPoses[0];
        return Vector3.Distance(player.transform.position, tentacleBase) < finalGrabRange;
    }

    private bool PlayerInRange()
    {
        if (player == null) return false;

        tentacleBase = segmentPoses[0];
        return Vector3.Distance(player.transform.position, tentacleBase) < reachRange;
    }
    #endregion

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && !isForShow) {
            Attack();
        }
    }
}

[Serializable]
public class TentacleStats
{
    public float targetDistance;
    public float grabRange;
    public float wiggleSpeed;
    public float wiggleMagnitude;
}
