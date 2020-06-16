using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class KnightMovement : KnightParts {
    public bool isMoving = false;
    public KnightView view;
    readonly int moveFrame = 5;

    void Start () {
        core.Message
            .Where (x => x == KnightAction.move)
            .Subscribe (_ => MoveToPoint (core.next_pos));

        core.Message
            .Where (x => x == KnightAction.finish)
            .Subscribe (_ => core.prev_pos = core.status.pos);

        core.Message
            .Where (x => x == KnightAction.select_cancel)
            .Subscribe (_ => OnCancel());

    }

    public void MoveToPoint (Vector2 goal) {
        if (isMoving) return;
        core.NextAction (KnightAction.look_cancel);
        if (!CheckMovable (goal)) {
            core.NextAction (KnightAction.move_cancel);
            return;
        }
        var sa = core.selectedArea
            .Where(s => s.type == AreaType.move || s.type == AreaType.move_attack)
            .Where(m => m.pos == goal).First();
        StartCoroutine (MoveToPointCoroutine (sa));
        if(sa.root.Length > 0) core.storedCoolDown += 3;
        GetComponent<BoxCollider> ().enabled = true;
    }

    IEnumerator MoveToPointCoroutine (SelectedArea area) {
        isMoving = true;
        var nowDir = Direction.NONE;
        foreach (var d in area.root) {
            Vector3 dir = d == 'r' ? Vector3.right :
                d == 'l' ? Vector3.left :
                d == 'u' ? Vector3.back : Vector3.forward;
            if (nowDir != MapStatus.VectorToDirection (dir)) {
                nowDir = MapStatus.VectorToDirection (dir);
                view.ActionView ("move", nowDir);
            }
            for (int i = 0; i < moveFrame; i++) {
                transform.position += dir * MapStatus.MAPCHIP_SIZE / moveFrame;
                yield return null;
            }
        }
        core.status.pos = area.pos;
        view.ActionView ("idle", nowDir);
        core.status.dir = nowDir;
        core.NextAction (KnightAction.select);
        isMoving = false;
    }

    bool CheckMovable (Vector2 point) {
        return core.selectedArea.Where(s => s.type == AreaType.move || s.type == AreaType.move_attack)
            .Select (m => m.pos).Contains (point);
    }

    public void OnCancel() {
        var diff = core.prev_pos - core.status.pos;
        core.transform.position += Vector3.right * MapStatus.MAPCHIP_SIZE * diff.x +
            Vector3.back * MapStatus.MAPCHIP_SIZE * diff.y;
        core.status.pos = core.prev_pos;
        core.storedCoolDown -= 3;
        core.NextAction(KnightAction.look);
    }

}