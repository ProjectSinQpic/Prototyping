﻿using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Linq;

public class KnightAttack : KnightParts {

    public KnightView view;
    KnightDisplayArea disp;
    bool iscanceled;

    void Awake () {
        disp = core.GetComponent<KnightDisplayArea> ();
        iscanceled = false;
    }

    void Start () {
        core.Message
            .Where (x => x == "attack_set")
            .Subscribe (_ => SelectOpponent ());

        core.Message
            .Where (x => x == "attack")
            .Subscribe (_ => Attack (core.next_target));

        core.Message
            .Where (x => x == "attack_cancel")
            .Where (_ => !iscanceled)
            .Subscribe (_ => CancelAttack ());

    }

    void SelectOpponent () {
        disp.DisplayArea(AreaShapeType.attackable, core.status.pos, core.statusData.attackRange);

    }

    void Attack (KnightCore target) {
        if (!CheckAttackable (target)) {
            core.NextAction ("attack_cancel");
            return;
        }
        StartCoroutine (AttackCoroutine (target));
        core.storedCoolDown += 3;
    }

    public void AttackInSkill(KnightCore target) {
        StartCoroutine (AttackCoroutine (target));
    }

    IEnumerator AttackCoroutine (KnightCore target) {
        SoundPlayer.instance.PlaySoundEffect("attack01");
        view.ActionView ("attack", core.status.dir); //TODO 相手の方向を向くように修正したい
        DealDamage (core, target);
        yield return new WaitForSeconds (0.4f);
        if (target.status.HP <= 0) target.NextAction ("die");
        //else yield return StartCoroutine (CounterAttackCoroutine (target));
        //yield return new WaitForSeconds (0.2f);
        disp.RemoveArea ();
        core.NextAction ("finish");
    }

    /*IEnumerator CounterAttackCoroutine (KnightCore target) {
        target.GetComponent<KnightDisplayArea> ().CalcAttackable ();
        if (!target.GetComponent<KnightAttack> ().CheckAttackable (core)) yield break;
        yield return new WaitForSeconds (0.2f);
        target.GetComponent<KnightView> ().ActionView ("attack", target.status.dir); //TODO 変更する
        DealDamage (target, core);
        yield return new WaitForSeconds (0.4f);
        if (core.status.HP <= 0) core.NextAction ("die");
    }*/

    void DealDamage (KnightCore off, KnightCore def) {
        //TODO ダメージ計算のシステム考える
        var damage = Mathf.Max (0, off.statusData.attack - def.statusData.defense);
        def.status.HP -= damage;
    }

    void CancelAttack () {
        disp.RemoveArea ();
        core.NextAction ("select");
    }

    public bool CheckAttackable (KnightCore target) {
        if (tag == target.tag) return false;
        var attackArea = disp.selectedArea.Where(s => s.type == AreaType.attack).Select(a => a.pos);
        return attackArea.Contains (target.status.pos);
    }
}