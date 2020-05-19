using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum AreaShapeType {
    attackable,
    line,
}

public class AreaCalculator {

    static Dictionary<AreaShapeType, System.Func<Vector2, int, List<SelectedArea>>> dict = new Dictionary<AreaShapeType, System.Func<Vector2, int, List<SelectedArea>>>() {
        {AreaShapeType.attackable, CalcAttackable},
        {AreaShapeType.line, CalcLineArea}
    };

    public static List<SelectedArea> GetArea(AreaShapeType shape, Vector2 pos, int value) {
        return dict[shape](pos, value);
    }

    static public List<SelectedArea> CalcMovable (KnightCore core) {
        var sa = new List<SelectedArea>();
        FindMovable (new SelectedArea () { pos = core.status.pos, root = "" , type = AreaType.move}, core.statusData.moveRange, sa, core);
        return sa;
    }

    static void FindMovable (SelectedArea p, int mp, List<SelectedArea> sa, KnightCore core) {
        if (MapStatus.IsOutOfMap (p.pos)) return;
        int next_mp = mp - MapStatus.MAP_MP[(int) MapStatus.MapTypeOf (p.pos)];
        if (KnightCore.all.Where (c => c != core).Where (c => !c.isDead).Select (c => c.status.pos).Contains (p.pos)) next_mp = -1;
        if (next_mp < 0) {
            FindAttackable (p.pos, core.statusData.attackRange, sa);
            return;
        }
        if (!sa.Select (m => m.pos).Contains (p.pos)) sa.Add (p);
        else {
            var x = sa.Find(m => m.pos == p.pos);
            if (x.type == AreaType.attack || x.root.Length > p.root.Length) {
                sa.Remove (x);
                sa.Add (p);
            }
        }
        FindMovable (new SelectedArea () { pos = p.pos + Vector2.right, root = p.root + "r" }, next_mp, sa, core);
        FindMovable (new SelectedArea () { pos = p.pos + Vector2.left, root = p.root + "l" }, next_mp, sa, core);
        FindMovable (new SelectedArea () { pos = p.pos + Vector2.up, root = p.root + "u" }, next_mp, sa, core);
        FindMovable (new SelectedArea () { pos = p.pos + Vector2.down, root = p.root + "d" }, next_mp, sa, core);
    }

    static List<SelectedArea> CalcAttackable (Vector2 pos, int value) {
        List<SelectedArea> sa = new List<SelectedArea>();
        FindAttackable (pos + Vector2.right, value, sa);
        FindAttackable (pos + Vector2.left, value, sa);
        FindAttackable (pos + Vector2.up, value, sa);
        FindAttackable (pos + Vector2.down, value, sa);
        return sa;
    }

    static void FindAttackable (Vector2 pos, int ap, List<SelectedArea> sa) {
        if (MapStatus.IsOutOfMap (pos) || MapStatus.MapTypeOf (pos) == Map_type.MAP_NONE) return;
        int next_ap = ap - 1;
        if (next_ap < 0) return;
        if (!sa.Select(s => s.pos).Contains(pos)) {
            sa.Add(new SelectedArea () { pos = pos, type = AreaType.attack});
        }

        FindAttackable (pos + Vector2.right, next_ap, sa);
        FindAttackable (pos + Vector2.left, next_ap, sa);
        FindAttackable (pos + Vector2.up, next_ap, sa);
        FindAttackable (pos + Vector2.down, next_ap, sa);
    }

    static List<SelectedArea> CalcLineArea(Vector2 pos, int value) {
        List<SelectedArea> sa = new List<SelectedArea>();
        for(int i = 1; i <= value; i++) {
            sa.Add(new SelectedArea() {pos = pos + Vector2.right * i, type = AreaType.attack});
            sa.Add(new SelectedArea() {pos = pos + Vector2.left * i, type = AreaType.attack});
            sa.Add(new SelectedArea() {pos = pos + Vector2.up * i, type = AreaType.attack});
            sa.Add(new SelectedArea() {pos = pos + Vector2.down * i, type = AreaType.attack});
        }
        sa = sa.Where(s => !MapStatus.IsOutOfMap (s.pos) && MapStatus.MapTypeOf (s.pos) != Map_type.MAP_NONE).ToList();
        return sa;
    }

}
