using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    [SerializeField] private float guideDuration = 5f;
    [SerializeField] private bool showGuide = true;
    [SerializeField] private Animator playerGuide;
    [SerializeField] private SpriteRenderer guideRenderer;
    [SerializeField] private AnimationClip inventoryGuide;
    [SerializeField] private AnimationClip interactGuide;
    [SerializeField] private KeyCode guideKey;
    private Coroutine guideCoroutine;

    [Space(10)]
    [SerializeField] private float damageCooldown = 4f;
    private bool canTakeDamage = true;

    public static Player Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

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

    private void StopActiveGuide()
    {
        if (guideCoroutine != null) {
            StopCoroutine(guideCoroutine);
            guideRenderer.enabled = false;
        }
    }

    public void StopGuide()
    {

    }

    private IEnumerator TakeDamageCooldown()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canTakeDamage = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Monster") && canTakeDamage) {
            Monster monster = collision.gameObject.GetComponent<Monster>();
            monster.MonsterAttack();
            StartCoroutine(TakeDamageCooldown());
        }
    }
}
