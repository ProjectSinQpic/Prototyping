using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KnightInitializer : MonoBehaviour {

    public List<KnightStatus> players_blue, players_red; //TODO 後にプレハブから生成したい
    void Awake () {

        if (InventoryLoader.Knights_blue != null) {
            for (int i = 0; i < players_blue.Count; i++) {
                var player = players_blue[i];
                var inventory = InventoryLoader.Knights_blue[i];
                player.data = inventory.data;
                player.level = inventory.level;
                player.SP = inventory.SP;
            }
        }

        if (InventoryLoader.Knights_red != null) {
            for (int i = 0; i < players_red.Count; i++) {
                var player = players_red[i];
                var inventory = InventoryLoader.Knights_red[i];
                player.data = inventory.data;
                player.level = inventory.level;
                player.SP = inventory.SP;
            }
        }

        players_blue.ForEach (x => x.Init ());
        players_red.ForEach (x => x.Init ());

    }

}