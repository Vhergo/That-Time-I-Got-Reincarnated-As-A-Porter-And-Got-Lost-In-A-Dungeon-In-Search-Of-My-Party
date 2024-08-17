using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    public void onClick(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        var rayHit = Physics2D.GetRayIntersection(mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()));
        if (!rayHit.collider) return;

        int instanceID = rayHit.collider.gameObject.GetInstanceID();
        Debug.Log(instanceID.ToString());

        if (rayHit.collider.gameObject.tag.Equals("Placeholder"))
        {
            PlaceholderManager.instance.setStrength(rayHit.collider.gameObject);
        }
    }
}
