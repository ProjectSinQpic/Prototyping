using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightActionWindow : UIWindow {

    public static KnightActionWindow instance;


    void Awake () {
        if (instance == null) instance = this;
    }

    public void DisplayMenu (KnightCore core) {
        GenericWindow.instance.Create (new Dictionary<string, UnityEngine.Events.UnityAction> { 
            { "攻撃", () => OnAttack (core) },
            { "スキル", () => OnSkill (core) },
            { "待機", () => OnWait (core) },
            { "キャンセル", () => OnCancel (core) },
        }, new Vector3 (Screen.width / 2 - 250, Screen.height / 2 - 250, 0), "knight_choice", true);
        GameState.knight_state.Value = Knight_State.select;
    }

    void OnAttack (KnightCore core) {
        SoundPlayer.instance.PlaySoundEffect(SoundEffect.menu_select);
        core.NextAction (KnightAction.attack_look);
        GameState.knight_state.Value = Knight_State.attack;
        GenericWindow.instance.Close ();
    }

    void OnSkill (KnightCore core) {
        SkillSelectWindow.instance.Open(core);
    }

    void OnWait (KnightCore core) {
        SoundPlayer.instance.PlaySoundEffect(SoundEffect.menu_select);
        core.NextAction (KnightAction.finish);
        GameState.knight_state.Value = Knight_State.move;
        GenericWindow.instance.Close ();
    }

    void OnCancel (KnightCore core) {
        SoundPlayer.instance.PlaySoundEffect(SoundEffect.menu_cancel);
        core.NextAction(KnightAction.select_cancel);
        GenericWindow.instance.Close ();
        GameState.knight_state.Value = Knight_State.move;
    }
}