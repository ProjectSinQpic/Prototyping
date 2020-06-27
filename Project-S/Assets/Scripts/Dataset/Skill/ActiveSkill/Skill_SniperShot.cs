using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Skill/SniperShot")]
public class Skill_SniperShot : KnightSelectSkill {

    protected override void OnInit() {
        areaCenterPos = owner.status.pos;
        AddParam("areaRange", owner.statusData.attackRange + GetParam("additionalRange"));
    }
    protected override void OnSpell() {
        owner.skillDamage = owner.statusData.attack;
        owner.NextAction(KnightAction.skill_attack);
    }

}
