using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Skill/Active/SimpleAttack")]

/**


*/
public class Skill_SimpleAttack : KnightSelectSkill {


    public override int GetRequiredMana() {
        return -GetParam("attackerMP");
    }

    protected override void OnWaitForTarget() { }

    public override void OnTargetSelected(KnightCore target) {
        var targetHP = GetParam("targetHP");
        var targetMP = GetParam("targetMP");
        var targetRest = GetParam("targetRest");
        var attackerHP = GetParam("attackerHP");
        var attackerMP = GetParam("attackerMP");
        var attackerRest = GetParam("attackerRest");
        owner.attackResult.SetTarget(owner.targets[0]);
        owner.attackResult.AddValue(true, targetHP, targetMP, targetRest);
        owner.attackResult.AddValue(false, attackerHP, attackerMP, attackerRest);
    }

    public override void OnSpell() {
        Debug.Log("skill!!!");
    }

}
