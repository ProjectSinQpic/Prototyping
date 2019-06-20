using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusUI : MonoBehaviour
{

    public Text obj_attack;
    public Text obj_HP;

    static StatusUI instance = null;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    public static StatusUI Instance() {
        return instance;
    }

    public void UpdateUI(KnightStatus status) {
        obj_attack.text = "攻撃力 : " + status.attack.ToString();
        obj_HP.text = "HP : " + status.HP.ToString();
    }

}
