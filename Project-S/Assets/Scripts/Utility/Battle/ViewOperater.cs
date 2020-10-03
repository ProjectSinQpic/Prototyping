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
    public bool isLocked;
    public bool isFocusing;

    public float maxDistance;
    public int zoomMax, zoomMin;
    int zoom;

    public float moveSpeed;

    public float focusSpeed;
    public float focusRate;

    public float focusOffsetX;
    public Ease focusEasing; 

    bool isActive;

    public static ViewOperater instance;


    void Awake () {
        if(instance == null) instance = this;
        target = null;
        viewDir = new ReactiveProperty<Direction> (Direction.NORTH);
        isFocusing = false;
        isLocked = false;
        isActive = true;
    }

    void Start () {
        isTurning = false;
        GameState.instance.selected
            .Subscribe (k => target = k ? k.transform : null);
        StatusWindow.Instance().target
            .Subscribe(t => {
                if(t == null) FocusOut();
                else FocusIn(t.transform);
            });    

    }

    void Update () {
        FollowTarget ();
        if(!UIWindow.isLocked && !isLocked && isActive) {
            MoveCamera();
            ZoomCamera();
            if(Input.GetMouseButtonDown (2) && !isTurning) TurnCamera();
        }
    }

    void FollowTarget () {
        if (target == null) return;
        ViewPos.localPosition += (target.localPosition - ViewPos.localPosition) * 0.15f;
    }
    void MoveCamera() {
        var v = new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height) * 2 - Vector2.one;
        var newPos = ViewPos.localPosition;
        if(Mathf.Abs(v.x) > 0.75f) {
            target = null;
            newPos += transform.right * v.x * moveSpeed;
        }
        if(Mathf.Abs(v.y) > 0.75f) {
            target = null;
            newPos += transform.forward * v.y * moveSpeed;
        }
        if(newPos.magnitude <= maxDistance)
            ViewPos.localPosition = newPos;
    }

    void ZoomCamera() {
        var wheel = Input.GetAxis ("Mouse ScrollWheel");
        if (wheel > 0 && zoom < zoomMax) {
            cameraPos.localPosition *= 0.8f;
            //cameraPos.GetComponent<Camera>().orthographicSize -= 10;
            zoom++;
        }
        else if (wheel < 0 && zoom > zoomMin) {
            cameraPos.localPosition *= 1.25f;
            //cameraPos.GetComponent<Camera>().orthographicSize += 10;
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
        cameraPos.DOLocalMove(cameraPos.localPosition / focusRate + cameraPos.right * focusOffsetX, focusSpeed).OnComplete(() => isFocusing = false).SetEase(focusEasing);
        //var camera = cameraPos.GetComponent<Camera>();
        //cameraPos.DOLocalMoveX(cameraPos.localPosition.x + focusOffsetX, focusSpeed);
        //DOTween.To(() => camera.orthographicSize, x => camera.orthographicSize = x, camera.orthographicSize / focusRate, focusSpeed)
        //    .OnComplete(() => isFocusing = false).SetEase(focusEasing);

        isLocked = true;
    }

    public void FocusOut() {
        this.target = null;
        if(isLocked) {
            isFocusing = true;
            cameraPos.DOLocalMove((cameraPos.localPosition - cameraPos.right * focusOffsetX) * focusRate, focusSpeed).OnComplete(() => isFocusing = false).SetEase(focusEasing);
            //var camera = cameraPos.GetComponent<Camera>();
            //cameraPos.DOLocalMoveX(cameraPos.localPosition.x - focusOffsetX, focusSpeed);
            //DOTween.To(() => camera.orthographicSize, x => camera.orthographicSize = x, camera.orthographicSize * focusRate, focusSpeed)
            //    .OnComplete(() => isFocusing = false).SetEase(focusEasing);
        }
        isLocked = false;
    }

    public void SetActive(bool isActive) {
        this.isActive = isActive;
    }

}