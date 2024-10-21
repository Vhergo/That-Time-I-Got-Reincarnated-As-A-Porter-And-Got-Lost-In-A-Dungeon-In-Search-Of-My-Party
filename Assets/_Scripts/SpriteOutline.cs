using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteOutline : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private Color outlineColor = Color.black; // Outline color
    [SerializeField] private float outlineWidth = 0.05f; // Width of the outline

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Create or fetch LineRenderer
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null) {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        lineRenderer.startWidth = outlineWidth;
        lineRenderer.endWidth = outlineWidth;
        lineRenderer.startColor = outlineColor;
        lineRenderer.endColor = outlineColor;
        lineRenderer.loop = true;
        lineRenderer.useWorldSpace = false;
        lineRenderer.sortingLayerID = spriteRenderer.sortingLayerID;
        lineRenderer.sortingOrder = spriteRenderer.sortingOrder - 1; // Behind sprite

        CreateOutline();
    }

    private void CreateOutline()
    {
        Sprite sprite = spriteRenderer.sprite;

        // Get sprite's rect to calculate vertices on the edges
        Rect rect = sprite.rect;

        // Normalize the vertices positions
        float width = rect.width / sprite.pixelsPerUnit;
        float height = rect.height / sprite.pixelsPerUnit;

        // Add small offsets around the sprite edges
        Vector3[] outlineVertices = new Vector3[4]
        {
            new Vector3(-width / 2 - outlineWidth, -height / 2 - outlineWidth),  // Bottom left
            new Vector3(-width / 2 - outlineWidth, height / 2 + outlineWidth),   // Top left
            new Vector3(width / 2 + outlineWidth, height / 2 + outlineWidth),    // Top right
            new Vector3(width / 2 + outlineWidth, -height / 2 - outlineWidth)    // Bottom right
        };

        // Set line renderer positions based on outline vertices
        lineRenderer.positionCount = outlineVertices.Length;
        lineRenderer.SetPositions(outlineVertices);
    }
}
