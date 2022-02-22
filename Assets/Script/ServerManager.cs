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
        Debug.Log(PhotonNetwork.NickName + "���� �κ� ����Ǿ����ϴ�.");
    }

    public void JoinClass()
    {
        PhotonNetwork.JoinOrCreateRoom("���ǽ�", new RoomOptions { MaxPlayers = 5 }, null);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("���ǽ��� �����Ǿ����ϴ�.");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.NickName + "���� ���ǽǿ� �����߽��ϴ�.");
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("���� ���� ����");
    }
}
