using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System.Linq;
using UnityEngine.UI;


public class GameJudge : MonoBehaviour {

    public Text clearUI;

    void Start() {
        this.UpdateAsObservable()
            .Where(_ => KnightCore_Enemy.enemy_all.All(x => x.isDead))
            .Subscribe(_ => clearUI.text = "CLEAR");
    }

}
