using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformOneWay : MonoBehaviour
{
    [SerializeField] private float dropDelayTimer = 0.1f;
    [SerializeField] private float restoreDelay = 0.25f;
    private float dropDelayCounter;

    private Collider2D boxCol;
    private CompositeCollider2D platformCol;
    private bool isDropping = false;

    private void Start() {
        boxCol = GetComponent<Collider2D>();
        dropDelayCounter = dropDelayTimer;
    }

    private void Update() {
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            if (!isDropping) DropThroughPlatform();
        }

        if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow)) {
            dropDelayCounter = dropDelayTimer;
        }
    }

    private void DropThroughPlatform() {
        if (dropDelayCounter <= 0) {
            if (platformCol != null) Physics2D.IgnoreCollision(boxCol, platformCol);
            dropDelayCounter = dropDelayTimer;
            isDropping = true;

            Invoke("RestoreCollision", restoreDelay);
        }else {
            dropDelayCounter -= Time.deltaTime;
        }
    }

    private void RestoreCollision() {
        if (platformCol != null) Physics2D.IgnoreCollision(boxCol, platformCol, false);
        isDropping = false;
    }

    private void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "TwoWayPlatform") {
            if (!isDropping) platformCol = col.gameObject.GetComponent<CompositeCollider2D>();
        }
    }
}
