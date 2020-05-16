using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkill : SkillBase {

    public int mana;
    public int rest;

    [HideInInspector]
    public bool isActive = false;

    //効果が何ターン続くか
    public int duration;
    protected int count = 0;

    protected List<KnightCore> targets;

    public void Activate() {
        owner.GetComponent<KnightSkill>().SubscribeSkill(this);
        OnWait();
    }

    protected virtual void OnWait(){}
    protected virtual void OnSpell(){}
    protected virtual void OnFinish(){}


    protected void OnStart(List<KnightCore> t) {
        isActive = true;
        count = 0;
        targets = t;
        owner.GetComponent<KnightSkill>().OnSpell();
        Update();
    }

    //継続効果(duration > 1)の場合、必ずこのメソッドをどこかに記述する
    protected void Update() {
        if(!isActive) return;
        OnSpell();
        count++;
        if(count >= duration) {
            OnFinish();
            isActive = false;
        }

    }

    protected void OnTargeted(List<KnightCore> targets) {
        MenuGenerator.Instance ().Create (new Dictionary<string, UnityEngine.Events.UnityAction> { 
            {"決定", () => { MenuGenerator.Instance().Close(); OnStart(targets);}},
            {"キャンセル", () => { MenuGenerator.Instance().Close(); owner.GetComponent<KnightSkill>().OnCancel();}}
        }, new Vector3 (0, -Screen.height / 2 + 200, 0), "skill_target", true);
    }


}
