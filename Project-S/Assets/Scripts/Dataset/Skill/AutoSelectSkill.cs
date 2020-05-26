using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//特定の条件を満たすユニットに対して発動するスキル
public class AutoSelectSkill : ActiveSkill {

    protected override void OnWait() {
        owner.targets = GetTargets();
        owner.NextAction(KnightAction.skill_prepare);        
    }

    //スキルを発動する対象を決定する
    protected virtual List<KnightCore> GetTargets() { return new List<KnightCore>(); }

}
