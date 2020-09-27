using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class KnightCore_Player02 : KnightCore_Player {

    public static List<KnightCore> player_all = new List<KnightCore> ();

    protected override void Init () {
        base.Init();
        player_all.Add (this);

        GameState.instance.turn
            .Where (x => x == Turn_State.red)
            .Subscribe (_ => isFinished = false);
    }

    protected override bool isOperable () {
        return GameState.instance.selected.Value == this && GameState.instance.turn.Value == Turn_State.red
            && status.coolDown == 0;
    }
}