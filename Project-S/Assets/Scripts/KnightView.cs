using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class KnightView : KnightParts {

    public Sprite front, back; // 後で変更
    SpriteRenderer sp;
    
    void Start() {
        sp = GetComponent<SpriteRenderer>();
        ViewOperater.viewDir
            .Subscribe(d => ChangeDir(d));
    }

    void ChangeDir(Direction dir) {
        var rot = new Vector3(0, (int)ViewOperater.viewDir.Value * 90 + 45, 0);
        transform.rotation = Quaternion.Euler(rot);

        Direction spriteDir = (Direction)Mathf.Repeat((int)core.status.dir - (int)ViewOperater.viewDir.Value, 4);
        Debug.Log(spriteDir);
        //後で変更↓
        var s = transform.localScale; 
        switch (spriteDir) { 
            case Direction.NORTH:
                sp.sprite = front;
                transform.localScale = new Vector3(Mathf.Abs(s.x), s.y, s.z);
                    break;
            case Direction.EAST:
                sp.sprite = back;
                transform.localScale = new Vector3(Mathf.Abs(s.x), s.y, s.z);
                break;
            case Direction.SOUTH:
                sp.sprite = back;
                transform.localScale = new Vector3(-Mathf.Abs(s.x), s.y, s.z);
                break;
            case Direction.WEST:
                sp.sprite = front;
                transform.localScale = new Vector3(-Mathf.Abs(s.x), s.y, s.z);
                break;
        }
        //後で変更↑
    }
}
