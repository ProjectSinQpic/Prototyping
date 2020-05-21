using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

[System.Serializable]
public class SelectedArea {
    public Vector2 pos;
    public string root;

    public AreaType type;
}

public enum AreaType {
    move,
    attack,
    move_attack
}

public class KnightCore : MonoBehaviour {
    public KnightStatus status;

    public KnightStatusData statusData {
        get{
            return KnightStatusData.Add(status.actual, status.delta);
        }
    }

    [HideInInspector]
    public Vector2 next_pos, prev_pos;
    
    [HideInInspector]
    public int storedCoolDown;

    [HideInInspector]
    public KnightCore next_target;

    [HideInInspector]


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
        GetComponent<BoxCollider> ().enabled = false;
        NextAction ("look");
    }

    void OnNotSelected () {
        GetComponent<BoxCollider> ().enabled = true;
        NextAction ("look_cancel");
    }

    void OnFinish() {
        isFinished = true;
        status.coolDown += storedCoolDown;
        storedCoolDown = 0;
    }


    public static List<KnightCore> GetAllies(KnightCore core) {
        if(KnightCore_Player01.player_all.Contains(core)) return KnightCore_Player01.player_all;
        else if(KnightCore_Player02.player_all.Contains(core)) return KnightCore_Player02.player_all;
        else if(KnightCore_Enemy.enemy_all.Contains(core)) return KnightCore_Enemy.enemy_all;
        else return new List<KnightCore>();
    }
}