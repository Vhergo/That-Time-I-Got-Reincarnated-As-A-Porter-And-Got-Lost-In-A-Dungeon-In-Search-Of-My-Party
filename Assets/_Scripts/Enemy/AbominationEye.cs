using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbominationEye : MonoBehaviour
{
    [SerializeField] private float maxDistance = 0.5f;
    [SerializeField][Range(0, 0.25f)] private float followFactor = 0.05f;
    [SerializeField][Range(0, 1)] private float scaleFactor = 0.05f;
    private Transform player;
    private Transform iris;

    private Vector3 irisStart;
    private Vector3 irisOriginalScale;

    private Vector3 playerStart;

    void Start()
    {
        player = Player.Instance.transform;
        iris = transform.GetChild(0);

        irisStart = iris.localPosition;
        irisOriginalScale = iris.localScale;
        playerStart = player.position;
    }

    void Update()
    {
        if (player == null || iris == null) return;

        FollowPlayerMovement();
    }

    private void FollowPlayerMovement()
    {
        Vector3 playerMovement = player.position - playerStart;
        Vector3 targetIrisPosition = irisStart + playerMovement * followFactor;

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
