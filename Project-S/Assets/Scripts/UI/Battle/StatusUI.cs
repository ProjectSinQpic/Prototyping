using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class StatusUI : MonoBehaviour
{

    public Text obj_attack;
    public Text obj_HP;
    public Text turn;

    static StatusUI instance = null;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    void Start() {
        GameState.isMyTurn
            .Subscribe(x => UpdateTurn(x));
    }

    public static StatusUI Instance() {
        return instance;
    }

    public void UpdateUI(KnightStatus status) {
        obj_attack.text = "攻撃力 : " + status.attack.ToString();
        obj_HP.text = "HP : " + status.HP.ToString();
    }

    void UpdateTurn(bool turnState) {
        turn.text = turnState ? "MY TURN" : "ENEMY TURN";
    }

}
