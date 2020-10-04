using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType {
    physical,
    magic,
    direct
}

public class DamageCalculator : ScriptableObject {

    public int CalcDamage(KnightStatusData status, DamageType type, int damage) {
        if(type == DamageType.direct) return damage;
        else if(type == DamageType.physical) return CalcPhysicalDamage(status.defense, damage);
        else if(type == DamageType.magic) return CalcMagicDamage(status.skillDefense, damage);
        else return 0;
    }

    protected virtual int CalcPhysicalDamage(int defense, int damage) { return 0; }
    protected virtual int CalcMagicDamage(int defense, int damage) { return 0; }

}
