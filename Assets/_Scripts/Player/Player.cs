using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Player : MonoBehaviour
{
    [Header("Player Light")]
    [SerializeField] private Light2D playerLight;
    [SerializeField] private List<PlayerLightStage> playerLightStages;
    private int currentLightStage = 0;

    [Header("Guide Settings")]
    [SerializeField] private float guideDuration = 5f;
    [SerializeField] private bool showGuide = true;
    [SerializeField] private Animator playerGuide;
    [SerializeField] private SpriteRenderer guideRenderer;
    [SerializeField] private AnimationClip inventoryGuide;
    [SerializeField] private AnimationClip interactGuide;
    [SerializeField] private KeyCode guideKey;
    private Coroutine guideCoroutine;

    public static Player Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void OnEnable() => PartyManager.OnPartyMemberFound += SetPlayerLightStage;
    private void OnDisable() => PartyManager.OnPartyMemberFound -= SetPlayerLightStage;

    private void Start()
    {
        guideRenderer.enabled = false;
    }

    private void Update()
    {
        if (guideKey != KeyCode.None && Input.GetKeyDown(guideKey)) {
            StopActiveGuide();
            guideKey = KeyCode.None;
        }
    }

    #region PARTY FINDER

    public void SetPlayerLightStage(int stage)
    {
        currentLightStage = stage;
        if (currentLightStage < playerLightStages.Count)
            playerLight.pointLightOuterRadius = playerLightStages[currentLightStage].outerRadius;

        currentLightStage++;
    }

    #endregion

    #region PLAYER GUIDE
    public void PlayGuide(GuideType guideType, KeyCode guideKey = KeyCode.None)
    {
        if (!showGuide) return;

        this.guideKey = guideKey;

        StopActiveGuide();

        switch (guideType) {
            case GuideType.Inventory:
                guideCoroutine = StartCoroutine(TriggerGuide(inventoryGuide));
                break;
            case GuideType.Interact:
                guideCoroutine = StartCoroutine(TriggerGuide(interactGuide));
                break;
        }
    }

    public IEnumerator TriggerGuide(AnimationClip guide)
    {
        guideRenderer.enabled = true;
        playerGuide.Play(guide.name);
        yield return new WaitForSeconds(guideDuration);

        guideRenderer.enabled = false;
        guideKey = KeyCode.None;
    }

    public void StopActiveGuide()
    {
        if (guideCoroutine != null) {
            StopCoroutine(guideCoroutine);
            guideRenderer.enabled = false;
        }
    }
    #endregion

    public void TakeDamage(float fearFactor)
    {
        FearManager.Instance.AddFear(fearFactor);
        // Potential hit animation trigger
    }
}

[Serializable]
public class PlayerLightStage
{
    public string stageName;
    public int outerRadius;
}
