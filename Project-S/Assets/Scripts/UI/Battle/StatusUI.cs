using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class StatusUI : MonoBehaviour {

    public Text obj_HP;
    public Text obj_MP;
    public Text obj_attack;
    public Text obj_defense;
    public Text turn;

    static StatusUI instance = null;

    void Awake () {
        if (instance == null) {
            instance = this;
        }
    }

    void Start () {
        GameState.isBlueTurn
            .Subscribe (x => UpdateTurn (x));
    }

    public static StatusUI Instance () {
        return instance;
    }

    public void UpdateUI (KnightStatus status) {
        obj_HP.text = "HP : " + status.HP.ToString ();
        obj_MP.text = "MP : " + status.MP.ToString ();
        obj_attack.text = "攻撃力 : " + status.attack.ToString ();
        obj_defense.text = "防御力 : " + status.defense.ToString ();
    }

    void UpdateTurn (bool turnState) {
        turn.text = turnState ? "BLUE TURN" : "RED TURN";
    }

}