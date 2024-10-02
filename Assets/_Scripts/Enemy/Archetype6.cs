using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Archetype6 : Monster
{
    [SerializeField] private bool canMove;
    [SerializeField] private float attackDelay;
    [SerializeField] private float chaseDelay;

    private Transform player;
    private bool canChase = true;

    [Header("Animation")]
    [SerializeField] private AnimationClip idleAnimation;
    [SerializeField] private AnimationClip walkAnimation;
    [SerializeField] private AnimationClip attackAnimation;

    [Header("Highlight")]
    [SerializeField] private SpriteRenderer slimeRenderer;
    [SerializeField] private Light2D lightRenderer;
    private Sprite lastSprite;

    protected override void Start()
    {
        base.Start();
        archetype = Archetype.Archetype6;
        player = Player.Instance.transform;


        slimeRenderer = GetComponent<SpriteRenderer>();
        lightRenderer.enabled = false;
        SetRendereres();
    }

    private void Update()
    {
        if (lastSprite != slimeRenderer.sprite) {
            SetRendereres();
        }
    }

    private void FixedUpdate()
    {
        if (canMove) Move();
        if (!GroundAhead()) rb.velocity += Vector2.down * 2f;
    }

    private void Move()
    {
        if (WallAhead() || LightAhead()) {
            TurnAround();
        }

        float moveDir = movingRight ? 1 : -1;
        rb.velocity = new Vector2(moveDir * moveSpeed, rb.velocity.y);
    }

    //private void FacePlayer()
    //{
    //    if (player.position.x < transform.position.x && movingRight)  TurnAround();
    //    else if (player.position.x > transform.position.x && !movingRight) TurnAround();
    //}
    private void SetRendereres()
    {
        lightRenderer.lightCookieSprite = slimeRenderer.sprite;
        lastSprite = slimeRenderer.sprite;
    }

    private void TriggerAttack()
    {
        if (!canChase) StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        canMove = false;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(attackDelay);
        anim.Play(attackAnimation.name);

        yield return null;
        yield return new WaitForSeconds(anim.GetCurrentAnimatorClipInfo(0).Length);
        anim.Play(idleAnimation.name);

        yield return new WaitForSeconds(attackCooldown);
        anim.Play(walkAnimation.name);
        canMove = true;
    }

    public void TriggerChase()
    {
        if (canChase) {
            StartCoroutine(StartChase());
            canChase = false;
        }
    }

    private IEnumerator StartChase()
    {
        lightRenderer.enabled = true;

        yield return new WaitForSeconds(chaseDelay);
        anim.Play(walkAnimation.name);
        canMove = true;
    }

    private bool GroundAhead() => Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    private bool WallAhead() => Physics2D.OverlapCircle(wallCheck.position, 0.2f, groundLayer);
    private bool LightAhead() => Physics2D.OverlapCircle(wallCheck.position, 0.2f, lightLayer);

    public override void MonsterDie()
    {
        base.MonsterDie();
        Debug.Log(archetype + " has been destroyed!");

        GameObject droppedLoot = LootManager.Instance.GetLoot(archetype);
        if (droppedLoot != null)
            Instantiate(droppedLoot, transform.position, Quaternion.identity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            TriggerAttack();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            if (transform.position.y < collision.gameObject.transform.position.y)
                TriggerChase();
        }
    }
}