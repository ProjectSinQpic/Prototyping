using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class AttackPredictionWindow : UIWindow {

    //とりあえず仮のUI表示を作る
    public GameObject ui;

    public Text text_nowHP_a, text_nowHP_b;
    public Text text_maxHP_a, text_maxHP_b;
    public GameObject bar_HP_a, bar_HP_b;
    public GameObject bar_applyHP_a, bar_applyHP_b;
    public Text text_nowMP_a, text_nowMP_b;
    public Text text_maxMP_a, text_maxMP_b;
    public GameObject bar_MP_a, bar_MP_b;
    public GameObject bar_applyMP_a, bar_applyMP_b;

    public Text text_attack_a, text_attack_b;
    public Text text_defense_a, text_defense_b;
    public Text text_moveRange_a, text_moveRange_b;
    public Text text_attackRange_a, text_attackRange_b;
    public Text text_rest_a, text_rest_b;


    public static AttackPredictionWindow instance;

    float bar_maxSize;


    public void Start() {
        if (instance == null) instance = this;
        bar_maxSize = bar_HP_a.GetComponent<RectTransform>().rect.width;
        ui.transform.localScale = Vector3.zero;
    }

    public void SetPredictionUI(AttackResult result) {
        ui.transform.localScale = Vector3.one;
        SetStatus(result.attacker, result.target, result);
        Lock("attack_prediction");
    }

    public void HidePredictionUI() {
        ui.transform.localScale = Vector3.zero;
        UnLock("attack_prediction");
    }

    void SetStatus(KnightCore core_a, KnightCore core_b, AttackResult result) {
        var statusData_a = KnightStatusData.Add(core_a.status.actual, core_a.status.delta);
        var statusData_b = KnightStatusData.Add(core_b.status.actual, core_b.status.delta);
        text_nowHP_a.text = core_a.status.HP.ToString () + " /";
        text_maxHP_a.text = statusData_a.maxHP.ToString ();
        SetBarWidth(bar_HP_a, core_a.status.HP, statusData_a.maxHP);
        text_nowMP_a.text = core_a.status.MP.ToString () + " /";
        text_maxMP_a.text = statusData_a.maxMP.ToString ();
        SetAppliedBarWidth(bar_MP_a, core_a.status.MP, statusData_a.maxMP, bar_applyMP_a, result.mana);
        text_attack_a.text = statusData_a.attack.ToString ();
        text_defense_a.text = statusData_a.defense.ToString ();
        text_moveRange_a.text = statusData_a.moveRange.ToString ();
        text_attackRange_a.text = statusData_a.attackRange.ToString ();
        text_rest_a.text =  core_a.status.coolDown.ToString () + " → " + result.rest;

        text_nowHP_b.text = core_b.status.HP.ToString () + " /";
        text_maxHP_b.text = statusData_b.maxHP.ToString ();
        SetAppliedBarWidth(bar_HP_b, core_b.status.HP, statusData_b.maxHP, bar_applyHP_b, result.damage);
        text_nowMP_b.text = core_b.status.MP.ToString () + " /";
        text_maxMP_b.text = statusData_b.maxMP.ToString ();
        SetBarWidth(bar_MP_b, core_b.status.MP, statusData_b.maxMP);
        text_attack_b.text = statusData_b.attack.ToString ();
        text_defense_b.text = statusData_b.defense.ToString ();
        text_moveRange_b.text = statusData_b.moveRange.ToString ();
        text_attackRange_b.text = statusData_b.attackRange.ToString ();
        text_rest_b.text =  core_b.status.coolDown.ToString ();
    }

    void SetBarWidth(GameObject bar, float now, float max) {
        var size = bar_maxSize * (now / max);
        bar.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
    }

    void SetAppliedBarWidth(GameObject bar, float now, float max, GameObject applied, float value) {
        var nowSize = bar_maxSize * (now / max);
        bar.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, nowSize);
        applied.GetComponent<RectTransform>().pivot = new Vector2(1, 0.5f); // TODO: 回復も考える
        var appliedSize = bar_maxSize * (Mathf.Abs(value) / max);
        applied.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, appliedSize);
    }

}
