using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class NetworkConnecter : MonoBehaviourPunCallbacks {

    public Button multi;

    void Start() {
        PhotonNetwork.ConnectUsingSettings();
    }

    public void StartGame(bool isSingle) {
        if(isSingle) SceneManager.LoadScene("Battle");
        else PhotonNetwork.JoinOrCreateRoom("room", new RoomOptions(), TypedLobby.Default);
    }

    public override void OnJoinedLobby() {
        Debug.Log("ロビーに入りました。");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnConnectedToMaster() {
        multi.interactable = true;
    }

    public override void OnJoinedRoom() {
        Debug.Log("ルームへ入室しました。");
        SceneManager.LoadScene("Battle_Network");
    }

    void OnPhotonRandomJoinFailed() {
        Debug.Log("ルームの入室に失敗しました。");
    }
}