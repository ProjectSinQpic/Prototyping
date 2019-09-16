using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

public class Enemy_Brain : MonoBehaviour {

    void Start() {
        GameState.isMyTurn
            .Where(_ => !KnightCore_Enemy.enemy_all.All(x => x.isDead))
            .Where(x => !x)
            .Subscribe(_ => Run());
    }

    void Run() {                                                //ここに敵の行動AIを記述する
        StartCoroutine(RunAI());
    }

    IEnumerator RunAI() {                                       //適当なユニットを適当な場所に移動させるAI
        var enemys = KnightCore_Enemy.enemy_all.Where(x => !x.isDead).ToList();
        var target = enemys[Random.Range(0, enemys.Count)];
        yield return new WaitForSeconds(0.5f);
        yield return RandomMoveKnight(target);
        yield return new WaitForSeconds(0.5f);
        target.NextAction("finish");
    }

    IEnumerator RandomMoveKnight(KnightCore knight) {
        var a = knight.GetComponent<KnightDisplayArea>();
        var movable = a.CalcMovableArea();
        var goal = movable[Random.Range(0, movable.Count)];
        knight.next_pos = goal.pos;
        var m = knight.GetComponent<KnightMovement>();
        StartCoroutine(m.MoveToPointCoroutine(goal));
        while (m.isMoving) { yield return null; }
    }
}
