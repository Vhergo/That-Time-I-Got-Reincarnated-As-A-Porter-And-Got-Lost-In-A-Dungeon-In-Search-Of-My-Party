using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public string monsterName;
    public float moveSpeed;
    [Range(0, 1)] public int fearFactor;
    private float attackRate;

    public Monster()
    {
        monsterName = "Monster";
        fearFactor = 1;
    }

    public Monster(string monster_name, int monsterStrength)
    {
        this.monsterName = monster_name;
        this.fearFactor = monsterStrength;
    }

    public virtual void MonsterAttack()
    {
        FearManager.Instance.AddFear(fearFactor);
    }

    public virtual void MonsterDie()
    {
        Destroy(gameObject);
    }
}
