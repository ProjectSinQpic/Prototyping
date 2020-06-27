using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Linq;

public class AttackResult {

    public AttackResult(KnightCore attacker, KnightCore target, int damage) {
        this.attacker = attacker;
        this.target = target;
        this.damage = damage;
    }
    public KnightCore attacker, target;

    public int damage;
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
            .Subscribe (_ => AttackPrepare (core.targets[0]));

        core.Message
            .Where (x => x == KnightAction.attack)
            .Subscribe (_ => Attack(core.attackResult));

        core.Message
            .Where (x => x == KnightAction.skill_attack)
            .Subscribe (_ => AttackInSkill(core.targets[0]));

        core.Message
            .Where (x => x == KnightAction.counter_attack)
            .Subscribe (_ => CounterAttack(core.attackResult));

        core.Message
            .Where (x => x == KnightAction.attack_cancel)
            .Where (_ => !iscanceled)
            .Subscribe (_ => CancelAttack ());

    }

    void AttackPrepare(KnightCore target) {
        var damage = Mathf.Max (0, core.statusData.attack - target.statusData.defense);
        core.attackResult = new AttackResult(core, target, damage);
    }

    void Attack (AttackResult result) {
        StartCoroutine (AttackCoroutine (result, false));
        core.storedCoolDown += 3;
    }

    public void AttackInSkill(KnightCore target) {
        var damage = Mathf.Max (0, core.skillDamage);
        var result = new AttackResult(core, target, damage);
        StartCoroutine (AttackCoroutine (result, false));
    }

    void CounterAttack(AttackResult result) { //TODO: あとで作る
        Debug.Log("counter : " + result.target.name);
    }

    IEnumerator AttackCoroutine (AttackResult result, bool isCounter) {
        var target = result.target;
        SoundPlayer.instance.PlaySoundEffect(SoundEffect.attack01);
        view.ActionView ("attack", core.status.dir); //TODO 相手の方向を向くように修正したい
        target.status.HP -= result.damage;
        yield return new WaitForSeconds (0.4f);
        if (target.status.HP <= 0) target.NextAction (KnightAction.die);
        else if(!isCounter) {
            target.attackResult = result;
            target.NextAction(KnightAction.counter_attack);
        }
        yield return new WaitForSeconds (0.2f);
        core.NextAction(KnightAction.look_cancel);
        core.NextAction(KnightAction.finish);
    }

    void CancelAttack () {
        core.NextAction(KnightAction.look_cancel);
        core.NextAction (KnightAction.select);
    }


}