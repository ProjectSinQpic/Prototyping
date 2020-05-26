using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class KnightSkill : KnightParts {

    void Start() {
        core.Message
            .Where (x => x == KnightAction.skill)
            .Subscribe (_ => OnSpell());

        core.Message
            .Where (x => x == KnightAction.skill_cancel)
            .Subscribe (_ => OnCancel());
    }

    void OnSpell() {
        core.nowSkill.OnStart();
    }

    public void OnCancel() {
        core.nowSkill = null;
        core.NextAction(KnightAction.look_cancel);
        core.NextAction(KnightAction.select);
    }

}
