using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//特定の条件を満たすユニットに対して発動するスキル
public class AutoSelectSkill : ActiveSkill {

    public override void OnSelected() {
        owner.targets = GetTargets();
        OnWaitForSpell();
        owner.NextAction(KnightAction.skill_prepare);        
    }

    //スキルを発動する対象を決定する
    protected virtual List<KnightCore> GetTargets() { return new List<KnightCore>(); }

    //スキルを発動する直前
    protected virtual void OnWaitForSpell() { }


}
