using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class CharacterView : ScriptableObject {

    public string characterName;    
    public Sprite characterImage;
    public Vector2 imageOffset_StatusUI;
    public Vector2 imageOffset_AttackResultUI_a, imageOffset_AttackResultUI_b;

    public List<Sprite> image_idle_front;
    public List<Sprite> image_idle_back;
    public List<Sprite> image_move_front;
    public List<Sprite> image_move_back;
    public List<Sprite> image_attack_front;
    public List<Sprite> image_attack_back;
}
