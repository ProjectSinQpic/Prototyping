using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillActionTrigger : MonoBehaviour {

    public static SkillActionTrigger instance;

    void Awake() {
        instance = this;
    }

    public void OnBeginTurn(Turn_State turn) {
        KnightCore.all.ForEach(knight => {
            knight.status.skills.ForEach(skill => skill.OnBeginTurn(turn));
        });
    }

}
