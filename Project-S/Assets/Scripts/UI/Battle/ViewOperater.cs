using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using UnityEngine;

public class ViewOperater : MonoBehaviour {

    public MapPointer pointer;
    [SerializeField] Transform ViewPos;
    [SerializeField] Transform cameraPos;
    public static ReactiveProperty<Direction> viewDir;

    public Transform target;
    bool isTurning;

    void Awake () {
        target = null;
        viewDir = new ReactiveProperty<Direction> (Direction.NORTH);
    }

    void Start () {
        isTurning = false;
        GameState.selected
            .Subscribe (k => target = k ? k.transform : null);
    }

    void Update () {
        FollowTarget ();
        DragMap ();
    }

    void FollowTarget () {
        if (target == null) return;
        ViewPos.position += (target.position - ViewPos.position) * 0.15f;
    }

    void DragMap () {
        var wheel = Input.GetAxis ("Mouse ScrollWheel");
        if (wheel > 0) cameraPos.localPosition *= 0.8f;
        else if (wheel < 0) cameraPos.localPosition *= 1.25f;

        if (Input.GetMouseButtonDown (2) && !isTurning) {
            //rotSpeed = new Vector2(0, 0);
            viewDir.Value = (Direction) (((int) viewDir.Value + 1) % 4);
            isTurning = true;
            var nextAngle = (transform.rotation.eulerAngles.y + 90) % 360;
            ViewPos.transform.DORotate (Vector3.up * nextAngle, 0.5f)
                .SetEase (Ease.OutCirc)
                .OnComplete (() => isTurning = false);
        }

        var v = new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height) * 2 - Vector2.one;

        if(Mathf.Abs(v.x) > 0.75f) {
            target = null;
            ViewPos.position += transform.right * v.x * 4f;
        }
        if(Mathf.Abs(v.y) > 0.75f) {
            target = null;
            ViewPos.position += transform.forward * v.y * 4f;
        }

    }
}