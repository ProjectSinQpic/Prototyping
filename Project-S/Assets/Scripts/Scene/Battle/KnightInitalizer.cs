using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class KnightInitalizer : MonoBehaviour {

    public List<KnightStatus> players, enemies; //TODO 後にプレハブから生成したい
    void Awake() {
        
        for(int i = 0; i < players.Count; i++) {
            players[i].data = InventoryLoader.Knights[i].data;
        }

        players.ForEach(x => x.Init());

        enemies.ForEach(x => x.Init());
    }

}
