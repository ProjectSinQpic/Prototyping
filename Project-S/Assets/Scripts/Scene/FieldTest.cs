﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using System.Linq;

public class FieldTest : MonoBehaviour {
    // Start is called before the first frame update
    public List<KnightInfo> knights;
    public bool isLoadMode;
    public bool isSaveMode;

    void Start() {
        InventoryLoader.knights = knights; 
        if(isLoadMode){
            InventoryLoader.Load();
        }
        if(isSaveMode){
            InventoryLoader.Save();
        }

        SceneManager.LoadScene("Battle");
    }

    // Update is called once per frame
    void Update() {
        
    }
}