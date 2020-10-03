using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Passive/SimpleBuff")]
public class Skill_SimpleBuff : PassiveSkill {

    public StatusDataType type;
    public bool isAddive;

    public override List<BuffData> GetBuff() {
        return new List<BuffData>(){ new BuffData(type, GetParam("value"), isAddive) };
    }

}
