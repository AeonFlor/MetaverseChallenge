using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class ServerManager : MonoBehaviourPunCallbacks
{
    private string nickname;

    private void Awake()
    {
        nickname = GameObject.FindWithTag("Player").GetComponent<AvatarController>().nickname;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = nickname;

        Connect();
    }

    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
        Debug.Log(PhotonNetwork.NickName + "님이 로비에 연결되었습니다.");
    }

    public void JoinClass()
    {
        PhotonNetwork.JoinOrCreateRoom("강의실", new RoomOptions { MaxPlayers = 5 }, null);
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
        Debug.Log("서버 연결 끊김");
    }
}
