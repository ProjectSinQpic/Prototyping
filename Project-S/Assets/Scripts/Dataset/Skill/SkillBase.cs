using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase : ScriptableObject {

    public string skillName;
    [TextArea] public string explainText;
    protected KnightCore owner;

    public virtual void Init(KnightCore core) {
        owner = core;
    }   

    //バトルが始まった時
    public virtual void OnStartBattle(){}
    //ターンが変わった時
    public virtual void OnBeginTurn(Turn_State turn){}
}
