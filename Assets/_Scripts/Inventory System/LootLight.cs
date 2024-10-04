using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(CircleCollider2D))]
public class LootLight : MonoBehaviour
{
    [Range(0, 1)][SerializeField] private float alphaThreshold = 0.35f;
    private CircleCollider2D detectionTrigger;
    private SpriteRenderer sprite;
    private Transform player;
    private Color adjustmentColor;

    private void Start()
    {
        detectionTrigger = GetComponent<CircleCollider2D>();
        detectionTrigger.isTrigger = true;

        sprite = GetComponent<SpriteRenderer>();
        adjustmentColor = sprite.color;
    }

    private void Update()
    {
        if (player == null) return;

        float thresholdDistance = detectionTrigger.radius * alphaThreshold;
        float adjustedDistance = Mathf.Clamp(GetDistanceFromPlayer() - thresholdDistance, 0, detectionTrigger.radius);
        float value = Mathf.Lerp(0, 100, adjustedDistance / (detectionTrigger.radius - thresholdDistance));

        adjustmentColor.a = Mathf.Clamp01(value / 255);
        sprite.color = adjustmentColor;
    }

    private float GetDistanceFromPlayer()
    {
        return Vector2.Distance(transform.position, player.position);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            player = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            player = null;
        }
    }
}
