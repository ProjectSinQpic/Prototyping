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
            .Subscribe (_ => AttackPrepare (core.next_target));

        core.Message
            .Where (x => x == "attack_cancel")
            .Where (_ => !iscanceled)
            .Subscribe (_ => CancelAttack ());

    }

    void SelectOpponent () {
        disp.DisplayArea(AreaShapeType.attackable, core.status.pos, core.statusData.attackRange);

    }

    void AttackPrepare(KnightCore target) {
        if (!CheckAttackable (target)) {
            core.NextAction ("attack_cancel");
            return;
        }
        var damage = Mathf.Max (0, core.statusData.attack - target.statusData.defense);
        var result = new AttackResult(core, target, damage);

        AttackPrediction.instance.SetPredictionUI(result);
        MenuGenerator.Instance ().Create (new Dictionary<string, UnityEngine.Events.UnityAction> { 
            {"決定", () => {
                    SoundPlayer.instance.PlaySoundEffect("menu_select");
                    MenuGenerator.Instance().Close();
                    AttackPrediction.instance.HidePredictionUI();
                    Attack(result);
                }
            },
            {"キャンセル", () => {
                    SoundPlayer.instance.PlaySoundEffect("menu_cancel");
                    MenuGenerator.Instance().Close();
                    AttackPrediction.instance.HidePredictionUI();
                    core.NextAction ("attack_cancel");
                }
            }
        }, new Vector3 (0, -Screen.height / 2 + 200, 0), "skill_target", true);

    }

    void Attack (AttackResult result) {

        StartCoroutine (AttackCoroutine (result));
        core.storedCoolDown += 3;
    }

    public void AttackInSkill(KnightCore target) {
        if (!CheckAttackable (target)) {
            GetComponent<KnightSkill>().OnCancel();
            return;
        }
        var damage = Mathf.Max (0, core.statusData.attack - target.statusData.defense);
        var result = new AttackResult(core, target, damage);
        StartCoroutine (AttackCoroutine (result));
    }

    IEnumerator AttackCoroutine (AttackResult result) {
        var target = result.target;
        SoundPlayer.instance.PlaySoundEffect("attack01");
        view.ActionView ("attack", core.status.dir); //TODO 相手の方向を向くように修正したい
        target.status.HP -= result.damage;
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