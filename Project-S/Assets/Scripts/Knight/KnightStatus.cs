using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightStatus : MonoBehaviour {
    public KnightDatabase data;

    public Vector2 pos;
    public Direction dir;

    public int HP;
    public int MP;

    public int coolDown;
    public int level;
    public int SP;

    public KnightStatusData actual, delta;

    public void Init () {
        actual = new KnightStatusData();
        delta = new KnightStatusData();
        StatusCalculator calculator = new TRLinear_StatusCalculator (this);
        calculator.Calc ();
        HP = actual.maxHP;
        MP = actual.maxMP;
        GetComponent<KnightView> ().Init ();    //TODO 改善の余地あり
    }

}

[System.Serializable]
public class KnightStatusData {
    public int maxHP;
    public int maxMP;
    public int attack;
    public int defense;
    public int moveRange;
    public int attackRange;

    public static KnightStatusData Add(KnightStatusData a, KnightStatusData b) {
        KnightStatusData result = new KnightStatusData();
        result.maxHP = a.maxHP + b.maxHP;
        result.maxMP = a.maxMP + b.maxMP;
        result.attack = a.attack + b.attack;
        result.defense = a.defense + b.defense;
        result.moveRange = a.moveRange + b.moveRange;
        result.attackRange = a.attackRange + b.attackRange;
        return result;
    }

}