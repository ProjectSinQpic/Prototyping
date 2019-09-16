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

    public static KnightCore operating;

    public static Knight_State knight_state;

    void Awake() {
        knight_state = Knight_State.move;
        isMyTurn = new ReactiveProperty<bool>(true);
    }

    void Start() {

        this.UpdateAsObservable()
            .Where(_ => KnightCore_Player.player_all.Any(x => x.isFinished))
            .Subscribe(_ => isMyTurn.Value = false);

        this.UpdateAsObservable()
            .Where(_ => KnightCore_Enemy.enemy_all.Any(x => x.isFinished))
            .Subscribe(_ => isMyTurn.Value = true);

        isMyTurn
            .Subscribe(_ => knight_state = Knight_State.move);

    }

}
