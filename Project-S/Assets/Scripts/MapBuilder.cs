using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBuilder : MonoBehaviour
{
    [SerializeField]
    Transform container;

    [SerializeField]
    GameObject[] MapChip_set;

    void Start() {
        MapStatus.field_map = MapFileDecoder.DecodeMap("test3.txt");
        BuildMap(MapStatus.field_map);
    }

    void BuildMap(Map_type[] mapdata) {
        Vector3 offset = new Vector3(-MapStatus.MAP_WIDTH, 0, MapStatus.MAP_HEIGHT) * MapStatus.MAPCHIP_SIZE / 2;
        for(int j = 0; j < MapStatus.MAP_HEIGHT; j++) {
            for(int i = 0; i < MapStatus.MAP_WIDTH; i++) {
                int map_index = (int)mapdata[j * MapStatus.MAP_WIDTH + i];
                if (map_index == 0) continue;
                var map_obj = Instantiate(MapChip_set[map_index], container);
                map_obj.transform.localPosition = new Vector3(i * MapStatus.MAPCHIP_SIZE, 0, -j * MapStatus.MAPCHIP_SIZE) + offset;
            }
        }
    }
}
