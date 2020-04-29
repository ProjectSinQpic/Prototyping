using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public struct MovableArea {
    public Vector2 pos;
    public string root;
}

public class KnightCore : MonoBehaviour {
    public KnightStatus status;
    public Vector2 next_pos, prev_pos;
    public int storedCoolDown;
    public KnightCore next_target;
    public bool isFinished, isDead;

    Subject<string> message;
    public IObservable<string> Message { get { return message.AsObservable (); } }

    public static List<KnightCore> all = new List<KnightCore> ();

    void Awake () {
        message = new Subject<string> ();
        isFinished = false;
        isDead = false;
        all.Add (this);
        transform.position = MapStatus.ToWorldPos (status.pos) /* + Vector3.up * 4f*/ ;

        prev_pos = status.pos;
        storedCoolDown = 0;
    }

    void Start () {

        GameState.selected
            .Where (b => b == this)
            .Subscribe (_ => OnSelected ());

        GameState.selected
            .Select (b => b == this)
            .DistinctUntilChanged ()
            .Where (b => !b)
            .Subscribe (_ => OnNotSelected ());

        message.Where (x => x == "finish")
            .Subscribe (_ => OnFinish());

        Init ();
    }

    protected virtual void Init () { }

    public void NextAction (string action) {
        Debug.Log ("Action : " + action);
        message.OnNext (action);
    }

    void OnSelected () {
        StatusUI.Instance ().UpdateUI (status);
        GetComponent<BoxCollider> ().enabled = false;
        NextAction ("look");
    }

    void OnNotSelected () {
        //TODO UI消去処理作っておく
        GetComponent<BoxCollider> ().enabled = true;
        NextAction ("look_cancel");
    }

    void OnFinish() {
        isFinished = true;
        status.coolDown += storedCoolDown;
        storedCoolDown = 0;
    }

}