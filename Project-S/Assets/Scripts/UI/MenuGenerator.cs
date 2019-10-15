using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuGenerator : MonoBehaviour {

    public GameObject prefab_menu;
    public GameObject prefab_bar;
    public Stack<GameObject> menu_list;
    Stack<string> menu_id;

    List<string> lockingMenues;
    public bool isLocked { get { return lockingMenues.Count > 0; } }

    static MenuGenerator instance = null;

    void Awake() {
        lockingMenues = new List<string>();
        menu_list = new Stack<GameObject>();
        menu_id = new Stack<string>();
        if (instance == null) {
            instance = this;
        }
    }

    public static MenuGenerator Instance() {
        return instance;
    }

    public void Create(Dictionary<string, UnityAction> items, Vector3 pos, string id, bool locking) {
        var menu = Instantiate(prefab_menu);
        var menu_t = menu.transform;
        menu_t.SetParent(GameObject.Find("Canvas").transform);
        menu.GetComponent<RectTransform>().localPosition = pos;
        foreach (var i in items) {
            var obj = Instantiate(prefab_bar);
            obj.transform.SetParent(menu_t);
            obj.transform.GetChild(0).GetComponent<Text>().text = i.Key;
            obj.GetComponent<Button>().onClick.AddListener(i.Value);
        }
        if(menu_list.Count >= 1) {
            var old_top = menu_list.Peek();
            foreach (var i in old_top.transform.GetComponentsInChildren<Button>()) i.interactable = false;
        }
        menu_list.Push(menu);
        menu_id.Push(id);
        if (locking) lockingMenues.Add(id);
    }

    public void Close() {
        var old_menu = menu_list.Pop();
        Destroy(old_menu);
        if (menu_list.Count >= 1) {
            var new_top = menu_list.Peek();
            foreach (var i in new_top.transform.GetComponentsInChildren<Button>()) i.interactable = true;
        }
        var c_id = menu_id.Pop();
        if (lockingMenues.Exists(x => x == c_id)) lockingMenues.Remove(c_id);
    }

}
