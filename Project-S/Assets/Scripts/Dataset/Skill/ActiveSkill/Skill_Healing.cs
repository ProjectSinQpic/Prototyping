using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Skill/Healing")]

public class Skill_Healing : AutoSelectSkill {

    protected override List<KnightCore> GetTargets() {
        return KnightCore.GetAllies(owner);
    }

    protected override void OnSpell() {
        foreach(var target in owner.targets) {
            target.status.HP = Mathf.Min(target.statusData.maxHP, target.status.HP + GetParam("healingHelth"));
        }
        owner.NextAction(KnightAction.finish);
    }

}
