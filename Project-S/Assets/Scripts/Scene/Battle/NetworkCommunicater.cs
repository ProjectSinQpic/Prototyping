using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UniRx;
using System.Linq;

/**

map x y
knight x y
action [ attack | mana | wait | cancel ]
attack [ go | cancel]
skill [ x | go | cancel ]


///
knight 6 10
map 3 11
action attack
knight 2 11
attack go
///

*/

public class NetworkCommunicater : MonoBehaviour {

    public static NetworkCommunicater instance;

    public IObservable<string[]> message { get { return _message; } }

    public GameObject terminalObj;
    public InputField terminalText;
    public Button terminalButton;

    Subject<string[]> _message;

    public bool canSend;

    void Awake() {
        canSend = true;
        instance = this;
        _message = new Subject<string[]>();
        terminalObj.SetActive(false);
    }

    void Start() {
        message.Subscribe(c => Debug.Log(String.Join(" ", c)));
    }

    public void SendMessageByTerminal() {
        var commands = terminalText.text.Split(new string[]{ Environment.NewLine }, StringSplitOptions.None);
        terminalText.text = "";
        StartCoroutine(SendMultipleMessageCoroutine(commands));
    }

    IEnumerator SendMultipleMessageCoroutine(string[] commands) {
        foreach(var line in commands) {
            while(!canSend) yield return null;
            //yield return new WaitForSeconds(0.5f);
            var words = line.Split(' ');
            _message.OnNext(words);
        }
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.LeftShift)) terminalObj.SetActive(!terminalObj.activeInHierarchy);
    }



}
