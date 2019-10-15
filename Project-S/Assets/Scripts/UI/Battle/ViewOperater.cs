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

    Transform target;
    Vector2 rotSpeed;
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
        MoveView ();
        FollowTarget ();
        DragMap ();
    }

    void MoveView () {
        ViewPos.transform.Rotate (Vector3.down * rotSpeed.x, Space.World);
        ViewPos.transform.Rotate (Vector3.right * rotSpeed.y, Space.World);
        rotSpeed *= 0.75f;
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
        //if (Input.GetMouseButton(2)) {
        //    rotSpeed += new Vector2(-Input.GetAxis("Mouse X"), 0);
        //}
        if (Input.GetMouseButton (1)) {
            target = null;
            var vx = Input.GetAxis ("Mouse X") * transform.right;
            var vy = Input.GetAxis ("Mouse Y") * transform.forward;
            ViewPos.position -= (vx + vy) * 4f;
        }
    }
}