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
        var damage = GameState.instance.param.damage
            .CalcDamage(core.targets[0].statusData, DamageType.physical, core.statusData.attack);
        core.attackResult.SetTarget(target);
        core.attackResult.AddValue(true, -damage, 0, 0);
        core.attackResult.AddValue(false, 0, 0, GameState.instance.param.attackRest);
    }


    //AttackResultを反映させる
    void ApplyAttack(AttackResult result) {
        var diffs = new List<AttackResult.KnightDiff>(){result.GetAttacker(), result.GetTarget()};
        foreach(var d in diffs) {
            if(d.knight == null) continue;
            d.knight.status.ApplyStatus(d.hpDiff, d.mpDiff, d.restDiff);
            d.knight.status.ApplyBuffs(d.buffs);
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