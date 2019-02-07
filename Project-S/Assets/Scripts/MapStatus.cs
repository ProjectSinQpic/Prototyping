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

}
