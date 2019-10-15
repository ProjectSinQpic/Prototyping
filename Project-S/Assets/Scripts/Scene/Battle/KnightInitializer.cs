using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KnightInitializer : MonoBehaviour {

    public List<KnightStatus> players, enemies; //TODO 後にプレハブから生成したい
    void Awake () {

        if (InventoryLoader.Knights != null) {
            for (int i = 0; i < players.Count; i++) {
                var player = players[i];
                var inventory = InventoryLoader.Knights[i];
                player.data = inventory.data;
                player.level = inventory.level;
                player.SP = inventory.SP;
            }
        }

        players.ForEach (x => x.Init ());

        enemies.ForEach (x => x.Init ());
    }

}