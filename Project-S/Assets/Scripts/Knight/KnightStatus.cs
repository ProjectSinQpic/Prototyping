using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightStatus : MonoBehaviour {
    public KnightDatabase data;

    public Vector2 pos;
    public Direction dir;

    public int level;

    public int SP;

    [HideInInspector] public int HP;
    [HideInInspector] public int MP;
    [HideInInspector] public int attack;
    [HideInInspector] public int defense;
    [HideInInspector] public int moveRange;
    [HideInInspector] public int attackRange;

    [HideInInspector] public int coolDown;



    public void Init () {
        StatusCalculator calculator = new TRLinear_StatusCalculator (this);
        calculator.Calc ();
        GetComponent<KnightView> ().Init ();    //TODO 改善の余地あり
    }

}