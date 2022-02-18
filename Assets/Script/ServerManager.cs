using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class ServerManager : MonoBehaviourPunCallbacks
{
    private bool connect = false;
    public string nickname;

    private void Awake()
    {
        Connect();
    }

    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("서버 접속 완료");
        PhotonNetwork.JoinOrCreateRoom("Lobby", new RoomOptions { MaxPlayers = 10 }, null);
        connect = true;
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("서버 연결 끊김");
    }
}
