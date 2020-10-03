using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class KnightCore_Enemy : KnightCore {

    protected override void Init () {
        base.Init();

        red_all.Add (this);
        
        GameState.instance.turn
            .Where (x => x == Turn_State.red)
            .Subscribe (_ => isFinished = false);

    }
}