using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TorchManager : MonoBehaviour, IInteractable
{
    public static TorchManager Instance { get; private set; }

    [Header("Torch")]
    [SerializeField] private TorchHolder activeTorch;
    [SerializeField] private float maxTorchFuel;
    [SerializeField] private float currentTorchFuel;
    [SerializeField] private float torchBurnRate;
    [SerializeField] private int torchPlaceRange;
    [SerializeField] private bool torchIsPlaced;

    [Space(10)]
    [SerializeField] private Image torchUI;

    private Coroutine burnFuelCoroutine;

    // Using an item book like this limits us to only have items and values listed in it
    // Using if we were to use ScriptableObjects instead, we would have a lot more flexibility in the 
    // quantity and variety of items we could easily implement

    [SerializeField] private List<TorchHolder> torches = new List<TorchHolder>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        SetupTorches();
    }
    
    public void Interact()
    {
        if (torchIsPlaced) RemoveTorch();
        else SetTorch();
    }

    public void SetTorch()
    {
        if (OutOfTorchFuel()) return;

        torchIsPlaced = true;
        activeTorch.PlaceTorch();
        burnFuelCoroutine = StartCoroutine(BurnFuel());
        ShowTorchUI();
    }

    public void RemoveTorch()
    {
        torchIsPlaced = false;
        activeTorch.RemoveTorch();
        activeTorch = null;
        if (burnFuelCoroutine != null) StopCoroutine(burnFuelCoroutine);
        HideTorchUI();
    }

    public IEnumerator BurnFuel()
    {
        while (currentTorchFuel > 0 && activeTorch != null) {
            currentTorchFuel -= torchBurnRate * Time.deltaTime;
            float torchFuelPercentage = currentTorchFuel / maxTorchFuel;
            UpdateTorchUI();
            //activeTorch.UpdateTorchLightRadius(torchFuelPercentage);

            // Automatically remove torch if fuel runs out
            if (currentTorchFuel <= 0) RemoveTorch();

            yield return null;
        }
    }

    public void AddTorchFuel(ItemData item)
    {
        currentTorchFuel = MathF.Min(currentTorchFuel + item.itemFuelValue, maxTorchFuel);
        UpdateTorchUI();
    }

    public void SetActiveTorch(TorchHolder torchHolder)
    {
        if (activeTorch != null && activeTorch != torchHolder) SetAnotherTorch();
        activeTorch = torchHolder;
        Interact();
    }

    private void SetAnotherTorch()
    {
        Debug.Log("OTHER TORCH");
        activeTorch.RemoveTorch();
        torchIsPlaced = false;
    }

    public void ShowTorchUI() => torchUI.transform.parent.gameObject.SetActive(true);
    public void HideTorchUI() => torchUI.transform.parent.gameObject.SetActive(false);

    public float GetTorchPlaceRange() => torchPlaceRange;
    public bool GetTorchState() => torchIsPlaced;
    public float GetTorchFuel() => currentTorchFuel;
    public float GetTorchBurnRate() => torchBurnRate;
    public TorchHolder GetActiveTorch() => activeTorch;
    private bool OutOfTorchFuel() => currentTorchFuel <= 0;
    private void UpdateTorchUI() => torchUI.fillAmount = currentTorchFuel / maxTorchFuel;

    private void SetupTorches()
    {
        torches.Clear();
        torches = FindObjectsOfType<TorchHolder>(false).ToList();

        HideTorchUI();
    }
}
