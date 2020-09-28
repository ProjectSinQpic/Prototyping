using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class KnightDeath : KnightParts {

    void Start () {
        core.Message
            .Where (x => x == KnightAction.die)
            .Subscribe (_ => Die ());
    }

    void Die () {
        core.isDead = true;
        GetComponent<BoxCollider> ().enabled = false;
        StartCoroutine (DieCoroutine ());
    }

    IEnumerator DieCoroutine () {
        const int frame = 15;
        var sp = GetComponent<SpriteRenderer> ();
        for (int i = 0; i < frame; i++) {
            sp.color -= new Color (0, 0, 0, 1f / frame);
            yield return null;
        }
        sp.color = new Color (1, 1, 1, 0);
    }
}