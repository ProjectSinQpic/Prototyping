﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class KnightStatus : KnightParts {
    public KnightDatabase data;

    public Vector2 pos;
    public Direction dir;

    public int HP;
    public int MP;

    public int rest;
    public int level;
    public int SP;

    public List<SkillBase> skills; //TODO: プロパティに変更
    public List<ActiveSkill> activeSkills;
    public List<PassiveSkill> passiveSkills;
    public List<StatusBuff> statusBuffs;

    public KnightStatusData actual, delta;
    

    public void Init () {
        actual = new KnightStatusData();
        delta = new KnightStatusData();
        StatusCalculator calculator = new TRLinear_StatusCalculator (this);
        calculator.Calc ();
        HP = actual.maxHP;
        MP = actual.maxMP;

        skills = new List<SkillBase>();
        foreach (var skill in data.skills) {
            skills.Add(ScriptableObject.Instantiate(skill));            
        }
        activeSkills = skills.Where(s => s is ActiveSkill).Select(s => (ActiveSkill)s).ToList();
        passiveSkills = skills.Where(s => s is PassiveSkill).Select(s => (PassiveSkill)s).ToList();
        skills.ForEach(s => s.Init(core));

        GetComponent<KnightView> ().Init ();    //TODO 改善の余地あり
        transform.position = MapStatus.ToWorldPos(pos);
        core.prev_pos = pos;
        core.isReady = true;
    }

    public void ApplyStatus(int hpDiff, int mpDiff, int restDiff) {
        HP = Mathf.Clamp(HP + hpDiff, 0, core.statusData.maxHP);
        MP = Mathf.Clamp(MP + mpDiff, 0, core.statusData.maxMP);
        core.storedCoolDown = Mathf.Max(core.storedCoolDown + restDiff, 0);  
    }

    public void ApplyBuffs(List<StatusBuff> buffs) {
        statusBuffs.AddRange(buffs);
        skills.AddRange(buffs);
    }

    //バフデバフを考慮した能力値を計算する
    public KnightStatusData GetStatusData() {
        var clone = KnightStatusData.Clone(KnightStatusData.Add(actual, delta));
        List<BuffData> addBuffs = new List<BuffData>();
        List<BuffData> mulBuffs = new List<BuffData>();
        foreach(var buffs in skills.Select(s => s.GetBuff())){
            foreach (var buff in buffs) {
                if(buff.isAddive) addBuffs.Add(buff);
                else mulBuffs.Add(buff);
            }
        }
        addBuffs.ForEach(buff => clone.ApplyBuff(buff.type, buff.value, buff.isAddive));
        mulBuffs.ForEach(buff => clone.ApplyBuff(buff.type, buff.value, buff.isAddive));
        return clone;
    }

}



[System.Serializable]
public class KnightStatusData {
    public int maxHP;
    public int maxMP;
    public int attack;
    public int defense;
    public int skillAttack;
    public int skillDefense;
    public int moveRange;
    public int attackRange;

    public static KnightStatusData Add(KnightStatusData a, KnightStatusData b) {
        KnightStatusData result = new KnightStatusData();
        result.maxHP = a.maxHP + b.maxHP;
        result.maxMP = a.maxMP + b.maxMP;
        result.attack = a.attack + b.attack;
        result.defense = a.defense + b.defense;
        result.skillAttack = a.skillAttack + b.skillAttack;
        result.skillDefense = a.skillDefense + b.skillDefense;
        result.moveRange = a.moveRange + b.moveRange;
        result.attackRange = a.attackRange + b.attackRange;
        return result;
    }

    public static KnightStatusData Clone(KnightStatusData data) {
        KnightStatusData clone = new KnightStatusData();
        clone.maxHP = data.maxHP;
        clone.maxMP = data.maxMP;
        clone.attack = data.attack;
        clone.defense = data.defense;
        clone.skillAttack = data.skillAttack;
        clone.skillDefense = data.skillDefense;
        clone.moveRange = data.moveRange;
        clone.attackRange = data.attackRange;
        return clone;
    }

    public KnightStatusData ApplyBuff(StatusDataType type, float value, bool isAddive) {
        if(type == StatusDataType.maxHP) 
            maxHP = (int)Mathf.Round(isAddive ? maxHP + value : maxHP * value);
        if(type == StatusDataType.maxMP) 
            maxMP = (int)Mathf.Round(isAddive ? maxMP + value : maxMP * value);
        if(type == StatusDataType.attack) 
            attack = (int)Mathf.Round(isAddive ? attack + value : attack * value);
        if(type == StatusDataType.defense) 
            defense = (int)Mathf.Round(isAddive ? defense + value : defense * value);
        if(type == StatusDataType.skillAttack) 
            skillAttack = (int)Mathf.Round(isAddive ? skillAttack + value : skillAttack * value);
        if(type == StatusDataType.skillDefense) 
            skillDefense = (int)Mathf.Round(isAddive ? skillDefense + value : skillDefense * value);
        if(type == StatusDataType.moveRange) 
            moveRange = (int)Mathf.Round(isAddive ? moveRange + value : moveRange * value);
        if(type == StatusDataType.attackRange) 
            attackRange = (int)Mathf.Round(isAddive ? attackRange + value : attackRange * value);
        return this;
    }

}

public enum StatusDataType {
    maxHP, 
    maxMP,
    attack,
    defense,
    skillAttack,
    skillDefense,
    moveRange,
    attackRange,   

    none
}

public class BuffData {
    public StatusDataType type;
    public float value;
    public bool isAddive;

    public BuffData (StatusDataType type, float value, bool isAddive){
        this.type = type;
        this.value = value;
        this.isAddive = isAddive;
    }
}