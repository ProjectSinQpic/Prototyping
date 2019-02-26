using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;

public class KnightDisplayArea : KnightParts
{

    public GameObject prefab_area;
    List<GameObject> objects_area;

    void Awake() {
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
        core.movableArea = new List<Vector2>();
        objects_area = new List<GameObject>();
        FindMovable(core.status.pos, 5);
        List<Vector2> movable = core.movableArea.Distinct().ToList();
        foreach (var m in movable) {
            objects_area.Add(Instantiate(prefab_area, MapStatus.ToWorldPos(m), Quaternion.identity));
        }
    }

    public void RemoveArea() {
        foreach (var o in objects_area) {
            Destroy(o);
        }
        objects_area = new List<GameObject>();
    }

    void FindMovable(Vector2 p, int mp) {
        if (MapStatus.IsOutOfMap(p)) return;
        int next_mp = mp - MapStatus.MAP_MP[(int)MapStatus.MapTypeOf(p)];
        if (next_mp < 0) return;
        core.movableArea.Add(p);
        FindMovable(p + Vector2.right, next_mp);
        FindMovable(p - Vector2.right, next_mp);
        FindMovable(p + Vector2.up, next_mp);
        FindMovable(p - Vector2.up, next_mp);
    }

}
