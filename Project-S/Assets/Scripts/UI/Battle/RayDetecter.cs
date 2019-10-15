using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class RayDetecter : MonoBehaviour {
    public IObservable<RaycastHit> OnPointedObject { get { return _onPointedObject; } }
    Subject<RaycastHit> _onPointedObject;

    void Awake () {
        _onPointedObject = new Subject<RaycastHit> ();

    }

    void Update () {
        Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
        var hit = Physics.RaycastAll (ray.origin, ray.direction, Mathf.Infinity);
        foreach (var i in hit) {
            if (i.collider.gameObject.layer != LayerMask.NameToLayer ("Detectable")) continue;
            _onPointedObject.OnNext (i);
        }
    }

}