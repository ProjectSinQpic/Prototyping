using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightActionMenu : MonoBehaviour {

    public static KnightActionMenu instance { get { return _instance; } }
    static KnightActionMenu _instance = null;


    void Awake () {
        if (_instance == null) _instance = this;
    }

    public void DisplayMenu (KnightCore core) {
        MenuGenerator.Instance ().Create (new Dictionary<string, UnityEngine.Events.UnityAction> { 
            { "攻撃", () => OnAttack (core) },
            { "スキル", () => OnSkill (core) },
            { "待機", () => OnWait (core) },
            { "キャンセル", () => OnCancel (core) },
        }, new Vector3 (Screen.width / 2 - 250, Screen.height / 2 - 250, 0), "knight_choice", true);
        GameState.knight_state.Value = Knight_State.select;
    }

    void OnAttack (KnightCore core) {
        SoundPlayer.instance.PlaySoundEffect("menu_select");
        core.NextAction (KnightAction.attack_look);
        GameState.knight_state.Value = Knight_State.attack;
        MenuGenerator.Instance ().Close ();
    }

    void OnSkill (KnightCore core) { //TODO: あとで分離
        var choices = new Dictionary<string, UnityEngine.Events.UnityAction>();
        SoundPlayer.instance.PlaySoundEffect("menu_select");
        foreach (var skill in core.status.activeSkills) {
            choices[skill.skillName] = () => {
                if(skill.mana > core.status.MP) return;
                SoundPlayer.instance.PlaySoundEffect("menu_select");
                MenuGenerator.Instance ().Close ();
                MenuGenerator.Instance ().Close ();
                skill.Activate();
            };
        }
        choices["キャンセル"] = () => {
            SoundPlayer.instance.PlaySoundEffect("menu_cancel");
            MenuGenerator.Instance ().Close ();
        };
        MenuGenerator.Instance ().Create (choices, new Vector3 (Screen.width / 2 - 360, Screen.height / 2 - 750, 0), "knight_skill", true);
    }

    void OnWait (KnightCore core) {
        SoundPlayer.instance.PlaySoundEffect("menu_select");
        core.NextAction (KnightAction.finish);
        GameState.knight_state.Value = Knight_State.move;
        MenuGenerator.Instance ().Close ();
    }

    void OnCancel (KnightCore core) {
        SoundPlayer.instance.PlaySoundEffect("menu_cancel");
        core.NextAction(KnightAction.select_cancel);
        MenuGenerator.Instance ().Close ();
        GameState.knight_state.Value = Knight_State.move;
    }
}