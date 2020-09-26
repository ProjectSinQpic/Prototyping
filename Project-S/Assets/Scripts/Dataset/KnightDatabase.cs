using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class KnightDatabase : ScriptableObject {

    public int maxHP;
    public int maxMP;
    public int attack;
    public int defense;

    public int moveRange;
    public int attackRange;

    public List<SkillBase> skills;

    public CharacterView view;

}