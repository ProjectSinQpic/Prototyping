using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Skill/Healing")]

public class Skill_Healing : AutoSelectSkill {

    public int healingHelth;

    protected override List<KnightCore> GetTargets() {
        return KnightCore.GetAllies(owner);
    }

    protected override void OnSpell() {
        foreach(var target in targets) {
            target.status.HP = Mathf.Min(target.statusData.maxHP, target.status.HP + healingHelth);
        }
        owner.NextAction("finish");
    }

}
