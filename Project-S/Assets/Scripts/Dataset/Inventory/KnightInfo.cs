using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KnightInfo {

    public KnightDatabase data;
    public int level;
    public int exp;

    public static KnightInfo Create(KnightDatabase data, int level = 1, int exp = 0) {
        var knight = new KnightInfo();
        knight.data = data;
        knight.level = level;
        knight.exp = exp;
        return knight;
    }


}
