using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

public class KnightMovement : KnightParts
{
    readonly int moveFrame = 10;
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
        StartCoroutine(MoveToPointCoroutine(goal));
    }


    IEnumerator MoveToPointCoroutine(Vector2 goal) {
        isMoving = true;
        Vector2 diff = goal - core.status.pos;
        for(int i = 0; i < moveFrame * Mathf.Abs(diff.x); i++) {
            transform.position += Vector3.right * MapStatus.MAPCHIP_SIZE / moveFrame * (diff.x > 0 ? 1 : -1);
            yield return null;
        }
        for (int i = 0; i < moveFrame * Mathf.Abs(diff.y); i++) {
            transform.position += Vector3.forward * MapStatus.MAPCHIP_SIZE / moveFrame * (diff.y > 0 ? -1 : 1);
            yield return null;
        }
        core.status.pos = goal;
        isMoving = false;
    }

    bool CheckMovable(Vector2 point) {
        return core.movableArea.Contains(point);
    }

}
