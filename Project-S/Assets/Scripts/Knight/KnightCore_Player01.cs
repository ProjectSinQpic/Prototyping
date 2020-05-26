using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class KnightCore_Player01 : KnightCore_Player {

    public static List<KnightCore> player_all = new List<KnightCore> ();

    protected override void Init () {
        base.Init();
        player_all.Add (this);

        GameState.turn
            .Where (x => x == Turn_State.blue)
            .Subscribe (_ => isFinished = false);
    }

    protected override bool isOperable () {
        return GameState.selected.Value == this && GameState.turn.Value == Turn_State.blue
            && status.coolDown == 0;
    }
}