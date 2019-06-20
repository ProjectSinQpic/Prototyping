using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class MapPointer : MonoBehaviour
{

    public static MapPointer instance;

    public IObservable<Vector2> OnClickedMap;
    public IObservable<GameObject> OnClickedKnight;

    [SerializeField] RayDetecter detecter;
    [SerializeField] GameObject cursor;
    [SerializeField] Transform cameraPos;
    [SerializeField] Transform ViewPos;

    Vector2 cursorPos;
    public GameObject pointedKnight;
    Vector2 rotSpeed;


    void Awake() {

        if (instance == null) instance = this;

        OnClickedMap = this.UpdateAsObservable()
                           .Where(_ => Input.GetMouseButtonDown(0) && pointedKnight == null)
                           .Where(_ => !MenuGenerator.Instance().isLocked)
                           .Select(_ => cursorPos);

        OnClickedKnight = this.UpdateAsObservable()
                              .Where(_ => Input.GetMouseButtonDown(0) && pointedKnight != null)
                              .Where(_ => !MenuGenerator.Instance().isLocked)
                              .Select(_ => pointedKnight);

        this.UpdateAsObservable()
            .Select(_ => MenuGenerator.Instance().isLocked)
            .DistinctUntilChanged()
            .Subscribe(x => cursor.GetComponent<MeshRenderer>().enabled = !x);

    }

    void Start() {
        detecter.OnPointedObject.Where(o => o.collider.tag == "MapDetect")
                                .Subscribe(o => {
                                    UpdateCursorPos(o.point);
                                    pointedKnight = null;
                                });

        detecter.OnPointedObject.Where(o => o.collider.tag == "Knight")
                                .Subscribe(o => {
                                    pointedKnight = o.collider.gameObject;
                                });
    }

    void Update() {
        MoveView();
        DragMap();
    }

    void UpdateCursorPos(Vector3 point) {
        float ms = MapStatus.MAPCHIP_SIZE;
        Vector3 v = new Vector3(Mathf.Floor(point.x / ms), 0, Mathf.Floor(point.z / ms)) * ms;
        cursor.transform.position = v + Vector3.up;
        cursorPos = MapStatus.ToMapPos(v);
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

        if (Input.GetMouseButtonDown(1)) rotSpeed = new Vector2(0, 0);
        if (Input.GetMouseButton(1)) {
            rotSpeed += new Vector2(-Input.GetAxis("Mouse X"), 0);
        }
        if (Input.GetMouseButton(2)) {
            cameraPos.localPosition -= new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0) * 2f;
        }
    }

}
