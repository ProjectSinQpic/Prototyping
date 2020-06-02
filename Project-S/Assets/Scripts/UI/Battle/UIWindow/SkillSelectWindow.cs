using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSelectWindow : UIWindow {

    public static SkillSelectWindow instance;

    void Awake () {
        if (instance == null) instance = this;
    }

    public void Open(KnightCore core) {
        var choices = new Dictionary<string, UnityEngine.Events.UnityAction>();
        SoundPlayer.instance.PlaySoundEffect("menu_select");
        foreach (var skill in core.status.activeSkills) {
            choices[skill.skillName] = () => {
                if(skill.mana > core.status.MP) return;
                SoundPlayer.instance.PlaySoundEffect("menu_select");
                GenericWindow.instance.Close ();
                GenericWindow.instance.Close ();
                skill.Activate();
            };
        }
        choices["キャンセル"] = () => {
            SoundPlayer.instance.PlaySoundEffect("menu_cancel");
            GenericWindow.instance.Close ();
        };
        GenericWindow.instance.Create (choices, new Vector3 (Screen.width / 2 - 360, Screen.height / 2 - 750, 0), "knight_skill", true);
    }


}
