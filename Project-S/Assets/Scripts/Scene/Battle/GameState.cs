using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

public enum Knight_State {
    move,
    select,
    attack,
    skill_knight, //ユニット指定のスキル
    skill_area,  //地点指定のスキル
}

public enum Turn_State {
    blue,
    red,
    none
}

public class GameState : MonoBehaviour {

    public ReactiveProperty<Turn_State> turn;

    public ReactiveProperty<KnightCore> selected;

    public ReactiveProperty<Knight_State> knight_state;

    public Text clearUI;

    public static GameState instance;

    public BattleParameterSet param;

    public Turn_State firstTurn;

    void Awake () {
        if (instance == null) instance = this;
        knight_state = new ReactiveProperty<Knight_State> (Knight_State.move);
        turn = new ReactiveProperty<Turn_State> (Turn_State.none);
        selected = new ReactiveProperty<KnightCore> (null);
    }

    void Start () {
        Turn_State secondTurn = firstTurn == Turn_State.blue ? Turn_State.red : Turn_State.blue;
        var firstTeam = firstTurn == Turn_State.blue ? KnightCore.blue_all : KnightCore.red_all;
        var secondTeam = firstTurn == Turn_State.red ? KnightCore.blue_all : KnightCore.red_all;

        //ターン制御
        this.UpdateAsObservable ()
            .Where(_ => turn.Value == firstTurn)
            .Where (_ => firstTeam.Any (x => x.isFinished)
                || firstTeam.Where(x => !x.isDead).All (x => x.status.coolDown > 0))
            .Subscribe (_ => turn.Value = secondTurn);

        this.UpdateAsObservable ()
            .Where(_ => turn.Value == secondTurn)
            .Where (_ => secondTeam.Any (x => x.isFinished)
                || secondTeam.Where(x => !x.isDead).All (x => x.status.coolDown > 0))
            .Subscribe (_ => {
                turn.Value = Turn_State.none;
                StartCoroutine(WasteTimeCoroutine());
            });
        ////

        MapPointer.instance.OnClickedKnight
            .Where (_ => knight_state.Value == Knight_State.move)
            .Subscribe (o => selected.Value = o.GetComponent<KnightCore> ());

        NetworkCommunicater.instance.message
            .Where(c => c[0] == "knight")
            .Where (_ => knight_state.Value == Knight_State.move)
            .Subscribe (c => {
                var pos = new Vector2(int.Parse(c[1]), int.Parse(c[2]));
                var core = KnightCore.GetKnightFromPos(pos);
                if(core != null) {
                    selected.Value = core;
                }
            });

        turn.Subscribe (_ => knight_state.Value = Knight_State.move);

        this.UpdateAsObservable ()
            .Where (_ => KnightCore.blue_all.All (x => x.isDead))
            .Subscribe (_ => clearUI.text = "RED WIN");

        this.UpdateAsObservable ()
            .Where (_ => KnightCore.red_all.All (x => x.isDead))
            .Subscribe (_ => clearUI.text = "BLUE WIN");


        SoundPlayer.instance.PlayBackGroundMusic(BackGroundMusic.battle01);
    }

    IEnumerator WasteTimeCoroutine() {
        MapPointer.instance.SetActive(false, false);
        ViewOperater.instance.SetActive(false);
        KnightCore.blue_all.Where(x => !x.isDead).ToList().ForEach(x => x.status.coolDown = Mathf.Max(0, x.status.coolDown - 1));
        KnightCore.red_all.Where(x => !x.isDead).ToList().ForEach(x => x.status.coolDown = Mathf.Max(0, x.status.coolDown - 1));
        yield return new WaitForSeconds(2f);
        turn.Value = firstTurn;
        MapPointer.instance.SetActive(true, true);
        ViewOperater.instance.SetActive(true);
    }

    public void ResetState() {
        selected.Value = null;
        knight_state.Value = Knight_State.move;
    }

    public void StartGame(Turn_State yourTurn) {
        firstTurn = yourTurn;
        turn.Value = yourTurn;
    }

}