using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

public class KnightAttack : KnightParts {

    public KnightView view;
    KnightDisplayArea _disp;
    bool iscanceled;


    void Awake() {
        _disp = core.GetComponent<KnightDisplayArea>();
        iscanceled = false;
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
            .Where(_ => !iscanceled)
            .Subscribe(_ => CancelAttack());

    }

    void SelectOpponent() {
        _disp.DisplayAttackArea();

    }

    void Attack(KnightCore target) {
        if (!CheckAttackable(target)) {
            core.NextAction("attack_cancel");
            return;
        }
        StartCoroutine(AttackCoroutine(target));
    }

    IEnumerator AttackCoroutine(KnightCore target) {
        view.ActionView("attack", core.status.dir); //TODO 相手の方向を向くように修正したい
        DealDamage(core, target);
        yield return new WaitForSeconds(0.4f);
        if (target.status.HP <= 0) target.NextAction("die");
        else yield return StartCoroutine(CounterAttackCoroutine(target));
        yield return new WaitForSeconds(0.2f);
        StatusUI.Instance().UpdateUI(core.status); //TODO 後にpull型にしたい
        _disp.RemoveArea();
        core.NextAction("finish");
    }

    IEnumerator CounterAttackCoroutine(KnightCore target) {
        target.GetComponent<KnightDisplayArea>().CalcAttackable();
        if (!target.GetComponent<KnightAttack>().CheckAttackable(core)) yield break;
        yield return new WaitForSeconds(0.2f);
        target.GetComponent<KnightView>().ActionView("attack", target.status.dir);
        DealDamage(target, core);
        yield return new WaitForSeconds(0.4f);
        if (core.status.HP <= 0) core.NextAction("die");
    }

    void DealDamage(KnightCore off, KnightCore def) {
        //TODO ダメージ計算のシステム考える
        def.status.HP -= off.status.attack;
    }

    void CancelAttack() {
        _disp.RemoveArea();
        core.NextAction("select");
    }

    public bool CheckAttackable(KnightCore target) {
        if (tag == target.tag) return false;
        return _disp.attackableArea.Contains(target.status.pos);
    }
}