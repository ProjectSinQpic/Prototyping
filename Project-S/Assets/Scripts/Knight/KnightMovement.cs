using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class KnightMovement : KnightParts {
    public bool isMoving = false;
    public KnightView view;

    KnightDisplayArea _disp;

    readonly int moveFrame = 5;

    void Awake () {
        _disp = core.GetComponent<KnightDisplayArea> ();
    }

    void Start () {
        core.Message
            .Where (x => x == "move")
            .Subscribe (_ => MoveToPoint (core.next_pos));

        core.Message
            .Where (x => x == "finish")
            .Subscribe (_ => core.prev_pos = core.status.pos);

    }

    public void MoveToPoint (Vector2 goal) {
        if (isMoving) return;
        core.NextAction ("look_cancel");
        if (!CheckMovable (goal)) {
            GameState.selected.Value = null;
            return;
        }
        StartCoroutine (MoveToPointCoroutine (_disp.selectedArea
            .Where(s => s.type == AreaType.move || s.type == AreaType.move_attack)
            .Where(m => m.pos == goal).First()));
        core.storedCoolDown += 3;
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
        core.NextAction ("select");
        isMoving = false;
    }

    bool CheckMovable (Vector2 point) {
        return _disp.selectedArea.Where(s => s.type == AreaType.move || s.type == AreaType.move_attack)
            .Select (m => m.pos).Contains (point);
    }

}