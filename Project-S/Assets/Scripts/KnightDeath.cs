using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

public class KnightDeath : KnightParts {

    void Start() {

        core.Message
            .Where(x => x == "die")
            .Subscribe(_ => Die());
    }

    void Die() {
        //Destroy(gameObject);
        core.isDead = true;
        GetComponent<MeshRenderer>().enabled = false;
    }

}
