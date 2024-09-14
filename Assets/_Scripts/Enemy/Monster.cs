using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster:MonoBehaviour
{
    protected string monster_name { get; set; }
    protected int monster_strength { get; set; }
    public Monster()
    {
        monster_name = "Monster";
        monster_strength = 5;
    }

    public Monster(string monster_name, int monster_strength)
    {
        this.monster_name = monster_name;
        this.monster_strength = monster_strength;
    }

    public virtual void monster_attack()
    {
        FearScript.instance.addFear(monster_strength);
    }

    public virtual void monster_die()
    {
        Destroy(this);
    }
}
