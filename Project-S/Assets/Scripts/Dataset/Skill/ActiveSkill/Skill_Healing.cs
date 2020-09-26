using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Skill/Healing")]

public class Skill_Healing : AutoSelectSkill {

    public override int GetRequiredMana() {
        return GetParam("mana");
    }

    protected override List<KnightCore> GetTargets() {
        return KnightCore.GetAllies(owner);
    }

    protected override void OnWaitForSpell() {
        var mana = GetParam("mana");
        var rest = GetParam("rest");
        owner.attackResult.AddValue(false, 0, -mana, rest);
    }

    public override void OnSpell() {
        foreach(var target in owner.targets) {
            target.status.HP = Mathf.Min(target.statusData.maxHP, target.status.HP + GetParam("healingHelth"));
        }
        owner.NextAction(KnightAction.finish);
    }

}
