using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class MapPointer : MonoBehaviour {

    public static MapPointer instance;

    public IObservable<Vector2> OnClickedMap;
    public IObservable<GameObject> OnClickedKnight;

    [SerializeField] RayDetecter detecter;
    [SerializeField] GameObject cursor;

    Vector2 cursorPos;
    Vector2 prevPos;
    public GameObject pointedKnight;
    Vector2 rotSpeed;

    void Awake () {

        if (instance == null) instance = this;

        OnClickedMap = this.UpdateAsObservable ()
            .Where (_ => Input.GetMouseButtonDown (0) && pointedKnight == null)
            .Where (_ => !MenuGenerator.Instance ().isLocked)
            .Select (_ => cursorPos);

        OnClickedKnight = this.UpdateAsObservable ()
            .Where (_ => Input.GetMouseButtonDown (0) && pointedKnight != null)
            .Where (_ => !MenuGenerator.Instance ().isLocked)
            .Select (_ => pointedKnight);

        OnClickedMap.Subscribe (_ => Debug.Log (cursorPos));
        OnClickedKnight.Subscribe (o => Debug.Log (o));

        this.UpdateAsObservable ()
            .Select (_ => MenuGenerator.Instance ().isLocked)
            .DistinctUntilChanged ()
            .Subscribe (x => cursor.GetComponent<MeshRenderer> ().enabled = !x);

    }

    void Start () {
        detecter.OnPointedObject.Where (o => o.collider.tag == "MapDetect")
            .Subscribe (o => {
                UpdateCursorPos (o.point);
            });

        detecter.OnPointedObject.Where (o => o.collider.tag.Contains ("Knight"))
            .Subscribe (o => {
                pointedKnight = o.collider.gameObject;
            });
    }

    void UpdateCursorPos (Vector3 point) {
        float ms = MapStatus.MAPCHIP_SIZE;
        Vector3 v = new Vector3 (Mathf.Floor (point.x / ms) + 0.5f, 0, Mathf.Floor (point.z / ms + 0.5f)) * ms;
        cursor.transform.position = v + Vector3.up;
        cursorPos = MapStatus.ToMapPos (v);
        if (prevPos != cursorPos) pointedKnight = null;
        prevPos = cursorPos;
    }

}