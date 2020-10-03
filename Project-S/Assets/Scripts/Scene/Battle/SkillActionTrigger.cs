using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SkillActionTrigger : MonoBehaviour {

    public static SkillActionTrigger instance;

    void Awake() {
        instance = this;
    }

    public void OnBeginTurn(Turn_State turn) {
        KnightCore.all.ForEach(knight => {
            knight.status.skills
                .Where(Skill => !Skill.GetIsDeleted())
                .ToList()
                .ForEach(skill => skill.OnBeginTurn(turn));
        });
    }


}
