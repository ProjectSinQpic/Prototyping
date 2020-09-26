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

    }

    void OnSpell() {
        core.NextAction(KnightAction.attack);
        core.nowSkill.OnSpell();
    }

    public void OnCancel() {
        core.nowSkill = null;
        core.NextAction(KnightAction.look_cancel);
        core.NextAction(KnightAction.select);
        core.attackResult = new AttackResult(core);
    }

}

