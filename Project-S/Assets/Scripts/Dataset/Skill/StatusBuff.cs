using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusBuff : SkillBase {

    public bool isDeleted; 

    public override void Init(KnightCore core) {
        owner = core;
        isDeleted = false;
        OnStart();
    }   
    
    //バフ効果が付与された時
    protected virtual void OnStart() { }

    //バフ効果を停止する
    protected void Delete() {
        OnDeleted();
        isDeleted = true;
    }

    //バフ効果が終わる時
    protected virtual void OnDeleted() { }

}