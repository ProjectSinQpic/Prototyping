using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWindow : MonoBehaviour {

    static List<string> lockingMenues = new List<string>();
    public static bool isLocked { get { return lockingMenues.Count > 0; } }

    protected void Lock(string id) {
        lockingMenues.Add(id);
    }

    protected void UnLock(string id) {
        if (lockingMenues.Exists(x => x == id)) lockingMenues.Remove(id);
    }
}
