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

    public List<Sprite> image_idle_front;
    public List<Sprite> image_idle_back;
    public List<Sprite> image_move_front;
    public List<Sprite> image_move_back;
    public List<Sprite> image_attack_front;
    public List<Sprite> image_attack_back;

}
