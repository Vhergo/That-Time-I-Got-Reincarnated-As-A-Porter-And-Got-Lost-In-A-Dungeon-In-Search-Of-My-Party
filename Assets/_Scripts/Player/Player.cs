using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Player Health")]
    [SerializeField] private Transform healthBar;
    [SerializeField] private GameObject healthIconPrefab;
    [SerializeField] private AnimationClip healthLossAnimation;
    [SerializeField] private float respawnHealthAnimationDelay;
    [SerializeField] private List<Animator> healthIcons;

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
    [SerializeField] private int guideActivationLimit;
    private int guideActivationCount;
    private Coroutine guideCoroutine;

    public static Player Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void OnEnable()
    {
        PartyManager.OnPartyMemberFound += SetPlayerLightStage;
        FearManager.OnPlayerRespawn += PlayerRespawn;
    }
    private void OnDisable()
    {
        PartyManager.OnPartyMemberFound -= SetPlayerLightStage;
        FearManager.OnPlayerRespawn -= PlayerRespawn;
    }

    private void Start()
    {
        guideRenderer.enabled = false;
        SetPlayerLightStage(currentLightStage);

        SetupHealthBar();
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
    public void PlayGuide(GuideType guideType, KeyCode guideKey = KeyCode.None, bool alwaysShow = false)
    {
        if (!showGuide && !alwaysShow) return;

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

        guideActivationCount++;
        if (guideActivationCount >= guideActivationLimit) showGuide = false;
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

    private void SetupHealthBar()
    {
        foreach (Transform child in healthBar) {
            Destroy(child.gameObject);
        }

        int respawnCount = FearManager.Instance.GetRespawnCount();
        for (int i = 0; i < respawnCount; i++) {
            GameObject newHealthIcon = Instantiate(healthIconPrefab, healthBar);
            healthIcons.Add(newHealthIcon.GetComponent<Animator>());
        }

        healthBar.gameObject.SetActive(false);
    }

    private void PlayerRespawn()
    {
        StartCoroutine(RespawnAnimation());
    }

    private IEnumerator RespawnAnimation()
    {
        healthBar.gameObject.SetActive(true);

        yield return new WaitForSeconds(respawnHealthAnimationDelay);
        Animator lastHealthIcon = healthIcons[healthIcons.Count - 1];
        healthIcons.Remove(lastHealthIcon);

        lastHealthIcon.Play(healthLossAnimation.name);
        yield return new WaitForSeconds(healthLossAnimation.length);

        lastHealthIcon.GetComponent<Image>().enabled = false;
        yield return new WaitForSeconds(1f);

        healthBar.gameObject.SetActive(false);
        lastHealthIcon.transform.GetChild(0).GetComponent<Image>().enabled = true;
    }
}

[Serializable]
public class PlayerLightStage
{
    public string stageName;
    public int outerRadius;
}
