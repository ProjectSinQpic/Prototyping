using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System.Linq;
using System;

public enum Knight_State {
    move,
    select,
    attack,
}

public class GameState : MonoBehaviour {

    public static ReactiveProperty<bool> isMyTurn;

    public static ReactiveProperty<KnightCore> selected;

    public static ReactiveProperty<Knight_State> knight_state;

    void Awake() {
        knight_state = new ReactiveProperty<Knight_State>(Knight_State.move);
        isMyTurn = new ReactiveProperty<bool>(true);
        selected = new ReactiveProperty<KnightCore>(null);
    }

    void Start() {

        this.UpdateAsObservable()
            .Where(_ => KnightCore_Player.player_all.Any(x => x.isFinished))
            .Subscribe(_ => isMyTurn.Value = false);

        this.UpdateAsObservable()
            .Where(_ => KnightCore_Enemy.enemy_all.Any(x => x.isFinished))
            .Subscribe(_ => isMyTurn.Value = true);

        MapPointer.instance.OnClickedKnight
           .Where(_ => knight_state.Value == Knight_State.move)
           .Subscribe(o => selected.Value = o.GetComponent<KnightCore>());

        isMyTurn.Subscribe(_ => knight_state.Value = Knight_State.move);
    }

}
