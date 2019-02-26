using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UniRx;

public struct MovableArea {
    public Vector2 pos;
    public string root;
}

public class KnightCore : MonoBehaviour
{
    public KnightStatus status;
    public ReactiveProperty<bool> isSelected;
    public List<MovableArea> movableArea;
    public static List<KnightCore> all = new List<KnightCore>();


    void Awake() {
        isSelected = new ReactiveProperty<bool>();
        movableArea = new List<MovableArea>();
    }

    void Start() {
        isSelected.Value = false;
        transform.position = MapStatus.ToWorldPos(status.pos) + Vector3.up * 4f;
        all.Add(this);
        MapPointer.instance.OnClickedKnight
                   .Where(o => o == gameObject)
                   .Subscribe(_ => isSelected.Value = !isSelected.Value);
        MapPointer.instance.OnClickedKnight
                           .Where(o => o != gameObject)
                           .Subscribe(_ => isSelected.Value = false);
    }
    
}
