using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TRLinear_StatusCalculator : StatusCalculator {

    /*  
     *  TR値に基づいてレベルに一次関数的に比例する計算式
     */

    const float TR_HP = 0.3f;
    const float TR_MP = 0.2f;
    const float TR_attack = 0.2f;
    const float TR_defense = 0.2f;

    public TRLinear_StatusCalculator (KnightStatus _status) : base (_status) { }

    public override void Calc () {
        status.HP = CalcStatus (status.data.maxHP, TR_HP, status.SP);
        status.MP = CalcStatus (status.data.maxMP, TR_MP, status.SP);
        status.attack = CalcStatus (status.data.attack, TR_attack, status.SP);
        status.defense = CalcStatus (status.data.defense, TR_defense, status.SP);
        status.moveRange = status.data.moveRange;
        status.attackRange = status.data.attackRange;
    }

    int CalcStatus (int _base, float _rate, int SP) {
        var x = _rate * _base + (1 - _rate) * _base * (status.level / 40f) + SP;
        return Mathf.RoundToInt (x);
    }

}