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
        MenuGenerator.Instance ().Create (new Dictionary<string, UnityEngine.Events.UnityAction> { { "攻撃", () => OnAttack (core) },
            { "待機", () => OnWait (core) },
            { "キャンセル", () => OnCancel (core) },
        }, new Vector3 (Screen.width / 2 - 180, Screen.height / 2 - 250, 0), "knight_choice", true);
        GameState.knight_state.Value = Knight_State.select;
    }

    void OnAttack (KnightCore core) {
        SoundPlayer.instance.PlaySoundEffect("menu_select");
        core.NextAction ("attack_set");
        GameState.knight_state.Value = Knight_State.attack;
        MenuGenerator.Instance ().Close ();
    }

    void OnWait (KnightCore core) {
        SoundPlayer.instance.PlaySoundEffect("menu_select");
        core.NextAction ("finish");
        GameState.knight_state.Value = Knight_State.move;
        MenuGenerator.Instance ().Close ();
    }

    void OnCancel (KnightCore core) {
        SoundPlayer.instance.PlaySoundEffect("menu_cancel");
        var diff = core.prev_pos - core.status.pos;
        core.transform.position += Vector3.right * MapStatus.MAPCHIP_SIZE * diff.x +
            Vector3.back * MapStatus.MAPCHIP_SIZE * diff.y;
        core.status.pos = core.prev_pos;
        MenuGenerator.Instance ().Close ();
        GameState.knight_state.Value = Knight_State.move;
        GameState.selected.Value = null;
    }
}