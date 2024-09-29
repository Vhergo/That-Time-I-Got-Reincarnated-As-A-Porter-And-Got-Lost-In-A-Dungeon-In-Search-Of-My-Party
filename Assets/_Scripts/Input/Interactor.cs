using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    private TorchHolder selectedHolder;
    private TorchManager torchManager;

    [SerializeField] private KeyCode interactKey;
    private bool isNearTorch;


    private void Start()
    {
        torchManager = TorchManager.Instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(interactKey) && isNearTorch) {
            Interact();
        }
    }

    private void Interact()
    {
        torchManager.SetActiveTorch(selectedHolder);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("TorchHolder")) {
            if (collision.TryGetComponent(out TorchHolder holder)) {
                selectedHolder = holder;
                isNearTorch = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("TorchHolder")) {
            selectedHolder = null;
            isNearTorch = false;
        }
    }
}
