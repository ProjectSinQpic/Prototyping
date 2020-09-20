using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Skill/AttackForOne")]

/**


*/
public class Skill_AttackForOne : KnightSelectSkill {


    protected override void OnInit() {
        areaCenterPos = owner.status.pos;
        AddParam("areaRange", owner.statusData.attackRange + GetParam("additionalRange"));
    }
    protected override void OnSpell() {
        Debug.Log("skill!!!");
        
    }

}
