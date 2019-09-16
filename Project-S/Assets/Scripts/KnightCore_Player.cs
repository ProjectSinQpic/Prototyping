using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class KnightCore_Player : KnightCore {

    public static List<KnightCore> player_all = new List<KnightCore>();

    protected override void Init() {
        player_all.Add(this);
        MapPointer.instance.OnClickedMap
                   .Where(_ => isDead == false)
                   .Where(_ => isFinished == false)
                   .Where(_ => GameState.operating == this)
                   .Where(_ => GameState.isMyTurn.Value == true)
                   .Where(_ => GameState.knight_state == Knight_State.move)
                   .Subscribe(v => { next_pos = v; NextAction("move"); });

        MapPointer.instance.OnClickedKnight
                   .Where(o => o.GetComponent<KnightCore>().isDead == false)
                   .Where(_ => isFinished == false)
                   .Where(_ => GameState.operating == this)
                   .Where(_ => GameState.isMyTurn.Value == true)
                   .Where(_ => GameState.knight_state == Knight_State.attack)
                   .Subscribe(n => { next_target = n.GetComponent<KnightCore>(); NextAction("attack"); });

        MapPointer.instance.OnClickedMap
                   .Where(_ => isFinished == false)
                   .Where(_ => GameState.operating == this)
                   .Where(_ => GameState.isMyTurn.Value == true)
                   .Where(_ => GameState.knight_state == Knight_State.attack)
                   .Subscribe(n => { NextAction("attack_cancel"); });


        GameState.isMyTurn
            .Where(x => x)
            .Subscribe(_ => isFinished = false);
    }
}
