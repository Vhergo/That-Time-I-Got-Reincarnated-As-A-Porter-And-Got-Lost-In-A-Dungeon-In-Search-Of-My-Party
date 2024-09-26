using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private Camera mainCamera;
    private Transform player;
    private GameObject selectedObject;
    private TorchHolder selectedHolder;
    private TorchManager torchManager;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Start()
    {
        player = Player.Instance.transform;
        torchManager = TorchManager.Instance;
    }

    public void onClick(InputAction.CallbackContext context)
    {
        Debug.Log("Click");
        if (!context.started) return;

        var rayHit = Physics2D.GetRayIntersection(mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()));
        if (!rayHit.collider) return;

        selectedObject = rayHit.collider.gameObject;


        if (!InTorchRange()) return;

        if (selectedObject.TryGetComponent(out TorchHolder torchHolder)) {
            selectedHolder = torchHolder;
        } else {
            return;
        }

        torchManager.SetActiveTorch(selectedHolder);
    }

    private bool InTorchRange()
    {
        return Vector3.Distance(player.position, selectedObject.transform.position) < torchManager.GetTorchPlaceRange();
    }
}
