using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class MapPointer : MonoBehaviour
{
    public IObservable<Vector3> OnClick;
    [SerializeField]
    GameObject cursor;
    Vector3 cursorPos;

    void Awake() {

        OnClick = this.UpdateAsObservable()
                      .Where(_ => Input.GetMouseButtonDown(0))
                      .Select(_ => cursorPos);
    }

    void Update() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity)) {
            if (hit.collider) {
                UpdateCursorPos(hit.point);
                SetCursor();
            }
        }
    }

    void UpdateCursorPos(Vector3 point) {
        cursorPos = new Vector3(Mathf.Floor(point.x / 10), 0, Mathf.Floor(point.z / 10));
    }

    void SetCursor() {
        Debug.Log(cursorPos);
        cursor.transform.position = cursorPos * 10;
    }

}
