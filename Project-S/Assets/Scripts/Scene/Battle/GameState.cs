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
}

public class GameState : MonoBehaviour {

    public static ReactiveProperty<bool> isBlueTurn;

    public static ReactiveProperty<KnightCore> selected;

    public static ReactiveProperty<Knight_State> knight_state;

    public Text clearUI;

    void Awake () {
        knight_state = new ReactiveProperty<Knight_State> (Knight_State.move);
        isBlueTurn = new ReactiveProperty<bool> (true);
        selected = new ReactiveProperty<KnightCore> (null);
    }

    void Start () {

        this.UpdateAsObservable ()
            .Where (_ => KnightCore_Player01.player_all.Any (x => x.isFinished))
            .Subscribe (_ => isBlueTurn.Value = false);

        this.UpdateAsObservable ()
            .Where (_ => KnightCore_Player02.player_all.Any (x => x.isFinished))
            .Subscribe (_ => isBlueTurn.Value = true);

        MapPointer.instance.OnClickedKnight
            .Where (_ => knight_state.Value == Knight_State.move)
            .Subscribe (o => selected.Value = o.GetComponent<KnightCore> ());

        isBlueTurn.Subscribe (_ => knight_state.Value = Knight_State.move);

        this.UpdateAsObservable ()
            .Where (_ => KnightCore_Player01.player_all.All (x => x.isDead))
            .Subscribe (_ => clearUI.text = "RED WIN");


        this.UpdateAsObservable ()
            .Where (_ => KnightCore_Player02.player_all.All (x => x.isDead))
            .Subscribe (_ => clearUI.text = "BLUE WIN");
    }

}