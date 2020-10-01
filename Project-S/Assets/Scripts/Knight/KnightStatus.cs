using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class KnightStatus : KnightParts {
    public KnightDatabase data;

    public Vector2 pos;
    public Direction dir;

    public int HP;
    public int MP;

    public int coolDown;
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
    }

    public void ApplyStatus(int hpDiff, int mpDiff, int restDiff) {
        HP = Mathf.Clamp(HP + hpDiff, 0, core.statusData.maxHP);
        MP = Mathf.Clamp(MP + mpDiff, 0, core.statusData.maxMP);
        core.storedCoolDown = Mathf.Max(core.storedCoolDown + restDiff, 0);  
    }

}

[System.Serializable]
public class KnightStatusData {
    public int maxHP;
    public int maxMP;
    public int attack;
    public int defense;
    public int moveRange;
    public int attackRange;

    public static KnightStatusData Add(KnightStatusData a, KnightStatusData b) {
        KnightStatusData result = new KnightStatusData();
        result.maxHP = a.maxHP + b.maxHP;
        result.maxMP = a.maxMP + b.maxMP;
        result.attack = a.attack + b.attack;
        result.defense = a.defense + b.defense;
        result.moveRange = a.moveRange + b.moveRange;
        result.attackRange = a.attackRange + b.attackRange;
        return result;
    }

}

public class StatusDataApplication {
    
    public enum StatusType {
    
    } 
}