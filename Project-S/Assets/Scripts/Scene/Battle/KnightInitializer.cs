using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KnightInitializer : MonoBehaviour {

    public static KnightInitializer instance;

    public List<KnightStatus> players_blue, players_red; //TODO 後にプレハブから生成したい
    public List<Vector2> blue_positions, red_positions;

    void Awake() {
        instance = this;
    }

    public void SetKnight (bool isMaster) {

        for (int i = 0; i < players_blue.Count; i++) {
            var player = players_blue[i];
            player.pos = isMaster ? blue_positions[i] : red_positions[i];
            player.dir = isMaster ? Direction.SOUTH : Direction.NORTH;
            if (InventoryLoader.Knights_blue != null) {
                var inventory = InventoryLoader.Knights_blue[i];
                player.data = inventory.data;
                player.level = inventory.level;
                player.SP = inventory.SP;
            }
        }

        for (int i = 0; i < players_red.Count; i++) {
            var player = players_red[i];
            player.pos = isMaster ? red_positions[i] : blue_positions[i];
            player.dir = isMaster ? Direction.NORTH : Direction.SOUTH;
            if (InventoryLoader.Knights_red != null) {
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