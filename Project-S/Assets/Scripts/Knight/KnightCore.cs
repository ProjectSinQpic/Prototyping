using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;


public enum KnightAction {
    look,
    look_cancel,
    move,
    move_cancel,
    select,
    select_cancel,
    attack_look,
    attack_prepare,
    attack,
    attack_cancel,
    counter_attack,
    skill_look_knight,
    skill_prepare,
    skill,
    skill_attack,
    skill_cancel,
    get_mana,
    die,
    finish,

}



/**
    全てのユニットに共通の処理，全KnightParts間で共有するフィールドを持つクラス
 */

public class KnightCore : MonoBehaviour {
    public KnightStatus status;

    public KnightStatusData statusData {
        get{
            return KnightStatusData.Add(status.actual, status.delta);
        }
    }

    //KnightParts間で共有するフィールド

    [HideInInspector] public List<SelectedArea> selectedArea;
    [HideInInspector] public Vector2 next_pos, prev_pos;
    [HideInInspector] public int storedCoolDown;
    [HideInInspector] public List<KnightCore> targets;
    [HideInInspector] public AttackResult attackResult;
    [HideInInspector] public ActiveSkill nowSkill;
    [HideInInspector] public bool isFinished, isDead;

    ////

    Subject<KnightAction> message;
    public IObservable<KnightAction> Message { get { return message.AsObservable (); } }

    public static List<KnightCore> all = new List<KnightCore> ();


    void Awake () {
        message = new Subject<KnightAction>();
        isFinished = false;
        isDead = false;
        all.Add (this);
        transform.position = MapStatus.ToWorldPos (status.pos) /* + Vector3.up * 4f*/ ;

        prev_pos = status.pos;
        storedCoolDown = 0;
    }

    void Start () {

        GameState.instance.selected
            .Where (b => b == this)
            .Subscribe (_ => OnSelected ());

        GameState.instance.selected
            .Select (b => b == this)
            .DistinctUntilChanged ()
            .Where (b => !b)
            .Subscribe (_ => OnNotSelected ());

        message.Where (x => x == KnightAction.finish)
            .Subscribe (_ => OnFinish());

        Init ();
        attackResult = new AttackResult(this);
    }

    protected virtual void Init () { }

    public void NextAction (KnightAction action) {
        Debug.Log (action);
        message.OnNext (action);
    }

    void OnSelected () {
        if(isOperable()) GetComponent<BoxCollider> ().enabled = false;
        NextAction (KnightAction.look);
    }

    void OnNotSelected () {
        GetComponent<BoxCollider> ().enabled = true;
        NextAction (KnightAction.look_cancel);
    }

    void OnFinish() {
        Debug.Log("turn end");
        isFinished = true;
        status.coolDown += storedCoolDown;
        storedCoolDown = 0;
        attackResult = new AttackResult(this);
    }


    public static List<KnightCore> GetAllies(KnightCore core) { //TODO: 余裕があれば修正したい
        if(KnightCore_Player01.player_all.Contains(core)) return KnightCore_Player01.player_all;
        else if(KnightCore_Player02.player_all.Contains(core)) return KnightCore_Player02.player_all;
        else if(KnightCore_Enemy.enemy_all.Contains(core)) return KnightCore_Enemy.enemy_all;
        else return new List<KnightCore>();
    }

    protected virtual bool isOperable() {
        return true;
    }
}