using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Passive/EveryTurnBuff")]
public class Skill_EveryTurnBuff : PassiveSkill {

    public override void OnBeginTurn(Turn_State turn) {
        owner.status.ApplyStatus(GetParam("HP"), GetParam("MP"), GetParam("rest"));
    }

}
