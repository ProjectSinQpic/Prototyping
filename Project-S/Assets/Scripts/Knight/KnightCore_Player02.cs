using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class KnightCore_Player02 : KnightCore {

    public static List<KnightCore> player_all = new List<KnightCore> ();

    protected override void Init () {
        player_all.Add (this);

        MapPointer.instance.OnClickedMap
            .Where (_ => isOperable ())
            .Where (_ => GameState.knight_state.Value == Knight_State.move)
            .Subscribe (v => { next_pos = v; NextAction ("move"); });

        MapPointer.instance.OnClickedKnight
            .Where (_ => isOperable ())
            .Where (_ => GameState.knight_state.Value == Knight_State.attack)
            .Subscribe (n => { next_target = n.GetComponent<KnightCore> (); NextAction ("attack"); });
        MapPointer.instance.OnClickedMap
            .Where (_ => isOperable ())
            .Where (_ => GameState.knight_state.Value == Knight_State.attack)
            .Subscribe (n => NextAction ("attack_cancel"));


        MapPointer.instance.OnClickedKnight
            .Where (_ => isOperable ())
            .Where (_ => GameState.knight_state.Value == Knight_State.skill_knight)
            .Subscribe (n => GetComponent<KnightSkill>().OnTargeted(new List<KnightCore>(){n.GetComponent<KnightCore>()}));
        MapPointer.instance.OnClickedMap
            .Where (_ => isOperable ())
            .Where (_ => GameState.knight_state.Value == Knight_State.skill_knight)
            .Subscribe (n => GetComponent<KnightSkill>().OnCancel());

        Message.Where (x => x == "select")
            .Subscribe (_ => KnightActionMenu.instance.DisplayMenu (this));

        Message.Where (x => x == "finish")
            .Subscribe (_ => GameState.selected.Value = null);

        GameState.turn
            .Where (x => x == Turn_State.red)
            .Subscribe (_ => isFinished = false);
    }

    protected override bool isOperable () {
        return GameState.selected.Value == this && GameState.turn.Value == Turn_State.red
            && status.coolDown == 0;
    }
}