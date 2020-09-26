public class AttackResult {

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

    public class KnightDiff {
        public KnightCore knight;
        public int hpDiff, mpDiff, restDiff;

        public KnightStatusData statusData;
        public KnightDiff() {
            this.knight = null;
            this.hpDiff = 0;
            this.mpDiff = 0;
            this.restDiff = 0;
            this.statusData = null;
        }

        public KnightDiff(KnightCore core) {
            this.knight = core;
            this.hpDiff = 0;
            this.mpDiff = 0;
            this.restDiff = 0;
            this.statusData = core.statusData;
        }
    }

    KnightDiff attacker, target;

}
