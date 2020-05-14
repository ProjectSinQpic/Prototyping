using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveSkill : SkillBase {

    //バトルが始まった時
    public abstract void OnStartBattle();
    //ターンが変わった時
    public abstract void OnBeginTurn(Turn_State turn);
}
