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
        base.Init();

        MoveDirection();
        AttackDirection();
        SkillDirection();
        SetOperationConfig();

        //行動選択画面を表示する
        Message.Where (x => x == KnightAction.select)
            .Subscribe (_ => KnightActionWindow.instance.DisplayMenu (this));

        //ユニットの状態をリセットする
        Message.Where (x => x == KnightAction.finish || x == KnightAction.move_cancel)
            .Subscribe (_ => GameState.instance.ResetState());



    }
    void MoveDirection() {
        //選択したマスにユニットを移動させる
        MapPointer.instance.OnClickedMap
            .Where (_ => isOperable ())
            .Where (_ => GameState.instance.knight_state.Value == Knight_State.move)
            .Subscribe (v => {
                next_pos = v;
                NextAction (KnightAction.move);
            });
    }

    void AttackDirection() {
        var attackStream = MapPointer.instance.OnClickedKnight
            .Where (_ => isOperable ())
            .Where (_ => GameState.instance.knight_state.Value == Knight_State.attack);

        //通常攻撃の対象を選択する
        attackStream.Subscribe (n => {
                if(!CheckAttackable(n.GetComponent<KnightCore>())) NextAction(KnightAction.attack_cancel);
                else {
                    targets = new List<KnightCore>(){ n.GetComponent<KnightCore>() };
                    NextAction(KnightAction.attack_prepare);
                }
            });

        //通常攻撃の戦闘予測画面を表示する
        attackStream.DelayFrame(1)
            .Subscribe(_ => OpenAttackWindow());

        //通常攻撃の対象の選択をキャンセルする
        MapPointer.instance.OnClickedMap
            .Where (_ => isOperable ())
            .Where (_ => GameState.instance.knight_state.Value == Knight_State.attack)
            .Subscribe (n => NextAction (KnightAction.attack_cancel));
    }

    void SkillDirection() {
        //スキルの対象を指定する
        MapPointer.instance.OnClickedKnight
            .Where (_ => isOperable ())
            .Where (_ => GameState.instance.knight_state.Value == Knight_State.skill_knight)
            .Subscribe (n => {
                if(!CheckAttackable(n.GetComponent<KnightCore>())) NextAction(KnightAction.skill_cancel);
                else {
                    targets = new List<KnightCore>(){ n.GetComponent<KnightCore>() };
                    var skill = nowSkill as KnightSelectSkill;
                    skill.OnTargetSelected(targets[0]);
                    NextAction(KnightAction.skill_prepare);
                }
            });

        //スキルの対象の選択をキャンセルする
        MapPointer.instance.OnClickedMap
            .Where (_ => isOperable ())
            .Where (_ => GameState.instance.knight_state.Value == Knight_State.skill_knight)
            .Subscribe (n => {
                NextAction(KnightAction.skill_cancel);
            });

        //スキルの戦闘予測画面を表示する
        Message.Where (x => x == KnightAction.skill_prepare)
            .DelayFrame(1)
            .Subscribe (_ => OpenSkillWindow());
    }

    void SetOperationConfig() {
        //マップカーソルとカメラの制御をOFFにする
        Message.Where (x => x == KnightAction.move || x == KnightAction.skill || x == KnightAction.attack)
            .Subscribe (_ => {
                MapPointer.instance.SetActive(false, false);
                ViewOperater.instance.SetActive(false);
            });

        //マップカーソルとカメラの制御をONにする
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
        bool isKnightSelectSkill = nowSkill is KnightSelectSkill;
        if(isKnightSelectSkill) AttackPredictionWindow.instance.SetPredictionUI(attackResult);
        GenericWindow.instance.Create (new Dictionary<string, UnityEngine.Events.UnityAction> { 
            {"決定", () => {
                    SoundPlayer.instance.PlaySoundEffect(SoundEffect.menu_select);
                    GenericWindow.instance.Close();
                    if(isKnightSelectSkill) AttackPredictionWindow.instance.HidePredictionUI();
                    NextAction(KnightAction.skill);
                }
            },
            {"キャンセル", () => {
                    SoundPlayer.instance.PlaySoundEffect(SoundEffect.menu_cancel);
                    GenericWindow.instance.Close();
                    if(isKnightSelectSkill) AttackPredictionWindow.instance.HidePredictionUI();
                    NextAction(KnightAction.skill_cancel);
                }
            }
        }, new Vector3 (0, -Screen.height / 2 + 200, 0), "skill_target", true);
    }

}