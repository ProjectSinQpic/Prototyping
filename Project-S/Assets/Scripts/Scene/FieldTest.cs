using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FieldTest : MonoBehaviour {
    // Start is called before the first frame update
    public List<KnightInfo> knights_blue, knights_red;
    public bool isLoadMode;
    public bool isSaveMode;

    void Start () {
        InventoryLoader.knights_blue = knights_blue;
        InventoryLoader.knights_blue = knights_red;
        if (isLoadMode) {
            InventoryLoader.Load ();
        }
        if (isSaveMode) {
            InventoryLoader.Save ();
        }

        SceneManager.LoadScene ("Battle");
    }

    // Update is called once per frame
    void Update () {

    }
}