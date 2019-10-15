using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;


public class KnightCore_Enemy : KnightCore {

    public static List<KnightCore> enemy_all = new List<KnightCore>();

    protected override void Init() {
        enemy_all.Add(this);
        GameState.isMyTurn
                 .Where(x => !x)
                 .Subscribe(_ => isFinished = false);

    }
}
