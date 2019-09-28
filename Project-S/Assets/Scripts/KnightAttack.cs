using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

public class KnightAttack : KnightParts {

    public KnightView view;
    KnightDisplayArea _disp;


    void Awake() {
        _disp = core.GetComponent<KnightDisplayArea>();
    }


    void Start() {
        core.Message
            .Where(x => x == "attack_set")
            .Subscribe(_ => SelectOpponent());

        core.Message
            .Where(x => x == "attack")
            .Subscribe(_ => Attack(core.next_target));

        core.Message
            .Where(x => x == "attack_cancel")
            .Subscribe(_ => EndAttack());

    }

    void SelectOpponent() {
        GameState.knight_state = Knight_State.attack;
        _disp.DisplayAttackArea();
    }

    void Attack(KnightCore target) {
        if (!CheckAttackable(target)) {
            core.NextAction("attack_cancel");
            EndAttack();
            return;
        }
        DealDamage(target);
        if (core.status.HP <= 0) core.NextAction("die");
        else view.ActionView("attack", core.status.dir);
        if (target.status.HP <= 0) target.NextAction("die");
        else target.GetComponent<KnightView>().ActionView("attack", target.status.dir);
        StatusUI.Instance().UpdateUI(core.status);
        EndAttack();
        core.NextAction("finish");
    }

    void DealDamage(KnightCore target) {
        target.status.HP -= core.status.attack;
        core.status.HP -= target.status.attack;
        
        
    }

    void EndAttack() {
        _disp.RemoveArea();
    }

    bool CheckAttackable(KnightCore target) {
        if (tag == target.tag) return false;
        return _disp.attackableArea.Contains(target.status.pos);
    }
}