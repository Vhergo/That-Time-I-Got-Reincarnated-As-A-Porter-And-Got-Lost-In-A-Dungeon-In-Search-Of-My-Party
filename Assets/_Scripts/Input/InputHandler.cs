using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private float torchDistance;
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

        GameObject torchPressed = rayHit.collider.gameObject;
        int instanceID = torchPressed.GetInstanceID();

        GameObject player = GameObject.Find("Player");
        float distance = Vector3.Distance(player.transform.position,torchPressed.transform.position);
        Debug.Log(distance.ToString());

        if (rayHit.collider.gameObject.tag.Equals("Placeholder") && torchPressed.GetComponent<Placeholder>().strength == 0.0f && distance <= torchDistance)
        {
            PlaceholderManager.instance.setTorch(torchPressed.gameObject);
        }
    }
}
