using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkill : SkillBase {

    public int mana;

    [HideInInspector]
    public bool isActive = false;

    //効果が何ターン続くか
    public int duration;
    int count = 0;

    public void Activate() {
        isActive = true;
        Update();
    }

    protected virtual void OnSpell(){}
    protected virtual void OnFinish(){}


    //継続効果(duration > 1)の場合、必ずこのメソッドをどこかに記述する
    protected void Update() {
        OnSpell();
        count++;
        if(count >= duration) {
            OnFinish();
            isActive = false;
        }
    }

}
