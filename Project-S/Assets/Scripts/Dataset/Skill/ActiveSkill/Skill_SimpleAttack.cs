using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Active/SimpleAttack")]
public class Skill_SimpleAttack : KnightSelectSkill {

    public DamageType damageType;
    public StatusBuff attackerBuff, targetBuff;

    public override int GetRequiredMana() {
        return -GetParam("mana");
    }

    protected override void OnWaitForTarget() { }

    public override void OnTargetSelected(KnightCore target) {
        var power = GetParam("power");
        var mana = GetParam("mana");
        var damage = GameState.instance.param.damage.CalcDamage(owner.targets[0].statusData, damageType, power);
        owner.attackResult.SetTarget(owner.targets[0]);
        owner.attackResult.AddValue(true, -damage, 0, 0);
        owner.attackResult.AddValue(false, 0, -mana, 0);
        if(attackerBuff != null) owner.attackResult.AddBuff(false, attackerBuff);
        if(targetBuff != null) owner.attackResult.AddBuff(true, targetBuff);
    }

    public override void OnSpell() {
        Debug.Log("skill!!!");
    }
}
