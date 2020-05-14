using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillBase : ScriptableObject {

    KnightCore owner;

    public void SetOwner(KnightCore core) {
        owner = core;
    }   


}
