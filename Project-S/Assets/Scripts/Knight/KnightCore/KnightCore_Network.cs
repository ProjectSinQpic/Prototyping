using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Linq;

/**
    ユーザがコマンドを入力することで操作できるユニット
 */

public class KnightCore_Network : KnightCore {

    protected override void Init () {

        MoveDirection();
        AttackDirection();
        SkillDirection();
        SetOperationConfig();

        //ユニットの状態をリセットする
        Message.Where (x => x == KnightAction.finish || x == KnightAction.move_cancel)
            .Subscribe (_ => GameState.instance.ResetState());

        red_all.Add (this);

        GameState.instance.turn
            .Where (x => x == Turn_State.red)
            .Subscribe (_ => isFinished = false);

    }
    void MoveDirection() {
        //選択したマスにユニットを移動させる
        NetworkCommunicater.instance.message
            .Where(c => c[0] == "map")
            .Where (_ => isOperable ())
            .Where (_ => GameState.instance.knight_state.Value == Knight_State.move)
            .Subscribe (c => {
                next_pos = new Vector2(int.Parse(c[1]), int.Parse(c[2]));
                NextAction (KnightAction.move);
            });
    }

    void AttackDirection() {
        var attackStream = NetworkCommunicater.instance.message
            .Where(c => c[0] == "knight")
            .Where (_ => isOperable ())
            .Where (_ => GameState.instance.knight_state.Value == Knight_State.attack);

        //通常攻撃の対象を選択する
        attackStream.Subscribe (c => {
                var pos = new Vector2(int.Parse(c[1]), int.Parse(c[2]));
                var core = KnightCore.GetKnightFromPos(pos);
                if(!CheckAttackable(core)) NextAction(KnightAction.attack_cancel);
                else {
                    targets = new List<KnightCore>(){ core };
                    NextAction(KnightAction.attack_prepare);
                }
            });

        //通常攻撃の対象の選択をキャンセルする
        NetworkCommunicater.instance.message
            .Where(c => c[0] == "map")
            .Where (_ => isOperable ())
            .Where (_ => GameState.instance.knight_state.Value == Knight_State.attack)
            .Subscribe (_ => NextAction (KnightAction.attack_cancel));

        NetworkCommunicater.instance.message
            .Where(c => c[0] == "attack")
            .Where (_ => isOperable ())
            .Subscribe(c => {
                if(c[1] == "go") GameState.instance.selected.Value.NextAction(KnightAction.attack);
                if(c[1] == "cancel") GameState.instance.selected.Value.NextAction(KnightAction.attack_cancel);
            });

        NetworkCommunicater.instance.message
            .Where(c => c[0] == "skill")
            .Where (_ => isOperable ())
            .Subscribe(c => {
                if(c[1] == "go") GameState.instance.selected.Value.NextAction(KnightAction.skill);
                if(c[1] == "cancel") GameState.instance.selected.Value.NextAction(KnightAction.skill_cancel);
            });

    }

    void SkillDirection() {
        //スキルの対象を指定する
        NetworkCommunicater.instance.message
            .Where(c => c[0] == "knight")
            .Where (_ => isOperable ())
            .Where (_ => GameState.instance.knight_state.Value == Knight_State.skill_knight)
            .Subscribe (c => {
                var pos = new Vector2(int.Parse(c[1]), int.Parse(c[2]));
                var core = KnightCore.GetKnightFromPos(pos);
                if(!CheckAttackable(core)) NextAction(KnightAction.skill_cancel);
                else {
                    targets = new List<KnightCore>(){ core };
                    var skill = nowSkill as KnightSelectSkill;
                    skill.OnTargetSelected(targets[0]);
                    NextAction(KnightAction.skill_prepare);
                }
            });

        //スキルの対象の選択をキャンセルする
        NetworkCommunicater.instance.message
            .Where(c => c[0] == "map")
            .Where (_ => isOperable ())
            .Where (_ => GameState.instance.knight_state.Value == Knight_State.skill_knight)
            .Subscribe (_ => {
                NextAction(KnightAction.skill_cancel);
            });

    }

    void SetOperationConfig() {
        //マップカーソルとカメラの制御をOFFにする
        Message.Where (x => x == KnightAction.move || x == KnightAction.skill || x == KnightAction.attack)
            .Subscribe (_ => {
                NetworkCommunicater.instance.canSend = false;
                MapPointer.instance.SetActive(false, false);
                ViewOperater.instance.SetActive(false);
            });

        //マップカーソルとカメラの制御をONにする
        Message.Where (x => x == KnightAction.select || x == KnightAction.move_cancel || x == KnightAction.finish)
            .DelayFrame(1)
            .Subscribe (_ => {
                NetworkCommunicater.instance.canSend = true;
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

    protected override bool isOperable () {
        return GameState.instance.selected.Value == this && GameState.instance.turn.Value == Turn_State.red
            && status.coolDown == 0;
    }

}