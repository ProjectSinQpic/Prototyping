using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class Enemy_Brain : MonoBehaviour {

    void Start () {
        GameState.isMyTurn
            .Where (_ => !KnightCore_Enemy.enemy_all.All (x => x.isDead))
            .Where (x => !x)
            .Subscribe (_ => Run ());
    }

    void Run () { //ここに敵の行動AIを記述する
        StartCoroutine (RunAI ());
    }

    IEnumerator RunAI () { //適当なユニットを適当な場所に移動させるAI
        var enemys = KnightCore_Enemy.enemy_all.Where (x => !x.isDead).ToList ();
        var target = enemys[Random.Range (0, enemys.Count)];

        yield return new WaitForSeconds (0.5f);

        GameState.selected.Value = target;
        DicideNextPos (target);

        yield return new WaitForSeconds (0.5f);
        target.NextAction ("move");

        target.NextAction ("finish");
    }

    void DicideNextPos (KnightCore target) {
        var disp = target.GetComponent<KnightDisplayArea> ();
        var goal = disp.movableArea[Random.Range (0, disp.movableArea.Count)];
        disp.core.next_pos = goal.pos;
    }
}