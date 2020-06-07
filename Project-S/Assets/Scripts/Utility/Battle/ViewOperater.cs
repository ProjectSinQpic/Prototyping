﻿using System.Collections;
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
    public static bool isLocked;
    public static bool isFocusing;

    public float maxDistance;
    public int zoomMax, zoomMin;
    int zoom;

    public float moveSpeed;

    public float focusSpeed;
    public float focusRate;
    public Ease focusEasing; 

    void Awake () {
        target = null;
        viewDir = new ReactiveProperty<Direction> (Direction.NORTH);
        isFocusing = false;
        isLocked = false;
    }

    void Start () {
        isTurning = false;
        GameState.instance.selected
            .Subscribe (k => target = k ? k.transform : null);
        StatusUI.Instance().target
            .Subscribe(t => {
                if(t == null) FocusOut();
                else FocusIn(t.transform);
            });    

    }

    void Update () {
        FollowTarget ();
        if(!UIWindow.isLocked && !isLocked) {
            MoveCamera();
            ZoomCamera();
            if(Input.GetMouseButtonDown (2) && !isTurning) TurnCamera();
        }
    }

    void FollowTarget () {
        if (target == null) return;
        ViewPos.position += (target.position - ViewPos.position) * 0.15f;
    }
    void MoveCamera() {
        var v = new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height) * 2 - Vector2.one;
        var newPos = ViewPos.position;
        if(Mathf.Abs(v.x) > 0.75f) {
            target = null;
            newPos += transform.right * v.x * moveSpeed;
        }
        if(Mathf.Abs(v.y) > 0.75f) {
            target = null;
            newPos += transform.forward * v.y * moveSpeed;
        }
        if(newPos.magnitude <= maxDistance)
            ViewPos.position = newPos;
    }

    void ZoomCamera() {
        var wheel = Input.GetAxis ("Mouse ScrollWheel");
        if (wheel > 0 && zoom < zoomMax) {
            cameraPos.localPosition *= 0.8f;
            zoom++;
        }
        else if (wheel < 0 && zoom > zoomMin) {
            cameraPos.localPosition *= 1.25f;
            zoom--;
        }
    }

    void TurnCamera() {
        //rotSpeed = new Vector2(0, 0);
        viewDir.Value = (Direction) (((int) viewDir.Value + 1) % 4);
        isTurning = true;
        var nextAngle = (transform.rotation.eulerAngles.y + 90) % 360;
        ViewPos.transform.DORotate (Vector3.up * nextAngle, 0.5f)
            .SetEase (Ease.OutCirc)
            .OnComplete (() => isTurning = false);
    }

    public void FocusIn(Transform target) {
        isFocusing = true;
        this.target = target;
        cameraPos.DOLocalMove(cameraPos.localPosition / focusRate, focusSpeed).OnComplete(() => isFocusing = false).SetEase(focusEasing);
        isLocked = true;
    }

    public void FocusOut() {
        this.target = null;
        if(isLocked) {
            isFocusing = true;
            cameraPos.DOLocalMove(cameraPos.localPosition * focusRate, focusSpeed).OnComplete(() => isFocusing = false).SetEase(focusEasing);
        }
        isLocked = false;
    }

}