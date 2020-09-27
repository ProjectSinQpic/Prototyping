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
        var fi = new FileInfo (Application.dataPath + MapStatus.MAP_FILE_LOCATION + file_name);

        using (StreamReader sr = new StreamReader (fi.OpenRead (), Encoding.UTF8)) {
            string line;
            int j = 0;
            while ((line = sr.ReadLine ()) != null && j < MapStatus.MAP_HEIGHT) {
                for (int i = 0; i < MapStatus.MAP_WIDTH; i++) {
                    map[j * MapStatus.MAP_WIDTH + i] = DecodeMapType (line[i]);
                }
                j++;
            }
            while ((line = sr.ReadLine ()) != null) {
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