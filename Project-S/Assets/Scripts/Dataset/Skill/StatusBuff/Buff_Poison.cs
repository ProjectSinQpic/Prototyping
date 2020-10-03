using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Buff/Poison")]

public class Buff_Poison : StatusBuff {

    int count;

    protected override void OnStart() {
        count = GetParam("duration");
    }


    public override void OnBeginTurn(Turn_State turn) {
        owner.status.ApplyStatus(-GetParam("damage"), 0, 0);
        count--;
        if(count <= 0) Delete();
    }

}
