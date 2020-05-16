using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//プレイヤーが対象を指定して発動するスキル
public class KnightSelectSkill : ActiveSkill {

    public AreaShapeType areaShape;

    protected Vector2 pos;
    protected int value;


    protected override void OnWait() {
        OnInit();
        owner.GetComponent<KnightSkill>().SelectKnight(areaShape, pos, value, OnTargeted);
    }

    protected virtual void OnInit() {}

}
