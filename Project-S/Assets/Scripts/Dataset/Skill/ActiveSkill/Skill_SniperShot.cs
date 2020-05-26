using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Skill/SniperShot")]
public class Skill_SniperShot : KnightSelectSkill {

    public int additionalRange;

    protected override void OnInit() {
        pos = owner.status.pos;
        value = owner.statusData.attackRange + additionalRange;
    }
    protected override void OnSpell() {
        Debug.Log("skill");
        owner.GetComponent<KnightAttack>().AttackInSkill(owner.targets[0]);
    }

}
