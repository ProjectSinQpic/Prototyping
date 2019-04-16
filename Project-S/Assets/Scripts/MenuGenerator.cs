using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuGenerator : MonoBehaviour
{

    public GameObject prefab_bar;
    public GameObject obj_menu;

    static MenuGenerator instance = null;
    public Vector2 original_menusize;

    void Start() {
        if (instance == null) {
            instance = this;
        }
        original_menusize = obj_menu.GetComponent<RectTransform>().sizeDelta;
        Close();
    }

    void Update() {
        
    }

    public static MenuGenerator Instance() {
        return instance;
    }

    public void Create(Dictionary<string, UnityAction> items) {
        var menu = obj_menu.transform;
        for (int i = 0; i < menu.childCount; i++) {
            Destroy(menu.GetChild(i).gameObject);
        }
        foreach(var i in items) {
            var obj = Instantiate(prefab_bar);
            obj.transform.SetParent(menu);
            obj.transform.GetChild(0).GetComponent<Text>().text = i.Key;
            obj.GetComponent<Button>().onClick.AddListener(i.Value);
        }
    }

    public void Open() {
        obj_menu.GetComponent<RectTransform>().sizeDelta = original_menusize;
        for (int i = 0; i < obj_menu.transform.childCount; i++) {
            obj_menu.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void Close() {
        obj_menu.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        for (int i = 0; i < obj_menu.transform.childCount; i++) {
            obj_menu.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
