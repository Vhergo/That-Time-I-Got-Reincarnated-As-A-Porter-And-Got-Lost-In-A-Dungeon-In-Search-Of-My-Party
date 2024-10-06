using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbominationEye : MonoBehaviour
{
    [SerializeField] private float maxDistance = 0.5f;
    private Transform player;
    private Transform iris;

    private void Start()
    {
        player = Player.Instance.transform;
        iris = transform.GetChild(0);
    }

    void Update()
    {
        TrackPlayer();
    }

    private void TrackPlayer()
    {
        if (player == null || iris == null)
            return;

        // Get the direction from the center of the eye to the player
        Vector3 direction = (player.position - transform.position).normalized;

        // Calculate a target position for the iris within the bounds of the eye
        Vector3 targetPosition = direction * maxDistance;

        // Set the iris position relative to the eye
        iris.localPosition = Vector3.ClampMagnitude(targetPosition, maxDistance);
    }
}
