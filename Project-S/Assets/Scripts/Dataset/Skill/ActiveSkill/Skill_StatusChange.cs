﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Skill/Active/StatusChange")]

/**
お互いのステータスを変更するスキル。ダメージは全て固定ダメージ扱い。
*/
public class Skill_StatusChange : KnightSelectSkill {

    public StatusBuff attackerBuff, targetBuff;

    public override int GetRequiredMana() {
        return -GetParam("attackerMP");
    }

    protected override void OnWaitForTarget() { }

    public override void OnTargetSelected(KnightCore target) {
        var targetHP = GetParam("targetHP");
        var targetMP = GetParam("targetMP");
        var targetRest = GetParam("targetRest");
        var attackerHP = GetParam("attackerHP");
        var attackerMP = GetParam("attackerMP");
        var attackerRest = GetParam("attackerRest");
        owner.attackResult.SetTarget(owner.targets[0]);
        owner.attackResult.AddValue(true, targetHP, targetMP, targetRest);
        owner.attackResult.AddValue(false, attackerHP, attackerMP, attackerRest);
        if(attackerBuff != null) owner.attackResult.AddBuff(false, attackerBuff);
        if(targetBuff != null) owner.attackResult.AddBuff(true, targetBuff);
    }

    public override void OnSpell() {
        Debug.Log("skill!!!");
    }

}
