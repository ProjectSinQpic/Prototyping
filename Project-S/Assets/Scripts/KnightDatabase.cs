using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class KnightDatabase : ScriptableObject {

    public AnimationCurve maxHP;
    public AnimationCurve attack;
    public AnimationCurve defense;
    public AnimationCurve maxMP;


    public int speed;

}
