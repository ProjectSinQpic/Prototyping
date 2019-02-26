using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Map_type {
    MAP_NONE,
    MAP_FIELD,
    MAP_WALL,
}

public class MapStatus
{
    public static Map_type[] field_map;
    public const int MAP_WIDTH = 20;
    public const int MAP_HEIGHT = 20;
    public const float MAPCHIP_SIZE = 10f;
    public const string MAP_FILE_LOCATION = "/Resources/Map/";

    public static int[] MAP_MP = { 10000, 1, 2 };

    public static Map_type MapTypeOf(Vector2 pos) {
        return field_map[(int)(pos.y * MAP_WIDTH + pos.x)];
    }

    public static Vector2 ToMapPos(Vector3 wp) {
        Vector3 offset = new Vector3(-MAP_WIDTH, 0, MAP_HEIGHT) * MAPCHIP_SIZE / 2;
        Vector3 v = wp - offset;
        return new Vector2((int)v.x / MAPCHIP_SIZE, (int)-v.z / MAPCHIP_SIZE);
    }

    public static Vector3 ToWorldPos(Vector2 mp) {
        Vector3 offset = new Vector3(-MAP_WIDTH, 0, MAP_HEIGHT) * MAPCHIP_SIZE / 2;
        return new Vector3(mp.x * MAPCHIP_SIZE, 0, -mp.y * MAPCHIP_SIZE) + offset;
    }

    public static bool IsOutOfMap(Vector2 pos) {
        if (pos.x < 0 || pos.x >= MAP_WIDTH) return true;
        if (pos.y < 0 || pos.y >= MAP_HEIGHT) return true;
        return false;
    }

}
