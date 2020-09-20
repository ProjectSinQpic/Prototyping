using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ActiveSkill : SkillBase {

    [HideInInspector]
    public bool isActive = false;

    //効果が何ターン続くか
    protected int count = 0;


    // スキル選択画面で選ばれた時
    public void Activate() {
        owner.nowSkill = this;
        OnWait();
    }

    protected virtual void OnWait(){}
    protected virtual void OnSpell(){}
    protected virtual void OnFinish(){}


    // 決定ボタンを押してスキルが開始する時
    public void OnStart() {
        isActive = true;
        count = 0;
        Update();
    }

    //継続効果(duration > 1)の場合、必ずこのメソッドをどこかに記述する
    protected void Update() {
        if(!isActive) return;
        OnSpell();
        count++;
        if(count >= GetParam("duration", 1)) {
            OnFinish();
            isActive = false;
        }

    }

}
