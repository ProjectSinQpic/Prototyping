using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class KnightSkill : KnightParts {

    void Start() {
        core.Message
            .Where (x => x == KnightAction.skill_prepare)
            .Subscribe (_ => SkillPrepare());

        core.Message
            .Where (x => x == KnightAction.skill)
            .Subscribe (_ => OnSpell());

        core.Message
            .Where (x => x == KnightAction.skill_cancel)
            .Subscribe (_ => OnCancel());
    }

    void SkillPrepare() {
        var predictDamage = core.nowSkill.GetParam("damage");
        var predictMana = core.nowSkill.GetParam("mana");
        var predictRest = core.nowSkill.GetParam("rest");
        core.attackResult.SetTarget(core.targets[0]);
        core.attackResult.AddValue(true, -predictDamage, -predictMana, -predictRest); //TODO: 符号を合わせる
    }

    void OnSpell() {
        core.NextAction(KnightAction.attack);
        core.nowSkill.OnStart();
    }

    public void OnCancel() {
        core.nowSkill = null;
        core.NextAction(KnightAction.look_cancel);
        core.NextAction(KnightAction.select);
    }

}

