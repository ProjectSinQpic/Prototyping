using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using UnityEngine;


public class MapFileDecoder : MonoBehaviour {

    public static Map_type[] DecodeMap (string file_name) {
        Map_type[] map = new Map_type[MapStatus.MAP_WIDTH * MapStatus.MAP_HEIGHT];
        var tx = Resources.Load("Map/" + file_name) as TextAsset;
        var lines = tx.text.Split('\n');
        int j = 0;
        foreach (var line in lines) {
            if (j < MapStatus.MAP_HEIGHT) {
                Debug.Log(j + "  " + line);
                for (int i = 0; i < MapStatus.MAP_WIDTH; i++) {
                    map[j * MapStatus.MAP_WIDTH + i] = DecodeMapType (line[i]);
                }
                j++;
            }
            else {
                var info = line.Split(',').Select(x => int.Parse(x)).ToList();
                FieldManaPlacer.instance.PlaceMana(new Vector2(info[0], info[1]), info[2]);
                j++;
            }

        }
        return map;
    }

    static Map_type DecodeMapType (Char c) {
        if (c == '.') return Map_type.MAP_NONE;
        if (c == 'f') return Map_type.MAP_FIELD;
        if (c == 'w') return Map_type.MAP_WALL;
        else return Map_type.MAP_NONE;
    }

}