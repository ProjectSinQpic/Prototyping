﻿using System.Collections;
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

        Message.Where (x => x == KnightAction.finish)
            .Subscribe (_ => GameState.instance.selected.Value = null);

    }

    bool CheckAttackable (KnightCore target) {
        if (tag == target.tag) return false;
        var attackArea = selectedArea.Where(s => s.type == AreaType.attack).Select(a => a.pos);
        return attackArea.Contains (target.status.pos);
    }

    void OpenAttackWindow() {
        AttackPrediction.instance.SetPredictionUI(attackResult);
        GenericWindow.instance.Create (new Dictionary<string, UnityEngine.Events.UnityAction> { 
            {"決定", () => {
                    SoundPlayer.instance.PlaySoundEffect(SoundEffect.menu_select);
                    GenericWindow.instance.Close();
                    AttackPrediction.instance.HidePredictionUI();
                    NextAction(KnightAction.attack);
                }
            },
            {"キャンセル", () => {
                    SoundPlayer.instance.PlaySoundEffect(SoundEffect.menu_cancel);
                    GenericWindow.instance.Close();
                    AttackPrediction.instance.HidePredictionUI();
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