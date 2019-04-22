using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

public class KnightMovement : KnightParts
{
    readonly int moveFrame = 5;
    bool isMoving = false;
    KnightDisplayArea _disp;

    void Awake() {
        _disp = core.GetComponent<KnightDisplayArea>();
    }

    void Start() {
        MapPointer.instance.OnClickedMap
                           .Where(_ => core.isSelected.Value == true)
                           .Subscribe(v => MoveToPoint(v));
    }

    public void MoveToPoint(Vector2 goal) {
        if (isMoving) return;
        core.isSelected.Value = false; //あとで変更する
        if (!CheckMovable(goal)) return;
        StartCoroutine(MoveToPointCoroutine(_disp.movableArea.Find(m => m.pos == goal)));
    }


    IEnumerator MoveToPointCoroutine(MovableArea area) {
        isMoving = true;
        foreach(var d in area.root) {
            Vector3 dir = d == 'r' ? Vector3.right :
                          d == 'l' ? Vector3.left :
                          d == 'u' ? Vector3.back : Vector3.forward;
            for (int i = 0; i < moveFrame; i++) {
                transform.position += dir * MapStatus.MAPCHIP_SIZE / moveFrame;
                yield return null;
            }
        }
        core.status.pos = area.pos;
        MenuGenerator.Instance().Create(new Dictionary<string, UnityEngine.Events.UnityAction> {
            { "攻撃", () => { Debug.Log("こうげき！"); MenuGenerator.Instance().Close(); } },
            { "待機", () => { MenuGenerator.Instance().Close(); } },
        }, new Vector3(Screen.width / 2 - 180, Screen.height / 2 - 150, 0));
        isMoving = false;
    }

    bool CheckMovable(Vector2 point) {
        return _disp.movableArea.Select(m => m.pos).Contains(point);
    }

}
