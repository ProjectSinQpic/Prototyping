using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;

public class KnightDisplayArea : KnightParts
{

    public List<MovableArea> movableArea;
    public GameObject prefab_area;
    List<GameObject> objects_area;
    readonly int move_point = 5;

    void Awake() {
        movableArea = new List<MovableArea>();
        objects_area = new List<GameObject>();
    }

    void Start() {
        core.isSelected
            .Where(b => b == true)
            .Subscribe(_ => DisplayArea());

        core.isSelected
            .Where(b => b == false)
            .Subscribe(_ => RemoveArea());
    }

    public void DisplayArea() {
        movableArea = new List<MovableArea>();
        objects_area = new List<GameObject>();
        FindMovable(new MovableArea() { pos = core.status.pos, root = ""}, move_point);
        foreach (var m in movableArea) {
            var obj = Instantiate(prefab_area, MapStatus.ToWorldPos(m.pos) + Vector3.up, Quaternion.identity);
            objects_area.Add(obj);
            var r = obj.GetComponent<Renderer>();
            var col = r.material.color;
            col.a = 0.8f - (0.8f / move_point) * m.root.Length;
            r.material.color = col;
        }
    }

    public void RemoveArea() {
        foreach (var o in objects_area) {
            Destroy(o);
        }
        objects_area = new List<GameObject>();
    }

    void FindMovable(MovableArea p, int mp) {
        if (MapStatus.IsOutOfMap(p.pos)) return;
        int next_mp = mp - MapStatus.MAP_MP[(int)MapStatus.MapTypeOf(p.pos)];
        if (KnightCore.all.Where(c => c != core).Select(c => c.status.pos).Contains(p.pos)) next_mp = -1;
        if (next_mp < 0) return;
        if (!movableArea.Select(m => m.pos).Contains(p.pos)) movableArea.Add(p);
        else {
            var x = movableArea.Find(m => m.pos == p.pos);
            if(x.root.Length > p.root.Length) {
                movableArea.Remove(x);
                movableArea.Add(p);
            }
        }
        FindMovable(new MovableArea() { pos = p.pos + Vector2.right, root = p.root + "r" }, next_mp);
        FindMovable(new MovableArea() { pos = p.pos + Vector2.left, root = p.root + "l" }, next_mp);
        FindMovable(new MovableArea() { pos = p.pos + Vector2.up, root = p.root + "u" }, next_mp);
        FindMovable(new MovableArea() { pos = p.pos + Vector2.down, root = p.root + "d" }, next_mp);
    }

}

