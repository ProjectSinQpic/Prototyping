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

        core.Message
            .Where (x => x == KnightAction.get_mana)
            .Subscribe (_ => OnGetFieldMana());

        GameState.instance.turn.Where(t => t != Turn_State.none).DelayFrame(1).Subscribe(_ => OnGetTurnMana());
    }

    void SkillPrepare() {

    }

    void OnSpell() {
        core.NextAction(KnightAction.attack);
        core.nowSkill.OnSpell();
    }

    void OnCancel() {
        core.nowSkill = null;
        core.NextAction(KnightAction.look_cancel);
        core.NextAction(KnightAction.select);
        core.attackResult = new AttackResult(core);
    }

    void OnGetFieldMana() {
        FieldManaPlacer.instance.GetMana(core);
        core.NextAction(KnightAction.finish);
    }

    void OnGetTurnMana() {
        core.status.MP = Mathf.Min(core.status.MP + 5, core.statusData.maxMP);
    }

}

