using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KnightInfo {

    public KnightDatabase data;
    public int level;
    public int exp;
    public int SP;

    public static KnightInfo Create(KnightDatabase data, int level = 1) {
        var knight = new KnightInfo();
        knight.data = data;
        knight.level = level;
        knight.exp = 0;
        knight.SP = 0;
        return knight;
    }


}
