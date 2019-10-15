using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class KnightDisplayArea : KnightParts {

    public List<MovableArea> movableArea;
    public List<Vector2> attackableArea;
    public GameObject prefab_area;
    List<GameObject> objects_area;

    public Color moveColor, attackColor;

    void Awake () {
        movableArea = new List<MovableArea> ();
        attackableArea = new List<Vector2> ();
        objects_area = new List<GameObject> ();
    }

    void Start () {

        core.Message
            .Where (x => x == "look")
            .Subscribe (_ => DisplayMoveArea ());

        core.Message
            .Where (x => x == "look_cancel")
            .Subscribe (_ => RemoveArea ());

    }

    public void DisplayMoveArea () {
        movableArea = new List<MovableArea> ();
        attackableArea = new List<Vector2> ();
        objects_area = new List<GameObject> ();
        CalcMovable ();

        var m = movableArea.Select (x => x.pos);

        attackableArea = attackableArea.Where (x => !m.Contains (x)).ToList ();
        foreach (var i in movableArea) {
            var obj = Instantiate (prefab_area, MapStatus.ToWorldPos (i.pos) + Vector3.up, Quaternion.identity);
            objects_area.Add (obj);
            var r = obj.GetComponent<Renderer> ();
            var col = moveColor;
            //col.a = 0.8f - (0.8f / move_point) * i.root.Length;
            r.material.color = col;
        }

        foreach (var i in attackableArea) {
            var obj = Instantiate (prefab_area, MapStatus.ToWorldPos (i) + Vector3.up, Quaternion.identity);
            objects_area.Add (obj);
            var r = obj.GetComponent<Renderer> ();
            var col = attackColor;
            r.material.color = col;
        }
    }

    public void DisplayAttackArea () {
        RemoveArea ();
        attackableArea = new List<Vector2> ();
        objects_area = new List<GameObject> ();
        CalcAttackable ();

        foreach (var i in attackableArea) {
            var obj = Instantiate (prefab_area, MapStatus.ToWorldPos (i) + Vector3.up, Quaternion.identity);
            objects_area.Add (obj);
            var r = obj.GetComponent<Renderer> ();
            var col = attackColor;
            r.material.color = col;
        }
    }

    //public List<MovableArea> CalcMovableArea() {
    //    movableArea = new List<MovableArea>();
    //    objects_area = new List<GameObject>();
    //    FindMovable(new MovableArea() { pos = core.status.pos, root = "" }, core.status.moveRange);
    //    return movableArea;
    //}

    public void RemoveArea () {
        foreach (var o in objects_area) {
            Destroy (o);
        }
        objects_area = new List<GameObject> ();
    }

    public void CalcMovable () {
        FindMovable (new MovableArea () { pos = core.status.pos, root = "" }, core.status.moveRange);
    }

    void FindMovable (MovableArea p, int mp) {
        if (MapStatus.IsOutOfMap (p.pos)) return;
        int next_mp = mp - MapStatus.MAP_MP[(int) MapStatus.MapTypeOf (p.pos)];
        if (KnightCore.all.Where (c => c != core).Where (c => !c.isDead).Select (c => c.status.pos).Contains (p.pos)) next_mp = -1;
        if (next_mp < 0) {
            FindAttackable (p.pos, core.status.attackRange);
            return;
        }
        if (!movableArea.Select (m => m.pos).Contains (p.pos)) movableArea.Add (p);
        else {
            var x = movableArea.Find (m => m.pos == p.pos);
            if (x.root.Length > p.root.Length) {
                movableArea.Remove (x);
                movableArea.Add (p);
            }
        }
        FindMovable (new MovableArea () { pos = p.pos + Vector2.right, root = p.root + "r" }, next_mp);
        FindMovable (new MovableArea () { pos = p.pos + Vector2.left, root = p.root + "l" }, next_mp);
        FindMovable (new MovableArea () { pos = p.pos + Vector2.up, root = p.root + "u" }, next_mp);
        FindMovable (new MovableArea () { pos = p.pos + Vector2.down, root = p.root + "d" }, next_mp);
    }

    public void CalcAttackable () {
        FindAttackable (core.status.pos + Vector2.right, core.status.attackRange);
        FindAttackable (core.status.pos + Vector2.left, core.status.attackRange);
        FindAttackable (core.status.pos + Vector2.up, core.status.attackRange);
        FindAttackable (core.status.pos + Vector2.down, core.status.attackRange);
    }

    void FindAttackable (Vector2 pos, int ap) {
        if (MapStatus.IsOutOfMap (pos) || MapStatus.MapTypeOf (pos) == Map_type.MAP_NONE) return;
        int next_ap = ap - 1;
        if (next_ap < 0) return;
        if (!attackableArea.Contains (pos)) attackableArea.Add (pos);
        FindAttackable (pos + Vector2.right, next_ap);
        FindAttackable (pos + Vector2.left, next_ap);
        FindAttackable (pos + Vector2.up, next_ap);
        FindAttackable (pos + Vector2.down, next_ap);
    }

}