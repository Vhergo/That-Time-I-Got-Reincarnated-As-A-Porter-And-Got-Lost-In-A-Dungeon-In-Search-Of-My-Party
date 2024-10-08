using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archetype5 : Monster
{
    [Header("Pathing")]
    [SerializeField] private Transform monster;
    [SerializeField] private Transform path;
    [SerializeField] private List<Transform> waypoints;
    [SerializeField] private bool loop;
    [SerializeField] private bool reversed;

    [SerializeField] private Transform currentWaypoint;
    [SerializeField] private int waypointIndex;

    [SerializeField] private SpriteRenderer sprite;

    protected override void Start()
    {
        Initialize();
    }

    private void Update()
    {
        // Always moving towards the next waypoint
        monster.position = Vector2.MoveTowards(monster.position, currentWaypoint.position, moveSpeed * Time.deltaTime);
        Flip();
    }

    private void Flip()
    {
        if (monster.position.x < currentWaypoint.position.x) sprite.flipX = true;
        else sprite.flipX = false;
    }

    private IEnumerator FollowPath()
    {
        while (monster) {
            if (HasReachedWaypoint()) {
                if (IsLastWaypoint()) {
                    // Debug.Log("LAST");
                    if (!loop) reversed = !reversed;
                }
                GetNextWaypoint();
            }
            yield return null;
        }
    }

    private bool HasReachedWaypoint()
    {
        return Vector3.Distance(monster.position, currentWaypoint.position) < 0.1f;
    }

    private bool IsLastWaypoint()
    {
        if (reversed) return currentWaypoint == waypoints[0];
        else return currentWaypoint == waypoints[waypoints.Count - 1];
    }

    private void GetNextWaypoint()
    {
        if (loop) {
            waypointIndex = ++waypointIndex % waypoints.Count;
            currentWaypoint = waypoints[waypointIndex];
            return;
        }

        if (reversed) currentWaypoint = waypoints[--waypointIndex];
        else currentWaypoint = waypoints[++waypointIndex];
    }

    private void Initialize()
    {
        waypoints.Clear();
        foreach (Transform child in path) waypoints.Add(child);
        if (waypoints.Count < 2) {
            Debug.LogWarning("Not enough waypoints in the path", this);
        }

        monster.position = waypoints[waypointIndex++].position;
        currentWaypoint = waypoints[waypointIndex];

        StartCoroutine(FollowPath());
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.cyan;

        foreach(Transform waypoint in waypoints) {
            Gizmos.DrawWireSphere(waypoint.position, 0.2f);
        }

        for (int i = 0; i < waypoints.Count - 1; i++) {
            Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
        }
    }
}
