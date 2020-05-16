using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class KnightDisplayArea : KnightParts {

    public List<SelectedArea> selectedArea;
    public GameObject prefab_area;
    List<GameObject> objects_area;

    public Color moveColor, attackColor;

    void Awake () {
        selectedArea = new List<SelectedArea> ();
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
        RemoveArea ();
        selectedArea = AreaCalculator.CalcMovable(core);
        var m = selectedArea.Select (x => x.pos);
        foreach (var i in selectedArea) {
            var obj = Instantiate (prefab_area, MapStatus.ToWorldPos (i.pos) + Vector3.up, Quaternion.identity);
            objects_area.Add (obj);
            var r = obj.GetComponent<Renderer> ();
            var col = i.type == AreaType.attack ? attackColor : moveColor;
            //col.a = 0.8f - (0.8f / move_point) * i.root.Length;
            r.material.color = col;
        }
    }

    public void DisplayAttackArea () {
        RemoveArea ();
        selectedArea = AreaCalculator.CalcAttackable(core);
        foreach (var i in selectedArea) {
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