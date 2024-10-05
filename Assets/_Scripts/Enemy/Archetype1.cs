using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Archetype1 : Monster
{
    [Space(10)]
    [SerializeField] private Animator swordAnim;
    [SerializeField] private bool startOnCeiling;
    private bool hasDropped;

    [Space(10)]
    [SerializeField] private List<Color> slimeColors = new List<Color>();

    [Space(10)]
    [SerializeField] private BoxCollider2D dropdownDetection;


    private Transform player;
    private bool canMove = true;


    protected override void Start()
    {
        base.Start();
        archetype = Archetype.Archetype1;
        player = Player.Instance.transform;

        SetRandomColor();
        RandomTurn();

        if (dropdownDetection != null) dropdownDetection.enabled = false;

        if (startOnCeiling) {
            if (dropdownDetection != null) dropdownDetection.enabled = true;

            rb.isKinematic = true;
            canMove = false;

            anim.SetBool("Idle", true);
            swordAnim.SetBool("Idle", true);
        }
    }

    private void RandomTurn()
    {
        if (UnityEngine.Random.Range(0, 1) > 0.5f) TurnAround();
    }

    private void FixedUpdate()
    {
        if (canMove) Move();
    }

    private void Move()
    { 
        if (!GroundAhead() || WallAhead() || LightAhead()) {
            TurnAround();
        }

        float moveDir = movingRight ? 1 : -1;
        rb.velocity = new Vector2(moveDir * moveSpeed, rb.velocity.y);
    }

    [ContextMenu("Drop Down")]
    public void TriggerDropDown() => StartCoroutine(DropDownSequence());

    private IEnumerator DropDownSequence()
    {
        hasDropped = true;
        rb.isKinematic = false;
        yield return new WaitForSeconds(0.1f);

        Flip();

        yield return new WaitUntil(() => GroundAhead());
        anim.SetBool("Idle", false);
        swordAnim.SetBool("Idle", false);
        canMove = true;
        if (dropdownDetection != null) dropdownDetection.enabled = false;
    }

    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.y *= -1;
        transform.localScale = scale;
    }

    private void SetRandomColor()
    {
        if (slimeColors.Count == 0) return;

        int randomIndex = UnityEngine.Random.Range(0, slimeColors.Count);
        GetComponent<SpriteRenderer>().color = slimeColors[randomIndex];
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

    private bool IsCollidingWithAnotherSlime(Collider2D other) => other.GetComponent<Archetype1>() != null;

    private bool IsGoingTowardsPlayer()
    {
        float playerDir = player.position.x - transform.position.x;
        return (playerDir > 0 && movingRight) || (playerDir < 0 && !movingRight);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && IsGoingTowardsPlayer()) {
            TurnAround();
        }

        Collider2D collider = collision.gameObject.GetComponent<Collider2D>();
        if (collision.gameObject.CompareTag("Monster") && IsCollidingWithAnotherSlime(collider)) {
            Physics2D.IgnoreCollision(collider, GetComponent<Collider2D>());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && startOnCeiling && !hasDropped) {
            TriggerDropDown();
        }

        if (collision.CompareTag("Monster") && IsCollidingWithAnotherSlime(collision)) {
            Physics2D.IgnoreCollision(collision, GetComponent<Collider2D>());
        }
    }
}
