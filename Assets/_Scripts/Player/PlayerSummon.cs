using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerSummon : MonoBehaviour
{
    [SerializeField] private SpriteRenderer player;
    [SerializeField] private SpriteRenderer summonSprite;
    [SerializeField] private AnimationClip summonAnimation;
    [SerializeField] private Light2D summonLight;
    [SerializeField] private float summonDelay = 2f;
    private Animator anim;
    private Sprite lastSprite;
    private bool summonStarted;

    private void Start()
    {
        anim = GetComponent<Animator>();
        summonSprite = GetComponent<SpriteRenderer>();
        summonLight = GetComponent<Light2D>();
        player.enabled = false;

        summonSprite.enabled = false;
        summonLight.enabled = false;
    }

    public void Update()
    {
        if (!summonStarted) return;
        if (lastSprite != summonSprite.sprite) SetRenereres();
    }

    private void SetRenereres()
    {
        summonLight.lightCookieSprite = summonSprite.sprite;
        lastSprite = summonSprite.sprite;
    }

    public void TriggerSummonSequence() => StartCoroutine(PlaySummonSequence());
    private IEnumerator PlaySummonSequence()
    {
        yield return new WaitForSeconds(summonDelay);
        summonSprite.enabled = true;
        summonLight.enabled = true;
        anim.Play(summonAnimation.name);
        summonStarted = true;
        SetRenereres();

        yield return new WaitForSeconds(summonAnimation.length);
        Destroy(gameObject);
    }

    public void SpawnPlayer()
    {
        player.enabled = true;
        FearManager.Instance.SetSpawned();
    }
}
