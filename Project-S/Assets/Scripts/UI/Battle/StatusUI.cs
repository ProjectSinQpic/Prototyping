﻿using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class StatusUI : MonoBehaviour {

    public Text obj_HP;
    public Text obj_MP;
    public Text obj_attack;
    public Text obj_defense;
    public Text obj_rest;
    public Text turn;

    public GameObject statusBox;


    static StatusUI instance = null;

    void Awake () {
        if (instance == null) {
            instance = this;
        }
        statusBox.transform.localScale = Vector3.zero;
    }

    void Start () {
        GameState.turn
            .Subscribe (x => UpdateTurn (x));
    }

    void Update() {
        if(Input.GetMouseButtonDown(1) && MapPointer.instance.pointedKnight != null) {
            UpdateUI(MapPointer.instance.pointedKnight.GetComponent<KnightStatus>());
            statusBox.transform.localScale = Vector3.one;
        }
        if(Input.GetMouseButtonUp(1)) statusBox.transform.localScale = Vector3.zero;
    }

    public static StatusUI Instance () {
        return instance;
    }

    public void UpdateUI (KnightStatus status) {
        var statusData = KnightStatusData.Add(status.actual, status.delta);
        obj_HP.text = "HP : " + status.HP.ToString ();
        obj_MP.text = "MP : " + status.MP.ToString ();
        obj_attack.text = "攻撃力 : " + statusData.attack.ToString ();
        obj_defense.text = "防御力 : " + statusData.defense.ToString ();
        obj_rest.text = "レスト : " + status.coolDown.ToString ();
    }

    void UpdateTurn (Turn_State turnState) {
        if(turnState == Turn_State.blue) turn.text = "BLUE TURN";
        if(turnState == Turn_State.red) turn.text = "RED TURN";
        if(turnState == Turn_State.none) turn.text = "WAIT...";

    }



}