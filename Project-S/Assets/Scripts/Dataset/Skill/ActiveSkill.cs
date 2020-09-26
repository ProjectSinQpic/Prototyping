using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ActiveSkill : SkillBase {

    //スキルの使用に必要なMPを計算し、返す
    public virtual int GetRequiredMana() { return 0; }


    // スキル選択画面で選ばれた時
    public virtual void OnSelected(){}

    // 決定ボタンを押してスキルが開始する時
    public virtual void OnSpell(){}



    ////継続効果(duration > 1)の場合、必ずこのメソッドをどこかに記述する
    //protected void Update() {
    //    if(!isActive) return;
    //    OnSpell();
    //    count++;
    //    if(count >= GetParam("duration", 1)) {
    //        OnFinish();
    //        isActive = false;
    //    }
    //}


}
