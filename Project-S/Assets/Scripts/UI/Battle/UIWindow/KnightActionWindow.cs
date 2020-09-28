using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class KnightActionWindow : UIWindow {

    public static KnightActionWindow instance;


    void Awake () {
        if (instance == null) instance = this;
    }

    void Start() {
        NetworkCommunicater.instance.message
            .Where(c => c[0] == "action")
            .Subscribe(c => DoActionFromCommand(c));
    }

    void DoActionFromCommand(string[] command) {
        var core = GameState.instance.selected.Value;
        Debug.Log(command[1] + " : " + "attack");
        if(command[1] == "attack") OnAttack(core, false);
        if(command[1] == "mana") OnGetMana(core, false);
        if(command[1] == "wait") OnWait(core, false);
        if(command[1] == "cancel") OnCancel(core, false);
    }

    public void DisplayMenu (KnightCore core) {
        var menu = new Dictionary<string, UnityEngine.Events.UnityAction> { 
            { "攻撃", () => OnAttack (core, true) },
            { "スキル", () => OnSkill (core, true) },
        };
        if(FieldManaPlacer.instance.IsGettableMana(core.next_pos)) {
            menu.Add("マナ獲得", () => OnGetMana(core, true));
        }
        menu.Add("待機", () => OnWait (core, true));
        menu.Add("キャンセル", () => OnCancel (core, true));
        GenericWindow.instance.Create (menu, new Vector3 (Screen.width / 2 - 250, Screen.height / 2 - 250, 0), "knight_choice", true);
        GameState.instance.knight_state.Value = Knight_State.select;
    }

    void OnAttack (KnightCore core, bool isFromMenu) {
        Debug.Log("!!!");
        core.NextAction (KnightAction.attack_look);
        GameState.instance.knight_state.Value = Knight_State.attack;
        if(isFromMenu) {
            SoundPlayer.instance.PlaySoundEffect(SoundEffect.menu_select);
            GenericWindow.instance.Close ();
        }
    }

    void OnSkill (KnightCore core, bool isFromMenu) {
        SkillSelectWindow.instance.Open(core);
        if(isFromMenu) {
            SoundPlayer.instance.PlaySoundEffect(SoundEffect.menu_select);
        }
    }

    void OnWait (KnightCore core, bool isFromMenu) {
        core.NextAction (KnightAction.finish);
        GameState.instance.knight_state.Value = Knight_State.move;
        if(isFromMenu) {
            SoundPlayer.instance.PlaySoundEffect(SoundEffect.menu_select);
            GenericWindow.instance.Close ();
        }
    }

    void OnGetMana(KnightCore core, bool isFromMenu) {
        core.NextAction (KnightAction.get_mana);
        GameState.instance.knight_state.Value = Knight_State.move;
        if(isFromMenu) {
            SoundPlayer.instance.PlaySoundEffect(SoundEffect.menu_select);
            GenericWindow.instance.Close ();
        }
    }

    void OnCancel (KnightCore core, bool isFromMenu) {
        core.NextAction(KnightAction.select_cancel);
        GameState.instance.knight_state.Value = Knight_State.move;
        if(isFromMenu) {
            SoundPlayer.instance.PlaySoundEffect(SoundEffect.menu_select);
            GenericWindow.instance.Close ();
        }
    }
}