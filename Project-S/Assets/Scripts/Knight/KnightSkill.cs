using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightSkill : KnightParts {

    System.Action<List<KnightCore>> skillAction;

    ActiveSkill nowSkill;

    public void SubscribeSkill(ActiveSkill skill) {
        nowSkill = skill;
    }

    public void SelectKnight(AreaShapeType shape, Vector2 pos, int value, System.Action<List<KnightCore>> act) {
        GameState.knight_state.Value = Knight_State.skill_knight;
        GetComponent<KnightDisplayArea>().DisplayArea(shape, pos, value);
        skillAction = act;
    }

    public void OnTargeted(List<KnightCore> targets){
        GetComponent<KnightDisplayArea>().RemoveArea();
        skillAction(targets);
    } 

    public void OnSpell() {
        core.status.MP -= nowSkill.mana;
        core.storedCoolDown += nowSkill.rest;
    }

    public void OnCancel() {
        nowSkill = null;
        GetComponent<KnightDisplayArea>().RemoveArea();
        core.NextAction("select");
    }

}
