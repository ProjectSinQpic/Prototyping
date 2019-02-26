using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

public class KnightMovement : KnightParts
{
    readonly int moveFrame = 5;
    bool isMoving = false;

    void Start() {
        MapPointer.instance.OnClickedMap
                           .Where(_ => core.isSelected.Value == true)
                           .Subscribe(v => MoveToPoint(v));
    }

    public void MoveToPoint(Vector2 goal) {
        if (isMoving) return;
        core.isSelected.Value = false;
        if (!CheckMovable(goal)) return;
        StartCoroutine(MoveToPointCoroutine(core.movableArea.Find(m => m.pos == goal)));
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
        isMoving = false;
    }

    bool CheckMovable(Vector2 point) {
        return core.movableArea.Select(m => m.pos).Contains(point);
    }

}
