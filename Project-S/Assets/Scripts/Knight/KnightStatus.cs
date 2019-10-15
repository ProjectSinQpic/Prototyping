using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightStatus : MonoBehaviour
{
    public KnightDatabase data;

    public Vector2 pos;
    public Direction dir;



    [HideInInspector] public int HP;
    [HideInInspector] public int MP;
    [HideInInspector] public int attack;
    [HideInInspector] public int defense;
    [HideInInspector] public int moveRange;
    [HideInInspector] public int attackRange;

    public void Init() {
        HP = data.maxHP;
        MP = data.maxMP;
        attack = data.attack;
        defense = data.defense;
        moveRange = data.moveRange;
        attackRange = data.attackRange;
        GetComponent<KnightView>().Init();
    }

}
