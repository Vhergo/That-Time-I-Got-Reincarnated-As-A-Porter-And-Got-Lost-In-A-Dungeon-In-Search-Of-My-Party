using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbominationEye : MonoBehaviour
{
    [SerializeField] private float maxDistance = 0.5f;
    [SerializeField][Range(0, 1f)] private float followFactor = 0.05f;
    [SerializeField][Range(0, 1f)] private float scaleFactor = 0.05f;

    [SerializeField] private Transform player;
    private Transform iris;

    private Vector3 irisStart;
    private Vector3 irisOriginalScale;

    void Start()
    {
        if (player == null) player = Player.Instance.transform;
        iris = transform.GetChild(0);

        irisStart = iris.localPosition;
        irisOriginalScale = iris.localScale;
    }

    void Update()
    {
        if (player == null || iris == null) return;

        FollowPlayerMovement();
    }

    private void FollowPlayerMovement()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Vector3 targetIrisPosition = irisStart + directionToPlayer * followFactor * Vector3.Distance(transform.position, player.position);
        Vector3 clampedPosition = Vector3.ClampMagnitude(targetIrisPosition - irisStart, maxDistance) + irisStart;
        iris.localPosition = Vector3.Lerp(iris.localPosition, clampedPosition, Time.deltaTime * 5f);

        ScaleIrisToEmulateVolume(clampedPosition);
    }

    private void ScaleIrisToEmulateVolume(Vector3 position)
    {
        Vector3 distanceFromCenter = position - irisStart;

        float scaleFactorX = 1f - (Mathf.Abs(distanceFromCenter.x) / maxDistance) * scaleFactor;
        float scaleFactorY = 1f - (Mathf.Abs(distanceFromCenter.y) / maxDistance) * scaleFactor;

        iris.localScale = new Vector3(
            irisOriginalScale.x * scaleFactorX,
            irisOriginalScale.y * scaleFactorY,
            irisOriginalScale.z
        );
    }
}
