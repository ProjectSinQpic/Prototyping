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
        BuildMap();
    }

    void BuildMap() {
        for(int j = 0; j < MapStatus.MAP_HEIGHT; j++) {
            for(int i = 0; i < MapStatus.MAP_WIDTH; i++) {
                int map_index = (int)MapStatus.MapTypeOf(new Vector2(i, j));
                if (map_index == 0) continue;
                var map_obj = Instantiate(MapChip_set[map_index], container);
                map_obj.transform.localPosition = MapStatus.ToWorldPos(new Vector2(i, j));
            }
        }
    }
}
