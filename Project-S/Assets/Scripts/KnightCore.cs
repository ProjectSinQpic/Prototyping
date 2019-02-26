using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UniRx;

public class KnightCore : MonoBehaviour
{
    public KnightStatus status;
    public ReactiveProperty<bool> isSelected;
    public List<Vector2> movableArea;


    void Awake() {
        isSelected = new ReactiveProperty<bool>();
        movableArea = new List<Vector2>();
    }

    void Start() {
        isSelected.Value = false;
        transform.position = MapStatus.ToWorldPos(status.pos) + Vector3.up * 4;
        MapPointer.instance.OnClickedKnight
                   .Where(o => o == gameObject)
                   .Subscribe(_ => isSelected.Value = !isSelected.Value);
        MapPointer.instance.OnClickedKnight
                           .Where(o => o != gameObject)
                           .Subscribe(_ => isSelected.Value = false);
    }
    
}
