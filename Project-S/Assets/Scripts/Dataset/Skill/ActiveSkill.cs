using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ActiveSkill : SkillBase {

    public int mana;
    public int rest;

    [HideInInspector]
    public bool isActive = false;

    //効果が何ターン続くか
    public int duration;
    protected int count = 0;

    public void Activate() {
        owner.nowSkill = this;
        OnWait();
    }

    protected virtual void OnWait(){}
    protected virtual void OnSpell(){}
    protected virtual void OnFinish(){}


    public void OnStart() {
        isActive = true;
        count = 0;
        owner.status.MP -= owner.nowSkill.mana;
        owner.storedCoolDown += owner.nowSkill.rest;
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




}
