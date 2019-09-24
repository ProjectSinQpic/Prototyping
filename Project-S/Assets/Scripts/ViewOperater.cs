using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;

public class ViewOperater : MonoBehaviour {

    [SerializeField] Transform ViewPos;
    [SerializeField] Transform cameraPos;
    Vector2 rotSpeed;
    bool isTurning;
    public static ReactiveProperty<Direction> viewDir;

    void Awake() {
        viewDir = new ReactiveProperty<Direction>(Direction.NORTH);
    }

    void Start() {
        isTurning = false;
    }

    void Update() {
        MoveView();
        DragMap();
    }

    void MoveView() {
        ViewPos.transform.Rotate(Vector3.down * rotSpeed.x, Space.World);
        ViewPos.transform.Rotate(Vector3.right * rotSpeed.y, Space.World);
        rotSpeed *= 0.75f;
    }

    void DragMap() {
        var wheel = Input.GetAxis("Mouse ScrollWheel");
        if (wheel > 0) cameraPos.localPosition *= 0.8f;
        else if (wheel < 0) cameraPos.localPosition *= 1.25f;

        if (Input.GetMouseButtonDown(2) && !isTurning) {
            //rotSpeed = new Vector2(0, 0);
            viewDir.Value = (Direction)(((int)viewDir.Value + 1) % 4);
            isTurning = true;
            var nextAngle = (transform.rotation.eulerAngles.y + 90) % 360;
            ViewPos.transform.DORotate(Vector3.up * nextAngle, 0.5f)
                .SetEase(Ease.OutCirc)
                .OnComplete(() => isTurning = false);
        }  
        //if (Input.GetMouseButton(2)) {
        //    rotSpeed += new Vector2(-Input.GetAxis("Mouse X"), 0);
        //}
        if (Input.GetMouseButton(1)) {
            var vx = Input.GetAxis("Mouse X") * transform.right;
            var vy = Input.GetAxis("Mouse Y") * transform.forward;
            ViewPos.position -= (vx + vy) * 4f;
        }
    }
}
