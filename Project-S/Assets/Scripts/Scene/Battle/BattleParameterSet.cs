using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//戦闘全般のバランス調整に関わるパラーメータを保持するクラス
[CreateAssetMenu]
public class BattleParameterSet : ScriptableObject {
    public int moveRest;
    public int attackRest;
    public int turnMana;

    public DamageCalculator damage;

}
