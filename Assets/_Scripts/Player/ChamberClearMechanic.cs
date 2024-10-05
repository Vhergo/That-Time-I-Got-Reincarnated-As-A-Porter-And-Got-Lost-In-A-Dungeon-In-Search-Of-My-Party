using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChamberClearMechanic : MonoBehaviour
{
    public static ChamberClearMechanic Instance { get; private set; }

    [SerializeField] private List<ChamberTentacle> chamberTentacles;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void OnEnable() => ChamberTentacle.OnChamberTentacleDestroyed += ChamberTentacleDestroyed;
    private void OnDestroy() => ChamberTentacle.OnChamberTentacleDestroyed -= ChamberTentacleDestroyed;

    private void Start()
    {
        InitializeChamberTentacles();
    }

    private void InitializeChamberTentacles()
    {
        chamberTentacles = new List<ChamberTentacle>(FindObjectsOfType<ChamberTentacle>(false));
    }

    private void ChamberTentacleDestroyed(ChamberTentacle tentacle)
    {
        chamberTentacles.Remove(tentacle);
        if (chamberTentacles.Count == 0) {
            // All chamber tentacles have been destroyed
            // Win condition met (add logic here)
            GameManager.Instance.DungeonCleared();
        }
    }

}
