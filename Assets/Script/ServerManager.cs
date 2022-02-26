using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class ServerManager : MonoBehaviourPunCallbacks
{
    public string nickname, player;


    public void openLobby(string player, string nickname)
    {
        Connect();

        this.player = player;
        this.nickname = nickname;
    }

    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(nickname + "님이 " + player + " 아바타로 로비에 서버에 연결되었습니다.");

        PhotonNetwork.AutomaticallySyncScene = true;

        var Player = PhotonNetwork.Instantiate(player, new Vector3(0, 10, 0), Quaternion.identity);
        Debug.Log(Player);
        PhotonNetwork.LocalPlayer.NickName = nickname;

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log(PhotonNetwork.NickName + "님이 로비에 연결되었습니다.");
    }

    public void JoinClass()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinOrCreateRoom("강의실", new RoomOptions { MaxPlayers = 5 }, null);
        }

        else
        {
            Connect();
        }
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("강의실이 생성되었습니다.");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.NickName + "님이 강의실에 참가했습니다.");
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("서버 연결 끊김 - 재접속 시도 중 ...");
        Connect();
    }
}
