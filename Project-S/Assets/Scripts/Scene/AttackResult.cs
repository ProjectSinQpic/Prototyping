using System.Collections.Generic;
using UnityEngine;
public class AttackResult {

    KnightDiff attacker, target;

    public AttackResult(KnightCore core) {
        attacker = new KnightDiff(core);
        target = new KnightDiff();
    }

    public void SetTarget(KnightCore target) {
        this.target = new KnightDiff(target);
    }

    public KnightDiff GetTarget() {
        return this.target;
    }

    public KnightDiff GetAttacker() {
        return this.attacker;
    }

    public void AddValue(bool isTarget, int hp, int mp, int rest) {
        var core = isTarget ? target : attacker;
        core.hpDiff += hp;
        core.mpDiff += mp;
        core.restDiff += rest;
    }

    public void AddBuff(bool isTarget, StatusBuff buff) {
        var core = isTarget ? target : attacker;
        var buffInstance = ScriptableObject.Instantiate(buff);
        buffInstance.Init(core.knight);
        core.buffs.Add(buffInstance);
    }

    public class KnightDiff {
        public KnightCore knight;
        public int hpDiff, mpDiff, restDiff;
        public List<StatusBuff> buffs; 

        public KnightDiff() {
            this.knight = null;
            this.hpDiff = 0;
            this.mpDiff = 0;
            this.restDiff = 0;
            this.buffs = new List<StatusBuff>();
        }

        public KnightDiff(KnightCore core) {
            this.knight = core;
            this.hpDiff = 0;
            this.mpDiff = 0;
            this.restDiff = 0;
            this.buffs = new List<StatusBuff>();
        }
    }


}
