using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//プレイヤーが対象を指定して発動するスキル
public class KnightSelectSkill : ActiveSkill {

    public AreaShapeType areaShape;
    [HideInInspector] public Vector2 areaCenterPos;

    public virtual void OnTargetSelected(KnightCore target) {}

    protected override void OnWait() {
        OnInit();
        GameState.instance.knight_state.Value = Knight_State.skill_knight;
        owner.NextAction(KnightAction.skill_look_knight);
    }

    protected virtual void OnInit() {}

}
