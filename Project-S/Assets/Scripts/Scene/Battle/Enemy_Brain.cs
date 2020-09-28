using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class Enemy_Brain : MonoBehaviour {

    void Start () {
        GameState.instance.turn
            .Where (_ => !KnightCore.red_all.All (x => x.isDead))
            .Where (x => x == Turn_State.red)
            .Subscribe (_ => Run ());
    }

    void Run () { //ここに敵の行動AIを記述する
        StartCoroutine (RunAI ());
    }

    IEnumerator RunAI () { //適当なユニットを適当な場所に移動させるAI
        var enemys = KnightCore.red_all.Where (x => !x.isDead).ToList ();
        var target = enemys[Random.Range (0, enemys.Count)];

        yield return new WaitForSeconds (0.5f);

        GameState.instance.selected.Value = target;
        DicideNextPos (target);

        yield return new WaitForSeconds (0.5f);
        target.NextAction (KnightAction.move);

        target.NextAction (KnightAction.finish);
    }

    void DicideNextPos (KnightCore target) {
        var movableArea = target.selectedArea.Where(s => s.type == AreaType.move || s.type == AreaType.move_attack).ToList();
        var goal = movableArea[Random.Range (0, movableArea.Count)];
        target.next_pos = goal.pos;
    }
}