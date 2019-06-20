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
    
    public static List<KnightCore> all = new List<KnightCore>();


    void Awake() {
        isSelected = new ReactiveProperty<bool>();
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

        isSelected
            .Where(b => b == true)
            .Subscribe(_ => StatusUI.Instance().UpdateUI(status));
    }

    public void Attack() {
        Debug.Log("こうげき！");
    }
    

    public void Wait() {

    }
}
