using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Damage/Percentage")]
public class Per_DamageCalculator : DamageCalculator {

    protected override int CalcPhysicalDamage(int defense, int damage) {
        float reducePer = 100f / (100f + defense);
        return (int)Mathf.Round(damage * reducePer);
    }
    protected override int CalcMagicDamage(int defense, int damage) {
        float reducePer = 100f / (100f + defense);
        return (int)Mathf.Round(damage * reducePer);
    }
}
