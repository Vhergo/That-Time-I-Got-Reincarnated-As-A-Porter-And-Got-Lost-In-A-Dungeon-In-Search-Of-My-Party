using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    protected string monsterName { get; set; }
    protected int monsterStrength { get; set; }

    public Monster()
    {
        monsterName = "Monster";
        monsterStrength = 5;
    }

    public Monster(string monster_name, int monsterStrength)
    {
        this.monsterName = monster_name;
        this.monsterStrength = monsterStrength;
    }

    public virtual void MonsterAttack()
    {
        FearManager.Instance.AddFear(monsterStrength);
    }

    public virtual void MonsterDie()
    {
        Destroy(this);
    }
}
