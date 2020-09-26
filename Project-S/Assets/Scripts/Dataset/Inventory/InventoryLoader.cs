using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryLoader {

    public static List<KnightInfo> Knights_blue { get { return knights_blue; } }
    public static List<KnightInfo> Knights_red { get { return knights_blue; } }

    public static List<KnightInfo> knights_blue, knights_red; //TODO privateに変更する

    public static void Save () {
        var b = knights_blue.Aggregate ("", (a, x) => a + ";" + JsonUtility.ToJson (x));
        Debug.Log ("save : " + b);
        PlayerPrefs.SetString ("knight_blue", b);

        var r = knights_blue.Aggregate ("", (a, x) => a + ";" + JsonUtility.ToJson (x));
        Debug.Log ("save : " + r);
        PlayerPrefs.SetString ("knight_red", r);
    }

    public static void Load () {
        var b = PlayerPrefs.GetString ("knight_blue");
        Debug.Log ("load : " + b);
        knights_blue = b.Split (new char[] { ';' })
            .Where (x => x != "")
            .Select (x => JsonUtility.FromJson<KnightInfo> (x))
            .ToList ();

        var r = PlayerPrefs.GetString ("knight_red");
        Debug.Log ("load : " + r);
        knights_red = r.Split (new char[] { ';' })
            .Where (x => x != "")
            .Select (x => JsonUtility.FromJson<KnightInfo> (x))
            .ToList ();
    }

}