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

    public static ReactiveProperty<Turn_State> turn;

    public static ReactiveProperty<KnightCore> selected;

    public static ReactiveProperty<Knight_State> knight_state;

    public Text clearUI;

    void Awake () {
        knight_state = new ReactiveProperty<Knight_State> (Knight_State.move);
        turn = new ReactiveProperty<Turn_State> (Turn_State.blue);
        selected = new ReactiveProperty<KnightCore> (null);
    }

    void Start () {

        this.UpdateAsObservable ()
            .Where(_ => turn.Value == Turn_State.blue)
            .Where (_ => KnightCore_Player01.player_all.Any (x => x.isFinished)
                || KnightCore_Player01.player_all.All (x => x.status.coolDown > 0))
            .Subscribe (_ => turn.Value = Turn_State.red);

        this.UpdateAsObservable ()
            .Where(_ => turn.Value == Turn_State.red)
            .Where (_ => KnightCore_Player02.player_all.Any (x => x.isFinished)
                || KnightCore_Player02.player_all.All (x => x.status.coolDown > 0))
            .Subscribe (_ => {
                turn.Value = Turn_State.none;
                StartCoroutine(WasteTimeCoroutine());
            });

        MapPointer.instance.OnClickedKnight
            .Where (_ => knight_state.Value == Knight_State.move)
            .Subscribe (o => selected.Value = o.GetComponent<KnightCore> ());

        turn.Subscribe (_ => knight_state.Value = Knight_State.move);

        this.UpdateAsObservable ()
            .Where (_ => KnightCore_Player01.player_all.All (x => x.isDead))
            .Subscribe (_ => clearUI.text = "RED WIN");


        this.UpdateAsObservable ()
            .Where (_ => KnightCore_Player02.player_all.All (x => x.isDead))
            .Subscribe (_ => clearUI.text = "BLUE WIN");


        SoundPlayer.instance.PlayBackGroundMusic(BackGroundMusic.battle01);
    }

    IEnumerator WasteTimeCoroutine() {
        KnightCore_Player01.player_all.ForEach(x => x.status.coolDown = Mathf.Max(0, x.status.coolDown - 1));
        KnightCore_Player02.player_all.ForEach(x => x.status.coolDown = Mathf.Max(0, x.status.coolDown - 1));
        yield return new WaitForSeconds(2f);
        turn.Value = Turn_State.blue;
    }

}