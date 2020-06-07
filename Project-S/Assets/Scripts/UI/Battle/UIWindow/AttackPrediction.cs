﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class AttackPrediction : UIWindow {

    //とりあえず仮のUI表示を作る
    public GameObject ui;

    public Text attacker;
    public Text target;
    public Text damage;

    public static AttackPrediction instance;

    public void Start() {
        if (instance == null) instance = this;
    }

    public void SetPredictionUI(AttackResult result) {
        ui.transform.localScale = Vector3.one;
        attacker.text = "攻撃：" + result.attacker.status.data.name;
        target.text = "被攻撃：" + result.target.status.data.name;
        damage.text = "ダメージ：" + result.damage;
        Lock("attack_prediction");
    }

    public void HidePredictionUI() {
        ui.transform.localScale = Vector3.zero;
        UnLock("attack_prediction");
    }

}