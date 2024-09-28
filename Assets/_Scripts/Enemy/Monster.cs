using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public Archetype archetype;
    public float moveSpeed;
    [Range(0, 1)] public int fearFactor;
    private float attackRate;
    protected bool movingRight;

    public Monster()
    {
        fearFactor = 1;
    }

    public Monster(Archetype archetype, int fear)
    {
        this.archetype = archetype;
        this.fearFactor = fear;
    }

    public virtual void TurnAround()
    {
        movingRight = !movingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public virtual void MonsterAttack()
    {
        FearManager.Instance.AddFear(fearFactor);
    }

    [ContextMenu("Die")]
    public virtual void MonsterDie()
    {
        Destroy(gameObject);
    }
}
