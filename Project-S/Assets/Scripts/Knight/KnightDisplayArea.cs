using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class KnightDisplayArea : KnightParts {

    public GameObject prefab_area;
    List<GameObject> objects_area;

    public Color moveColor, attackColor;

    void Awake () {
        core.selectedArea = new List<SelectedArea> ();
        objects_area = new List<GameObject> ();
    }

    void Start () {

        core.Message
            .Where (x => x == KnightAction.attack_look)
            .Subscribe (_ => DisplayArea(AreaShapeType.attackable, core.status.pos, core.statusData.attackRange));

        core.Message
            .Where (x => x == KnightAction.look)
            .Subscribe (_ => DisplayMoveArea ());

        core.Message
            .Where (x => x == KnightAction.look_cancel)
            .Subscribe (_ => RemoveArea ());

        core.Message
            .Where (x => x == KnightAction.skill_look_knight)
            .Subscribe (_ => {
                var skill = core.nowSkill as KnightSelectSkill;
                DisplayArea(skill.areaShape, skill.pos, skill.value);
            });

    }

    public void DisplayMoveArea () {
        RemoveArea ();
        core.selectedArea = AreaCalculator.CalcMovable(core);
        foreach (var i in core.selectedArea) {
            var obj = Instantiate (prefab_area, MapStatus.ToWorldPos (i.pos) + Vector3.up, Quaternion.identity);
            objects_area.Add (obj);
            var r = obj.GetComponent<Renderer> ();
            var col = i.type == AreaType.attack ? attackColor : moveColor;
            //col.a = 0.8f - (0.8f / move_point) * i.root.Length;
            r.material.color = col;
        }
    }

    public void DisplayArea(AreaShapeType type, Vector2 pos, int value) {
        RemoveArea ();
        core.selectedArea = AreaCalculator.GetArea(type, pos, value);
        foreach (var i in core.selectedArea) {
            var obj = Instantiate (prefab_area, MapStatus.ToWorldPos (i.pos) + Vector3.up, Quaternion.identity);
            objects_area.Add (obj);
            var r = obj.GetComponent<Renderer> ();
            var col = attackColor;
            r.material.color = col;
        }
    }


    public void RemoveArea () {
        foreach (var o in objects_area) {
            Destroy (o);
        }
        objects_area = new List<GameObject> ();
    }


}