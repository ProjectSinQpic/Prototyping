using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UniRx;

public class RayDetecter : MonoBehaviour
{
    public IObservable<RaycastHit> OnPointedObject { get { return _onPointedObject; } }
    Subject<RaycastHit> _onPointedObject;

    void Awake() {
        _onPointedObject = new Subject<RaycastHit>();

    }

    void Update() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity)) {
            if (hit.collider) {
                _onPointedObject.OnNext(hit);
            }
        }
    }

}
