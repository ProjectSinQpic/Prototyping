using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class StatusWindow : UIWindow {

    public Text obj_HP;
    public Text obj_MP;
    public Text obj_attack;
    public Text obj_defense;
    public Text obj_rest;
    public Text turn;

    public GameObject statusBox;

    static StatusWindow instance = null;

    public ReactiveProperty<KnightCore> target = new ReactiveProperty<KnightCore>(null);

    void Awake () {
        if (instance == null) instance = this;
        statusBox.transform.localScale = Vector3.zero;
    }

    void Start () {
        GameState.instance.turn
            .Subscribe (x => UpdateTurn (x));

        MapPointer.instance.OnPressedRightButton
            .Subscribe(x => {
                if(x.Item2) OpenWindow(x.Item1);
                else HideWindow();
            });
    }

    void OpenWindow(GameObject obj) {
        if(ViewOperater.instance.isFocusing) return;
        if(ViewOperater.instance.isLocked) return;
        if(MapPointer.instance.pointedKnight == null ) return;
        target.Value = obj.GetComponent<KnightCore>();
        UpdateUI(target.Value.status);
        statusBox.transform.localScale = Vector3.one;
        MapPointer.instance.SetActive(false, true);
    }

    void HideWindow(){
        if(ViewOperater.instance.isFocusing) return;
        if(!ViewOperater.instance.isLocked) return;
        target.Value = null;
        statusBox.transform.localScale = Vector3.zero;
        MapPointer.instance.SetActive(true, true);
    }

    public static StatusWindow Instance () {
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