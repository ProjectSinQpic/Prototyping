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
    public Vector2 next_pos;
    public KnightCore next_target;
    public bool isFinished, isDead;

    Subject<string> message;
    public IObservable<string> Message { get { return message.AsObservable(); } }

    public static List<KnightCore> all = new List<KnightCore>();


    void Awake() {
        message = new Subject<string>();
        isSelected = new ReactiveProperty<bool>();
        isSelected.Value = false;
        isFinished = false;
        isDead = false;
        all.Add(this);
        transform.position = MapStatus.ToWorldPos(status.pos) + Vector3.up * 4f;
    }

    void Start() {
        MapPointer.instance.OnClickedKnight
                   .Where(o => !o.GetComponent<KnightCore>().isDead)
                   .Where(o => GameState.knight_state == Knight_State.move)
                   .Where(o => o == gameObject)
                   .Subscribe(_ => isSelected.Value = !isSelected.Value);
        MapPointer.instance.OnClickedKnight
                           .Where(o => !o.GetComponent<KnightCore>().isDead)
                           .Where(o => GameState.knight_state == Knight_State.move)
                           .Where(o => o != gameObject)
                           .Subscribe(_ => isSelected.Value = false);
        isSelected
            .Subscribe(b => {
                GetComponent<SphereCollider>().enabled = !b;
            });

        isSelected
            .Where(b => b == true)
            .Subscribe(_ => { StatusUI.Instance().UpdateUI(status); GameState.operating = this; });
        Init();

        message.Where(x => x == "finish")
            .Subscribe(_ => isFinished = true);
    }

    protected virtual void Init() { }

    public void NextAction(string action) {
        message.OnNext(action);
    }
}
