using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

public class KnightMovement : KnightParts {
    public bool isMoving = false;
    public KnightView view;

    KnightDisplayArea _disp;
    Vector2 prev_pos;

    readonly int moveFrame = 5;

    void Awake() {
        prev_pos = core.status.pos;
        _disp = core.GetComponent<KnightDisplayArea>();
    }

    void Start() {
        core.Message
            .Where(x => x == "move")
            .Subscribe(_ => MoveToPoint(core.next_pos));

        core.Message
            .Where(x => x == "finish")
            .Subscribe(_ => prev_pos = core.status.pos);

        core.Message
            .Where(x => x == "attack_cancel")
            .Subscribe(_ => DisplayMenu());

    }

    public void MoveToPoint(Vector2 goal) {
        if (isMoving) return;
        core.isSelected.Value = false; //あとで変更する
        if (!CheckMovable(goal)) return;
        StartCoroutine(MoveAndSelectCoroutine(_disp.movableArea.Find(m => m.pos == goal)));
    }

    IEnumerator MoveAndSelectCoroutine(MovableArea area) {
        yield return MoveToPointCoroutine(area);
        DisplayMenu();
    }

    public IEnumerator MoveToPointCoroutine(MovableArea area) {
        isMoving = true;
        var nowDir = Direction.NONE;
        foreach (var d in area.root) {
            Vector3 dir = d == 'r' ? Vector3.right :
                          d == 'l' ? Vector3.left :
                          d == 'u' ? Vector3.back : Vector3.forward;
            if (nowDir != MapStatus.VectorToDirection(dir)) {
                nowDir = MapStatus.VectorToDirection(dir);
                view.ActionView("move", nowDir);
            }
            for (int i = 0; i < moveFrame; i++) {
                transform.position += dir * MapStatus.MAPCHIP_SIZE / moveFrame;
                yield return null;
            }
        }
        core.status.pos = area.pos;
        view.ActionView("idle", nowDir);
        core.status.dir = nowDir;
        isMoving = false;
    }

    public void DisplayMenu() {
        MenuGenerator.Instance().Create(new Dictionary<string, UnityEngine.Events.UnityAction> {
            { "攻撃", () => OnAttack() },
            { "待機", () => OnWait() },
            { "キャンセル", () => OnCancel() },
        }, new Vector3(Screen.width / 2 - 180, Screen.height / 2 - 250, 0), "knight_choice", true);
        GameState.knight_state = Knight_State.select;
    }

    bool CheckMovable(Vector2 point) {
        return _disp.movableArea.Select(m => m.pos).Contains(point);
    }

    void OnAttack() {
        Debug.Log("111");
        core.NextAction("attack_set");
        MenuGenerator.Instance().Close();
    }

    void OnWait() {
        core.NextAction("finish");
        MenuGenerator.Instance().Close();
    }

    void OnCancel() {
        var diff = prev_pos - core.status.pos;
        transform.position += Vector3.right * MapStatus.MAPCHIP_SIZE * diff.x
            + Vector3.back * MapStatus.MAPCHIP_SIZE * diff.y;
        core.status.pos = prev_pos;
        MenuGenerator.Instance().Close();
        GameState.knight_state = Knight_State.move;
    }
}
