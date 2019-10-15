using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryLoader {

    public static List<KnightInfo> Knights { get { return knights; } }

    public static List<KnightInfo> knights; //TODO privateに変更する

    public static void Save () {
        var k = knights.Aggregate ("", (a, x) => a + ";" + JsonUtility.ToJson (x));
        Debug.Log ("save : " + k);
        PlayerPrefs.SetString ("knight", k);
    }

    public static void Load () {
        var k = PlayerPrefs.GetString ("knight");
        Debug.Log ("load : " + k);
        knights = k.Split (new char[] { ';' })
            .Where (x => x != "")
            .Select (x => JsonUtility.FromJson<KnightInfo> (x))
            .ToList ();
    }

}