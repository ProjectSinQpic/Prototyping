using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

public class KnightMovement : KnightParts {
    readonly int moveFrame = 5;
    bool isMoving = false;
    KnightDisplayArea _disp;
    Vector2 prev_pos;

    void Awake() {
        prev_pos = core.status.pos;
        _disp = core.GetComponent<KnightDisplayArea>();
    }

    void Start() {
        core.Message
            .Where(x => x == "move")
            .Subscribe(_ => MoveToPoint(core.next_pos));
    }

    public void MoveToPoint(Vector2 goal) {
        if (isMoving) return;
        core.isSelected.Value = false; //あとで変更する
        if (!CheckMovable(goal)) return;
        StartCoroutine(MoveToPointCoroutine(_disp.movableArea.Find(m => m.pos == goal)));
    }


    IEnumerator MoveToPointCoroutine(MovableArea area) {
        isMoving = true;
        foreach (var d in area.root) {
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
            { "攻撃", () => OnAttack() },
            { "待機", () => OnWait() },
            { "キャンセル", () => OnCancel() },
        }, new Vector3(Screen.width / 2 - 180, Screen.height / 2 - 150, 0), "knight_choice", true);
        isMoving = false;
    }

    bool CheckMovable(Vector2 point) {
        return _disp.movableArea.Select(m => m.pos).Contains(point);
    }

    void OnAttack() {
        prev_pos = core.status.pos;
        MenuGenerator.Instance().Close();
    }

    void OnWait() {
        prev_pos = core.status.pos;
        MenuGenerator.Instance().Close();
    }

    void OnCancel() {
        var diff = prev_pos - core.status.pos;
        transform.position += Vector3.right * MapStatus.MAPCHIP_SIZE * diff.x
            + Vector3.back * MapStatus.MAPCHIP_SIZE * diff.y;
        core.status.pos = prev_pos;
        MenuGenerator.Instance().Close();
    }
}
