using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Linq;

/**
    ユーザがコマンドを入力することで操作できるユニット
 */

public class KnightCore_Player : KnightCore {

    protected override void Init () {

        MapPointer.instance.OnClickedMap
            .Where (_ => isOperable ())
            .Where (_ => GameState.instance.knight_state.Value == Knight_State.move)
            .Subscribe (v => {
                next_pos = v;
                NextAction (KnightAction.move);
            });

        var attackStream = MapPointer.instance.OnClickedKnight
            .Where (_ => isOperable ())
            .Where (_ => GameState.instance.knight_state.Value == Knight_State.attack);


        attackStream.Subscribe (n => {
                if(!CheckAttackable(n.GetComponent<KnightCore>())) NextAction(KnightAction.attack_cancel);
                else {
                    targets = new List<KnightCore>(){ n.GetComponent<KnightCore>() };
                    NextAction(KnightAction.attack_prepare);
                }
            });

        attackStream.DelayFrame(1)
            .Subscribe(_ => OpenAttackWindow());

        MapPointer.instance.OnClickedMap
            .Where (_ => isOperable ())
            .Where (_ => GameState.instance.knight_state.Value == Knight_State.attack)
            .Subscribe (n => NextAction (KnightAction.attack_cancel));


        //スキルの対象指定

        MapPointer.instance.OnClickedKnight
            .Where (_ => isOperable ())
            .Where (_ => GameState.instance.knight_state.Value == Knight_State.skill_knight)
            .Subscribe (n => {
                if(!CheckAttackable(n.GetComponent<KnightCore>())) NextAction(KnightAction.skill_cancel);
                else {
                    targets = new List<KnightCore>(){ n.GetComponent<KnightCore>() };
                    NextAction(KnightAction.skill_prepare);
                }
            });
        MapPointer.instance.OnClickedMap
            .Where (_ => isOperable ())
            .Where (_ => GameState.instance.knight_state.Value == Knight_State.skill_knight)
            .Subscribe (n => {
                NextAction(KnightAction.skill_cancel);
            });


        Message.Where (x => x == KnightAction.skill_prepare)
            .Subscribe (_ => OpenSkillWindow());

        ////

        Message.Where (x => x == KnightAction.select)
            .Subscribe (_ => KnightActionWindow.instance.DisplayMenu (this));

        Message.Where (x => x == KnightAction.finish || x == KnightAction.move_cancel)
            .Subscribe (_ => GameState.instance.ResetState());


        Message.Where (x => x == KnightAction.move || x == KnightAction.skill || x == KnightAction.attack)
            .Subscribe (_ => {
                MapPointer.instance.SetActive(false, false);
                ViewOperater.instance.SetActive(false);
            });

        Message.Where (x => x == KnightAction.select || x == KnightAction.move_cancel || x == KnightAction.finish)
            .DelayFrame(1)
            .Subscribe (_ => {
                MapPointer.instance.SetActive(true, true);
                ViewOperater.instance.SetActive(true);
            });

        

    }

    bool CheckAttackable (KnightCore target) {
        if (tag == target.tag) return false;
        var attackArea = selectedArea.Where(s => s.type == AreaType.attack).Select(a => a.pos);
        return attackArea.Contains (target.status.pos);
    }

    void OpenAttackWindow() {
        AttackPredictionWindow.instance.SetPredictionUI(attackResult);
        GenericWindow.instance.Create (new Dictionary<string, UnityEngine.Events.UnityAction> { 
            {"決定", () => {
                    SoundPlayer.instance.PlaySoundEffect(SoundEffect.menu_select);
                    GenericWindow.instance.Close();
                    AttackPredictionWindow.instance.HidePredictionUI();
                    NextAction(KnightAction.attack);
                }
            },
            {"キャンセル", () => {
                    SoundPlayer.instance.PlaySoundEffect(SoundEffect.menu_cancel);
                    GenericWindow.instance.Close();
                    AttackPredictionWindow.instance.HidePredictionUI();
                    NextAction (KnightAction.attack_cancel);
                }
            }
        }, new Vector3 (0, -Screen.height / 2 + 200, 0), "attack_target", true);
    }

    void OpenSkillWindow() {
        GenericWindow.instance.Create (new Dictionary<string, UnityEngine.Events.UnityAction> { 
            {"決定", () => {
                    SoundPlayer.instance.PlaySoundEffect(SoundEffect.menu_select);
                    GenericWindow.instance.Close();
                    NextAction(KnightAction.skill);
                }
            },
            {"キャンセル", () => {
                    SoundPlayer.instance.PlaySoundEffect(SoundEffect.menu_cancel);
                    GenericWindow.instance.Close();
                    NextAction(KnightAction.skill_cancel);
                }
            }
        }, new Vector3 (0, -Screen.height / 2 + 200, 0), "skill_target", true);
    }

}