using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Skill/SniperShot")]
public class Skill_SniperShot : ActiveSkill {

    protected override void OnSpell() {
        Debug.Log(skillName + " : " + owner.name);
    }

}
