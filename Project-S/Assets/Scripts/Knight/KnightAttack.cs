using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Linq;

public class AttackResult {

    public AttackResult(KnightCore attacker, KnightCore target, int damage, int mana, int rest) {
        this.attacker = attacker;
        this.target = target;
        this.damage = damage;
        this.mana = mana;
        this.rest = rest;    
    }

    public KnightCore attacker, target;

    public int damage, mana, rest;
}

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
            .Subscribe (_ => NormalAttack(core.attackResultPrediction));

        core.Message
            .Where (x => x == KnightAction.attack_cancel)
            .Where (_ => !iscanceled)
            .Subscribe (_ => CancelAttack ());

    }

    void NormalAttackPrepare(KnightCore target) {
        var damage = Mathf.Max (0, core.statusData.attack - target.statusData.defense);
        core.attackResultPrediction = new AttackResult(core, target, damage, 0, 3);
    }

    void NormalAttack (AttackResult result) {
        StartCoroutine (AttackCoroutine (result, false));
        core.storedCoolDown += 3;
    }

    IEnumerator AttackCoroutine (AttackResult result, bool isCounter) {
        var target = result.target;
        SoundPlayer.instance.PlaySoundEffect(SoundEffect.attack01);
        view.ActionView ("attack", core.status.dir); //TODO 相手の方向を向くように修正したい
        target.status.HP -= result.damage;
        yield return new WaitForSeconds (0.4f);
        if (target.status.HP <= 0) target.NextAction (KnightAction.die);
        yield return new WaitForSeconds (0.2f);
        core.NextAction(KnightAction.look_cancel);
        core.NextAction(KnightAction.finish);
    }

    void CancelAttack () {
        core.NextAction(KnightAction.look_cancel);
        core.NextAction (KnightAction.select);
    }

}