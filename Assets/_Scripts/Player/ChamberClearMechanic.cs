using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ChamberClearMechanic : MonoBehaviour
{
    public static ChamberClearMechanic Instance { get; private set; }

    [SerializeField] private List<ChamberTentacle> chamberTentacles;
    [SerializeField] private PlayableAsset chamberClearedCutscene;
    private bool dungeonCleared;

    public static Action OnChamberCleared;

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
            Debug.Log("Chamber Cleared!");
            CutsceneManager.Instance.PlayCutscene(chamberClearedCutscene);
            OnChamberCleared?.Invoke();
        }
    }

    public void FinalCutsceneOver()
    {
        GameManager.Instance.DungeonCleared();
    }

}
