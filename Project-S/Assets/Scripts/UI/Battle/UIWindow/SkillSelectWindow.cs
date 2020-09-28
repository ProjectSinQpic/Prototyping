using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class SkillSelectWindow : UIWindow {

    public static SkillSelectWindow instance;

    void Awake () {
        if (instance == null) instance = this;
    }

    void Start() {
        NetworkCommunicater.instance.message
            .Where(c => c[0] == "skill" && c[1] != "go")
            .Subscribe(c => {
                var index = int.Parse(c[1]);
                var core = GameState.instance.selected.Value;
                OnSkillSelected(core, core.status.activeSkills[index], false);
            });
    }

    public void Open(KnightCore core) {
        var choices = new Dictionary<string, UnityEngine.Events.UnityAction>();
        SoundPlayer.instance.PlaySoundEffect(SoundEffect.menu_select);
        foreach (var skill in core.status.activeSkills) {
            choices[skill.skillName] = () => OnSkillSelected(core, skill, true);
        }
        choices["キャンセル"] = () => OnCanceled();
        GenericWindow.instance.Create (choices, new Vector3 (Screen.width / 2 - 360, Screen.height / 2 - 750, 0), "knight_skill", true);
    }

    void OnSkillSelected(KnightCore core, ActiveSkill skill, bool isFromMenu) {
        if(skill.GetRequiredMana() > core.status.MP) return;
        core.nowSkill = skill;
        skill.OnSelected();
        if(isFromMenu) {
            SoundPlayer.instance.PlaySoundEffect(SoundEffect.menu_select);
            GenericWindow.instance.Close ();
            GenericWindow.instance.Close ();
            NetworkCommunicater.instance.SendCommand(string.Format("skill {0}", core.status.activeSkills.IndexOf(skill)));
        }
    }

    void OnCanceled() {
        SoundPlayer.instance.PlaySoundEffect(SoundEffect.menu_cancel);
        GenericWindow.instance.Close ();
    }

}
