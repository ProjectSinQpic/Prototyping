using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UniRx;
using System.Linq;
using Photon.Pun;

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

    PhotonView view;

    public bool isNetworkMode;

    void Awake() {
        canSend = true;
        instance = this;
        _message = new Subject<string[]>();
        if(isNetworkMode) terminalObj.SetActive(false);
    }

    void Start() {
        message.Subscribe(c => Debug.Log(String.Join(" ", c)));
        if(isNetworkMode) view = GetComponent<PhotonView>();
        StartGame();
    }

    void StartGame() {
        bool isFirst = !isNetworkMode || PhotonNetwork.IsMasterClient;
        Turn_State myTurn = isFirst ? Turn_State.blue : Turn_State.red;
        GameState.instance.StartGame(myTurn);
        KnightInitializer.instance.SetKnight(isFirst);
    }

    public void SendCommand(string message) {
        if(!isNetworkMode) return;
        view.RPC("ReceiveCommand", RpcTarget.Others, message);
    }

    [PunRPC]
    public void ReceiveCommand(string message) {
        Debug.Log("received : " + message);
        var words = message.Split(' ');
        _message.OnNext(words);  
    }

    public void SendCommandByTerminal() {
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
