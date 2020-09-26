using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Linq;



public class KnightAttack : KnightParts {

    public KnightView view;
    bool iscanceled;

    void Awake () {
        iscanceled = false;
    }

    void Start () {
        core.Message
            .Where (x => x == KnightAction.attack_prepare)
            .Subscribe (_ => NormalAttackPrepare (core.targets[0]));

        core.Message
            .Where (x => x == KnightAction.attack)
            .Subscribe (_ => ApplyAttack(core.attackResult));

        core.Message
            .Where (x => x == KnightAction.attack_cancel)
            .Where (_ => !iscanceled)
            .Subscribe (_ => CancelAttack ());

    }

    void NormalAttackPrepare(KnightCore target) {
        var damage = Mathf.Max (0, core.statusData.attack - target.statusData.defense);
        core.attackResult.SetTarget(target);
        core.attackResult.AddValue(true, -damage, 0, 0);
        core.attackResult.AddValue(false, 0, 0, 3);
    }


    //AttackResultを反映させる
    void ApplyAttack(AttackResult result) {
        var diffs = new List<AttackResult.KnightDiff>(){result.GetAttacker(), result.GetTarget()};
        foreach(var d in diffs) {
            if(d.knight == null) continue;
            d.knight.status.HP = Mathf.Clamp(d.knight.status.HP + d.hpDiff, 0, d.knight.statusData.maxHP);
            d.knight.status.MP = Mathf.Clamp(d.knight.status.MP + d.mpDiff, 0, d.knight.statusData.maxMP);
            d.knight.storedCoolDown = Mathf.Max(d.knight.storedCoolDown + d.restDiff, 0);
        }
        StartCoroutine (AttackCoroutine (diffs[0].knight, diffs[1].knight)); //TODO: あとで消す
    }


    //TODO: 攻撃アニメーション　あとで分離
    IEnumerator AttackCoroutine (KnightCore attcacker, KnightCore target) {
        SoundPlayer.instance.PlaySoundEffect(SoundEffect.attack01);
        view.ActionView ("attack", core.status.dir); //TODO 相手の方向を向くように修正したい
        yield return new WaitForSeconds (0.4f);
        if (target != null && target.status.HP <= 0) target.NextAction(KnightAction.die);
        yield return new WaitForSeconds (0.2f);
        core.NextAction(KnightAction.look_cancel);
        core.NextAction(KnightAction.finish);
    }

    void CancelAttack () {
        core.NextAction(KnightAction.look_cancel);
        core.NextAction (KnightAction.select);
        core.attackResult = new AttackResult(core);
    }

}