using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//特定の条件を満たすユニットに対して発動するスキル
public class AutoSelectSkill : ActiveSkill {

    protected override void OnWait() {
        OnTargeted(GetTargets());
    }

    //スキルを発動する対象を決定する
    protected virtual List<KnightCore> GetTargets() { return new List<KnightCore>(); }

}
